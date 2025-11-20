using Recs.Core.Events;
using Recs.Exceptions;
using static Recs.EcsConfig;

namespace Recs.Core.Managers
{
    internal class ComponentManager   
    {
        private readonly EventManager _eventManager;

        private Dictionary<Type, ComponentType> _componentTypes;
        private Dictionary<Type, IComponentArray> _componentArrays;        
        private ComponentType _nextComponentType;

        public ComponentManager(EventManager eventManager)
        {
            _componentTypes = new Dictionary<Type, ComponentType>();
            _componentArrays = new Dictionary<Type, IComponentArray>();
            _nextComponentType = 0;

            _eventManager = eventManager;
            _eventManager.Subscribe<OnEntityDeletedEvent>(HandleEntityDeleted);
        }
        private ComponentArray<T> GetComponentArray<T>() where T : struct, IComponentData
        {
            Type componentType = typeof(T);

            if (!_componentArrays.ContainsKey(componentType))
            {
                throw new ComponentNotRegisteredException(componentType);
            }

            return (ComponentArray<T>)_componentArrays[componentType];
        }
        public void RegisterComponent<T>() where T : struct, IComponentData
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

            if (!_componentTypes.TryGetValue(componentType, out byte id))
                throw new ComponentNotRegisteredException(componentType);
            return id;
        }
        public ComponentType GetComponentType(Type componentType)
        {
            if (!_componentTypes.TryGetValue(componentType, out var id))
                throw new ComponentNotRegisteredException(componentType);

            return id;
        }
        public void AddComponent<T>(Entity entity, T component) where T : struct, IComponentData
        {
            GetComponentArray<T>().InsertData(entity, component);
        }
        public void RemoveComponent<T>(Entity entity) where T : struct, IComponentData
        {
            GetComponentArray<T>().RemoveData(entity); 
        }
        public ref T GetComponent<T>(Entity entity) where T : struct, IComponentData
        {
            return ref GetComponentArray<T>().GetData(entity);
        }
        public bool HasComponent<T>(Entity entity) where T : struct, IComponentData
        {
            return GetComponentArray<T>().HasData(entity);
        }
        // notify each component array that an entity has been destroyed
        // if it has a component for that entity, it will remove it
        private void EntityDestroyed(Entity entity)
        {
            foreach (var pair in _componentArrays)
            {
                var componentArray = pair.Value;
                componentArray.EntityDestroyed(entity);
            }
        }

        public void HandleEntityDeleted(OnEntityDeletedEvent e)
        {
            EntityDestroyed(e.Entity);
        }
    }
}
