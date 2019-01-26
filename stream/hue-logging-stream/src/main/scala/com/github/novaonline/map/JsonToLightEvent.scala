package com.github.novaonline.map

import java.time.OffsetDateTime

import com.github.novaonline.model.light.{Light, LightEvent, LightState}
import org.apache.flink.api.common.functions.MapFunction
import org.apache.flink.shaded.jackson2.com.fasterxml.jackson.databind.node.ObjectNode
import org.slf4j.{Logger, LoggerFactory}

/**
  * TODO: There must be a better way of doing this.
  */
object JsonToLightEvent extends MapFunction[ObjectNode, LightEvent] {

  val LOG: Logger = LoggerFactory.getLogger("JsonToLightEvent")

  override def map(t: ObjectNode): LightEvent = {
    val o = t.get("value")
    val lightPath = o.path("Light")
    val statePath = o.path("State")

    val light = Light(
      id = lightPath.get("Id").asText(),
      hueType = lightPath.get("Id").asText(),
      name = lightPath.get("Name").asText(),
      modelId = lightPath.get("ModelId").asText(),
      sWVersion = lightPath.get("SWVersion").asText(),
      raw = Some(lightPath)
    )

    val addDate = OffsetDateTime.parse(o.get("AddDate").asText())

    val state = LightState(
      on = statePath.get("On").asBoolean(),
      brightness = statePath.get("Brightness").asInt(),
      saturation = statePath.get("Saturation").asInt(),
      hue = statePath.get("Hue").asInt(),
      reachable = statePath.get("Reachable").asBoolean(),
      addDate = addDate.toEpochSecond,
      raw = Some(statePath)
    )

    LOG.debug("Created Light Event Object")
    LightEvent(light, state)
  }
}
