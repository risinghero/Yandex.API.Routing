using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Yandex.API
{
    public class RoutingRequest
    {
        public RoutingRequest((decimal Latitude, decimal Longitude)[] waypoints)
        {
            Waypoints = waypoints;
        }

        /// <summary>
        /// Route points specified in decimal degrees (WGS84 standard).
        /// </summary>
        /// <remarks>
        ///  If driving or truck is specified in the mode field, the maximum number of elements is 50.
        ///If walking or transit is specified in the mode field, the maximum number of elements is 25.
        /// </remarks>
        public (decimal Latitude, decimal Longitude)[] Waypoints { get; }

        /// <summary>
        /// Type of travel on the route
        /// </summary>
        public RoutingMode Mode { get; set; }

        /// <summary>
        /// The departure time. Used for calculating the expected traffic congestion. If this parameter is not specified, the traffic forecast is made for time when the request was processed. This value can't be in the past.
        /// </summary>
        public DateTime? DepartureTime { get; set; }

        /// <summary>
        /// No toll roads. When true, the route avoids toll roads. Default value: false.
        /// </summary>
        public bool AvoidTolls { get; set; }

        /// <summary>
        /// Vehicle weight in tons (only for mode=truck).
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// Actual vehicle axle load in tons (only for mode=truck).
        /// </summary>
        public decimal? AxleWeight { get; set; }

        /// <summary>
        /// Maximum allowed vehicle weight in tons (only for mode=truck).
        /// </summary>
        public decimal? MaxWeight { get; set; }

        /// <summary>
        /// Vehicle height in meters (only for mode=truck).
        /// </summary>
        public decimal? Height { get; set; }

        /// <summary>
        /// Vehicle width in meters (only for mode=truck).
        /// </summary>
        public decimal? Width { get; set; }

        /// <summary>
        /// Vehicle length in meters (only for mode=truck).
        /// </summary>
        public decimal? Length { get; set; }

        /// <summary>
        /// Maximum vehicle load capacity in tons (only for mode=truck).
        /// </summary>
        public decimal? Payload { get; set; }

        /// <summary>
        /// Vehicle emission standard (only for mode=truck).
        /// </summary>
        public string EcoClass { get; set; }

        /// <summary>
        /// Trailer (only for mode=truck). Default value: false.
        /// </summary>
        public bool HasTrailer { get; set; }

        public NameValueCollection Fill(NameValueCollection result)
        {
            result["waypoints"] = Waypoints.Aggregate(String.Empty, (h, t) =>
            {
                var appendString = t.Latitude.ToString(CultureInfo.InvariantCulture) + ","
                + t.Longitude.ToString(CultureInfo.InvariantCulture);
                if (string.IsNullOrEmpty(h))
                    return appendString;
                return h + "|" + appendString;
            });
            result["mode"] = Mode.ToString().ToLower();
            if(this.DepartureTime != null)
                result["departure_time"] = DepartureTime.ToString();
            if (this.AvoidTolls)
                result["avoid_tolls"] = "true";
            if (this.Weight != null)
                result["weight"] = this.Weight.Value.ToString(CultureInfo.InvariantCulture);
            if (this.AxleWeight!=null)
                result["axle_weight"]= this.AxleWeight.Value.ToString(CultureInfo.InvariantCulture);
            if(this.MaxWeight!=null)
                result["max_weight"] = this.MaxWeight.Value.ToString(CultureInfo.InvariantCulture);
            if(this.Height != null)
                result["height"] = this.Height.Value.ToString(CultureInfo.InvariantCulture);
            if(this.Width != null)
                result["width"] = this.Width.Value.ToString(CultureInfo.InvariantCulture);
            if (this.Length != null)
                result["length"] = this.Length.Value.ToString(CultureInfo.InvariantCulture);
            if(this.Payload != null)
                result["payload"] = this.Payload.Value.ToString(CultureInfo.InvariantCulture);
            if(this.EcoClass!=null)
                result["eco_class"] = this.EcoClass;
            if (this.HasTrailer)
                result["has_trailer"] = "true";
            return result;
        }

        public override string ToString() => Fill(new NameValueCollection()).ToString();
    }

    public enum RoutingMode
    {
        /// <summary>
        /// Passenger car route. Used by default.
        /// </summary>
        Driving,
        /// <summary>
        /// Truck route.
        /// </summary>
        Truck,
        /// <summary>
        ///  Walking route.
        /// </summary>
        Walking,
        /// <summary>
        /// Public transit route.
        /// </summary>
        Transit
    }
}
