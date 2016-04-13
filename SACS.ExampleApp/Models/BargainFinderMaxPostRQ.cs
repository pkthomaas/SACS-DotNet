using System;
using SACS.Library.Activities.InputData;

namespace SACS.ExampleApp.Models
{
    /// <summary>
    /// The POST request from the BargainFinderMax form.
    /// </summary>
    public class BargainFinderMaxPostRQ : IBargainFinderMaxData
    {
        public DateTime DepartureDate { get; set; }

        public string OriginAirportCode { get; set; }

        public string DestinationAirportCode { get; set; }

        public string RPH { get; set; }

        public string RequestorID { get; set; }

        public string RequestorType { get; set; }

        public string RequestorCompanyCode { get; set; }

        public string PassengerTypeCode { get; set; }

        public int NumberOfPassengers { get; set; }

        public string RequestType { get; set; }
    }
}