using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TwitchLib.Api;
using TwitchLib.Api.Core.Enums;
using TwitchLib.Api.Core;

namespace TwitchLibrary
{
    public class Authentication
    {
        private readonly HttpListener TwitchListener;
        public ApiSettings api;
        public bool _isTwitchAuthenticated = false;


        private const string ReturnUrl = "http://localhost:56207";

        public Authentication()
        {
            TwitchListener = new HttpListener();
            TwitchListener.Prefixes.Add(ReturnUrl + "/");
            api = new ApiSettings();
            api.ClientId = "al4m3812ef41zmx0536o4a9s5mlepr";
            api.AccessToken = "access_token"; // App Secret is not an Accesstoken
        }


        public async Task<string> GetOauthToken()
        {
            Console.WriteLine("Fetching Twitch Details");
            var sb = new StringBuilder();
            sb.Append("https://api.twitch.tv/kraken/oauth2/authorize");
            sb.Append("?response_type=token");
            sb.Append("&client_id=").Append(api.ClientId);
            sb.Append("&redirect_uri=").Append(ReturnUrl);
            sb.Append("&scope=clips:edit user_read");

            Uri uri = new Uri(sb.ToString());
            Process.Start(sb.ToString());


            var result =  GetAuthenticationValuesAsync()
                .Result;

            if (result != null)
            {
                _isTwitchAuthenticated = true;
                api.AccessToken = result.Token;
                api.Scopes = new List<AuthScopes>(){AuthScopes.Helix_Clips_Edit};
                return result.Scopes;
            }

            return "";

        }

        /// <summary>
        /// Get Twitch Authentication Async
        /// </summary>
        /// <returns></returns>
        public Task<TwitchAuthenticationModel> GetAuthenticationValuesAsync()
        {
            return Task.Run(() => GetAuthenticationValues());
        }

        /// <summary>
        /// Start the listener
        /// </summary>
        /// <returns>returns IsListening Value</returns>
        private bool StartListener()
        {
            try
            {
                TwitchListener.Start();
            }
            catch (HttpListenerException ex)
            {
                throw new Exception("Cant start listener for Twitch Authentication" + Environment.NewLine + ex);
            }

            return TwitchListener.IsListening;
        }

        /// <summary>
        /// Stop the listener
        /// </summary>
        private void StopListener()
        {
            TwitchListener.Stop();
        }

        /// <summary>
        /// Get Twitch Auths
        /// </summary>
        /// <returns></returns>
        public TwitchAuthenticationModel GetAuthenticationValues()
        {
            StartListener();

            TwitchAuthenticationModel Values = null;
            while (TwitchListener.IsListening)
            {
                var context = TwitchListener.GetContext();

                if (context.Request.QueryString.HasKeys())
                {
                    if (context.Request.RawUrl.Contains("access_token"))
                    {
                        Uri myUri = new Uri(context.Request.Url.OriginalString);
                        string scope = HttpUtility.ParseQueryString(myUri.Query).Get("scope");
                        string access_token = HttpUtility.ParseQueryString(myUri.Query).Get(0).Replace("access_token=", "");

                        if (!String.IsNullOrEmpty(scope) && !String.IsNullOrEmpty(access_token))
                        {
                            Values = GetModel(access_token, scope);
                        }
                    }
                }

                byte[] b = Encoding.UTF8.GetBytes(GetResponse());
                context.Response.StatusCode = 200;
                context.Response.KeepAlive = false;
                context.Response.ContentLength64 = b.Length;

                var output = context.Response.OutputStream;
                output.Write(b, 0, b.Length);
                context.Response.Close();

                if (Values != null)
                {
                    StopListener();
                    return Values;
                }
            }

            return null;
        }

        /// <summary>
        /// Creates the Response for TwitchOAuth
        /// </summary>
        /// <returns>Response</returns>
        private string GetResponse()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<html>");
            builder.Append(Environment.NewLine);
            builder.Append("<head>");
            builder.Append(Environment.NewLine);
            builder.Append("<title>Master Orders Twitch Oauth</title>");
            builder.Append(Environment.NewLine);
            builder.Append("<script language=\"JavaScript\">");
            builder.Append(Environment.NewLine);
            builder.Append("if(window.location.hash) {");
            builder.Append(Environment.NewLine);
            builder.Append("window.location.href = window.location.href.replace(\"/#\",\"?=\");");
            builder.Append(Environment.NewLine);
            builder.Append("}");
            builder.Append(Environment.NewLine);
            builder.Append("</script>");
            builder.Append(Environment.NewLine);
            builder.Append("</head>");
            builder.Append(Environment.NewLine);
            builder.Append("<body>You can close this tab</body>");
            builder.Append(Environment.NewLine);
            builder.Append("</html>");
            builder.Append("<script type="+'"'+"text/javascript" + '"' +"> setTimeout(" + '"' + "window.close();" + '"' +", 500);</script>");

            return builder.ToString();
        }

        /// <summary>
        /// Creates the Model to return
        /// </summary>
        /// <param name="token">Twitch Token</param>
        /// <param name="scopes">Twitch Scopes</param>
        /// <returns></returns>
        private TwitchAuthenticationModel GetModel(string token, string scopes)
        {
            return new TwitchAuthenticationModel
            {
                Token = token,
                Scopes = scopes
            };
        }
    }
}
