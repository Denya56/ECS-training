using ECS_training.Core.Events;
using ECS_training.Exceptions;
using static ECS_training.EcsConfig;

namespace ECS_training.Core.Managers
{
    internal class EntityManager
    {
        private readonly EventManager _eventManager;
        public Queue<Entity> AvailableEntities { get; set; }
        private int _livingEntityCount;
        private Signature[] Signatures = new Signature[MAX_ENTITIES];
        private HashSet<int> _aliveEntities = new HashSet<int>();

        public EntityManager(EventManager eventManager)
        {
            AvailableEntities = new Queue<Entity>();
            _livingEntityCount = 0;

            for (int i = 0; i < MAX_ENTITIES; ++i)
                AvailableEntities.Enqueue(new Entity(i));

            _eventManager = eventManager;

            _eventManager.Subscribe<OnEntitySignatureChangedEvent>(HandleEntitySignatureChanged);
        }
        public Entity CreateEntity()
        {
            if (_livingEntityCount >= MAX_ENTITIES)
                throw new EntityLimitExceededException();

            Entity newEntity = AvailableEntities.Dequeue();
            _aliveEntities.Add(newEntity.Id);
            ++_livingEntityCount;
            return newEntity;
        }
        public void DestroyEntity(Entity entity)
        {
            if (entity.Id >= MAX_ENTITIES)
                throw new EntityOutOfRangeException(entity.Id);

            if (!_aliveEntities.Contains(entity.Id))
                throw new EntityNotAliveException(entity.Id);


            Signatures[entity.Id].Reset();
            _aliveEntities.Remove(entity.Id);
            AvailableEntities.Enqueue(entity);
            --_livingEntityCount;
        }
        private void SetSignature(Entity entity, Signature signature)
        {
            if (entity.Id >= MAX_ENTITIES)
                throw new EntityOutOfRangeException(entity.Id);

            if (!_aliveEntities.Contains(entity.Id))
                throw new EntityNotAliveException(entity.Id);

            Signatures[entity.Id] = signature;
        }
        // Return a readonly reference to avoid modifying the signature directly
        public ref readonly Signature GetSignature(Entity entity)
        {
            if (entity.Id >= MAX_ENTITIES)
                throw new EntityOutOfRangeException(entity.Id);

            if (!_aliveEntities.Contains(entity.Id))
                throw new EntityNotAliveException(entity.Id);

            return ref Signatures[entity.Id];
        }

        public void HandleEntitySignatureChanged(OnEntitySignatureChangedEvent e) =>
            SetSignature(e.Entity, e.Signature);
    }
}
