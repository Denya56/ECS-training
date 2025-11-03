using ECS_training.Core;

namespace ECS_training.Systems
{
    public abstract class System
    {
        internal readonly HashSet<Entity> entities;
        public IReadOnlyCollection<Entity> Entities => entities;
        public Coordinator Coordinator { get; set; }

        public System()
        {
            entities = new HashSet<Entity>();
        }
        public void Update(float dt)
        {
            if (Coordinator == null)
                throw new InvalidOperationException("Coordinator must be assigned before calling Update.");

            UpdateInternal(dt);
        }

        protected abstract void UpdateInternal(float dt);
    }
}
