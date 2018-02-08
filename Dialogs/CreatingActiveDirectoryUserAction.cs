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
    [LuisActionBinding("Creating Active Directory User", FriendlyName = "Creating Active Directory User Service Request")]
    public class CreatingActiveDirectoryUserAction : BaseLuisAction
    {
        

        [Required(ErrorMessage = "Give me your organization unit please")]
        [LuisActionBindingParam(CustomType = "OrganizationUnit_Name", Order = 1)]
        public string OrganizationUnit_Name { get; set; }

        [Required(ErrorMessage = "May I know sam name for your account")]
        [LuisActionBindingParam(CustomType = "samaccountname", Order = 2)]
        public string samaccountname { get; set; }

        [Required(ErrorMessage = "Enter username of your choice")]
        [LuisActionBindingParam(CustomType = "User_Name", Order = 4)]
        public string User_Name { get; set; }

        [Required(ErrorMessage = "What name you would like on display?")]
        [LuisActionBindingParam(CustomType = "Display_Name", Order = 3)]
        public string Display_Name { get; set; }

        [Required(ErrorMessage = "And what password would you like to set?")]
        [LuisActionBindingParam(CustomType = "Password", Order = 5)]
        public string Password { get; set; }

        
        

        public override Task<object> FulfillAsync()
        {
            Dictionary<string, string> MyEntities = new Dictionary<string, string>();

            MyEntities.Add("OrganizationUnit_Name", this.OrganizationUnit_Name);
            MyEntities.Add("samaccountname", this.samaccountname);
            MyEntities.Add("User_Name", this.User_Name);
            MyEntities.Add("Display_Name", this.Display_Name);
            MyEntities.Add("Password", this.Password);

            CreateJSON createJSON = new CreateJSON();

            createJSON.AECall(MyEntities, "Creating Active Directory User");
        
            return Task.FromResult((object)$"I will create AD account for {this.samaccountname} soon... Visit me again whenever you need my help... Have a great day :)");
        }
    }
}