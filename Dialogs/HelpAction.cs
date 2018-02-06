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
            return Task.FromResult((object)$"I am unable to understand you... Please ask me about IT Services... ");
        }
    }
}