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
    [LuisActionBinding("Unlock AD Account", FriendlyName = "Unlock AD Account Service Request")]
    public class UnlockADAccountAction : BaseLuisAction
    {
        [Required(ErrorMessage = "Please give me your sam account name")]
        [LuisActionBindingParam(CustomType = "samaccountname", Order = 1)]
        public string samaccountname { get; set; }


        public override Task<object> FulfillAsync()
        {
            Dictionary<string, string> MyEntities = new Dictionary<string, string>();

            MyEntities.Add("samaccountname", samaccountname);

            CreateJSON createJSON = new CreateJSON();

            createJSON.AECall(MyEntities, "Creating Active Directory User");

            return Task.FromResult((object)$"I will unlock account for  {this.samaccountname} as soon as possible... Visit me again whenever you need my help. Have a great day :)");
        }
    }
}