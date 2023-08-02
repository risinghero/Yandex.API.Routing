# Yandex.API.Routing
Unofficial implementation of Yandex Routing API
Usage:

```c#
var result = await new RoutingClient(apiKeyOption).Route(new RoutingRequest(waypointOption) { 
        Mode=modeOption
    });
```
[Link to yandex routing docs](https://yandex.ru/dev/router/doc/en/request)