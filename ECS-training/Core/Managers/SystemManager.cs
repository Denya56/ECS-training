using ECS_training.Core.Events;
using ECS_training.Exceptions;
using ECS_training.Systems;
using System.Reflection;

namespace ECS_training.Core.Managers
{
    internal class SystemManager
    {
        private readonly EventManager _eventManager;

        private Dictionary<Type, Systems.ECSSystem> _systems;
        private Dictionary<Type, Signature> _signatures;

        public SystemManager(EventManager eventManager) 
        {
            _systems = new Dictionary<Type, Systems.ECSSystem>();
            _signatures = new Dictionary<Type, Signature>();

            _eventManager = eventManager;

            _eventManager.Subscribe<OnEntityDeletedEvent>(HandleEntityDeleted);
            _eventManager.Subscribe<OnEntitySignatureChangedEvent>(HandleEntitySignatureChanged);
        }
        public T RegisterSystem<T>() where T : Systems.ECSSystem, new()
        {
            Type systemType = typeof(T);

            if (_systems.ContainsKey(systemType))
            {
                throw new SystemAlreadyRegisteredException(systemType);
            }

            var system = new T();
            _systems.Add(systemType, system);
            SetSignature<T>();
            return system;
        }
        private void SetSignature<T>()
        {
            Type systemType = typeof(T);

            if (!_systems.ContainsKey(systemType))
            {
                throw new SystemNotRegisteredException(systemType);
            }

            _signatures[systemType] = ExtractSignature(systemType);
        }
        public void SetSignature<T>(Signature signature)
        {
            Type systemType = typeof(T);

            if (!_systems.ContainsKey(systemType))
            {
                throw new SystemNotRegisteredException(systemType);
            }

            _signatures[systemType] = signature;
        }
        private void EntityDestroyed(Entity entity)
        {
            foreach (var system in _systems.Values)
            {
                system.entities.Remove(entity);
            }
        }
        private void EntitySignatureChanged(Entity entity, Signature entitySignature)
        {
            foreach (var pair in _systems)
            {
                var type = pair.Key;
                var system = pair.Value;
                // can remove exception after safer way to set system signatures is implemented
                // now will throw if system signature not set right after registering it
                if (!_signatures.TryGetValue(type, out var systemSignature))
                    throw new InvalidOperationException($"Signature for system {type.Name} not set before updating entity {entity}.");

                if (entitySignature.HasComponents(systemSignature))
                {
                    system.AddEntity(entity);
                }
                else
                {
                    system.RemoveEntity(entity);
                }
            }
        }
        private Signature ExtractSignature(Type systemType)
        {
            Signature signature = new Signature();

            var fields = systemType.GetFields(BindingFlags.Public);

            foreach (var field in fields)
            {
                if (field.GetCustomAttribute<RequireComponentAttribute>() != null)
                {
                    Type componentType = field.FieldType;

                    var componentId = Coordinator.Instance.GetComponentType(componentType);
                    signature.AddComponent(componentId);
                }
            }

            return signature;
        }
        public void HandleEntityDeleted(OnEntityDeletedEvent e)
        {
            EntityDestroyed(e.Entity);
        }
        public void HandleEntitySignatureChanged(OnEntitySignatureChangedEvent e)
        {
            EntitySignatureChanged(e.Entity, e.Signature);
        }
    }
}
