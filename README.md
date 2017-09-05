# Tweety
Tweety is a server-side, .NET Standard Library to help managing Twitter Webhook APIs,
Currently, *Account Activity API* is the only supported API, it provides **Direct Messages** webhook API.
Read more about it: https://dev.twitter.com/webhooks.

# NuGet
[![NuGet](https://img.shields.io/badge/nuget-Tweety-blue.svg)](https://nuget.org/packages/Tweety/)


# Before you start
As of July, 20th 2017 - You have to request access to the webhook APIs from Twitter using this [Form](https://gnipinc.formstack.com/forms/account_activity_api_configuration_request_form), the process will take few day.
Anyways, check this page before you start: https://dev.twitter.com/webhooks/account-activity.


# How it works
You **Register a webhook** by url, that's your server, to **Handle incoming messages**.
Then you **Subscribe** to a webhook by `Webhook Id`, then your server will recieve **Events** for any incoming OR outgoing direct message for/ from the signed in/ subscribed user (in most cases, the user is a bot). 


# What for?
Well, I'd say mainly for Bots, the **WebhooksManager** and the **SubscriptionsManager** will be used once, mostly to register and subscribe your bot twitter account to your server, so you can get any DM as an event and handle it.


# Tweety APIs

  - **WebhooksManager**: Register a webhook, Get a list of registered webhooks and Unregister a webhook.
  
  - **SubscriptionsManager**: Subscribe to a webhook, Unsubscribe, or Check if the user is already subscribed or not.
  
  - **WebhookInterceptor**: Handles the *Challenge Response Check (CRC)* [ref](https://dev.twitter.com/webhooks/securing#required-challenge-response-check)  requests from Twitter and invoking `Action<DirectMessageEvent>` if received a direct message webhook event after chcecking if the event is really from Twitter or not [ref](https://dev.twitter.com/webhooks/securing#validating-the-signature-header).. will return a `InterceptionResult`, if IsHandled is true, then return the response to the client, otherwise, you've to handle the request. 
  
  - **TweetyAuthContext**: Containes the needed information to perform authorizerd Twitter requests, i.e. Consumer Key/ Secret, Access Token/ Secret.
  
  - **DirectMessageSender**: Provides an easy way to send Direct Messages to a Twitter user by screen name.
  
# Example: Registerig a Webhook:
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
             Console.WriteLine($"Webhook Id {result.Data.Id}");
       }
       else
       {
             Console.WriteLine($"Failed to register webhook, Error: {result.Error.ToString()}");
       }

  ```
# Example: Subscribing to a Webhook:

  ```csharp

       SubscriptionsManager subManager = new SubscriptionsManager(authContext);
       Result<bool> result = await subManager.Subscribe(webhookId);
     
       if(result.Success && result.Data)
       {
            Console.WriteLine($"Successfully subscribed to {webhookId}");
       }
       else
       {
            Console.WriteLine($"Failed to subscribe to a webhook, Error: {result.Error?.ToString() ?? "Error isn't available"}");
       }
  ```

# Example: Intercepting server request:

I've used Webhook Azure Function to test it, follow [Setup Azure Function](https://github.com/mmgrt/Tweety#setup-azure-function) below if you're interested.

 ```csharp
       WebhookInterceptor interceptor = new WebhookInterceptor(CONSUMER_SECRET);
       InterceptionResult result = await interceptor.InterceptIncomingRequest(requestMessage, onMessage);
           
       if (result.IsHandled)
       {
            return result.Response;
       }
       else
       {
            //handle req
       }
       //..
       
       private void onMessage(DirectMessageEvent message)
       {
            Console.WriteLine($"Recieved {message.MessageText} from {message.Sender.Name}.");
       }
 ```

# Setup Azure Function
   - Go to [Azure Portal](https://portal.azure.com).
   - Create Azure Function App.
   - Create a `Generic Webhook` Function.
   - Import Tweety.dll:
      - Clone this repo.
      - Compile using Visual Studio 2017.
      - Create a 'bin' folder in your function folder (same level as `run.csx`).
      - Upload Tweety.dll to the bin folder.
      - Insert `#r "Tweety.dll"` at the head of the `run.csx` file (before the `using` statements).
   - In the `Run` method, intercept incoming requests, see the sample: [Intercepting server request](https://github.com/mmgrt/Tweety#sample-intercepting-server-request).
   
   
# Todo
- [ ] Provide better documentation.
- [ ] Do the todos in the code :).


# Find me

- Twitter: [@mmg_rt](https://www.twitter.com/mmg_rt)

- My blog: [Devread.net](http://devread.net)

