using System;
using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS.ActionBinding;

namespace LuisBot.Dialogs
{
    [Serializable]
    [LuisActionBinding("Cancel", FriendlyName = "Cancel")]
    public class CancelAction : BaseLuisAction
    {
        public override Task<object> FulfillAsync()
        {
            return Task.FromResult((object)$"I cancelled your action");
        }
    }
}