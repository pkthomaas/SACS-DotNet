using System;
using SACS.Library.Configuration;
using SACS.Library.SabreSoapApi;

namespace SACS.Library.Soap
{
    /// <summary>
    /// The SOAP service factory. It creates service clients and common models used in many calls.
    /// </summary>
    public class SoapServiceFactory
    {
        /// <summary>
        /// Value used as "from party identifier" value
        /// </summary>
        private const string FromPartyId = "sample.url.of.sabre.client.com";

        /// <summary>
        /// Value used as "from party identifier" type
        /// </summary>
        private const string FromType = "";

        /// <summary>
        /// Value used as "to party identifier" value
        /// </summary>
        private const string ToPartyId = "webservices.sabre.com";

        /// <summary>
        /// Value used as "to party identifier" type
        /// </summary>
        private const string ToType = "";

        /// <summary>
        /// The configuration provider
        /// </summary>
        private readonly IConfigProvider configProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoapServiceFactory"/> class.
        /// </summary>
        /// <param name="configProvider">The configuration provider.</param>
        public SoapServiceFactory(IConfigProvider configProvider)
        {
            this.configProvider = configProvider;
        }

        /// <summary>
        /// Creates the message header.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <returns>The message header</returns>
        public MessageHeader CreateHeader(string action, string conversationId)
        {
            string pseudoCityCode = this.configProvider.Group;

            return new MessageHeader
            {
                Service = new Service
                {
                    type = "OTA",
                    Value = action
                },
                Action = action,
                From = new From
                {
                    PartyId = new[]
                    {
                        new PartyId { type = FromType, Value = FromPartyId }
                    }
                },
                To = new To
                {
                    PartyId = new[]
                    {
                        new PartyId { type = ToType, Value = ToPartyId }
                    }
                },
                ConversationId = conversationId,
                CPAId = pseudoCityCode
            };
        }

        /// <summary>
        /// Creates the PassengerDetails service.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <param name="security">The security.</param>
        /// <returns>The PassengerDetails service</returns>
        public PassengerDetailsService CreatePassengerDetailsService(string conversationId, Security security)
        {
            return new PassengerDetailsService
            {
                MessageHeaderValue = this.CreateHeader("PassengerDetailsRQ", conversationId),
                SecurityValue = security,
                Url = this.configProvider.Environment
            };
        }

        /// <summary>
        /// Creates the Ping request.
        /// </summary>
        /// <returns>The Ping request</returns>
        public OTA_PingRQ CreatePingRequest()
        {
            return new OTA_PingRQ
            {
                EchoData = "echo",
                Version = "1.0.0",
                TimeStamp = DateTime.Now
            };
        }

        /// <summary>
        /// Creates the Ping service.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <param name="security">The security token.</param>
        /// <returns>The Ping service.</returns>
        public SWSService CreatePingService(string conversationId, Security security)
        {
            var header = this.CreateHeader("OTA_PingRQ", conversationId);
            return new SWSService
            {
                MessageHeader = new MessageHeader1
                {
                    Service = header.Service,
                    Action = header.Action,
                    From = header.From,
                    To = header.To,
                    ConversationId = header.ConversationId,
                    CPAId = header.CPAId
                },
                Security = new Security1
                {
                    BinarySecurityToken = new SecurityBinarySecurityToken { Value = security.BinarySecurityToken }
                },
                Url = this.configProvider.Environment
            };
        }

        /// <summary>
        /// Creates the SessionCreate request.
        /// </summary>
        /// <returns>The SessionCreate request</returns>
        public SessionCreateRQ CreateSessionCreateRequest()
        {
            return new SessionCreateRQ
            {
                POS = new SessionCreateRQPOS
                {
                    Source = new SessionCreateRQPOSSource
                    {
                        PseudoCityCode = this.configProvider.Group
                    }
                }
            };
        }

        /// <summary>
        /// Creates the SessionCreate service.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <returns>The SessionCreate service.</returns>
        public SessionCreateRQService CreateSessionCreateService(string conversationId)
        {
            string endpoint = this.configProvider.Environment;

            MessageHeader header = this.CreateHeader("SessionCreateRQ", "AuthConversation");
            Security security = this.CreateUsernameSecurity();

            return new SessionCreateRQService
            {
                MessageHeaderValue = header,
                SecurityValue = security,
                Url = endpoint
            };
        }

        /// <summary>
        /// Creates the TravelItineraryRead service.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <param name="security">The security.</param>
        /// <returns>The TravelItineraryRead service.</returns>
        public TravelItineraryReadService CreateTravelItineraryReadService(string conversationId, Security security)
        {
            return new TravelItineraryReadService
            {
                MessageHeaderValue = this.CreateHeader("TravelItineraryReadRQ", conversationId),
                SecurityValue = security,
                Url = this.configProvider.Environment
            };
        }

        /// <summary>
        /// Creates the BargainFinderMax service.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <param name="security">The security.</param>
        /// <returns>The BargainFinderMax service.</returns>
        public BargainFinderMaxService CreateBargainFinderMaxService(string conversationId, Security security)
        {
            return new BargainFinderMaxService
            {
                MessageHeaderValue = this.CreateHeader("BargainFinderMaxRQ", conversationId),
                SecurityValue = security,
                Url = this.configProvider.Environment
            };
        }

        /// <summary>
        /// Creates the EnhancedAirBook service.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <param name="security">The security.</param>
        /// <returns>The EnhancedAirBook service.</returns>
        public EnhancedAirBookService CreateEnhancedAirBookService(string conversationId, Security security)
        {
            return new EnhancedAirBookService
            {
                MessageHeaderValue = this.CreateHeader("EnhancedAirBookRQ", conversationId),
                SecurityValue = security,
                Url = this.configProvider.Environment
            };
        }

        /// <summary>
        /// Creates the IgnoreTransaction service.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <param name="security">The security.</param>
        /// <returns>The IgnoreTransaction service.</returns>
        public IgnoreTransactionService CreateIgnoreTransactionService(string conversationId, Security security)
        {
            return new IgnoreTransactionService
            {
                MessageHeaderValue = this.CreateHeader("IgnoreTransactionLLSRQ", conversationId),
                SecurityValue = security,
                Url = this.configProvider.Environment
            };
        }

        /// <summary>
        /// Creates the security token used for authorization call and containing the username token.
        /// </summary>
        /// <returns>The security token.</returns>
        public Security CreateUsernameSecurity()
        {
            string userName = this.configProvider.UserId;
            string password = this.configProvider.ClientSecret;
            string domain = this.configProvider.Domain;
            string pseudoCityCode = this.configProvider.Group;

            return new Security
            {
                UsernameToken = new SecurityUsernameToken
                {
                    Username = userName,
                    Password = password,
                    Organization = pseudoCityCode,
                    Domain = domain
                }
            };
        }
    }
}