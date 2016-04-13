# Introduction
This documentation describes the Sabre API Code Samples for .NET.

The package is a Visual Studio solution containing an example ASP.NET MVC application and a small library which can be used to simplify the process of communication with Sabre SOAP and REST APIs. The library is also a set of code snippets ready for use in other projects.

# Installation and configuration

After obtaining credentials for SOAP and REST services, one should put them into the configuration files in the ```SACS.ExampleApp``` project.

In the file, each configuration property is defined as follows:

```
propertyName=propertyValue
```

All lines starting with a hash character (#) are treated as comments and ignored.

The required configuration properties are:

- userId: REST user ID or SOAP username,
- group: REST group or SOAP organization,
- domain: the domain (AA can be used as default domain),
- clientSecret: REST client secret or SOAP password,
- environment: the URL of the API.

For SOAP, the configuration file is located at: ```SACS.ExampleApp\Soap\SACSConfig.properties.```
For REST, the configuration file is located at: ```SACS.ExampleApp\Rest\SACSConfig.properties.```

The credentials can be obtained after registering on https://developer.sabre.com.

# Dependencies

The Sabre API Code Samples for .NET is a standard ASP.NET MVC application, containing all default dependencies. The project also requires following libraries, which are easily obtainable through NuGet:

- [Apache log4net](https://logging.apache.org/log4net/index.html): used for logging,
- [Newtonsoft Json.NET](http://www.newtonsoft.com/json): used for serialization.

Additionaly, the [wsdl.exe](https://msdn.microsoft.com/en-us/library/7h3ystb6.aspx) tool can be used for generating SOAP models. The tool is a part of Visual Studio SDK and should be present if both Visual Studio and .NET Framework are installed.

# Consuming/referencing the sample code/library

## Running the example application
The easiest method to run the application is to open the solution file ```SACS.sln``` in Visual Studio. Then set ```SACS.ExampleApp``` as startup project and run it. The ASP.NET MVC application will start and open in the selected browers window.
## Referencing the library
If you want to use the code from SACS library in your own project, you can just copy the ```SACS.Library``` folder from original location to your solution folder. Then add it to the solution (in Visual Studio Solution Explorer: right click on the main solution element, then choose Add -> Existing Project and select SACS.Library.csproj file).

Afterwards, you can just add reference to the library in the project you want to use it (right click on References -> Add Reference -> Solution).
## SOAP calls
The ```SoapController.SoapWorkflow``` action is the entry point for an example SOAP workflow. The Workflow object is created and ```InitialSoapActivity``` is set as the first activity to be run. The data needed for the SOAP calls are passed from the application’s form through the ```SoapWorkflowPostRQ``` model.

The ```SoapServiceFactory``` is a class used to create common models and service clients used in SOAP calls.

```cs
IActivity activity = new InitialSoapActivity (
		new SoapServiceFactory(ConfigFactory.CreateForSoap()),
		SessionPool,
  	requestModel);
Workflow workflow = new Workflow(activity);
SharedContext sharedContext = await workflow.RunAsync();
```

The first activity is ```InitialSoapActivity``` which only sets the shared context up with necessary data to be used by our SOAP calls.

The ```RunAsync()``` method of each next activity performs a SOAP call and then returns the next activity to be started. When an activity returns null, no further activities will run in this workflow.

The ```SharedContext``` argument is used to pass additional parameters to the method. It can for example contain results from previous calls.

Inside this method (for example in ```BargainFinderMaxSoapActivity```), first, the security token (authentication) is obtained from the session pool:
```cs
var security = await this.sessionPool.TakeSessionAsync(
    sharedContext.ConversationId);
```

```ConversationId``` is an unique identifier of the workflow.

Then, the service client and request model are built, the request is performed and result is saved to the shared context:

```cs
	var service = this.soapServiceFactory.CreateBargainFinderMaxService(
		sharedContext.ConversationId, security);
		var request = this.CreateRequest(data);
		var result = service.BargainFinderMaxRQ(request);
		sharedContext.AddResult(SharedContextKey, response);
```
Then, the next activity, ```PassengerDetailsContactActivity``` is created and returned to the workflow. It will be started in next step.

The steps of the example SOAP workflow are:
1. ```InitialSoapActivity``` – initializes the data used in requests,

2. ```BargainFinderMaxSoapActivity``` -  searches for flights,

3. ```PassengerDetailsContactActivity``` - sets the passengers given name, surname and phone numbers,

4. ```EnhancedAirBookActivity``` -  books the flights,

5. ```PassengerDetailsAgencyActivity``` - adds rest of the PNR information, like ticketing type and agency address,

6. ```TravelItineraryReadActivity``` - reads the saved PNR information.


```PassengerDetailsContactActivity```, ```EnhancedAirBookActivity``` and ```PassengerDetailsAgencyActivity``` requests have the ```IgnoreOnError``` property set to true in order to ignore the transaction if an error occurs.

Through the ```ISessionPool``` interface the developer can inject his own implementation of the session pool. The basic implementation, ```SessionPoolSimple```, creates a new session for each conversation and releases it after completion. Additional features, like session reuse and refreshing are implemented in the ```SessionPool``` class.

Besides the synchronous SOAP calls in which the thread waits for the server’s response, it is also possible to use task-based asynchronous methods. Example of this approach is in the ```CreateSessionAsync``` method of SoapAuth class:

```cs
var source = new TaskCompletionSource<SoapResult<SessionCreateRS>>();
service.SessionCreateRQCompleted += (s, e) =>
{
    if (SoapHelper.HandleErrors(e, source))
    {
   	 source.TrySetResult(SoapResult<SessionCreateRS>.Success(
           e.Result, service.SecurityValue));
    }
};
service.SessionCreateRQAsync(request);
return await source.Task;
```

## GET requests
Example of a REST workflow is in the ```RestController.LeadPriceCalendar``` action. ```RestClient``` (an object which simplifies performing the HTTP API requests) is created and additional data are passed from application form to the ```LeadPriceCalendarActivity``` through the ```LeadPriceCalendarPostRQ``` model.

```cs
RestClient restClient = RestClientFactory.Create();
IActivity activity = new LeadPriceCalendarActivity(
    requestModel, restClient, true);
```

Inside the ```RunAsync()``` method of ```LeadPriceCalendarActivity```, arguments of the query string are first placed in a dictionary:

```cs
IDictionary<string, string> queryDictionary = new Dictionary<string, string>
{
{ "origin", data.Origin },
      { "destination", data.Destination },
      { "lengthofstay", data.LengthOfStay.ToString() },
      { "pointofsalecountry", data.PointOfSaleCountry }
};
```

Then, the request is performed with ```RestClient``` and JSON from response body is automatically converted to the ```LeadPriceCalendarRS``` model:

```cs
var httpResponse = await this.restClient.GetAsync<LeadPriceCalendarRS>(
    Path, queryDictionary);
```

The response is placed in a generic ```HttpResponse<TResponse>``` object. It can be used to obtain the status code, error message and returned object:

```cs
if (httpResponse.IsSuccess)
{
    sharedContext.AddResult(key, httpResponse.Value);
}
else
{
    sharedContext.AddResult(key, httpResponse.Message);
    sharedContext.IsFaulty = true;
}
```

## POST requests
When a POST call is needed, one can use the ```PostAsync``` method from ```HttpClient```. First you need to create the request model (class that will be serialized to JSON request body). Then call the ```PostAsync``` method which requires two generic parameters: type of the request model and type of the response model (the type to which the response will be deserialized).

The following sample is from the BargainFinderMaxActivity class:

```cs
BargainFinderMaxRQ request = this.CreateRequest(data);
var httpResponse = await this.restClient
    .PostAsync<BargainFinderMaxRQ, BargainFinderMaxRS>(Endpoint, request);
sharedContext.AddRestResult<BargainFinderMaxRS>(
    SharedContextKey, httpResponse);
```

You can find examples of request and response models in the ```SACS.Library\Rest\Models``` folder.

REST authentication is performed in RestAuthorizationManager class. At first, the token holder is initialised as empty:

```cs
private TokenHolder tokenHolder = TokenHolder.Empty();
```

Then, during each request, if refreshing is needed (either token is empty or it has expired or refresh was required by another service), new client ID is created from configuration paramaters (according to the rules set in [https://developer.sabre.com/docs/read/rest_basics/authentication](https://developer.sabre.com/docs/read/rest_basics/authentication)). Then the ID is passed to API. New token and its expiration date are obtained from the response.

```cs
string clientId = CreateCredentialsString(
    userId, group, secret, domain, formatVersion);
var response = await this.AuthorizeAsync(clientId);
if (response.IsSuccess)
{
    var value = response.Value;
    tokenHolder = TokenHolder.Valid(value.AccessToken, value.ExpiresIn);
}
```

# Extending the sample code/library
## Creating workflows and activities
The ```Workflow``` class is used to handle a chain of activities running one after another.

To create your own workflow, implement the ```IActivity``` response as in above examples. The ```RunAsync()``` method should return the next activity or null if it’s last in the chain. Then the first activity should be passed to Workflow as a constructor argument:

```cs
Workflow workflow = new Workflow(activity);
```

When an error has occurred and you want to break the workflow, you should just set the ```IsFaulty``` flag of the shared context to true, which causes the workflow to stop after the activity has finished.

```cs
sharedContext.IsFaulty = true;
```

The ```SharedContext``` object also has an ```AddRestResult``` method, which can be used to handle errors from ```HttpResponse```:

```cs
sharedContext.AddRestResult(SharedContextKey, httpResponse);
```

## Generating the SOAP classes

SOAP calls are using additional classes (clients and request models) that can be generated from .wsdl (Web Service Definition Language) files.

To generate models on Windows, one can use wsdl.exe. To run it easily, find the program’s executable and add it to the PATH environment variable. For example, on Windows 10 it can be found in the following path:
```
C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6 Tools\wsdl.exe
```
The wsdl.exe tool uses a configuration file which allows one to skip some of the parameters. In this case, the file is named parameters.xml and located in the main folder of the application. To add WSDL file for a new SOAP service, you need to edit the ```<documents>``` section and add the required URL. For example:

```xml
<documents>
    <document>file:///C:/wsdls/SessionCreateRQ/SessionCreateRQ.wsdl</document>document>
</documents>documents>
```

To test the default settings, one can just copy the wsdls folder to ```C:\wsdls```.

Then one can run a command:
```
wsdl /par:parameters.xml
```
from the main folder of the application. The generated code will be put in the ```SACS.Library\SabreSoapApi\SabreSoapApi.cs``` file.

More information about this tool can be found at MSDN: [https://msdn.microsoft.com/en-us/library/7h3ystb6.aspx](https://msdn.microsoft.com/en-us/library/7h3ystb6.aspx).

There is a bug which can sometimes result in properties not being properly generated. The bug is described with more detail at: [http://stackoverflow.com/questions/10109689/xsd-exe-m    issing-nested-properties](http://stackoverflow.com/questions/10109689/xsd-exe-missing-nested-properties).

A simple solution is to manually add missing properties through partial classes. A working example with necessary description is presented in ```SACS.Library\SabreSoapApi\UniqueID_Type.cs.```

## Adding REST models
Adding your own REST calls requires creating additional request and response models. They will be automatically parsed and serialized by the RestClient class. For example models, look in the ```SACS.Library\Rest\Models``` folder.

To specify target JSON property name, use the DataMember attributes.

```cs
[DataMember(Name = "TPA_Extensions")]
public ItineraryTPAExtensions TPAExtensions { get; set; }
```

The project also contains two additional JSON converters (in the Serialization folder), which you can use to handle special cases.

```cs
[JsonConverter(typeof(ObjectOrStringConverter<LowestFare>))]
[DataMember(Name = "LowestFare")]
public LowestFare LowestFare { get; set; }
```

Since ```LowestFare``` field can contain “n/a” string instead of a proper object, ```ObjectOrStringConverter``` allows you to specify what to do in such case (LowestFare needs to implement the ```IObjectOrString``` interface).

[JsonConverter(typeof(ArrayOrObjectConverter<AirItineraryPricingInfo>))]
[DataMember(Name = "AirItineraryPricingInfo")]
public IList<AirItineraryPricingInfo> AirItineraryPricingInfo { get; set; }

In this case, the ```AirItineraryPricingInfo``` can be either a single object, or an array of objects. ```AirItineraryPricingInfo``` attribute will deserialize the object to a singleton list and handle arrays normally.

## Additional information – TLS 1.2
Since both REST and SOAP services require TLS 1.2 protocol for security, the following lines are included in Startup.cs file of the example application:

```cs
ServicePointManager.Expect100Continue = true;
ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
```

This is a global setting for the whole application and may cause problems if there’s need to communicate with third-party services that don’t support TLS 1.2.

More information about the service point settings can be found in MSDN: [https://msdn.microsoft.com/en-us/library/system.net.servicepointmanager.aspx](https://msdn.microsoft.com/en-us/library/system.net.servicepointmanager.aspx)

### Support

- [Stack Overflow](http://stackoverflow.com/questions/tagged/sabre)
- Need to report an issue/improvement? Use the built-in [issues](https://github.com/SabreDevStudio/SACS-DotNet/issues) section.
- [Sabre Dev Studio](https://developer.sabre.com/)

### Disclaimer of Warranty and Limitation of Liability
This software and any compiled programs created using this software are furnished “as is” without warranty of any kind, including but not limited to the implied warranties of merchantability and fitness for a particular purpose. No oral or written information or advice given by Sabre, its agents or employees shall create a warranty or in any way increase the scope of this warranty, and you may not rely on any such information or advice. Sabre does not warrant, guarantee, or make any representations regarding the use, or the results of the use, of this software, compiled programs created using this software, or written materials in terms of correctness, accuracy, reliability, currentness, or otherwise. The entire risk as to the results and performance of this software and any compiled applications created using this software is assumed by you. Neither Sabre nor anyone else who has been involved in the creation, production or delivery of this software shall be liable for any direct, indirect, consequential, or incidental damages (including damages for loss of business profits, business interruption, loss of business information, and the like) arising out of the use of or inability to use such product even if Sabre has been advised of the possibility of such damages.
