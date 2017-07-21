using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tweety.Extensions;
using Tweety.Models;
using Tweety.Models.Twitter;

namespace Tweety.Webhooks
{

    /// <summary>
    /// This class is responsible to intercept incoming requests to the server
    /// and handle them properly.
    /// </summary>
    public class WebhookInterceptor
    {

        /// <summary>
        /// Twitter App Consumer Secret.
        /// Used to Hash incoming/ outgoing data.
        /// </summary>
        public string ConsumerSecret { get; set; }

        public WebhookInterceptor(string consumerSecret)
        {
            ConsumerSecret = consumerSecret;
        }


        /// <summary>
        /// Intercept incoming requests to the server to handle them, Currently:
        ///     - Challenge Response Check.
        ///     - Incoming DirectMessage.
        /// </summary>
        /// <param name="requestMessage">Thr request message object you recieved.</param>
        /// <param name="OnDirectMessageRecieved">If this is an incoming direct message, this callback will be fired along with the message object <see cref="DirectMessageEvent"/>.</param>
        /// <returns>
        /// <see cref="bool"/> true if handled/ false if not.
        /// <see cref="HttpResponseMessage"/> If handled, it's good to return this as the server response, if not handled, this message will be 'OK 200' empty response message.
        /// </returns>
        public async Task<(bool handled, HttpResponseMessage response)> InterceptIncomingRequest(HttpRequestMessage requestMessage, Action<DirectMessageEvent> OnDirectMessageRecieved)
        {
            if (string.IsNullOrEmpty(ConsumerSecret))
            {
                throw new TweetyException("Consumer Secret can't be null.");
            }

            if (requestMessage.Method == HttpMethod.Get)
            {
                Dictionary<string, string> requestParams = requestMessage.RequestUri.GetParams();
                if (!requestParams.ContainsKey("crc_token"))
                {

                    goto Finally;
                }

                // Challenge Response Check Request:
                string crcToken = requestParams["crc_token"];
                HttpResponseMessage response = AcceptChallenge(crcToken);

                return (true, response);
            }
            else if (requestMessage.Method == HttpMethod.Post)
            {

                if (!requestMessage.Headers.Contains("x-twitter-webhooks-signature"))
                {
                    goto Finally;
                }

                string payloadSignature = requestMessage.Headers.GetValues("x-twitter-webhooks-signature").First();
                string payload = await requestMessage.Content.ReadAsStringAsync();


                bool signatureMatch = IsValidTwitterPostRequest(payloadSignature, payload);
                if (!signatureMatch)
                {
                    //This is interseting, a twitter signature key 'x-twitter-webhooks-signature' is there but it's wrong..!
                    return (true, new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
                }

                try
                {
                    WebhookDMResult dmResult = JsonConvert.DeserializeObject<WebhookDMResult>(payload);
                    DirectMessageEvent eventResult = dmResult;

                    OnDirectMessageRecieved?.Invoke(eventResult);
                }
                catch 
                {
                    //Failed to deserialize twitter direct message,
                    //TODO: Handle in a better way.. perhaps send the json?
                }

            }

            Finally:
            return (false, OK());
        }


        private HttpResponseMessage AcceptChallenge(string crcToken)
        {

            byte[] hashKeyArray = Encoding.UTF8.GetBytes(ConsumerSecret);
            byte[] crcTokenArray = Encoding.UTF8.GetBytes(crcToken);

            HMACSHA256 hmacSHA256Alog = new HMACSHA256(hashKeyArray);

            byte[] computedHash = hmacSHA256Alog.ComputeHash(crcTokenArray);

            string challengeToken = $"sha256={Convert.ToBase64String(computedHash)}";

            CRCResponseToken responseToken = new CRCResponseToken()
            {
                Token = challengeToken
            };

            string jsonResponse = JsonConvert.SerializeObject(responseToken);

            return OK(jsonResponse);
        }
        private bool IsValidTwitterPostRequest(string twWebhookSignature, string payload)
        {
            byte[] hashKeyArray = Encoding.UTF8.GetBytes(ConsumerSecret);
            HMACSHA256 hmacSHA256Alog = new HMACSHA256(hashKeyArray);

            byte[] computedHash = hmacSHA256Alog.ComputeHash(Encoding.UTF8.GetBytes(payload));
            string localHashedSignature = $"sha256={Convert.ToBase64String(computedHash)}";

            return localHashedSignature == twWebhookSignature;
        }

        private static HttpResponseMessage OK(string content = "") => new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(content) };

    }
}
