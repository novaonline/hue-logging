package com.github.novaonline.model.light

case class LightEvent(light: Light, state: LightState, addDate: String)
{
  def toCassandraTuple: (String, String, Light, LightState ) = (light.name, light.id,  light, state )
}