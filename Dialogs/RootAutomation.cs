using System.Collections.Generic;
namespace LuisBot.Dialogs
{
    public class RootAutomation
    {
        public string orgCode { get; set; }
        public string workflowName { get; set; }
        public string userId { get; set; }
        public string sourceId { get; set; }
        public string source { get; set; }
        public string responseMailSubject { get; set; }
        public IList<AutomationParameter> @params { get; set; }
    }
}