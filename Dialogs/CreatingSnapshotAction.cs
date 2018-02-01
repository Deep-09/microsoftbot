using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using LuisBot.Dialogs;
using Microsoft.Cognitive.LUIS.ActionBinding;
using Newtonsoft.Json;
using RestSharp;

namespace Microsoft.Bot.Sample.LuisBot
{
    [Serializable]
    [LuisActionBinding("Creating Snapshot", FriendlyName = "Creating Snapshot Service Request")]
    public class CreatingSnapshotAction : BaseLuisAction
    {
        [Required(ErrorMessage = "Please give me your VM name")]
        [LuisActionBindingParam(CustomType ="VM_Name", Order = 1)]
        public string VM_Name { get; set; }

        [Required(ErrorMessage = "With what name you want me to save this snap?")]
        [LuisActionBindingParam(CustomType ="snapshot_Name", Order = 2)]
        public string snapshot_Name { get; set; }


        public override Task<object> FulfillAsync()
        {
            Dictionary<string, string> MyEntities = new Dictionary<string, string>();

            MyEntities.Add("VM_Name", VM_Name);
            MyEntities.Add("snapshot_Name", snapshot_Name);
 
            CreateJSON createJSON = new CreateJSON();

            createJSON.AECall(MyEntities, "Creating Active Directory User");

            return Task.FromResult((object)$"I will take a snapshot named {this.snapshot_Name} as soon as possible... Visit me again whenever you need my help. Have a great day :)");
        }
    }
}