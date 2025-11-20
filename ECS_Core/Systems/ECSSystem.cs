using Recs.Core;

namespace Recs.Systems
{
    public abstract class ECSSystem
    {
        protected internal readonly HashSet<Entity> entities;
        public IReadOnlyCollection<Entity> Entities => entities;

        public ECSSystem()
        {
            entities = new HashSet<Entity>();
        }
        public void Update(float dt)
        {
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
