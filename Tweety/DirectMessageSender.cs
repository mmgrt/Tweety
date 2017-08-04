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

    /// <summary>
    /// Helper class to send Direct Message to twitter user using the screen name.
    /// </summary>
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
        /// <param name="messageText">Message Text to send.. Less than 140 char.</param>
        /// <returns>
        /// </returns>
        [Obsolete("Use SendAsync instead.")]
        public async Task<Result<MessageCreate>> Send(string toScreenName, string messageText)
        {

            //TODO: Provide a generic class to make Twitter API Requests.

            if (string.IsNullOrEmpty(messageText))
            {
                throw new TweetyException("You can't send an empty message.");
            }
            if (messageText.Length > 140)
            {
                throw new TweetyException("You can't send more than 140 char using this end point, use SendAsync instead.");
            }

            messageText = Uri.EscapeDataString(messageText);
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


        /// <summary>
        /// Send a direct message to a user by userId, from the current user (using AuthContext).
        /// </summary>
        /// <param name="userId">The Twitter User Id to send the message to.</param>
        /// <param name="messageText">The text of the message, should be less than 10,000 chars.</param>
        /// <returns></returns>
        public async Task<Result<DirectMessageResult>> SendAsync(long userId, string messageText)
        {

            //TODO: Provide a generic class to make Twitter API Requests.

            if (string.IsNullOrEmpty(messageText))
            {
                throw new TweetyException("You can't send an empty message.");
            }

            if (messageText.Length > 10000)
            {
                throw new TweetyException("Invalid message, the length of the message should be less than 10000 chars.");
            }

            if (userId == default(long))
            {
                throw new TweetyException("Invalid userId.");
            }

            string resourceUrl = $"https://api.twitter.com/1.1/direct_messages/events/new.json";

            NewDirectMessageObject newDmEvent = new NewDirectMessageObject();
            newDmEvent.@event = new Event() { type = "message_create" };
            newDmEvent.@event.message_create = new NewEvent_MessageCreate() { message_data = new NewEvent_MessageData { text = messageText }, target = new Target { recipient_id = userId.ToString() } };
            string jsonObj = JsonConvert.SerializeObject(newDmEvent);


            HttpResponseMessage response;
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("Authorization", AuthHeaderBuilder.Build(AuthContext, HttpMethod.Post, resourceUrl));


                response = await client.PostAsync(resourceUrl, new StringContent(jsonObj, Encoding.UTF8, "application/json"));
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string msgCreateJson = await response.Content.ReadAsStringAsync();
                NewDmResult mCreateObj = JsonConvert.DeserializeObject<NewDmResult>(msgCreateJson);
                return new Result<DirectMessageResult>(mCreateObj.@event);
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(jsonResponse))
            {
                TwitterError err = JsonConvert.DeserializeObject<TwitterError>(jsonResponse);
                return new Result<DirectMessageResult>(err);
            }
            else
            {
                //TODO: Provide a way to return httpstatus code 

                return new Result<DirectMessageResult>();
            }
             
        }



    }
}
