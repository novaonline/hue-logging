package com.github.novaonline.model.light

case class LightState(
                       on: Boolean,
                       brightness: Int,
                       saturation: Int,
                       hue: Int,
                       reachable: Boolean,
                       addDate: Long
                     )