namespace Microsoft.Bot.Sample.LuisBot
{
    using System;
    using Microsoft.Bot.Builder.FormFlow;

    [Serializable]
    public class HotelsQuery
    {
        [Prompt("Please enter your Organization Name")]
        [Optional]
        public string OrganizationName { get; set; }

        [Prompt("Please enter your Passsword")]
        [Optional]
        public string AirportCode { get; set; }

        [Prompt("Please enter your Sam Account Name")]
        [Optional]
        public string SamAccountName { get; set; }

        [Prompt("Please enter your Display Name")]
        [Optional]
        public string DisplayName { get; set; }

        [Prompt("Please enter your Passsword")]
        [Optional]
        public string Password { get; set; }

   
    }
}