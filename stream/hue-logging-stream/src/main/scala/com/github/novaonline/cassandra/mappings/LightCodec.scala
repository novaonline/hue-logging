package com.github.novaonline.cassandra.mappings

import java.nio.ByteBuffer

import com.datastax.driver.core.{ProtocolVersion, TypeCodec, UDTValue, UserType}
import com.github.novaonline.model.light.Light


class LightCodec(innerCodec: TypeCodec[UDTValue], javaType: Class[Light]) extends TypeCodec[Light](innerCodec.getCqlType, javaType) {

  override def serialize(t: Light, protocolVersion: ProtocolVersion): ByteBuffer = innerCodec.serialize(toUDTValue(t), protocolVersion)

  override def deserialize(byteBuffer: ByteBuffer, protocolVersion: ProtocolVersion): Light = toLight(innerCodec.deserialize(byteBuffer, protocolVersion))

  override def parse(s: String): Light = if (s == null || s.isEmpty) null  else toLight(innerCodec.parse(s))

  override def format(t: Light): String = if (t == null) null else innerCodec.format(toUDTValue(t))

  protected def toLight(value: UDTValue): Light = {
    if (value == null) null
    else Light(value.getString("id"), value.getString("hue_type"),value.getString("name"), value.getString("model_id"), value.getString("sw_version"))

  }

  protected def toUDTValue(value: Light): UDTValue =  {
    if (value == null) null
    else {
      val userType = innerCodec.getCqlType.asInstanceOf[UserType]
      val udt = userType.newValue()
      udt.setString("id", value.id)
      udt.setString("hue_type", value.hueType)
      udt.setString("name", value.name)
      udt.setString("model_id", value.modelId)
      udt.setString("sw_version", value.sWVersion)
      udt
    }
  }
}
