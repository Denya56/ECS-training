namespace ESC_training.Core
{
    internal interface IObserver<TEvent>
    {
        void Update(Entity entity);
    }
}
