using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Cognitive.LUIS.ActionBinding;

namespace LuisBot.Dialogs
{
    [Serializable]
    [LuisActionBinding("Help", FriendlyName = "Help")]
    public class HelpAction : BaseLuisAction
    {
        public override Task<object> FulfillAsync()
        {
            return Task.FromResult((object)$"Hello! I am your IT Service Desk Virtual Assistant *Maggi* :) . I can help you with IT service related issues and requests...");
        }
    }
}