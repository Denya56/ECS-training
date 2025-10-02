namespace ESC_training.Core
{
    internal struct OnEntityDeletedEvent
    {
        public Entity Entity { get; }
        public OnEntityDeletedEvent(Entity entity)
        {
            Entity = entity;
        }
    }
}
