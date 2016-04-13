using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SACS.Library.Serialization;

namespace SACS.Library.Rest.Models.Common
{
    [DataContract]
    public class FlightSegment
    {
        [DataMember(Name = "DepartureAirport")]
        public Location DepartureAirport { get; set; }

        [DataMember(Name = "ArrivalAirport")]
        public Location ArrivalAirport { get; set; }

        [DataMember(Name = "MarketingAirline")]
        public CompanyCode MarketingAirline { get; set; }

        [DataMember(Name = "ArrivalTimeZone")]
        public TimeZone ArrivalTimeZone { get; set; }

        [DataMember(Name = "TPA_Extensions")]
        public TPAExtensions TPAExtensions { get; set; }

        [DataMember(Name = "StopQuantity")]
        public int StopQuantity { get; set; }

        [DataMember(Name = "ElapsedTime")]
        public int ElapsedTime { get; set; }

        [DataMember(Name = "ResBookDesigCode")]
        public string ResBookDesigCode { get; set; }

        [DataMember(Name = "MarriageGrp")]
        public string MarriageGrp { get; set; }

        [JsonConverter(typeof(ArrayOrObjectConverter<Equipment>))]
        [DataMember(Name = "Equipment")]
        public IList<Equipment> Equipment { get; set; }

        [DataMember(Name = "DepartureDateTime")]
        public DateTime? DepartureDateTime { get; set; }

        [DataMember(Name = "ArrivalDateTime")]
        public DateTime? ArrivalDateTime { get; set; }

        [DataMember(Name = "FlightNumber")]
        public int FlightNumber { get; set; }

        [DataMember(Name = "OnTimePerformance")]
        public OnTimePerformance OnTimePerformance { get; set; }

        [DataMember(Name = "OperatingAirline")]
        public OperatingAirline OperatingAirline { get; set; }

        [DataMember(Name = "DepartureTimeZone")]
        public TimeZone DepartureTimeZone { get; set; }
    }
}