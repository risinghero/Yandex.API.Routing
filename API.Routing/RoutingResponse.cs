using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yandex.API
{
    public class RoutingResponse
    {
        /// <summary>
        /// Type of information about traffic used when building the route. Possible values
        /// </summary>
        public RoutingTraffic Traffic { get; set; }

        public RoutingResponseRoute Route { get; set; }
    }

    public class RoutingResponseRoute
    {
        public RoutingResponseRouteLeg[] Legs { get; set; }
    }

    public class RoutingResponseRouteLeg
    {
        public RoutingResponseRouteLegStatus Status { get; set; }

        public RoutingResponseRouteLegStep[] Steps { get; set; }
    }

    public class RoutingResponseRouteLegStep
    {
        /// <summary>
        /// Step length in meters.
        /// </summary>
        public decimal Length { get; set; }

        /// <summary>
        /// Time required to complete the step in seconds.
        /// </summary>
        public decimal Duration { get; set; }

        /// <summary>
        /// Routing mode
        /// </summary>
        public RoutingMode Mode { get; set; }

        /// <summary>
        /// Class of the road section the route segment is built on.
        /// </summary>
        [JsonProperty("feature_class")]
        public string FeatureClass { get; set; }

        /// <summary>
        /// Polyline making up a route.
        /// </summary>
        public RoutingResponseRouteLegPolyline Polyline { get; set; }
    }

    public class RoutingResponseRouteLegPolyline
    {
        /// <summary>
        /// Points that make up a line. Specified in decimal degrees (WGS84 standard). Each point is defined as a coordinate pair in the format: {latitude},{longitude}
        /// </summary>
        public decimal[][] Points { get; set; }
    }

    public enum RoutingResponseRouteLegStatus
    {
        OK,
        FAIL
    }

    public enum RoutingTraffic
    {
        /// <summary>
        /// Uses information about traffic at the time of the request.
        /// </summary>
        Realtime,
        /// <summary>
        /// Uses the traffic forecast for the next hour.
        /// </summary>
        Forecast
    }
}
