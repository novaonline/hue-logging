package com.github.novaonline.model.light

import org.apache.flink.shaded.jackson2.com.fasterxml.jackson.databind.JsonNode

case class LightState(
                       on: Boolean,
                       brightness: Int,
                       saturation: Int,
                       hue: Int,
                       reachable: Boolean,
                       addDate: Long,
                       raw: Option[JsonNode] = None
                     )