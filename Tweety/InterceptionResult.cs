using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Tweety
{

    /// <summary>
    /// HttpRequestMessage Interception Result, See: <see cref="Webhooks.WebhookInterceptor"/>.
    /// </summary>
    public class InterceptionResult
    {
        /// <summary>
        /// Determines of the request is handled internally, in this case, you should return <see cref="Response"/>.
        /// </summary>
        public bool IsHandled { get; internal set; }

        /// <summary>
        /// The <see cref="HttpResponseMessage"/> to return to the client if <see cref="IsHandled"/> is true.
        /// If  <see cref="IsHandled"/> is false, this will be empty <see cref="HttpResponseMessage"/> with status code: <see cref="HttpStatusCode.OK"/>.
        /// </summary>
        public HttpResponseMessage Response { get; internal set; }

        /// <summary>
        /// The original request message, for reference purposes only.
        /// </summary>
        public HttpRequestMessage RequestMessage { get; internal set; }

        private InterceptionResult()
        {

        }

        internal static InterceptionResult CreateHandled(HttpResponseMessage response, HttpRequestMessage request)
        {
            return new InterceptionResult()
            {
                IsHandled = true,
                RequestMessage = request,
                Response = response
            };
        }

        internal static InterceptionResult CreateUnhandled(HttpResponseMessage response, HttpRequestMessage request)
        {
            return new InterceptionResult()
            {
                IsHandled = false,
                RequestMessage = request,
                Response = response
            };
        }

    }
}
