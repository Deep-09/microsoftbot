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
    [LuisActionBinding("Check Incident Status", FriendlyName = "Check Incident Status")]
    public class CheckIncidentStatusAction : BaseLuisAction
    {
        [Required(ErrorMessage = "Please provide your ticket ID")]
        [LuisActionBindingParam(BuiltinType = BuiltInTypes.Number, Order = 1)]
        //CustomType/BuiltinType = (Entity name in LUIS)
        public string incidentnumber { get; set; }
        public override Task<object> FulfillAsync()
        {
            Dictionary<string, string> MyEntities = new Dictionary<string, string>();

            MyEntities.Add("Incident_Number", this.incidentnumber);

            CreateJSON createJSON = new CreateJSON();

            createJSON.AECall(MyEntities, "Unlock AD Account");

            return Task.FromResult((object)$"I will let you know status of ticked ID {this.incidentnumber} as soon as possible... Visit me again whenever you need my help. Have a great day :)");
        }
    }
}