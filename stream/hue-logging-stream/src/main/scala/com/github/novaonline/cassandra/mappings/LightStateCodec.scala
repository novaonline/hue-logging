package com.github.novaonline.cassandra.mappings

import java.nio.ByteBuffer

import com.datastax.driver.core.{ProtocolVersion, TypeCodec, UDTValue, UserType}
import com.github.novaonline.model.light.LightState

class LightStateCodec(innerCodec: TypeCodec[UDTValue],javaType: Class[LightState]) extends TypeCodec[LightState](innerCodec.getCqlType, javaType) {

  override def serialize(t: LightState, protocolVersion: ProtocolVersion): ByteBuffer = innerCodec.serialize(toUDTValue(t), protocolVersion)

  override def deserialize(byteBuffer: ByteBuffer, protocolVersion: ProtocolVersion): LightState = toLightState(innerCodec.deserialize(byteBuffer, protocolVersion))

  override def parse(s: String): LightState = if (s == null || s.isEmpty) null  else toLightState(innerCodec.parse(s))

  override def format(t: LightState): String = if (t == null) null else innerCodec.format(toUDTValue(t))

  protected def toLightState(value: UDTValue): LightState = {
    if (value == null) null
    else
      LightState(
        on = value.getBool("is_on"),
        brightness = value.getInt("brightness"),
        saturation = value.getInt("saturation"),
        hue = value.getInt("hue"),
        reachable = value.getBool("is_reachable"),
        addDate = value.getTimestamp("event_date").getTime
      )
  }

  protected def toUDTValue(value: LightState): UDTValue =  {
    if (value == null) null
    else {
      val userType = innerCodec.getCqlType.asInstanceOf[UserType]
      val udt = userType.newValue()
      udt.setBool("is_on", value.on)
      udt.setInt("brightness", value.brightness)
      udt.setInt("saturation", value.saturation)
      udt.setInt("hue", value.hue)
      udt.setBool("is_reachable", value.reachable)
      udt.setLong("event_date", value.addDate)
      udt
    }
  }
}
