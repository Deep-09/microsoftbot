using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Cognitive.LUIS.ActionBinding;

namespace LuisBot.Dialogs
{
    [Serializable]
    [LuisActionBinding("Greeting", FriendlyName = "Greeting")]
    public class GreetingAction : BaseLuisAction
    {

        public override Task<object> FulfillAsync()
        {
            return Task.FromResult((object)$"Hello! I am your IT Service Desk Virtual Assistant *Maggi* :) . I can help you with IT service related issues and requests. How may I help you today?");
        }
    }
}