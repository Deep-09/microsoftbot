using System;
using System.Configuration;
using System.Threading.Tasks;
using RestSharp;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using LuisBot.Dialogs;
using System.Collections.Generic;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;


namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        
        string oname = "";
        string sname ;
        string uname = "";
        string dname = "";
        string pass = "";
        string vcenter_IP = "";
        string Port = "";
        string Admin_Username = "";
        string Admin_Password = "";
        string VMHost_IP = "";
        string Datastore = "";
        string Template_Name = "";
        string VM_Name = "";
        private const string EntitySamAccountName = "samaccountname";


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

        
        [LuisIntent("Unlock AD")]
        public async Task Search(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            //var message = await activity;
            //await context.PostAsync($"Welcome to the Hotels finder! We are analyzing your message: '{message.Text}'...");

            var hotelsQuery = new HotelsQuery();

            EntityRecommendation samaccountnameEntityRecommendation;

            if (result.TryFindEntity(EntitySamAccountName, out samaccountnameEntityRecommendation))
            {
                samaccountnameEntityRecommendation.Type = "samaccountname";
            }

            var hotelsFormDialog = new FormDialog<HotelsQuery>(hotelsQuery, this.BuildHotelsForm, FormOptions.PromptInStart, result.Entities);

            context.Call(hotelsFormDialog, this.ResumeAfterHotelsFormDialog);
        }

        private IForm<HotelsQuery> BuildHotelsForm()
        {
            OnCompletionAsyncDelegate<HotelsQuery> processHotelsSearch = async (context, state) =>
            {
                
                var message = "";
                if (!string.IsNullOrEmpty(state.SamAccountName))
                {
                    var client = new RestClient("http://53d64268.ngrok.io/aeengine/rest/authenticate");
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
                    var request1 = new RestRequest("http://53d64268.ngrok.io/aeengine/rest/execute", Method.POST);
                    request1.AddHeader("X-session-token", token);

                    JavaScriptSerializer serialiser = new JavaScriptSerializer();
                    List<AutomationParameter> ListAutomationField = new List<AutomationParameter>();

                    AutomationParameter parameter1 = new AutomationParameter();
                    parameter1.name = "Sam_Account_Name";
                    parameter1.value = state.SamAccountName;
                    parameter1.type = "String";
                    parameter1.order = 1;
                    parameter1.secret = false;
                    parameter1.optional = false;
                    parameter1.displayName = "Sam_Account_Name";
                    parameter1.extension = null;
                    parameter1.poolCredential = false;

                    ListAutomationField.Add(parameter1);

                    Guid temp = Guid.NewGuid();

                    RootAutomation AutoRoot = new RootAutomation();
                    AutoRoot.orgCode = "ACTIVEDIREC";
                    AutoRoot.workflowName = "UnlockAD";
                    AutoRoot.userId = "Aishwarya Chaudhary";
                    AutoRoot.@params = ListAutomationField;
                    AutoRoot.sourceId = temp.ToString();
                    AutoRoot.source = "AutomationEdge HelpDesk";
                    AutoRoot.responseMailSubject = null;
                    string json = serialiser.Serialize(AutoRoot);



                    //string body = "{\"orgCode\":\"FUSION\",\"workflowName\":\"Software Installation\",\"userId\":\"Admin Fusion\",\"sourceId\":\"SID_5b-912-21f4-88-880eb-8a0b-91\",\"source\":\"AutomationEdge HelpDesk\",\"responseMailSubject\":\"null\",\"params\":[{\"name\":\"software\",\"value\":\"JDK\",\"type\":\"String\",\"order\":1,\"secret\":false,\"optional\":false,\"defaultValue\":null,\"displayName\":\"Incident Number\",\"extension\":null,\"poolCredential\":false},{\"name\":\"slackChannel\",\"value\":\"fdgvdfg\",\"type\":\"String\",\"order\":2,\"secret\":false,\"optional\":false,\"defaultValue\":null,\"displayName\":\"Slack Channel\",\"extension\":null,\"poolCredential\":false}]}";
                    //var json = JsonConvert.SerializeObject(body);
                    request1.AddHeader("content-type", "application/json");
                    request1.AddParameter("application/json", json, ParameterType.RequestBody);
                    request1.RequestFormat = DataFormat.Json;
                    IRestResponse response1 = client.Execute(request1);

                    //await context.PostAsync($"You reached {result.Intents[0].Intent} resonse is {response1.Content} .");
                    message += $"II will unlock account for  {state.SamAccountName} as soon as possible...";

                }
                
            };

            return new FormBuilder<HotelsQuery>()
               .Field(nameof(HotelsQuery.SamAccountName), (state) => string.IsNullOrEmpty(state.AirportCode))
               .Field(nameof(HotelsQuery.AirportCode), (state) => string.IsNullOrEmpty(state.SamAccountName))
               .OnCompletion(processHotelsSearch)
               .Build();
        }

        private async Task ResumeAfterHotelsFormDialog(IDialogContext context, IAwaitable<HotelsQuery> result)
        {
            try
            {
                var san = await result;
                
                await context.PostAsync("Visit me again whenever you need me .Have a great day :)");

           
            }
            catch (FormCanceledException ex)
            {
                string reply;

                if (ex.InnerException == null)
                {
                    reply = "You have canceled the operation.";
                }
                else
                {
                    reply = $"Oops! Something went wrong :( Technical Details: {ex.InnerException.Message}";
                }

                await context.PostAsync(reply);
            }
            finally
            {
                context.Done<object>(null);
            }
        }
       


        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Hello! I am your IT Service Desk Virtual Assistant *Maggie* :) . I can help you with IT service related issues and requests. How may I help you today?");
        }


        //[LuisIntent("Unlock AD")]
        public async Task UnlockADIntent(IDialogContext context, LuisResult result)
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

            AutomationParameter parameter1 = new AutomationParameter();
            parameter1.name = "Sam_Account_Name";
            parameter1.value = result.Entities[0].Entity;
            parameter1.type = "String";
            parameter1.order = 1;
            parameter1.secret = false;
            parameter1.optional = false;
            parameter1.displayName = "Sam_Account_Name";
            parameter1.extension = null;
            parameter1.poolCredential = false;

            ListAutomationField.Add(parameter1);

            Guid temp = Guid.NewGuid();

            RootAutomation AutoRoot = new RootAutomation();
            AutoRoot.orgCode = "ACTIVEDIREC";
            AutoRoot.workflowName = "UnlockAD";
            AutoRoot.userId = "Aishwarya Chaudhary";
            AutoRoot.@params = ListAutomationField;
            AutoRoot.sourceId = temp.ToString();
            AutoRoot.source = "AutomationEdge HelpDesk";
            AutoRoot.responseMailSubject = null;
            string json = serialiser.Serialize(AutoRoot);



            //string body = "{\"orgCode\":\"FUSION\",\"workflowName\":\"Software Installation\",\"userId\":\"Admin Fusion\",\"sourceId\":\"SID_5b-912-21f4-88-880eb-8a0b-91\",\"source\":\"AutomationEdge HelpDesk\",\"responseMailSubject\":\"null\",\"params\":[{\"name\":\"software\",\"value\":\"JDK\",\"type\":\"String\",\"order\":1,\"secret\":false,\"optional\":false,\"defaultValue\":null,\"displayName\":\"Incident Number\",\"extension\":null,\"poolCredential\":false},{\"name\":\"slackChannel\",\"value\":\"fdgvdfg\",\"type\":\"String\",\"order\":2,\"secret\":false,\"optional\":false,\"defaultValue\":null,\"displayName\":\"Slack Channel\",\"extension\":null,\"poolCredential\":false}]}";
            //var json = JsonConvert.SerializeObject(body);
            request1.AddHeader("content-type", "application/json");
            request1.AddParameter("application/json", json, ParameterType.RequestBody);
            request1.RequestFormat = DataFormat.Json;
            IRestResponse response1 = client.Execute(request1);

            await context.PostAsync($"I will unlock account for {result.Entities[0].Entity} as soon as possible... Visit me again whenever you need my help. Have a great day:)");

        }
       
        [LuisIntent("Add Snapshot")]
        public async Task AddSnapshotIntent_Test(IDialogContext context, LuisResult result)
        {
            PromptDialog.Text(context, ResumeAfterVMNameClarification, "Please give me your VM name ");
        }
        private async Task ResumeAfterVMNameClarification(IDialogContext context, IAwaitable<string> result)
        {
            VM_Name = await result;
            PromptDialog.Text(context, ResumeAfterSnapNameUnlockClarification, "With what name you want me to save this snap?");
            //await context.PostAsync($"You entered {sname}.");
        }
        private async Task ResumeAfterSnapNameUnlockClarification(IDialogContext context, IAwaitable<string> result)
        {
            sname = await result;
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

            AutomationParameter parameter1 = new AutomationParameter();
            parameter1.name = "VM_Name";
            parameter1.value = VM_Name;
            parameter1.type = "String";
            parameter1.order = 1;
            parameter1.secret = false;
            parameter1.optional = false;
            parameter1.displayName = "VM_Name";
            parameter1.extension = null;
            parameter1.poolCredential = false;

            ListAutomationField.Add(parameter1);

            AutomationParameter parameter2 = new AutomationParameter();
            parameter2.name = "snapshotname";
            parameter2.value = sname;
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
            AutoRoot.workflowName = "createSnap";
            AutoRoot.userId = "Aishwarya Chaudhary";
            AutoRoot.@params = ListAutomationField;
            AutoRoot.sourceId = temp.ToString();
            AutoRoot.source = "AutomationEdge HelpDesk";
            AutoRoot.responseMailSubject = null;
            string json = serialiser.Serialize(AutoRoot);



            //string body = "{\"orgCode\":\"FUSION\",\"workflowName\":\"Software Installation\",\"userId\":\"Admin Fusion\",\"sourceId\":\"SID_5b-912-21f4-88-880eb-8a0b-91\",\"source\":\"AutomationEdge HelpDesk\",\"responseMailSubject\":\"null\",\"params\":[{\"name\":\"software\",\"value\":\"JDK\",\"type\":\"String\",\"order\":1,\"secret\":false,\"optional\":false,\"defaultValue\":null,\"displayName\":\"Incident Number\",\"extension\":null,\"poolCredential\":false},{\"name\":\"slackChannel\",\"value\":\"fdgvdfg\",\"type\":\"String\",\"order\":2,\"secret\":false,\"optional\":false,\"defaultValue\":null,\"displayName\":\"Slack Channel\",\"extension\":null,\"poolCredential\":false}]}";
            //var json = JsonConvert.SerializeObject(body);
            request1.AddHeader("content-type", "application/json");
            request1.AddParameter("application/json", json, ParameterType.RequestBody);
            request1.RequestFormat = DataFormat.Json;
            IRestResponse response1 = client.Execute(request1);

            await context.PostAsync($"I will take a snapshot named {sname} as soon as possible... Visit me again whenever you need my help. Have a great day :)");
        }



        //[LuisIntent("Unlock AD Ask")]
        public async Task UnlockADIntent_Test(IDialogContext context, LuisResult result)
        {
            PromptDialog.Text(context, ResumeAfterSamNameUnlockClarification, "Pardon me I didn't get your sam account name there :)... I'm hoping you can help me with it..");
        }
        
        private async Task ResumeAfterSamNameUnlockClarification(IDialogContext context, IAwaitable<string> result)
        {
            sname = await result;
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

            AutomationParameter parameter1 = new AutomationParameter();
            parameter1.name = "Sam_Account_Name";
            parameter1.value = sname;
            parameter1.type = "String";
            parameter1.order = 1;
            parameter1.secret = false;
            parameter1.optional = false;
            parameter1.displayName = "Sam_Account_Name";
            parameter1.extension = null;
            parameter1.poolCredential = false;

            ListAutomationField.Add(parameter1);

            Guid temp = Guid.NewGuid();

            RootAutomation AutoRoot = new RootAutomation();
            AutoRoot.orgCode = "ACTIVEDIREC";
            AutoRoot.workflowName = "UnlockAD";
            AutoRoot.userId = "Aishwarya Chaudhary";
            AutoRoot.@params = ListAutomationField;
            AutoRoot.sourceId = temp.ToString();
            AutoRoot.source = "AutomationEdge HelpDesk";
            AutoRoot.responseMailSubject = null;
            string json = serialiser.Serialize(AutoRoot);



            //string body = "{\"orgCode\":\"FUSION\",\"workflowName\":\"Software Installation\",\"userId\":\"Admin Fusion\",\"sourceId\":\"SID_5b-912-21f4-88-880eb-8a0b-91\",\"source\":\"AutomationEdge HelpDesk\",\"responseMailSubject\":\"null\",\"params\":[{\"name\":\"software\",\"value\":\"JDK\",\"type\":\"String\",\"order\":1,\"secret\":false,\"optional\":false,\"defaultValue\":null,\"displayName\":\"Incident Number\",\"extension\":null,\"poolCredential\":false},{\"name\":\"slackChannel\",\"value\":\"fdgvdfg\",\"type\":\"String\",\"order\":2,\"secret\":false,\"optional\":false,\"defaultValue\":null,\"displayName\":\"Slack Channel\",\"extension\":null,\"poolCredential\":false}]}";
            //var json = JsonConvert.SerializeObject(body);
            request1.AddHeader("content-type", "application/json");
            request1.AddParameter("application/json", json, ParameterType.RequestBody);
            request1.RequestFormat = DataFormat.Json;
            IRestResponse response1 = client.Execute(request1);

            await context.PostAsync($"I will unlock account for {sname} as soon as possible... Visit me again whenever you need my help. Have a great day :)");
        }



        [LuisIntent("Add Account")]
        public async Task AddAccount(IDialogContext context, LuisResult result)
        {
            PromptDialog.Text(context, ResumeAfterOrgNameClarification, "Give me your organization unit please ");
        }

        private async Task ResumeAfterOrgNameClarification(IDialogContext context, IAwaitable<string> result)
        {
            oname = await result;
            PromptDialog.Text(context, ResumeAfterSamNameClarification, "May I know sam name for your account");
            //await context.PostAsync($"I see you want to order {food}.");
        }

        private async Task ResumeAfterSamNameClarification(IDialogContext context, IAwaitable<string> result)
        {
            sname = await result;
            PromptDialog.Text(context, ResumeAfterDispNameClarification, "What name you would like on display?");
            //await context.PostAsync($"You entered {sname}.");
        }

        private async Task ResumeAfterDispNameClarification(IDialogContext context, IAwaitable<string> result)
        {
            dname = await result;
            PromptDialog.Text(context, ResumeAfterUserNameClarification, "Enter username of your choice");
            //await context.PostAsync($"You entered {sname}.");
        }

        private async Task ResumeAfterUserNameClarification(IDialogContext context, IAwaitable<string> result)
        {
            uname = await result;
            PromptDialog.Text(context, ResumeAfterPasswordClarification, "And what password would you like to set?");
            //await context.PostAsync($"You entered {sname}.");
        }

        private async Task ResumeAfterPasswordClarification(IDialogContext context, IAwaitable<string> result)
        {
            pass = await result;

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

            AutomationParameter parameter1 = new AutomationParameter();
            parameter1.name = "OrganizationUnit_Name";
            parameter1.value = oname;
            parameter1.type = "String";
            parameter1.order = 1;
            parameter1.secret = false;
            parameter1.optional = false;
            parameter1.displayName = "OrganizationUnit_Name";
            parameter1.extension = null;
            parameter1.poolCredential = false;

            ListAutomationField.Add(parameter1);

            AutomationParameter parameter2 = new AutomationParameter();
            parameter2.name = "SamAccount_Name";
            parameter2.value = sname;
            parameter2.type = "String";
            parameter2.order = 2;
            parameter2.secret = false;
            parameter2.optional = false;
            parameter2.displayName = "SamAccount_Name";
            parameter2.extension = null;
            parameter2.poolCredential = false;

            ListAutomationField.Add(parameter2);

            AutomationParameter parameter3 = new AutomationParameter();
            parameter3.name = "User_Name";
            parameter3.value = uname;
            parameter3.type = "String";
            parameter3.order = 3;
            parameter3.secret = false;
            parameter3.optional = false;
            parameter3.displayName = "User_Name";
            parameter3.extension = null;
            parameter3.poolCredential = false;

            ListAutomationField.Add(parameter3);

            AutomationParameter parameter4 = new AutomationParameter();
            parameter4.name = "Display_Name";
            parameter4.value = dname;
            parameter4.type = "String";
            parameter4.order = 4;
            parameter4.secret = false;
            parameter4.optional = false;
            parameter4.displayName = "Display_Name";
            parameter4.extension = null;
            parameter4.poolCredential = false;

            ListAutomationField.Add(parameter4);

            AutomationParameter parameter5 = new AutomationParameter();
            parameter5.name = "Password";
            parameter5.value = pass;
            parameter5.type = "String";
            parameter5.order = 5;
            parameter5.secret = false;
            parameter5.optional = false;
            parameter5.displayName = "Password";
            parameter5.extension = null;
            parameter5.poolCredential = false;

            ListAutomationField.Add(parameter5);

            Guid temp = Guid.NewGuid();

            RootAutomation AutoRoot = new RootAutomation();
            AutoRoot.orgCode = "ACTIVEDIREC";
            AutoRoot.workflowName = "AD";
            AutoRoot.userId = "Aishwarya Chaudhary";
            AutoRoot.@params = ListAutomationField;
            AutoRoot.sourceId = temp.ToString();
            AutoRoot.source = "AutomationEdge HelpDesk";
            AutoRoot.responseMailSubject = null;
            string json = serialiser.Serialize(AutoRoot);



            //string body = "{\"orgCode\":\"FUSION\",\"workflowName\":\"Software Installation\",\"userId\":\"Admin Fusion\",\"sourceId\":\"SID_5b-912-21f4-88-880eb-8a0b-91\",\"source\":\"AutomationEdge HelpDesk\",\"responseMailSubject\":\"null\",\"params\":[{\"name\":\"software\",\"value\":\"JDK\",\"type\":\"String\",\"order\":1,\"secret\":false,\"optional\":false,\"defaultValue\":null,\"displayName\":\"Incident Number\",\"extension\":null,\"poolCredential\":false},{\"name\":\"slackChannel\",\"value\":\"fdgvdfg\",\"type\":\"String\",\"order\":2,\"secret\":false,\"optional\":false,\"defaultValue\":null,\"displayName\":\"Slack Channel\",\"extension\":null,\"poolCredential\":false}]}";
            //var json = JsonConvert.SerializeObject(body);
            request1.AddHeader("content-type", "application/json");
            request1.AddParameter("application/json", json, ParameterType.RequestBody);
            request1.RequestFormat = DataFormat.Json;
            IRestResponse response1 = client.Execute(request1);



            await context.PostAsync($"I will create AD account for {sname} soon... Visit me again whenever you need my help... Have a great day :)");
        }

        
        [LuisIntent("Add Virtual Machine")]
        public async Task AddVirtualMachinesIntent(IDialogContext context, LuisResult result)
        {
            PromptDialog.Text(context, ResumeAftervcenterIPClarification, "Give me your vCenter IP please ");
        }
        private async Task ResumeAftervcenterIPClarification(IDialogContext context, IAwaitable<string> result)
        {
            vcenter_IP = await result;
            PromptDialog.Text(context, ResumeAfterPortClarification, "May I know port number for the same ");
            //await context.PostAsync($"I see you want to order {food}.");
        }
        private async Task ResumeAfterPortClarification(IDialogContext context, IAwaitable<string> result)
        {
            Port = await result;
            PromptDialog.Text(context, ResumeAfterAdminUsernameClarification, "Please enter your admin username below ");
            //await context.PostAsync($"I see you want to order {food}.");
        }
        private async Task ResumeAfterAdminUsernameClarification(IDialogContext context, IAwaitable<string> result)
        {
            Admin_Username = await result;
            PromptDialog.Text(context, ResumeAfterAdminPasswordClarification, "Can I have password for the same ");
            //await context.PostAsync($"I see you want to order {food}.");
        }
        private async Task ResumeAfterAdminPasswordClarification(IDialogContext context, IAwaitable<string> result)
        {
            Admin_Password = await result;
            PromptDialog.Text(context, ResumeAfterVMHostIPClarification, "Give me VM Host IP please ");
            //await context.PostAsync($"I see you want to order {food}.");
        }
        private async Task ResumeAfterVMHostIPClarification(IDialogContext context, IAwaitable<string> result)
        {
            VMHost_IP = await result;
            PromptDialog.Text(context, ResumeAfterDatastoreClarification, "Datastore name please");
            //await context.PostAsync($"I see you want to order {food}.");
        }
        private async Task ResumeAfterDatastoreClarification(IDialogContext context, IAwaitable<string> result)
        {
            Datastore = await result;
            PromptDialog.Text(context, ResumeAfterTemplateNameClarification, "Give template a name of your choice ");
            //await context.PostAsync($"I see you want to order {food}.");
        }
        private async Task ResumeAfterTemplateNameClarification(IDialogContext context, IAwaitable<string> result)
        {
            Template_Name = await result;
            PromptDialog.Text(context, ResumeAfterVMName1Clarification, "And give this VM a name of your choice ");
            //await context.PostAsync($"I see you want to order {food}.");
        }
        private async Task ResumeAfterVMName1Clarification(IDialogContext context, IAwaitable<string> result)
        {
            VM_Name = await result;

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

            AutomationParameter parameter1 = new AutomationParameter();
            parameter1.name = "vcenter_IP";
            parameter1.value = vcenter_IP;
            parameter1.type = "String";
            parameter1.order = 1;
            parameter1.secret = false;
            parameter1.optional = false;
            parameter1.displayName = "vcenter_IP";
            parameter1.extension = null;
            parameter1.poolCredential = false;

            ListAutomationField.Add(parameter1);

            AutomationParameter parameter2 = new AutomationParameter();
            parameter2.name = "Port";
            parameter2.value = Port;
            parameter2.type = "String";
            parameter2.order = 2;
            parameter2.secret = false;
            parameter2.optional = false;
            parameter2.displayName = "Port";
            parameter2.extension = null;
            parameter2.poolCredential = false;

            ListAutomationField.Add(parameter2);

            AutomationParameter parameter3 = new AutomationParameter();
            parameter3.name = "Admin_Username";
            parameter3.value = Admin_Username;
            parameter3.type = "String";
            parameter3.order = 3;
            parameter3.secret = false;
            parameter3.optional = false;
            parameter3.displayName = "Admin_Username";
            parameter3.extension = null;
            parameter3.poolCredential = false;

            ListAutomationField.Add(parameter3);

            AutomationParameter parameter4 = new AutomationParameter();
            parameter4.name = "Admin_Password";
            parameter4.value = Admin_Password;
            parameter4.type = "String";
            parameter4.order = 4;
            parameter4.secret = false;
            parameter4.optional = false;
            parameter4.displayName = "Admin_Password";
            parameter4.extension = null;
            parameter4.poolCredential = false;

            ListAutomationField.Add(parameter4);

            AutomationParameter parameter5 = new AutomationParameter();
            parameter5.name = "VMHost_IP";
            parameter5.value = VMHost_IP;
            parameter5.type = "String";
            parameter5.order = 5;
            parameter5.secret = false;
            parameter5.optional = false;
            parameter5.displayName = "VMHost_IP";
            parameter5.extension = null;
            parameter5.poolCredential = false;

            ListAutomationField.Add(parameter5);

            AutomationParameter parameter6 = new AutomationParameter();
            parameter6.name = "Datastore";
            parameter6.value = Datastore;
            parameter6.type = "String";
            parameter6.order = 6;
            parameter6.secret = false;
            parameter6.optional = false;
            parameter6.displayName = "Datastore";
            parameter6.extension = null;
            parameter6.poolCredential = false;

            ListAutomationField.Add(parameter6);

            AutomationParameter parameter7 = new AutomationParameter();
            parameter7.name = "Template_Name";
            parameter7.value = Template_Name;
            parameter7.type = "String";
            parameter7.order = 7;
            parameter7.secret = false;
            parameter7.optional = false;
            parameter7.displayName = "Template_Name";
            parameter7.extension = null;
            parameter7.poolCredential = false;

            ListAutomationField.Add(parameter7);

            AutomationParameter parameter8 = new AutomationParameter();
            parameter8.name = "VM_Name";
            parameter8.value = VM_Name;
            parameter8.type = "String";
            parameter8.order = 8;
            parameter8.secret = false;
            parameter8.optional = false;
            parameter8.displayName = "VM_Name";
            parameter8.extension = null;
            parameter8.poolCredential = false;
        
            ListAutomationField.Add(parameter8);

            Guid temp = Guid.NewGuid();

            RootAutomation AutoRoot = new RootAutomation();
            AutoRoot.orgCode = "ACTIVEDIREC";
            AutoRoot.workflowName = "createVM";
            AutoRoot.userId = "Aishwarya Chaudhary";
            AutoRoot.@params = ListAutomationField;
            AutoRoot.sourceId = temp.ToString();
            AutoRoot.source = "AutomationEdge HelpDesk";
            AutoRoot.responseMailSubject = null;
            string json = serialiser.Serialize(AutoRoot);



            //string body = "{\"orgCode\":\"FUSION\",\"workflowName\":\"Software Installation\",\"userId\":\"Admin Fusion\",\"sourceId\":\"SID_5b-912-21f4-88-880eb-8a0b-91\",\"source\":\"AutomationEdge HelpDesk\",\"responseMailSubject\":\"null\",\"params\":[{\"name\":\"software\",\"value\":\"JDK\",\"type\":\"String\",\"order\":1,\"secret\":false,\"optional\":false,\"defaultValue\":null,\"displayName\":\"Incident Number\",\"extension\":null,\"poolCredential\":false},{\"name\":\"slackChannel\",\"value\":\"fdgvdfg\",\"type\":\"String\",\"order\":2,\"secret\":false,\"optional\":false,\"defaultValue\":null,\"displayName\":\"Slack Channel\",\"extension\":null,\"poolCredential\":false}]}";
            //var json = JsonConvert.SerializeObject(body);
            request1.AddHeader("content-type", "application/json");
            request1.AddParameter("application/json", json, ParameterType.RequestBody);
            request1.RequestFormat = DataFormat.Json;
            IRestResponse response1 = client.Execute(request1);


            await context.PostAsync($"I will add VM named {VM_Name} soon... Visit me again whenever you need my help... Have a great day :)");
        }
        
        private async Task ShowLuisResult(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
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
