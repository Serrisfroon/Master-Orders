using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Auth;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace TwitterLibrary
{
    public static class TweetVideo
    {
        public static TwitterClient userClient;
        public static TwitterClient appClient;
        public static IAuthenticationRequest authenticationRequest;



        async public static Task PromptPinCode()
        {
            // Create a client for your app
            appClient = new TwitterClient(TwitterKeys.APIKey, TwitterKeys.APISecret);

            // Start the authentication process
            authenticationRequest = appClient.Auth.RequestAuthenticationUrlAsync().Result;

            // Go to the URL so that Twitter authenticates the user and gives him a PIN code.
            Process.Start(new ProcessStartInfo(authenticationRequest.AuthorizationURL)
            {
                UseShellExecute = true
            });

        }
        async public static Task AuthenticateUser(string pinCode)
        {
            // With this pin code it is now possible to get the credentials back from Twitter
            ITwitterCredentials twitterCredentials = appClient.Auth.RequestCredentialsFromVerifierCodeAsync(pinCode, authenticationRequest).Result;
            // You can now save those credentials or use them as followed
            userClient = new TwitterClient(twitterCredentials);
            IAuthenticatedUser userAuth = userClient.Users.GetAuthenticatedUserAsync().Result;

            List<string> tokenStuff = new List<string>() { userClient.Credentials.AccessToken, userClient.Credentials.AccessTokenSecret };
            File.WriteAllLines("TwitterUserAuth.txt", tokenStuff);


            Console.WriteLine("Congratulation you have authenticated the user: " + userAuth);
            Console.ReadLine();

        }

        async public static Task SendTweet(string message)
        {
            var tweet = await userClient.Tweets.PublishTweetAsync(message);
            //userAuth.
        }

        async public static Task SendVideo(string videoFile, string message)
        {

            var videoBinary = File.ReadAllBytes(videoFile);
            var uploadedVideo = await userClient.Upload.UploadTweetVideoAsync(videoBinary);

            // IMPORTANT: you need to wait for Twitter to process the video
            await userClient.Upload.WaitForMediaProcessingToGetAllMetadataAsync(uploadedVideo);

            Thread.Sleep(1000);
            var tweetWithVideo = await userClient.Tweets.PublishTweetAsync(new PublishTweetParameters(message)
            {
                Medias = { uploadedVideo }
            });
        }
        public static string GetUser()
        {
            var authenticatedUser = userClient.Users.GetAuthenticatedUserAsync().Result;
            return authenticatedUser.ToString();
        }




    }
}
