using ESC_training.Core;

namespace ESC_training.Systems
{
    internal abstract class System
    {
        internal HashSet<Entity> entities;

        public System()
        {
            entities = new HashSet<Entity>();
        }
        public abstract void Update(float dt);
    }
}
