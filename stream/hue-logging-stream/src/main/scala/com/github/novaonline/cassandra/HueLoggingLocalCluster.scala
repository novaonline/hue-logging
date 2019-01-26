package com.github.novaonline.cassandra

import com.datastax.driver.core.{Cluster, TypeCodec, UserType}
import com.datastax.driver.extras.codecs.date.SimpleTimestampCodec
import com.github.novaonline.cassandra.mappings.{LightCodec, LightStateCodec}
import com.github.novaonline.model.light.{Light, LightState}
import org.apache.flink.streaming.connectors.cassandra.ClusterBuilder
import org.slf4j.LoggerFactory

class HueLoggingLocalCluster(cassandraHost: String) extends ClusterBuilder {
  override def buildCluster(builder: Cluster.Builder): Cluster = {

    val LOG = LoggerFactory.getLogger("Cassandra.HueLogging")

    //TODO: give the cassandra cluster and internal alias
    val cluster = builder.addContactPoint(cassandraHost).build()

    cluster.newSession().execute(
      s"""
         |CREATE KEYSPACE IF NOT EXISTS hue_logging
         |	WITH REPLICATION = {
         |		'class': 'org.apache.cassandra.locator.NetworkTopologyStrategy',
         |		'datacenter1': '1'
         |	}
         |	AND DURABLE_WRITES = true;
       """.stripMargin)

    cluster.newSession().execute(
      s"""
         |CREATE TYPE IF NOT EXISTS hue_logging.light_state(
         |	is_on boolean,
         |	brightness int,
         |	saturation int,
         |	hue int,
         |	is_reachable boolean,
         |	event_date timestamp
         |);
       """.stripMargin)


    cluster.newSession().execute(
      s"""
         |CREATE TYPE IF NOT EXISTS hue_logging.light_info(
         |	id text,
         |	hue_type text,
         |	name text,
         |	model_id text,
         |	sw_version text
         |);
       """.stripMargin)


    cluster.newSession().execute(
      s"""
         CREATE TABLE IF NOT EXISTS hue_logging.light_events_by_light_name(
         |	add_date timestamp,
         |	light frozen<light_info>,
         |	light_id text,
         |	light_name text,
         |	state frozen<light_state>,
         |	PRIMARY KEY(light_name, light_id, add_date)
         |) WITH CLUSTERING ORDER BY (light_id ASC, add_date DESC) AND
         |	dclocal_read_repair_chance = 0.1 AND
         |	gc_grace_seconds = 864000 AND
         |	bloom_filter_fp_chance = 0.01 AND
         |	caching = { 'keys': 'ALL', 'rows_per_partition': 'NONE' } AND
         |	comment = '' AND
         |	compaction = { 'class': 'org.apache.cassandra.db.compaction.SizeTieredCompactionStrategy', 'max_threshold': '32', 'min_threshold': '4' } AND
         |	compression = { 'chunk_length_in_kb': '64', 'class': 'org.apache.cassandra.io.compress.LZ4Compressor' } AND
         |	default_time_to_live = 0 AND
         |	speculative_retry = '99PERCENTILE' AND
         |	min_index_interval = 128 AND
         |	max_index_interval = 2048 AND
         |	crc_check_chance = 1;
       """.stripMargin)


    cluster.newSession().execute(
      s"""
         |CREATE TABLE IF NOT EXISTS hue_logging.light_sessions_by_light_name (
         |	add_date timestamp,
         |	duration_in_seconds bigint,
         |    light frozen<light_info>,
         |	light_id text,
         |	light_name text,
         |	state_end frozen<light_state>,
         |	state_start frozen<light_state>,
         |	PRIMARY KEY(light_name, light_id, add_date)
         |) WITH CLUSTERING ORDER BY (light_id ASC, add_date DESC) AND
         |	dclocal_read_repair_chance = 0.1 AND
         |	gc_grace_seconds = 864000 AND
         |	bloom_filter_fp_chance = 0.01 AND
         |	caching = { 'keys': 'ALL', 'rows_per_partition': 'NONE' } AND
         |	comment = '' AND
         |	compaction = { 'class': 'org.apache.cassandra.db.compaction.SizeTieredCompactionStrategy', 'max_threshold': '32', 'min_threshold': '4' } AND
         |	compression = { 'chunk_length_in_kb': '64', 'class': 'org.apache.cassandra.io.compress.LZ4Compressor' } AND
         |	default_time_to_live = 0 AND
         |	speculative_retry = '99PERCENTILE' AND
         |	min_index_interval = 128 AND
         |	max_index_interval = 2048 AND
         |	crc_check_chance = 1;
       """.stripMargin)

    cluster.newSession().execute(
      s"""
         |CREATE TABLE IF NOT EXISTS hue_logging.light_total_duration_by_light_name (
         |	add_date timestamp,
         |	duration_in_seconds bigint,
         |	light frozen<light_info>,
         |	light_id text,
         |	light_name text,
         |	PRIMARY KEY(light_name, light_id, add_date)
         |) WITH CLUSTERING ORDER BY (light_id ASC, add_date DESC) AND
         |	dclocal_read_repair_chance = 0.1 AND
         |	gc_grace_seconds = 864000 AND
         |	bloom_filter_fp_chance = 0.01 AND
         |	caching = { 'keys': 'ALL', 'rows_per_partition': 'NONE' } AND
         |	comment = '' AND
         |	compaction = { 'class': 'org.apache.cassandra.db.compaction.SizeTieredCompactionStrategy', 'max_threshold': '32', 'min_threshold': '4' } AND
         |	compression = { 'chunk_length_in_kb': '64', 'class': 'org.apache.cassandra.io.compress.LZ4Compressor' } AND
         |	default_time_to_live = 0 AND
         |	speculative_retry = '99PERCENTILE' AND
         |	min_index_interval = 128 AND
         |	max_index_interval = 2048 AND
         |	crc_check_chance = 1;
       """.stripMargin)

    LOG.info("Building Cluster...")

    val aType: UserType = cluster.getMetadata.getKeyspace("hue_logging").getUserType("light_info")
    val bType: UserType = cluster.getMetadata.getKeyspace("hue_logging").getUserType("light_state")

    val lightCodec = TypeCodec.userType(aType)
    val lightStateCodec = TypeCodec.userType(bType)

    cluster.getConfiguration.getCodecRegistry.register(new LightCodec(lightCodec, classOf[Light]))
    cluster.getConfiguration.getCodecRegistry.register(new LightStateCodec(lightStateCodec, classOf[LightState]))
    cluster.getConfiguration.getCodecRegistry.register(new SimpleTimestampCodec())

    cluster
  }
}
