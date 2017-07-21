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
        /// Check if the current auth context is valid or not.
        /// Null or Empty value for one on the auth properties will return false.
        /// </summary>
        public bool IsValid => !string.IsNullOrEmpty(ConsumerKey) && !string.IsNullOrEmpty(ConsumerSecret) && !string.IsNullOrEmpty(AccessToken) && !string.IsNullOrEmpty(AccessSecret);
    }

}
