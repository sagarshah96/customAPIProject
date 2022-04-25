using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace CustomAPIProject.Filters
{
    public class Validate : ActionFilterAttribute
    {
        public IEnumerable<string> Errors { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (context.HttpContext.Request.Method == "POST" && !context.ModelState.IsValid)
            {
                Errors = context.ModelState.SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).ToArray();
                context.Result = new BadRequestObjectResult(Errors);
            }
        }
    }
}
