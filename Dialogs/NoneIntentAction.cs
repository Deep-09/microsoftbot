using System;
using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS.ActionBinding;

namespace LuisBot.Dialogs
{
    [Serializable]
    [LuisActionBinding("None", FriendlyName = "Out of scope")]
    public class NoneIntentAction : BaseLuisAction
    {
        public override Task<object> FulfillAsync()
        {
            return Task.FromResult((object)$"I am unable to understand you... Please ask me about IT Services...");
        }
    }
}