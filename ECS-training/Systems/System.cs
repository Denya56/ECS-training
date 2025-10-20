using ESC_training.Core;

namespace ESC_training.Systems
{
    public abstract class System
    {
        internal HashSet<Entity> entities;
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
