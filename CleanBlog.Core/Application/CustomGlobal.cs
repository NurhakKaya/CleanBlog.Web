using System;
using System.Collections.Generic;
using System.Web;
using Umbraco.Core.Composing;
using Umbraco.Web;
using SlackBotMessages;
using SlackBotMessages.Models;
using System.Web.Configuration;

namespace CleanBlog.Core.Application
{
    public class CustomGlobal : UmbracoApplication
    {
        protected override void OnApplicationError(object sender, EventArgs evargs)
        {
            var request = HttpContext.Current.Request;
            var error = HttpContext.Current.Server.GetLastError();

            try
            {
                var url = request.Url.GetLeftPart(UriPartial.Authority) + request.Url;
                var client = new SbmClient(WebConfigurationManager.AppSettings["SlackBotMessagesWebHookUrl"]);

                var message = new Message
                {
                    Username = "Website Robot",
                    //Username = "Alien",
                    //Text = "Hello from an Alien",
                    IconEmoji = ":robot_face:",
                    Attachments = new List<Attachment>()
                    {
                        new Attachment()
                        {
                            Fallback=error.Message,
                            Color="danger",
                            Fields=new List<Field>()
                            {
                                new Field()
                                {
                                    Title=Emoji.Warning+" Error",
                                    Value=error.Message
                                },
                                new Field()
                                {
                                    Title="Stack Trace",
                                    Value=error.StackTrace
                                },
                                new Field()
                                {
                                    Title="Url",
                                    Value=url
                                }
                            }
                        }
                    }
                    //IconEmoji = Emoji.Alien
                };

                client.Send(message);
            }
            catch (Exception ex)
            {
                // If the slack messaging fails, we are logging it here!
                Current.Logger.Error(typeof(CustomGlobal), ex, "Unable to send slack message to notify unhandled exception.");
            }

            base.OnApplicationError(sender, evargs);
        }
    }
}
