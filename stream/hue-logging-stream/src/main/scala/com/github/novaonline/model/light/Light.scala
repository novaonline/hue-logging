package com.github.novaonline.model.light

import org.apache.flink.shaded.jackson2.com.fasterxml.jackson.databind.JsonNode

case class Light(
                  id: String,
                  hueType: String,
                  name: String,
                  modelId: String,
                  sWVersion: String,
                  raw: Option[JsonNode] = None
                )
