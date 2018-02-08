using System.Collections.Generic;
namespace LuisBot.Dialogs
{
    public class InnerJson
    {
        public string ServiceRequest { get; set; }
        public IList<JsonParam> @params { get; set; }
    }
}