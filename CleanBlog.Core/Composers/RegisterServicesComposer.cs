using CleanBlog.Core.Services;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace CleanBlog.Core.Composers
{
    public class RegisterServicesComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            // https://our.umbraco.com/documentation/implementation/services/
            // if your service makes use of the current UmbracoContext, eg AssignedContentItem - register the service in Request scope
            // composition.Register<ISiteService, SiteService>(Lifetime.Request);
            // if not then it is better to register in 'Singleton' Scope
            composition.Register<ISmtpService, SmtpService>(Lifetime.Singleton);
        }
    }
}
