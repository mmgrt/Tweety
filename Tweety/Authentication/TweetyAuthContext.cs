namespace Tweety.Authentication
{

    /// <summary>
    /// Required for any action, this represents the current user context.
    /// </summary>
    public class TweetyAuthContext
    {
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessSecret { get; set; }


        /// <summary>
        /// Read-only property to check if this auth data is valid or not.
        /// </summary>
        public bool IsValid => !string.IsNullOrEmpty(ConsumerKey) && !string.IsNullOrEmpty(ConsumerSecret) && !string.IsNullOrEmpty(AccessToken) && !string.IsNullOrEmpty(AccessSecret);
    }

}
