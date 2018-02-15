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
    [LuisActionBinding("Software Install", FriendlyName = "Software Install")]
    public class InstallSoftwareAction : BaseLuisAction
    {
        [Required(ErrorMessage = "Which software do you want to install ?")]
        [LuisActionBindingParam(CustomType = "Software_Name", Order = 1)]
        //CustomType/BuiltinType = (Entity name in LUIS)
        public string softwarename { get; set; }


        public override Task<object> FulfillAsync()
        {
            Dictionary<string, string> MyEntities = new Dictionary<string, string>();

            MyEntities.Add("softwarename", this.softwarename);

            CreateJSON createJSON = new CreateJSON();

            createJSON.AECall(MyEntities, "Software Install");

            return Task.FromResult((object)$"I will install  {this.softwarename} as soon as possible... Visit me again whenever you need my help. Have a great day :)");
        }
    }
}