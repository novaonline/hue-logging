# hue-logging
Polling Philips Hue Bridge the slow and painful way.

# Purpose
Philips hue bridge does not have a logging system available. This limits our ability to check how frequent a light is active for or how long it has been on for.

When you google search "philips hue logging" you'll end up [here](https://developers.meethue.com/content/logging-events-bridge), [here](https://developers.meethue.com/content/does-hue-bridge-have-logging-capability) or [here (actual response from dev support)](https://developers.meethue.com/content/event-logging). So as a temporary solution (hopefully), I created a web service that should be hosted locally to poll the bridge periodically.

# Dataflow
![high-level-dataflow-high-level](https://user-images.githubusercontent.com/6725940/47617210-fbd63500-da92-11e8-885b-7fcd8baa91d9.png)

# Setup
1. navigate to root of this repository & `docker compose -d`
