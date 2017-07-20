# Tweety
Tweety is a server-side, .NET Standard Library to help managing Twitter Webhook APIs,
Currently, *Account Activity API* is the only supported API, it provides **Direct Messages** webhook API.
Read more about it: https://dev.twitter.com/webhooks.


# Before you start
As of July, 20th 2017 - You have to request access to the webhook APIs from Twitter using this [Form](https://gnipinc.formstack.com/forms/account_activity_api_configuration_request_form), the process will take few day.
Anyways, check this page before you start: https://dev.twitter.com/webhooks/account-activity.


# How it works
You **Register a webhook** by url, that's your server, to **Handle incoming messages**.
Then you **Subscribe** to a webhook by `Webhook Id`, then your server will recieve **Events** for any incoming OR outgoing direct message for/ from the signed in/ subscribed user (in most cases, the user is a bot). 

# Documentaion

  - **WebhooksManager**: Register a webhook, Get a list of registered webhooks and Unregister a webhook.
  
  - **SubscriptionsManager**: Subscribe to a webhook, Unsubscribe, or Check if the user is already subscribed or not.
  
  - **WebhookInterceptor**: Handles the *Challenge Response Check (CRC)* [ref](https://dev.twitter.com/webhooks/securing#required-challenge-response-check)  requests from Twitter and invoking `Action<DirectMessageEvent>` if received a direct message webhook event after chcecking if the event is really from Twitter or not [ref](https://dev.twitter.com/webhooks/securing#validating-the-signature-header).. will return a `Tupel: (bool handled, HttpResponseMessage response)`, if handled, then return the response to the client, otherwise, you've to handle the request. 
  
  - **TweetyAuthContext**: Containes the needed information to perform authorizerd Twitter requests, i.e. Consumer Key/ Secret, Access Token/ Secret.
  
  # Sample: Registerig a Webhook:
  ```csharp
  TweetyAuthContext authContext = new Tweety.Authentication.TweetyAuthContext()
  {
       AccessSecret = ACCESS_TOKEN_SECRET,
       AccessToken = ACCESS_TOKEN,
       ConsumerKey = CONSUMER_KEY,
       ConsumerSecret = CONSUMER_SECRET
  };
            
    WebhooksManager webhooksManager = new WebhooksManager(authContext);
    Result<WebhookRegistration> result = webhooksManager.RegisterWebhook("https://something.com/Twitbot");
    
    if (result.Success)
    {
          Console.WriteLine($Webhook Id {result.Data.Id});
    }
    else
    {
          Console.WriteLine(result.Error.ToString());
    }


  ```
