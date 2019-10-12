using Prism.Events;

namespace Infrastructure.Events
{
    public class AfterUserLoggedoutEvent : PubSubEvent<UserAuthorizationEventArgs>
    {
    }
}
