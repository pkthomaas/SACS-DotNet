using System;
using System.Collections.Generic;
using SACS.Library.Activities.InputData;

namespace SACS.ExampleApp.Models
{
    /// <summary>
    /// The POST request from SOAP workflow form.
    /// </summary>
    public class SoapWorkflowPostRQ : ISoapWorkflowData
    {
        /// <summary>
        /// Gets or sets the name of the given.
        /// </summary>
        /// <value>
        /// The name of the given.
        /// </value>
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the phone1 location code.
        /// </summary>
        /// <value>
        /// The phone1 location code.
        /// </value>
        public string Phone1LocationCode { get; set; }

        /// <summary>
        /// Gets or sets the phone1.
        /// </summary>
        /// <value>
        /// The phone1.
        /// </value>
        public string Phone1 { get; set; }

        /// <summary>
        /// Gets or sets the type of the phone1 use.
        /// </summary>
        /// <value>
        /// The type of the phone1 use.
        /// </value>
        public string Phone1UseType { get; set; }

        /// <summary>
        /// Gets or sets the phone2 location code.
        /// </summary>
        /// <value>
        /// The phone2 location code.
        /// </value>
        public string Phone2LocationCode { get; set; }

        /// <summary>
        /// Gets or sets the phone2.
        /// </summary>
        /// <value>
        /// The phone2.
        /// </value>
        public string Phone2 { get; set; }

        /// <summary>
        /// Gets or sets the type of the phone2 use.
        /// </summary>
        /// <value>
        /// The type of the phone2 use.
        /// </value>
        public string Phone2UseType { get; set; }

        /// <summary>
        /// Gets or sets the passenger email.
        /// </summary>
        /// <value>
        /// The passenger email.
        /// </value>
        public string PassengerEmail { get; set; }

        /// <summary>
        /// Gets or sets the agency address line.
        /// </summary>
        /// <value>
        /// The agency address line.
        /// </value>
        public string AgencyAddressLine { get; set; }

        /// <summary>
        /// Gets or sets the name of the agency city.
        /// </summary>
        /// <value>
        /// The name of the agency city.
        /// </value>
        public string AgencyCityName { get; set; }

        /// <summary>
        /// Gets or sets the agency country code.
        /// </summary>
        /// <value>
        /// The agency country code.
        /// </value>
        public string AgencyCountryCode { get; set; }

        /// <summary>
        /// Gets or sets the agency postal code.
        /// </summary>
        /// <value>
        /// The agency postal code.
        /// </value>
        public string AgencyPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the agency street number.
        /// </summary>
        /// <value>
        /// The agency street number.
        /// </value>
        public string AgencyStreetNumber { get; set; }

        /// <summary>
        /// Gets or sets the agency state code.
        /// </summary>
        /// <value>
        /// The agency state code.
        /// </value>
        public string AgencyStateCode { get; set; }

        /// <summary>
        /// Gets or sets the type of the agency ticket.
        /// </summary>
        /// <value>
        /// The type of the agency ticket.
        /// </value>
        public string AgencyTicketType { get; set; }

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