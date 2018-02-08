using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Bot.Sample.LuisBot;
using Microsoft.Cognitive.LUIS.ActionBinding;

namespace LuisBot.Dialogs
{
    [Serializable]
    [LuisActionBinding("CreatingSnapshotAction-ChangeVMNameAction", FriendlyName = "Change VM Name")]
    public class ChangeVMNameAction : BaseLuisContextualAction<CreatingSnapshotAction>
    {
        [Required(ErrorMessage = "Please provide the new VM Name")]
        [LuisActionBindingParam(CustomType = "VM_Name")]
        public string VM_Name { get; set; }

        public override Task<object> FulfillAsync()
        {
            if (this.Context == null)
            {
                throw new InvalidOperationException("Action context not defined.");
            }

            this.Context.VM_Name = this.VM_Name;

            return Task.FromResult((object)$"VM Name changed to {this.VM_Name}");
        }
    }
}