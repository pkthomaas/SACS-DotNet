using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SACS.ExampleApp.Models;
using SACS.Library.Activities;
using SACS.Library.Rest.Models.Common;
using SACS.Library.Rest.Models.LeadPriceCalendar;
using SACS.Library.SabreSoapApi;
using SACS.Library.Serialization;
using SACS.Library.Workflow;

namespace SACS.ExampleApp.Factories
{
    /// <summary>
    /// The static factory for view models.
    /// </summary>
    public static class ViewModelFactory
    {
        /// <summary>
        /// Creates the view model for LeadPriceCalendar result.
        /// </summary>
        /// <param name="sharedContext">The shared context.</param>
        /// <returns>The view model</returns>
        public static LeadPriceCalendarPostVM CreateLeadPriceCalendarVM(SharedContext sharedContext)
        {
            if (sharedContext.IsFaulty)
            {
                return new LeadPriceCalendarPostVM
                {
                    ErrorMessage = sharedContext.GetResult<string>(LeadPriceCalendarActivity.SharedContextKey) ?? string.Empty,
                    RequestUrl = sharedContext.GetResult<string>(LeadPriceCalendarActivity.RequestUrlSharedContextKey),
                };
            }

            LeadPriceCalendarRS leadPriceCalendarResponse = sharedContext.GetResult<LeadPriceCalendarRS>(LeadPriceCalendarActivity.SharedContextKey);
            return new LeadPriceCalendarPostVM
            {
                ResponseDotNet = ObjectPrinter.CreateString(leadPriceCalendarResponse),
                ResponseJson = sharedContext.GetResult<string>(LeadPriceCalendarActivity.ResponseJsonSharedContextKey),
                RequestUrl = sharedContext.GetResult<string>(LeadPriceCalendarActivity.RequestUrlSharedContextKey),
                ErrorMessage = string.Empty
            };
        }

        /// <summary>
        /// Creates the miscellaneous segment types for a drop down list.
        /// </summary>
        /// <returns>List of select list items.</returns>
        public static List<SelectListItem> CreateMiscSegmentTypesList()
        {
            return Enum.GetValues(typeof(PassengerDetailsRQMiscSegmentSellRQMiscSegmentType))
                .Cast<PassengerDetailsRQMiscSegmentSellRQMiscSegmentType>()
                .Select(enumValue => new SelectListItem()
                {
                    Value = enumValue.ToString(),
                    Text = enumValue.ToString(),
                    Selected = enumValue == PassengerDetailsRQMiscSegmentSellRQMiscSegmentType.OTH
                }).ToList();
        }

        /// <summary>
        /// Creates the view model for PassengerDetails and TravelItineraryRead results.
        /// </summary>
        /// <param name="sharedContext">The shared context.</param>
        /// <returns>The view model</returns>
        public static SoapWorkflowVM CreateSoapWorkflowVM(SharedContext sharedContext)
        {
            PassengerDetailsRS passengerDetails = sharedContext.GetResult<PassengerDetailsRS>(PassengerDetailsContactActivity.SharedContextKey);
            Exception ex = sharedContext.GetResult<Exception>(CommonConstants.ExceptionSharedContextKey);
            return new SoapWorkflowVM
            {
                BargainFinderMaxRequestXml = sharedContext.GetResult<string>(BargainFinderMaxSoapActivity.RequestXmlSharedContextKey),
                BargainFinderMaxResponseXml = sharedContext.GetResult<string>(BargainFinderMaxSoapActivity.ResponseXmlSharedContextKey),
                PassengerDetailsContactRequestXml = sharedContext.GetResult<string>(PassengerDetailsContactActivity.RequestXmlSharedContextKey),
                PassengerDetailsContactResponseXml = sharedContext.GetResult<string>(PassengerDetailsContactActivity.ResponseXmlSharedContextKey),
                EnhancedAirBookRequestXml = sharedContext.GetResult<string>(EnhancedAirBookActivity.RequestXmlSharedContextKey),
                EnhancedAirBookResponseXml = sharedContext.GetResult<string>(EnhancedAirBookActivity.ResponseXmlSharedContextKey),
                PassengerDetailsAgencyRequestXml = sharedContext.GetResult<string>(PassengerDetailsAgencyActivity.RequestXmlSharedContextKey),
                PassengerDetailsAgencyResponseXml = sharedContext.GetResult<string>(PassengerDetailsAgencyActivity.ResponseXmlSharedContextKey),
                TravelItineraryReadRequestXml = sharedContext.GetResult<string>(TravelItineraryReadActivity.RequestXmlSharedContextKey),
                TravelItineraryReadResponseXml = sharedContext.GetResult<string>(TravelItineraryReadActivity.ResponseXmlSharedContextKey),
                ErrorMessage = ex == null ? string.Empty : ex.Message
            };
        }

        /// <summary>
        /// Creates the view model for InstaFlight result.
        /// </summary>
        /// <param name="sharedContext">The shared context.</param>
        /// <returns>The view model</returns>
        public static InstaFlightsPostVM CreateInstaFlightsVM(SharedContext sharedContext)
        {
            if (sharedContext.IsFaulty)
            {
                return new InstaFlightsPostVM
                {
                    ErrorMessage = sharedContext.GetResult<string>(InstaFlightsActivity.SharedContextKey) ?? string.Empty,
                    RequestUrl = sharedContext.GetResult<string>(InstaFlightsActivity.RequestUriSharedContextKey)
                };
            }

            object result = sharedContext.GetResult(InstaFlightsActivity.SharedContextKey);
            return new InstaFlightsPostVM
            {
                RequestUrl = sharedContext.GetResult<string>(InstaFlightsActivity.RequestUriSharedContextKey),
                ResponseJson = sharedContext.GetResult<string>(InstaFlightsActivity.JsonSharedContextKey),
                ResponseDotNet = ObjectPrinter.CreateString(result),
                ErrorMessage = string.Empty
            };
        }

        /// <summary>
        /// Creates the view model for BargainFinderMax result.
        /// </summary>
        /// <param name="sharedContext">The shared context.</param>
        /// <returns>The view model</returns>
        public static BargainFinderMaxVM CreateBargainFinderMaxVM(SharedContext sharedContext)
        {
            BargainFinderMaxVM viewModel = new BargainFinderMaxVM
            {
                RequestUrl = sharedContext.GetResult<string>(BargainFinderMaxActivity.RequestUrlSharedContextKey),
                RequestJson = sharedContext.GetResult<string>(BargainFinderMaxActivity.RequestJsonSharedContextKey),
            };

            if (sharedContext.IsFaulty)
            {
                viewModel.ErrorMessage = sharedContext.GetResult<string>(BargainFinderMaxActivity.SharedContextKey) ?? string.Empty;
            } 
            else 
            {
                object result = sharedContext.GetResult(BargainFinderMaxActivity.SharedContextKey);
                viewModel.ResponseJson = sharedContext.GetResult<string>(BargainFinderMaxActivity.ResponseJsonSharedContextKey);
                viewModel.ResponseDotNet = ObjectPrinter.CreateString(result);
                viewModel.ErrorMessage = string.Empty;
            }

            return viewModel;
        }
    }
}