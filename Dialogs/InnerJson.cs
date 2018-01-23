using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuisBot.Dialogs
{
    public class InnerJson
    {
        public string serviceRequestName { get; set; }
        public IList<JsonParam> @params { get; set; }
    }
}