using ESC_training.Entities;
using System.Collections;

namespace ESC_training.Systems
{
    internal class System
    {
        public HashSet<Entity> entities;

        public System()
        {
            entities = new HashSet<Entity>();
        }
    }
}
