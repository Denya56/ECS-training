namespace Recs.Core.Events
{
    internal struct OnEntitySignatureChangedEvent
    {
        public Entity Entity { get; }
        public Signature Signature { get; }

        public OnEntitySignatureChangedEvent(Entity entity, Signature signature)
        {
            Entity = entity;
            Signature = signature;
        }
    }
}
