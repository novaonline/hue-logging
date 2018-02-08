# hue-logging
Polling Philips Hue Bridge the slow and painful way.

# Purpose
Philips hue bridge does not have a logging system available. This limits our ability to check how frequent a light is active for or how long it has been on for.

When you google search "philips hue logging" you'll end up [here](https://developers.meethue.com/content/logging-events-bridge), [here](https://developers.meethue.com/content/does-hue-bridge-have-logging-capability) or [here (actual response from dev support)](https://developers.meethue.com/content/event-logging). So as a temporary solution (hopefully), I created a web service that should be hosted locally to poll the bridge periodically.

# Development Setup
1. Pull this repo
1. Configure database connection in appsettings.Development.json (please do not include this file when creating PRs unless it's needed; would like to setup a appsettings.local.json file)
1. Run Project. Database Migrations will be done upon app startup.
1. Go To Settings > follow the instructions to setup bridge
1. Click start in Navigation Bar

There are two tables that provide us hue log data. `LightEvent` and `HueSession`

- LightEvent: Tracks when a light's state has changed
- HueSession: Tracks the time a light was on, then turned off.
