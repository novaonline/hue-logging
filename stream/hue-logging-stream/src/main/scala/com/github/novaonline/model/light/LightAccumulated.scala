package com.github.novaonline.model.light

case class LightAccumulated(light: Light, durationSeconds: Long) {
  def toCassandraTuple(): (String, String, Light, Long) = (light.name, light.id, light, durationSeconds)
}