using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Xml;

namespace Evis.VMS.UI.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenInspector : DelegatingHandler
    {
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1" />. The task object representing the asynchronous operation.
        /// </returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                string token_name = "X-Token";

                var cookies = request.Headers.GetCookies("X-Token").FirstOrDefault();

                if (cookies != null)
                {
                    var token = cookies.Cookies.FirstOrDefault(x => x.Value == token_name);
                    try
                    {
                        if (token == null)
                        {
                            HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Token Missing");
                            return Task.FromResult(reply);
                        }
                        else if (token.Name != token_name)
                        {
                            HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Token");
                            return Task.FromResult(reply);
                        }
                    }
                    catch (Exception)
                    {
                        HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Token");
                        return Task.FromResult(reply);
                    }
                }
                else
                {
                    var token = request.Headers.GetValues("X-Token").FirstOrDefault();
                    if (token == null)
                    {
                        HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Token Missing");
                        return Task.FromResult(reply);
                    }

                    else if (token != token_name)
                    {
                        HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Token");
                        return Task.FromResult(reply);
                    }
                }
                return base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                //if no header found, we dont need to do anything. Need to find its alternate.
                //throw new Exception(ex.ToString());

                HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Token Missing");
                return Task.FromResult(reply);
            }
        }
    }
}