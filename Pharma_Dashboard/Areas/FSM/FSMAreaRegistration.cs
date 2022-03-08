using System.Web.Mvc;

namespace PAsia_Dashboard.Areas.FSM
{
    public class FSMAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FSM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FSM_default",
                "FSM/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}