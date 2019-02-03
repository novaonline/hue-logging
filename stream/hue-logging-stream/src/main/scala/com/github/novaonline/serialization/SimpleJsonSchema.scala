package com.github.novaonline.serialization

import com.fasterxml.jackson.databind.{DeserializationFeature, MapperFeature, ObjectMapper}
import com.fasterxml.jackson.module.scala.DefaultScalaModule
import org.apache.flink.api.common.typeinfo.TypeInformation
import org.apache.flink.api.java.typeutils.TypeExtractor
import org.apache.flink.streaming.util.serialization.{KeyedDeserializationSchema, KeyedSerializationSchema}

import scala.reflect.ClassTag

class SimpleJsonSchema[T](implicit ct: ClassTag[T]) extends KeyedSerializationSchema[T] with KeyedDeserializationSchema[T] {

  private var mapper: ObjectMapper = _

  override def serializeKey(t: T): Array[Byte] = null

  override def serializeValue(t: T): Array[Byte] = {
    if (this.mapper == null) {
      this.mapper = new ObjectMapper
      mapper.registerModule(DefaultScalaModule)
      mapper.configure(MapperFeature.ACCEPT_CASE_INSENSITIVE_PROPERTIES, true)
      mapper.configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false)

    }
    mapper.writeValueAsBytes(t)
  }

  override def getTargetTopic(t: T): String = null

  override def deserialize(k: Array[Byte], v: Array[Byte], s: String, i: Int, l: Long): T = {
    if (this.mapper == null) {
      this.mapper = new ObjectMapper
      mapper.registerModule(DefaultScalaModule)
      mapper.configure(MapperFeature.ACCEPT_CASE_INSENSITIVE_PROPERTIES, true)
      mapper.configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false)
    }
    mapper.readValue(v, ct.runtimeClass.asInstanceOf[Class[T]])
  }

  override def isEndOfStream(t: T): Boolean = false

  override def getProducedType: TypeInformation[T] = TypeExtractor.getForClass(ct.runtimeClass.asInstanceOf[Class[T]])
}
