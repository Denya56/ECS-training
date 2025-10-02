using ESC_training.Exceptions;
using static ESC_training.Config;

namespace ESC_training.Core
{
    internal class EntityManager : IObserver
    {
        public Queue<Entity> AvailableEntities { get; set; }
        private int _livingEntityCount;
        private Signature[] Signatures = new Signature[MAX_ENTITIES];

        public EntityManager()
        {
            AvailableEntities = new Queue<Entity>();
            _livingEntityCount = 0;

            for (int i = 0; i < MAX_ENTITIES; ++i)
            {
                AvailableEntities.Enqueue(new Entity(i));
            }
        }
        public Entity CreateEntity()
        {
            if (_livingEntityCount >= MAX_ENTITIES)
            {
                throw new EntityLimitExceededException();
            }

            Entity newEntity = AvailableEntities.Dequeue();
            ++_livingEntityCount;
            return newEntity;
        }

        public void DestroyEntity(Entity entity)
        {
            if (entity.Id >= MAX_ENTITIES)
            {
                throw new EntityOutOfRangeException(entity.Id);
            }

            Signatures[entity.Id].Reset();
            AvailableEntities.Enqueue(entity);
            --_livingEntityCount;
        }

        public void SetSignature(Entity entity, Signature signature)
        {
            if (entity.Id >= MAX_ENTITIES)
            {
                throw new EntityOutOfRangeException(entity.Id);
            }
            Signatures[entity.Id] = signature;
        }

        public Signature GetSignature(Entity entity)
        {
            if (entity.Id >= MAX_ENTITIES)
            {
                throw new EntityOutOfRangeException(entity.Id);
            }
            return Signatures[entity.Id];
        }

        public void Update(Entity entity)
        {
            DestroyEntity(entity);
        }
    }
}
