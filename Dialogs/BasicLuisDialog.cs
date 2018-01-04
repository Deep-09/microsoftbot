using System;
using System.Configuration;
using System.Threading.Tasks;
using RestSharp;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Newtonsoft.Json;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Gretting" with the name of your newly created intent in the following handler
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
    
        [LuisIntent("Add Account")]
        public async Task AddAccountIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        
        [LuisIntent("User Account Locked")]
        public async Task UserAccountLockedIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        
        [LuisIntent("Add Virtual Machines")]
        public async Task AddVirtualMachinesIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        
        [LuisIntent("Add Snapshot")]
        public async Task AddSnapshotIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        
        [LuisIntent("Software Installation")]
        public async Task SoftwareInstallationIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        
        [LuisIntent("Incident Status")]
        public async Task IncidentStatusIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        
        private async Task ShowLuisResult(IDialogContext context, LuisResult result)
        {
            string token = Authenticate();
            await context.PostAsync($"You reached {result.Intents[0].Intent} with entity {result.Entities[0].Entity} . Session Token: {token} ");
            context.Wait(MessageReceived);
        }
        public string Authenticate()
        {
            var client = new RestClient("https://c3663c91.ngrok.io/aeengine/rest/authenticate");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "ea502694-bf8a-9c2e-e27b-8082381ce137");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
            request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"username\"\r\n\r\nFusionAdmin\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"password\"\r\n\r\nFusion@123\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string jsonresult;
            jsonresult = response.Content;
            var myDetails = JsonConvert.DeserializeObject<MyDetail>(jsonresult);
            string token = myDetails.sessionToken;
            var request1 = new RestRequest("http://c3663c91.ngrok.io/aeengine/rest/execute", Method.POST);
            request1.AddHeader("X-session-token", token);
            string body = "{\"orgCode\":\"FUSION\",\"workflowName\":\"Software Installation\",\"userId\":\"Admin Fusion\",\"sourceId\":\"SID_5b-777-21f4-88-880eb-8a0b-90\",\"source\":\"AutomationEdge HelpDesk\",\"responseMailSubject\":\"null\",\"params\":[{\"name\":\"software\",\"value\":\"JDK\",\"type\":\"String\",\"order\":1,\"secret\":false,\"optional\":false,\"defaultValue\":null,\"displayName\":\"Incident Number\",\"extension\":null,\"poolCredential\":false},{\"name\":\"slackChannel\",\"value\":\"fdgvdfg\",\"type\":\"String\",\"order\":2,\"secret\":false,\"optional\":false,\"defaultValue\":null,\"displayName\":\"Slack Channel\",\"extension\":null,\"poolCredential\":false}]}";
            //var json = JsonConvert.SerializeObject(body);
            request1.AddHeader("content-type", "application/json");
            request1.AddParameter("application/json", body, ParameterType.RequestBody);
            request1.RequestFormat = DataFormat.Json;
            IRestResponse response1 = client.Execute(request1);
            return response1.Content;

        }
    }
    public class MyDetail
    {
        public string sessionToken
        {
            get;
            set;
        }
    }
}
