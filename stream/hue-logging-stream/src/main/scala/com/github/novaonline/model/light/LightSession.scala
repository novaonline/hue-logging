package com.github.novaonline.model.light

case class LightSession(
                         light: Light,
                         startState: LightState,
                         endState: Option[LightState] = None,
                         durationSeconds: Long
                       ) {
  def toCassandraTuple(): (String, String, Long, Light, LightState, LightState) =
    if (endState.isDefined)
      (light.name, light.id, durationSeconds, light, startState, endState.get)
    else
      (light.name, light.id, durationSeconds, light, startState, null)

}