/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package com.github.novaonline

import java.time.OffsetDateTime
import java.util.Properties

import com.github.novaonline.cassandra.HueLoggingLocalCluster
import com.github.novaonline.model.light._
import com.github.novaonline.serialization.SimpleJsonSchema
import org.apache.flink.api.java.utils.ParameterTool
import org.apache.flink.streaming.api.TimeCharacteristic
import org.apache.flink.streaming.api.functions.timestamps.BoundedOutOfOrdernessTimestampExtractor
import org.apache.flink.streaming.api.scala._
import org.apache.flink.streaming.api.windowing.time.Time
import org.apache.flink.streaming.connectors.cassandra.CassandraSink
import org.apache.flink.streaming.connectors.kafka.{FlinkKafkaConsumer010, FlinkKafkaProducer010}
import org.slf4j.LoggerFactory


/**
  * Simple Application that will record light event, light sessions, and accumulated light duration.
  */
object LightEvents {
  def main(args: Array[String]) {

    val LOG = LoggerFactory.getLogger("LightEvents")

    val parameter = ParameterTool.fromArgs(args)
    val kafkaHost = if (parameter.has("kafkaHost")) parameter.get("kafkaHost") else "kafka"
    val cassandraHost = if (parameter.has("cassandraHost")) parameter.get("cassandraHost") else "cassandra"

    // set up the streaming execution environment
    val env = StreamExecutionEnvironment.getExecutionEnvironment
    env.setStreamTimeCharacteristic(TimeCharacteristic.EventTime)

    val lightEventSchema = new SimpleJsonSchema[LightEvent]
    val lightSessionSchema = new SimpleJsonSchema[LightSession]
    val lightAccumlatedSchema = new SimpleJsonSchema[LightAccumulated]

    val properties = new Properties()
    properties.setProperty("bootstrap.servers", s"$kafkaHost:9092")
    properties.setProperty("group.id", "Hue-Logging")
    val lightEventSource = new FlinkKafkaConsumer010("hue-logging-light-event", lightEventSchema, properties)
    lightEventSource.setStartFromEarliest()

    val cassandraCluster = new HueLoggingLocalCluster(cassandraHost)

    // Start Map Reduce
    val lightEventStream = env.addSource(lightEventSource).map(x => {
      // enrich anything else missed from serializing
      x.copy(state = x.state.copy(addDate = OffsetDateTime.parse(x.addDate).toEpochSecond))
    })

    val watermarkedLightEventsStream = lightEventStream.assignTimestampsAndWatermarks(new BoundedOutOfOrdernessTimestampExtractor[LightEvent](Time.seconds(10)) {
      override def extractTimestamp(t: LightEvent): Long = OffsetDateTime.parse(t.addDate).toEpochSecond
    })

    val lightSessionsStream = watermarkedLightEventsStream
      .keyBy(x => x.light.id)
      .flatMapWithState[LightSession, LightState]((light: LightEvent, prevState: Option[LightState]) => {
       val currState = light.state
      if (prevState.isEmpty && currState.on) {
        LOG.info("Opened Light Session")
        (Iterator(LightSession(light.light, currState, None, 0)), Some(currState))
      } else if (prevState.isDefined && prevState.get.on && !currState.on) {
        LOG.info("Closed Light Session")
        val duration = currState.addDate - prevState.get.addDate
        (Iterator(LightSession(light.light, currState, Some(currState), duration)), None)
      } else {
        //LOG.info("Unknown State for Light Session")
        (Iterator.empty, None)
      }
    })
      .filter(x => x.endState.isDefined)

    val lightAccumulated = lightSessionsStream
      .keyBy(x => x.light.id)
      .sum("durationSeconds")
      .map(x => LightAccumulated(x.light, x.durationSeconds))


    // Publish Processed Streams to Kafka
    val kafkaLightSessionProducer = new FlinkKafkaProducer010("hue-logging-light-session", lightSessionSchema, properties)

    val kafkaLightAccumulatedProducer = new FlinkKafkaProducer010("hue-logging-light-accumulated", lightAccumlatedSchema, properties)

    lightSessionsStream.addSink(kafkaLightSessionProducer)
    lightAccumulated.addSink(kafkaLightAccumulatedProducer)


    CassandraSink.addSink(lightEventStream.map(x => {
      val r = x.toCassandraTuple
      LOG.info(s"mapped light event stream ${r.toString()}")
      r
    }))
      .setQuery("INSERT INTO hue_logging.light_events_by_light_name (light_name, light_id, add_date, light, state) VALUES ( ?, ? , toTimestamp(now()) , ?, ? );")
      .setClusterBuilder(cassandraCluster)
      .build()

    CassandraSink.addSink(lightSessionsStream.map(x => {
      val r = x.toCassandraTuple()
      LOG.info(s"mapped light session stream ${r.toString()}")
      r
    }))
      .setQuery("INSERT INTO hue_logging.light_sessions_by_light_name (light_name, light_id, add_date, duration_in_seconds,	light, state_end, state_start) VALUES (?, ?, toTimestamp(now()) , ?, ?, ?, ?);")
      .setClusterBuilder(cassandraCluster)
      .build()

    CassandraSink.addSink(lightAccumulated.map(x => {
      val r = x.toCassandraTuple()
      LOG.info(s"mapped light accumulated stream ${r.toString()}")
      r
    }))
      .setQuery("INSERT INTO hue_logging.light_total_duration_by_light_name (light_name, light_id, add_date, light, duration_in_seconds) VALUES (?, ?, toTimestamp(now()), ?, ?);")
      .setClusterBuilder(cassandraCluster)
      .build()

    // execute program
    env.execute("Hue Logging Streams")
  }
}
