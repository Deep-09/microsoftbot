﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS.ActionBinding;

namespace LuisBot.Dialogs
{
    [Serializable]
    [LuisActionBinding("Create Virtual Machine", FriendlyName = "Create Virtual Machine Service Request")]
    public class CreateVirtualMachine : BaseLuisAction
    {
        [Required(ErrorMessage = "Give me your vCenter IP please ")]
        [LuisActionBindingParam(CustomType = "vcenter_IP", Order = 1)]
        public string vcenter_IP { get; set; }

        [Required(ErrorMessage = "May I know port number for the same")]
        [LuisActionBindingParam(CustomType = "Port", Order = 2)]
        public string Port { get; set; }

        [Required(ErrorMessage = "Please enter your admin username below")]
        [LuisActionBindingParam(CustomType = "Admin_Username", Order = 3)]
        public string Admin_Username { get; set; }

        [Required(ErrorMessage = "Can I have password for the same")]
        [LuisActionBindingParam(CustomType = "Admin_Password", Order = 4)]
        public string Admin_Password { get; set; }

        [Required(ErrorMessage = "Give me VM Host IP please")]
        [LuisActionBindingParam(CustomType = "VMHost_IP", Order = 5)]
        public string VMHost_IP { get; set; }

        [Required(ErrorMessage = "Datastore name please")]
        [LuisActionBindingParam(CustomType = "Datastore", Order = 6)]
        public string Datastore { get; set; }

        [Required(ErrorMessage = "Give template a name of your choice")]
        [LuisActionBindingParam(CustomType = "Template_Name", Order = 7)]
        public string Template_Name { get; set; }

        [Required(ErrorMessage = "And give this VM a name of your choice")]
        [LuisActionBindingParam(CustomType = "VM_Name", Order = 8)]
        public string VM_Name { get; set; }


        public override Task<object> FulfillAsync()
        {
            Dictionary<string, string> MyEntities = new Dictionary<string, string>();

            MyEntities.Add("vcenter_IP", this.vcenter_IP);
            MyEntities.Add("Port", this.Port);
            MyEntities.Add("Admin_Username", this.Admin_Username);
            MyEntities.Add("Admin_Password", this.Admin_Password);
            MyEntities.Add("VMHost_IP", this.VMHost_IP);
            MyEntities.Add("Datastore", this.Datastore);
            MyEntities.Add("Template_Name", this.Template_Name);
            MyEntities.Add("VM_Name", this.VM_Name);

            CreateJSON createJSON = new CreateJSON();

            createJSON.AECall(MyEntities, "Create Virtual Machine");

            return Task.FromResult((object)$"I will add VM named {this.VM_Name} soon... Visit me again whenever you need my help... Have a great day :)");
        }
    }
}