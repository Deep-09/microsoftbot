using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Cognitive.LUIS.ActionBinding;

namespace LuisBot.Dialogs
{
    [Serializable]
    [LuisActionBinding("Create Incident", FriendlyName = "Create Incident")]
    public class CreateIncidentAction : BaseLuisAction
    {
        [Required(ErrorMessage = "Can you please elaborate ?")]
        [LuisActionBindingParam(CustomType = "Description", Order = 1)]
        //CustomType/BuiltinType = (Entity name in LUIS)
        public string description { get; set; }


        public override Task<object> FulfillAsync()
        {
            Dictionary<string, string> MyEntities = new Dictionary<string, string>();

            MyEntities.Add("description", this.description);

            CreateJSON createJSON = new CreateJSON();

            createJSON.AECall(MyEntities, "Create Incident");

            return Task.FromResult((object)$"I will create incident for  {this.description} as soon as possible... Visit me again whenever you need my help. Have a great day :)");
        }
    }
}