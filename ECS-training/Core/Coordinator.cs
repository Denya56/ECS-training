using ECS_training.Core.Events;
using ECS_training.Core.Managers;
using ECS_training.Exceptions;

namespace ECS_training.Core
{
    public sealed class Coordinator
    {
        private static Coordinator _instance;
        public static Coordinator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Coordinator();
                }
                return _instance;
            }
        }

        private readonly EventManager _eventManager;

        private readonly ComponentManager _componentManager;
        private readonly EntityManager _entityManager;
        private readonly SystemManager _systemManager;
        
        public Coordinator(int? maxComponents = null, int? maxEntities = null)
        {
            _eventManager = new EventManager();

            int componentsLimit = maxComponents ?? EcsConfig.MAX_COMPONENTS;
            int entitiesLimit = maxEntities ?? EcsConfig.MAX_ENTITIES;

            _componentManager = new ComponentManager(_eventManager);
            _entityManager = new EntityManager(_eventManager);
            _systemManager = new SystemManager(_eventManager);
        }

        #region Entity methods
        public Entity CreateEntity()
        {
            return _entityManager.CreateEntity();
        }
        public void DestroyEntity(Entity entity)
        {
            _entityManager.DestroyEntity(entity);

            // notify other managers
            _eventManager.Notify(new OnEntityDeletedEvent(entity));
        }

        public Signature GetEntitySignature(Entity entity)
        {
            return _entityManager.GetSignature(entity);
        }
        #endregion

        #region Component methods
        public void RegisterComponent<T>() where T : struct, IComponentData
        {
            _componentManager.RegisterComponent<T>();
        }
        public void AddComponent<T>(Entity entity, T component) where T : struct, IComponentData
        {
            _componentManager.AddComponent(entity, component);

            var signature = _entityManager.GetSignature(entity);
            signature.AddComponent(_componentManager.GetComponentType<T>());

            // notify other managers
            _eventManager.Notify(new OnEntitySignatureChangedEvent(entity, signature));
        }
        public void RemoveComponent<T>(Entity entity) where T : struct, IComponentData
        {
            _componentManager.RemoveComponent<T>(entity);

            var signature = _entityManager.GetSignature(entity);
            signature.RemoveComponent(_componentManager.GetComponentType<T>());

            _eventManager.Notify(new OnEntitySignatureChangedEvent(entity, signature));
        }
        public ref T GetComponent<T>(Entity entity) where T : struct, IComponentData
        {
            return ref _componentManager.GetComponent<T>(entity);
        }
        public ComponentType GetComponentType<T>()
        {
            return _componentManager.GetComponentType<T>();
        }
        internal ComponentType GetComponentType(Type componentType)
        {
            return _componentManager.GetComponentType(componentType);
        }
        public bool HasComponent<T>(Entity entity) where T : struct, IComponentData
        {
            return _componentManager.HasComponent<T>(entity);
        }
        #endregion

        #region System methods
        public T RegisterSystem<T>() where T : Systems.ECSSystem, new()
        {
            return _systemManager.RegisterSystem<T>();
        }
        public void SetSystemSignature<T>(Signature signature)
        {
            _systemManager.SetSignature<T>(signature);
        }
        #endregion
    }
}
