namespace ECS_training.Core.Managers
{
    internal class EventManager
    {
        private readonly Dictionary<Type, List<Delegate>> _subscribers = new();
        public void Subscribe<TEvent>(Action<TEvent> listener)
        {
            var eventType = typeof(TEvent);

            if (!_subscribers.TryGetValue(eventType, out var list))
            {
                list = new List<Delegate>();
                _subscribers[eventType] = list;
            }

            list.Add(listener);
        }

        public void Unsubscribe<TEvent>(Action<TEvent> listener)
        {
            var eventType = typeof(TEvent);

            if (_subscribers.TryGetValue(eventType, out var list))
            {
                list.Remove(listener);
                if (list.Count == 0)
                    _subscribers.Remove(eventType);
            }            
        }
        public void Notify<TEvent>(TEvent @event)
        {
            var eventType = typeof(TEvent);

            if (_subscribers.TryGetValue(eventType, out var list))
            {
                foreach (var subscriber in list.ToArray())
                {
                    if (subscriber is Action<TEvent> action)
                    {
                        action(@event);
                    }
                }
            }
        }
    }
}
