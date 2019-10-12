using Prism.Events;
using System;

namespace Infrastructure.Events
{
    public class AfterDetailDeletedEvent : PubSubEvent<AfterDetailDeletedEventArgs>
    {
    }

    public class AfterDetailDeletedEventArgs
    {
        public AfterDetailDeletedEventArgs()
        {
        }

        public Guid Id { get; set; }
        public string ViewModelName { get; set; }
    }
}