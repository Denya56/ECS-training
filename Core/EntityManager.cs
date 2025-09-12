using ESC_training.Entities;
using static ESC_training.Config;

namespace ESC_training.Core
{
    internal class EntityManager
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
            // add custom exceptions
            if(_livingEntityCount >= MAX_ENTITIES)
            {
                throw new InvalidOperationException("Too many entities in existence.");
            }

            Entity newEntity = AvailableEntities.Dequeue();
            ++_livingEntityCount;
            return newEntity;
        }

        public void DestroyEntity(Entity entity)
        {
            // add custom exceptions
            if (entity.Id >= MAX_ENTITIES)
            {
                throw new ArgumentOutOfRangeException("Entity out of range.");
            }

            Signatures[entity.Id].Reset();
            AvailableEntities.Enqueue(entity);
            --_livingEntityCount;
        }

        public void SetSignature(Entity entity, Signature signature)
        {
            // add custom exceptions
            if (entity.Id >= MAX_ENTITIES)
            {
                throw new ArgumentOutOfRangeException("Entity out of range.");
            }
            Signatures[entity.Id] = signature;
        }

        public Signature GetSignature(Entity entity)
        {
            // add custom exceptions
            if (entity.Id >= MAX_ENTITIES)
            {
                throw new ArgumentOutOfRangeException("Entity out of range.");
            }
            return Signatures[entity.Id];
        }
    }
}
