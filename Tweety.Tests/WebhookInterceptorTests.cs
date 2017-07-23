using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tweety.Webhooks;
using Tweety.Models;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace Tweety.Tests
{
    [TestClass]
    public class WebhookInterceptorTests
    {

        [TestMethod]
        public async Task InterceptIncomingMessage_CRCTest()
        {

            string consumerKey = DateTime.Now.Ticks.ToString();
            string crcToken = Guid.NewGuid().ToString();

            HttpRequestMessage crcRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"http://localhost?crc_token={crcToken}");
            WebhookInterceptor interceptor = new WebhookInterceptor(consumerKey);

            InterceptionResult result = await interceptor.InterceptIncomingRequest(crcRequestMessage, null);

            Assert.IsTrue(result.IsHandled);

            HMACSHA256 hmacSHA256Alog = new HMACSHA256(Encoding.UTF8.GetBytes(consumerKey));

            byte[] computedHash = hmacSHA256Alog.ComputeHash(Encoding.UTF8.GetBytes(crcToken));

            string expectedToken = $"sha256={Convert.ToBase64String(computedHash)}";


            string actuallJson = await result.Response.Content.ReadAsStringAsync();
            JObject actuallJsonObject = JObject.Parse(actuallJson);
            string actuallToken = actuallJsonObject["response_token"].ToString();

            Assert.AreEqual(expectedToken, actuallToken);
            Assert.AreEqual(result.RequestMessage, crcRequestMessage);

        }


        [TestMethod]
        public async Task InterceptIncomingMessage_UnhandledTest()
        {
            string consumerKey = DateTime.Now.Ticks.ToString();
            HttpRequestMessage emptyRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"http://localhost");

            WebhookInterceptor interceptor = new WebhookInterceptor(consumerKey);

            InterceptionResult result = await interceptor.InterceptIncomingRequest(emptyRequestMessage, null);

            Assert.IsFalse(result.IsHandled);
            Assert.AreEqual(result.RequestMessage, emptyRequestMessage);
        }

    }
}
