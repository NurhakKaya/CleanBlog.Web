using SlackBotMessages;
using SlackBotMessages.Models;
using System;
using System.Collections.Generic;
using System.Web.Configuration;
using Umbraco.Core.Composing;

namespace CleanBlog.Core.Composers
{
    public class ApplicationComposer : ComponentComposer<ApplicationComponent>, IUserComposer
    {

    }

    public class ApplicationComponent : IComponent
    {
        public void Initialize()
        {
            try
            {
                // A simple example of a message which looks like it has been send by an alien
                var client = new SbmClient(WebConfigurationManager.AppSettings["SlackBotMessagesWebHookUrl"]);
                
                var message = new Message
                {
                    Username = "Website Robot",
                    //Username = "Alien",
                    //Text = "Hello from an Alien",
                    //IconEmoji = Emoji.Alien
                    IconEmoji = ":robot_face:",
                    Attachments = new List<Attachment>()
                    {
                        new Attachment()
                        {
                            Fallback = "Clean Blog Website Started",
                            Color = "good",
                            Fields = new List<Field>()
                            {
                                new Field()
                                {
                                    Value = "Clean Blog Website Started"
                                }
                            }
                        }
                    }
                };

                client.Send(message);
                // or send it fully async like this:
                // await client.SendAsync(message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Current.Logger.Error(typeof(ApplicationComponent), ex, "Unable to send slack message to notify site starting up.");
            }
        }

        public void Terminate()
        {

        }
    }
}
