using ECS_training.Core;

namespace ECS_training.Systems
{
    public abstract class ECSSystem
    {
        internal readonly HashSet<Entity> entities;
        public IReadOnlyCollection<Entity> Entities => entities;
        //public Coordinator Coordinator { get; set; }

        public ECSSystem()
        {
            entities = new HashSet<Entity>();
        }
        public void Update(float dt)
        {
            /*if (Coordinator == null)
                throw new InvalidOperationException("Coordinator must be assigned before calling Update.");*/

            UpdateInternal(dt);
        }

        internal void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        internal void RemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        protected abstract void UpdateInternal(float dt);
    }
}
