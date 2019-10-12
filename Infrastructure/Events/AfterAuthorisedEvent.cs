using Prism.Events;

namespace Infrastructure.Events
{
    public class AfterUserLoggedinEvent : PubSubEvent<UserAuthorizationEventArgs>
    {
    }
}
