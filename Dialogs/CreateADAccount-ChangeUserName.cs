using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS.ActionBinding;

namespace LuisBot.Dialogs
{
    [Serializable]
    [LuisActionBinding("CreateADAccount-ChangeUserName", FriendlyName = "Change Username")]
    public class CreateADAccount_ChangeUserName : BaseLuisContextualAction<CreatingActiveDirectoryUserAction>
    {
        [Required(ErrorMessage = "Please provide a new  username")]
        [LuisActionBindingParam(CustomType = "User_Name")]
        public string User_Name { get; set; }

        public override Task<object> FulfillAsync()
        {
            if (this.Context == null)
            {
                throw new InvalidOperationException("Action context not defined.");
            }

            this.Context.User_Name = this.User_Name;

            return Task.FromResult((object)$"Username changed to {this.User_Name}");
        }
    }
}