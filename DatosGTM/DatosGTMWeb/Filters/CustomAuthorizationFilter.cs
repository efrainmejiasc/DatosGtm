using Microsoft.AspNetCore.Mvc.Filters;
//using System.Web.Http.Controllers;


namespace DatosGTMlWeb.Filters
{
    public class CustomAuthorizationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Executed before execution of an action method
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Executed after execution of an action method
        }
    }
}
