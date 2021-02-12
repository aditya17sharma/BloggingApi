using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BlogWebApi.Attributes
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {


        /// <summary>
        /// Checks for the membership of the user from accenture directory
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers.Authorization == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

                }
                else
                {
                    string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                    string decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));

                    string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');

                    string userName = usernamePasswordArray[0];
                    string userPassword = usernamePasswordArray[1];
                    
                    bool isAuthorize = false;

                    if (userName.Trim().ToLower() == ConfigurationManager.AppSettings["UserName"].ToString().Trim().ToLower()
                        && userPassword.Trim().ToLower() == ConfigurationManager.AppSettings["UserPassword"].ToString().Trim().ToLower())
                    {
                        isAuthorize = true;
                    }

                    if (isAuthorize)
                    {
                        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userName), null);
                    }
                    else
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }
            }
            catch (Exception ex)
            {
                actionContext.Response = actionContext.Request.CreateResponse(ex.Message + ex.InnerException);
            }
        }


    }
}