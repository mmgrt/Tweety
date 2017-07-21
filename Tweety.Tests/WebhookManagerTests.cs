using System;
using Tweety.Authentication;
using static Tweety.Tests.TestsConstants;
using Tweety.Webhooks;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tweety.Tests
{
    [TestClass]
    public class WebhookManagerTests
    {
        [TestMethod]
        [Priority(0)]
        public async Task GetRegisteredWebhookTest()
        {

            WebhooksManager webhookManager = new WebhooksManager(AuthContext);

            var result = await webhookManager.GetRegisteredWebhooks();

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Data.Count == 0);

        }

        [TestMethod]
        [Priority(1)]
        public async Task RegisterWebhookTest()
        {
            WebhooksManager webhookManager = new WebhooksManager(AuthContext);

            var result = await webhookManager.RegisterWebhook(WebhookUrl);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Data.IsValid);
            Assert.IsTrue(result.Data.RegisteredUrl == WebhookUrl);

            RegsiteredWebhookId = result.Data.Id;
        }



        [TestMethod]
        [Priority(2)]
        public async Task UnregisterWebhookTest()
        {
            WebhooksManager webhookManager = new WebhooksManager(AuthContext);

            var result = await webhookManager.UnregisterWebhook(RegsiteredWebhookId);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Data);

        }

    }
}
