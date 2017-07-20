using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Tweety.Extensions;

namespace Tweety.Authentication
{

    /// <summary>
    /// Twitter Authentication helper.
    /// </summary>
    public class AuthHeaderBuilder
    {

        /// <summary>
        /// Returns a ready 'OAuth ..' prefixed header to set in any call to Twitter API.
        /// </summary>
        /// <param name="authContext">Twitter app auth context.</param>
        /// <param name="method">The Request Http method.</param>
        /// <param name="requestUrl">The Request uri along with any query parameter.</param>
        /// <returns></returns>
        public static string Build(TweetyAuthContext authContext, HttpMethod method, string requestUrl)
        {

            if (!Uri.TryCreate(requestUrl, UriKind.RelativeOrAbsolute, out Uri resourceUri))
            {
                throw new Exception("Invalid Resource Url format.");
            }

            if (authContext == null || !authContext.IsValid)
            {
                throw new Exception("Invalid Tweety Auth Context.");
            }

            string oauthVersion = "1.0";
            string oauthSignatureMethod = "HMAC-SHA1";

            // It could be any random string..
            string oauthNonce = DateTime.Now.Ticks.ToString();

            double epochTimeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

            string oauthTimestamp = Convert.ToInt64(epochTimeStamp).ToString();

            Dictionary<string, string> signatureParams = new Dictionary<string, string>();
            signatureParams.Add("oauth_consumer_key", authContext.ConsumerKey);
            signatureParams.Add("oauth_nonce", oauthNonce);
            signatureParams.Add("oauth_signature_method", oauthSignatureMethod);
            signatureParams.Add("oauth_timestamp", oauthTimestamp);
            signatureParams.Add("oauth_token", authContext.AccessToken);
            signatureParams.Add("oauth_version", oauthVersion);

            Dictionary<string, string> qParams = resourceUri.GetParams();
            foreach (KeyValuePair<string, string> qp in qParams)
            {
                signatureParams.Add(qp.Key, qp.Value);
            }

            string baseString = string.Join("&", signatureParams.OrderBy(kpv => kpv.Key).Select(kpv => $"{kpv.Key}={kpv.Value}"));

            string resourceUrl = requestUrl.Contains("?") ? requestUrl.Substring(0, requestUrl.IndexOf("?")) : requestUrl;
            baseString = string.Concat(method.Method.ToUpper(), "&", Uri.EscapeDataString(resourceUrl), "&", Uri.EscapeDataString(baseString));

            string oauthSignatureKey = string.Concat(Uri.EscapeDataString(authContext.ConsumerSecret), "&", Uri.EscapeDataString(authContext.AccessSecret));

            string oauthSignature;
            using (HMACSHA1 hasher = new HMACSHA1(Encoding.ASCII.GetBytes(oauthSignatureKey)))
            {
                oauthSignature = Convert.ToBase64String(hasher.ComputeHash(Encoding.ASCII.GetBytes(baseString)));
            }

            string headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                               "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                               "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                               "oauth_version=\"{6}\"";

            string authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(oauthNonce),
                                    Uri.EscapeDataString(oauthSignatureMethod),
                                    Uri.EscapeDataString(oauthTimestamp),
                                    Uri.EscapeDataString(authContext.ConsumerKey),
                                    Uri.EscapeDataString(authContext.AccessToken),
                                    Uri.EscapeDataString(oauthSignature),
                                    Uri.EscapeDataString(oauthVersion));

            return authHeader;
        }

    }
}
