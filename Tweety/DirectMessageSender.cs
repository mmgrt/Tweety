using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tweety.Authentication;
using Tweety.Models;
using Tweety.Models.Twitter;

namespace Tweety
{
    public class DirectMessageSender
    {

        public TweetyAuthContext AuthContext { get; set; }

        public DirectMessageSender(TweetyAuthContext context)
        {
            AuthContext = context;
        }


        /// <summary>
        /// Send a direct message to User from the current user (using AuthContext).
        /// </summary>
        /// <param name="toScreenName">To (screen name without '@' sign)</param>
        /// <param name="messageText">Message Text to send.</param>
        /// <returns>
        /// </returns>
        public async Task<Result<MessageCreate>> Send(string toScreenName, string messageText)
        {

            //TODO: Provide a generic class to make Twitter API Requests.

            string resourceUrl = $"https://api.twitter.com/1.1/direct_messages/new.json?text={messageText}&screen_name={toScreenName}";

            HttpResponseMessage response;
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("Authorization", AuthHeaderBuilder.Build(AuthContext, HttpMethod.Post, resourceUrl));

                response = await client.PostAsync(resourceUrl, new StringContent(""));
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {

                string msgCreateJson = await response.Content.ReadAsStringAsync();
                MessageCreate mCreateObj = JsonConvert.DeserializeObject<MessageCreate>(msgCreateJson);
                return new Result<MessageCreate>(mCreateObj);
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(jsonResponse))
            {
                TwitterError err = JsonConvert.DeserializeObject<TwitterError>(jsonResponse);
                return new Result<MessageCreate>(err);
            }
            else
            {
                //TODO: Provide a way to return httpstatus code 

                return new Result<MessageCreate>();
            } 
        }
    }
}
