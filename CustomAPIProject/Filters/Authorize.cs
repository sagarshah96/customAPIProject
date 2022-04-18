using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAPIProject.Filters
{
    public class Authorize : Attribute, IAuthorizationFilter
    {
        Roles[] _roles;
        public Authorize(params Roles[] roles)
        {
            _roles = roles;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            int CustomerId = (int)context.HttpContext.Items["CustomerId"];
            int Role = (int)context.HttpContext.Items["RoleId"];


            if (CustomerId <= 0)
                UnAuthorized(context);

            if(CustomerId > 0 && Role > 0)
            {
                if (_roles.Contains((Roles)Role))
                    return;
                else
                    UnAuthorized(context);
            }

            return;

        }

        private void UnAuthorized(AuthorizationFilterContext context)
        {
            context.Result = new JsonResult(new { msg = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
