using ESC_training.Entities;

namespace ESC_training.Core
{
    internal class SystemManager
    {
        private Dictionary<Type, Systems.System> _systems;
        private Dictionary<Type, Signature> _signatures;

        public SystemManager() 
        {
            _systems = new Dictionary<Type, Systems.System>();
            _signatures = new Dictionary<Type, Signature>();
        }
        public T RegisterSystem<T>() where T : Systems.System, new()
        {
            Type systemType = typeof(T);

            if (_systems.ContainsKey(systemType))
            {
                throw new InvalidOperationException("Registering system more than once.");
            }

            var system = new T();
            _systems.Add(systemType, system);
            return system;
        }
        public void SetSignature<T>(Signature signature)
        {
            Type systemType = typeof(T);

            if (!_systems.ContainsKey(systemType))
            {
                throw new InvalidOperationException("System used before registered.");
            }

            _signatures[systemType] = signature;
        }
        public void EntityDestroyed(Entity entity)
        {
            foreach (var system in _systems.Values)
            {
                system.entities.Remove(entity);
            }
        }
        public void EntitySignatureChanged(Entity entity, Signature entitySignature)
        {
            foreach (var pair in _systems)
            {
                var type = pair.Key;
                var system = pair.Value;
                if (!_signatures.TryGetValue(type, out var systemSignature))
                    throw new InvalidOperationException($"Signature for system {type.Name} not set before updating entity {entity}.");

                if (entitySignature.HasComponents(systemSignature))
                {
                    system.entities.Add(entity);
                }
                else
                {
                    system.entities.Remove(entity);
                }
            }
        }
    }
}
