namespace ESC_training.Core
{
    internal interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(Entity entity);
    }
}
