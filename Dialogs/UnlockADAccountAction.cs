using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Microsoft.Bot.Sample.LuisBot;
using Microsoft.Cognitive.LUIS.ActionBinding;
using Newtonsoft.Json;
using RestSharp;

namespace LuisBot.Dialogs
{
    [Serializable]
    [LuisActionBinding("Unlock AD Account", FriendlyName = "Unlock AD Account Service Request")]
    public class UnlockADAccountAction : BaseLuisAction
    {
        [Required(ErrorMessage = "Please give me your sam account name")]
        [LuisActionBindingParam(CustomType = "samaccountname", Order = 1)]
        public string samaccountname { get; set; }


        public override Task<object> FulfillAsync()
        {
            var client = new RestClient("http://96a7bf35.ngrok.io/aeengine/rest/authenticate");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "ea502694-bf8a-9c2e-e27b-8082381ce137");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
            request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"username\"\r\n\r\naishwarya\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"password\"\r\n\r\nPune@123\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string jsonresult;
            jsonresult = response.Content;
            var myDetails = JsonConvert.DeserializeObject<MyDetail>(jsonresult);
            string token = myDetails.sessionToken;
            var request1 = new RestRequest("http://96a7bf35.ngrok.io/aeengine/rest/execute", Method.POST);
            request1.AddHeader("X-session-token", token);

            JavaScriptSerializer serialiser = new JavaScriptSerializer();
            List<AutomationParameter> ListAutomationField = new List<AutomationParameter>();

            List<JsonParam> InnerJsonParam = new List<JsonParam>();

            JsonParam[] jparameter = new JsonParam[2];



            JsonParam jparameter1 = new JsonParam();
            jparameter1.question = "samaccountname";
            jparameter1.answer = this.samaccountname;
            InnerJsonParam.Add(jparameter1);

            InnerJson innerjsonobject = new InnerJson();
            innerjsonobject.ServiceRequest = "Creating Snapshot";
            innerjsonobject.@params = InnerJsonParam;

            string json1 = serialiser.Serialize(innerjsonobject);

            AutomationParameter parameter1 = new AutomationParameter();
            parameter1.name = "jsonInput";
            parameter1.value = json1;
            parameter1.type = "String";
            parameter1.order = 1;
            parameter1.secret = false;
            parameter1.optional = false;
            parameter1.displayName = "jsonInput";
            parameter1.extension = null;
            parameter1.poolCredential = false;

            ListAutomationField.Add(parameter1);

            AutomationParameter parameter2 = new AutomationParameter();
            parameter2.name = "clientEmail";
            parameter2.value = "satyendar.daragani@3i-infotech.com";
            parameter2.type = "String";
            parameter2.order = 2;
            parameter2.secret = false;
            parameter2.optional = false;
            parameter2.displayName = "snapshotname";
            parameter2.extension = null;
            parameter2.poolCredential = false;

            ListAutomationField.Add(parameter2);

            Guid temp = Guid.NewGuid();

            RootAutomation AutoRoot = new RootAutomation();
            AutoRoot.orgCode = "ACTIVEDIREC";
            AutoRoot.workflowName = "CreateServiceRequestInRemedyForce";
            AutoRoot.userId = "Aishwarya Chaudhary";
            AutoRoot.@params = ListAutomationField;
            AutoRoot.sourceId = temp.ToString();
            AutoRoot.source = "AutomationEdge HelpDesk";
            AutoRoot.responseMailSubject = null;
            string json = serialiser.Serialize(AutoRoot);
            //await context.PostAsync($"{json}");



            request1.AddHeader("content-type", "application/json");
            request1.AddParameter("application/json", json, ParameterType.RequestBody);
            request1.RequestFormat = DataFormat.Json;
            IRestResponse response1 = client.Execute(request1);

            return Task.FromResult((object)$"I will unlock account for  {this.samaccountname} as soon as possible... Visit me again whenever you need my help. Have a great day :)");
        }
    }
}