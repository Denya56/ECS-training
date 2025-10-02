using ESC_training.Exceptions;
using static ESC_training.Config;

namespace ESC_training.Core
{
    internal class ComponentManager : IObserver<OnEntityDeletedEvent>
    {
        private Dictionary<Type, ComponentType> _componentTypes;
        private Dictionary<Type, IComponentArray> _componentArrays;        
        private ComponentType _nextComponentType;

        public ComponentManager()
        {
            _componentTypes = new Dictionary<Type, ComponentType>();
            _componentArrays = new Dictionary<Type, IComponentArray>();
            _nextComponentType = 0;
        }
        private ComponentArray<T> GetComponentArray<T>()
        {
            Type componentType = typeof(T);

            if (!_componentArrays.ContainsKey(componentType))
            {
                throw new ComponentNotRegisteredException(componentType);
            }

            return (ComponentArray<T>)_componentArrays[componentType];
        }
        public void RegisterComponent<T>()
        {
            Type componentType = typeof(T);

            if (_nextComponentType >= MAX_COMPONENTS)
                throw new ComponentLimitExceededException(MAX_COMPONENTS);

            if (_componentTypes.ContainsKey(componentType))
            {
                throw new ComponentAlreadyRegisteredException(componentType);
            }

            _componentTypes.Add(componentType, _nextComponentType);
            _componentArrays.Add(componentType, new ComponentArray<T>());

            ++_nextComponentType;
        }
        public ComponentType GetComponentType<T>()
        {
            Type componentType = typeof(T);

            if (!_componentTypes.ContainsKey(componentType))
            {
                throw new ComponentNotRegisteredException(componentType);
            }
            return _componentTypes[componentType];
        }
        public void AddComponent<T>(Entity entity, T component)
        {
            GetComponentArray<T>().InsertData(entity, component);
        }
        public void RemoveComponent<T>(Entity entity)
        {
            GetComponentArray<T>().RemoveData(entity);
        }
        public ref T GetComponent<T>(Entity entity)
        {
            return ref GetComponentArray<T>().GetData(entity);
        }
        public bool HasComponent<T>(Entity entity)
        {
            return GetComponentArray<T>().HasData(entity);
        }
        public void EntityDestroyed(Entity entity)
        {
            // notify each component array that an entity has been destroyed
            // if it has a component for that entity, it will remove it
            // replace with observer later
            foreach (var pair in _componentArrays)
            {
                var componentArray = pair.Value;
                componentArray.EntityDestroyed(entity);
            }
        }

        public void Update(OnEntityDeletedEvent @event)
        {
            EntityDestroyed(@event.Entity);
        }
    }
}
