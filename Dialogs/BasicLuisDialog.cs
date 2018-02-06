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
using Microsoft.Cognitive.LUIS.ActionBinding.Bot;
using System.Reflection;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisActionDialog<object>
    {
       
        public BasicLuisDialog() : base(
            new Assembly[] { typeof(CreatingSnapshotAction).Assembly },
            (action, context) =>
            { 
            },
            new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"],
            ConfigurationManager.AppSettings["LuisAPIKey"],
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }


        [LuisIntent("Greeting")]
        public async Task IntentGreetingHandlerAsync(IDialogContext context, object actionResult)
        {
            var message = context.MakeMessage();
            message.Text = actionResult != null ? actionResult.ToString() : "Cannot resolve your query";
            await context.PostAsync(message);
        }

        /*
        [LuisIntent("Cancel")]
        public async Task IntentCancelHandlerAsync(IDialogContext context, object actionResult)
        {
            var message = context.MakeMessage();
            message.Text = actionResult != null ? actionResult.ToString() : "Cannot resolve your query";
            await context.PostAsync(message);
            
        }
        */
        [LuisIntent("Help")]
        public async Task IntentHelpHandlerAsync(IDialogContext context, object actionResult)
        {
            var message = context.MakeMessage();
            message.Text = actionResult != null ? actionResult.ToString() : "Cannot resolve your query";
            await context.PostAsync(message);
            await context.PostAsync("You may say something like this\n Add ad user\n Please unlock ad user\n Take a snap\n Add VM");
        }

        [LuisIntent("None")]
        public async Task IntentNoneHandlerAsync(IDialogContext context, object actionResult)
        {
            var message = context.MakeMessage();
            message.Text = actionResult != null ? actionResult.ToString() : "Cannot resolve your query";
            await context.PostAsync(message);
            await context.PostAsync("\nYou may say something like this\n Add ad user\n Please unlock ad user\n Take a snap\n Add VM");
        }

        [LuisIntent("Creating Snapshot")]
        public async Task IntentActionResultHandlerAsync(IDialogContext context, object actionResult)
        {
            var message = context.MakeMessage();
            message.Text = actionResult != null ? actionResult.ToString() : "Cannot resolve your query";
            await context.PostAsync(message);
        }

        [LuisIntent("Create Virtual Machine​​")]
        public async Task IntentCreateVirtualMachine​​ActionResultHandlerAsync(IDialogContext context, object actionResult)
        {
            var message = context.MakeMessage();
            message.Text = actionResult != null ? actionResult.ToString() : "Cannot resolve your query";
            await context.PostAsync(message);
        }

        [LuisIntent("Unlock AD Account")]
        public async Task IntentUnlockADAccountActionResultHandlerAsync(IDialogContext context, object actionResult)
        {
            var message = context.MakeMessage();
            message.Text = actionResult != null ? actionResult.ToString() : "Cannot resolve your query";   
            await context.PostAsync(message);
        }

        [LuisIntent("Creating Active Directory User")]
        public async Task IntentCreatingActiveDirectoryUserActionResultHandlerAsync(IDialogContext context, object actionResult)
        {
            var message = context.MakeMessage();
            message.Text = actionResult != null ? actionResult.ToString() : "Cannot resolve your query";
            await context.PostAsync(message);
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
