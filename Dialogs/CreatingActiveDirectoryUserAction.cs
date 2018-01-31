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
    [LuisActionBinding("Creating Active Directory User", FriendlyName = "Creating Active Directory User Service Request")]
    public class CreatingActiveDirectoryUserAction : BaseLuisAction
    {
        [Required(ErrorMessage = "Give me your organization unit please")]
        [LuisActionBindingParam(CustomType = "OrganizationUnit_Name", Order = 1)]
        public string OrganizationUnit_Name { get; set; }

        [Required(ErrorMessage = "May I know sam name for your account")]
        [LuisActionBindingParam(CustomType = "samaccountname", Order = 2)]
        public string samaccountname { get; set; }

        [Required(ErrorMessage = "Enter username of your choice")]
        [LuisActionBindingParam(CustomType = "User_Name", Order = 4)]
        public string User_Name { get; set; }

        [Required(ErrorMessage = "What name you would like on display?")]
        [LuisActionBindingParam(CustomType = "Display_Name", Order = 3)]
        public string Display_Name { get; set; }

        [Required(ErrorMessage = "And what password would you like to set?")]
        [LuisActionBindingParam(CustomType = "Password", Order = 5)]
        public string Password { get; set; }

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
            jparameter1.question = "OrganizationUnit_Name";
            jparameter1.answer = this.OrganizationUnit_Name;
            InnerJsonParam.Add(jparameter1);

            JsonParam jparameter2 = new JsonParam();
            jparameter2.question = "samaccountname";
            jparameter2.answer = this.samaccountname;
            InnerJsonParam.Add(jparameter2);

            JsonParam jparameter3 = new JsonParam();
            jparameter3.question = "User_Name";
            jparameter3.answer = this.User_Name;
            InnerJsonParam.Add(jparameter3);

            JsonParam jparameter4 = new JsonParam();
            jparameter4.question = "Display_Name";
            jparameter4.answer = this.Display_Name;
            InnerJsonParam.Add(jparameter4);

            JsonParam jparameter5 = new JsonParam();
            jparameter5.question = "Password";
            jparameter5.answer = this.Password;
            InnerJsonParam.Add(jparameter1);




            InnerJson innerjsonobject = new InnerJson();
            innerjsonobject.ServiceRequest = "Creating Active Directory User";
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

            return Task.FromResult((object)$"I will create AD account for {this.samaccountname} soon... Visit me again whenever you need my help... Have a great day :)");
        }
    }
}