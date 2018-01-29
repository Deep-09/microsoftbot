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
using System.Linq;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        Dictionary<string, string> MyEntities = new Dictionary<string, string>();
        int count;
        string intentsr;
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


        //[LuisIntent("Unlock AD Account")]
        public async Task Search(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            //var message = await activity;
            //await context.PostAsync($"Welcome to the Hotels finder! We are analyzing your message: '{message.Text}'...");
            count = result.Entities.Count;
            var hotelsQuery = new HotelsQuery();

            EntityRecommendation samaccountnameEntityRecommendation;

            if (result.TryFindEntity(EntitySamAccountName, out samaccountnameEntityRecommendation))
            {
                samaccountnameEntityRecommendation.Type = "SamAccountName";
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
                    message += $"I will unlock account for  {state.SamAccountName} as soon as possible...";
                   
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
                    JsonParam jparameter1 = new JsonParam();
                    jparameter1.question = "Sam_Account_Name";
                    jparameter1.answer = state.SamAccountName;
                    InnerJsonParam.Add(jparameter1);

                    InnerJson innerjsonobject = new InnerJson();
                    innerjsonobject.ServiceRequest = "Unlock AD Account";
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
                    //await context.PostAsync($"{response1.Content}");

                    //await context.PostAsync($"You reached {result.Intents[0].Intent} resonse is {response1.Content} .");
                }
                await context.PostAsync(message);

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

        
        [LuisIntent("Unlock AD Account")]
        public async Task UnlockADIntent(IDialogContext context, LuisResult result)
        {

            await this.AECallAsync(context, result);

            await context.PostAsync($"I will unlock account for {result.Entities[0].Entity} as soon as possible... Visit me again whenever you need my help. Have a great day:)");

        }


        [LuisIntent("Creating Snapshot")]
        public async Task AddSnapshotIntent_Test(IDialogContext context, LuisResult result)
        {
            intentsr = result.Intents[0].Intent;
            PromptDialog.Text(context, ResumeAfterVMNameClarification, "Please give me your VM name ");
        }
        private async Task ResumeAfterVMNameClarification(IDialogContext context, IAwaitable<string> result)
        {
            VM_Name = await result;
            MyEntities.Add("VM_Name", VM_Name);
            PromptDialog.Text(context, ResumeAfterSnapNameUnlockClarification, "With what name you want me to save this snap?");
            //await context.PostAsync($"You entered {sname}.");
        }
        private async Task ResumeAfterSnapNameUnlockClarification(IDialogContext context, IAwaitable<string> result)
        {
            sname = await result;
            MyEntities.Add("snapshotname", sname);
            await this.AECallAsync(context, result);
            //await context.PostAsync($"{response1.Content}");
            await context.PostAsync($"I will take a snapshot named {sname} as soon as possible... Visit me again whenever you need my help. Have a great day :)");
        }

        [LuisIntent("Creating Active Directory User")]
        public async Task AddAccount(IDialogContext context, LuisResult result)
        {
            intentsr = result.Intents[0].Intent;
            PromptDialog.Text(context, ResumeAfterOrgNameClarification, "Give me your organization unit please ");
        }

        private async Task ResumeAfterOrgNameClarification(IDialogContext context, IAwaitable<string> result)
        {
            oname = await result;
            MyEntities.Add("OrganizationUnit_Name", oname);
            PromptDialog.Text(context, ResumeAfterSamNameClarification, "May I know sam name for your account");
            //await context.PostAsync($"I see you want to order {food}.");
        }

        private async Task ResumeAfterSamNameClarification(IDialogContext context, IAwaitable<string> result)
        {
            sname = await result;
            MyEntities.Add("SamAccount_Name", sname);
            PromptDialog.Text(context, ResumeAfterDispNameClarification, "What name you would like on display?");
            //await context.PostAsync($"You entered {sname}.");
        }

        private async Task ResumeAfterDispNameClarification(IDialogContext context, IAwaitable<string> result)
        {
            dname = await result;
            MyEntities.Add("Display_Name", dname);
            PromptDialog.Text(context, ResumeAfterUserNameClarification, "Enter username of your choice");
            //await context.PostAsync($"You entered {sname}.");
        }

        private async Task ResumeAfterUserNameClarification(IDialogContext context, IAwaitable<string> result)
        {
            uname = await result;
            MyEntities.Add("User_Name", uname);
            PromptDialog.Text(context, ResumeAfterPasswordClarification, "And what password would you like to set?");
            //await context.PostAsync($"You entered {sname}.");
        }

        private async Task ResumeAfterPasswordClarification(IDialogContext context, IAwaitable<string> result)
        {
            pass = await result;
            MyEntities.Add("Password", pass);
            await this.AECallAsync(context, result);
            await context.PostAsync($"I will create AD account for {sname} soon... Visit me again whenever you need my help... Have a great day :)");
        }
        
        [LuisIntent("Create Virtual Machine")]
        public async Task AddVirtualMachinesIntent(IDialogContext context, LuisResult result)
        {
            intentsr = result.Intents[0].Intent;
            PromptDialog.Text(context, ResumeAftervcenterIPClarification, "Give me your vCenter IP please ");
        }
        private async Task ResumeAftervcenterIPClarification(IDialogContext context, IAwaitable<string> result)
        {
            vcenter_IP = await result;
            MyEntities.Add("vcenter_IP", vcenter_IP);
            PromptDialog.Text(context, ResumeAfterPortClarification, "May I know port number for the same ");
            //await context.PostAsync($"I see you want to order {food}.");
        }
        private async Task ResumeAfterPortClarification(IDialogContext context, IAwaitable<string> result)
        {
            
            Port = await result;
            MyEntities.Add("Port", Port);
            PromptDialog.Text(context, ResumeAfterAdminUsernameClarification, "Please enter your admin username below ");
            //await context.PostAsync($"I see you want to order {food}.");
        }
        private async Task ResumeAfterAdminUsernameClarification(IDialogContext context, IAwaitable<string> result)
        {
            
            Admin_Username = await result;
            MyEntities.Add("Admin_Username", Admin_Username);
            PromptDialog.Text(context, ResumeAfterAdminPasswordClarification, "Can I have password for the same ");
            //await context.PostAsync($"I see you want to order {food}.");
        }
        private async Task ResumeAfterAdminPasswordClarification(IDialogContext context, IAwaitable<string> result)
        {
            
            Admin_Password = await result;
            MyEntities.Add("Admin_Password", Admin_Password);
            PromptDialog.Text(context, ResumeAfterVMHostIPClarification, "Give me VM Host IP please ");
            //await context.PostAsync($"I see you want to order {food}.");
        }
        private async Task ResumeAfterVMHostIPClarification(IDialogContext context, IAwaitable<string> result)
        {
            
            VMHost_IP = await result;
            MyEntities.Add("VMHost_IP", VMHost_IP);
            PromptDialog.Text(context, ResumeAfterDatastoreClarification, "Datastore name please");
            //await context.PostAsync($"I see you want to order {food}.");
        }
        private async Task ResumeAfterDatastoreClarification(IDialogContext context, IAwaitable<string> result)
        {
            
            Datastore = await result;
            MyEntities.Add("Datastore", Datastore);
            PromptDialog.Text(context, ResumeAfterTemplateNameClarification, "Give template a name of your choice ");
            //await context.PostAsync($"I see you want to order {food}.");
        }
        private async Task ResumeAfterTemplateNameClarification(IDialogContext context, IAwaitable<string> result)
        {
            
            Template_Name = await result;
            MyEntities.Add("Template_Name", Template_Name);
            PromptDialog.Text(context, ResumeAfterVMName1Clarification, "And give this VM a name of your choice ");
            //await context.PostAsync($"I see you want to order {food}.");
        }
        private async Task ResumeAfterVMName1Clarification(IDialogContext context, IAwaitable<string> result)
        {
            VM_Name = await result;
            MyEntities.Add("VM_Name", VM_Name);
            await this.AECallAsync(context, result);
            await context.PostAsync($"I will add VM named {VM_Name} soon... Visit me again whenever you need my help... Have a great day :)");
        }
        
        private async Task ShowLuisResult(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }

        private async Task AECallAsync(IDialogContext context, LuisResult result)
        {

            await context.PostAsync($"{result.Intents[0].Intent}");
            count = result.Entities.Count;

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
            for (int i = 0; i < count; i++)
            {
                jparameter[i] = new JsonParam();
                jparameter[i].question = result.Entities[i].Type;
                jparameter[i].answer = result.Entities[i].Entity;
                InnerJsonParam.Add(jparameter[i]);
            }

            /*
            JsonParam jparameter1 = new JsonParam();
            jparameter1.question = "VM_Name";
            jparameter1.answer = VM_Name;
            InnerJsonParam.Add(jparameter1);

            JsonParam jparameter2 = new JsonParam();
            jparameter2.question = "snapshotname";
            jparameter2.answer = sname;
            InnerJsonParam.Add(jparameter2);
            */

            InnerJson innerjsonobject = new InnerJson();
            innerjsonobject.ServiceRequest = result.Intents[0].Intent;
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
        }

        private async Task AECallAsync(IDialogContext context, IAwaitable<string> result)
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
            for (int i = 0; i < MyEntities.Count; i++)
            {
                jparameter[i] = new JsonParam();
                jparameter[i].question = MyEntities.ElementAt(i).Key;
                jparameter[i].answer = MyEntities.ElementAt(i).Value;
                InnerJsonParam.Add(jparameter[i]);
            }
            MyEntities.Clear();

            /*
            JsonParam jparameter1 = new JsonParam();
            jparameter1.question = "VM_Name";
            jparameter1.answer = VM_Name;
            InnerJsonParam.Add(jparameter1);

            JsonParam jparameter2 = new JsonParam();
            jparameter2.question = "snapshotname";
            jparameter2.answer = sname;
            InnerJsonParam.Add(jparameter2);
            */

            InnerJson innerjsonobject = new InnerJson();
            innerjsonobject.ServiceRequest = intentsr;
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
