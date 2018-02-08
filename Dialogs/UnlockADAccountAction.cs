using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS.ActionBinding;
using Newtonsoft.Json;


namespace LuisBot.Dialogs
{
    [Serializable]
    [LuisActionBinding("Unlock AD Account", FriendlyName = "Unlock AD Account Service Request")]
    public class UnlockADAccountAction : BaseLuisAction
    {
        [Required(ErrorMessage = "Please give me your sam account name")]
        [LuisActionBindingParam(CustomType = "samaccountname", Order = 1)]
        //CustomType/BuiltinType = (Entity name in LUIS)
        public string samaccountname { get; set; }


        public override Task<object> FulfillAsync()
        {
            Dictionary<string, string> MyEntities = new Dictionary<string, string>();

            MyEntities.Add("samaccountname", this.samaccountname);

            CreateJSON createJSON = new CreateJSON();

            createJSON.AECall(MyEntities, "Unlock AD Account");

            return Task.FromResult((object)$"I will unlock account for  {this.samaccountname} as soon as possible... Visit me again whenever you need my help. Have a great day :)");
        }
    }
}