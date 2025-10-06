namespace ESC_training.Core
{
    internal interface ISubject
    {
        void Attach<TEvent>(IObserver<TEvent> observer);
        void Detach<TEvent>(IObserver<TEvent> observer);
        void Notify<TEvent>(TEvent @event);
    }
}
