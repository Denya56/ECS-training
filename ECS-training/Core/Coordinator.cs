using ESC_training.Core.Events;
using ESC_training.Core.Managers;
using ESC_training.Systems;

namespace ESC_training.Core
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

        private readonly EventManager _eventManager = new EventManager();

        private readonly ComponentManager _componentManager;
        private readonly EntityManager _entityManager;
        private readonly SystemManager _systemManager;
        
        public Coordinator()
        {
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
        #endregion

        #region Component methods
        public void RegisterComponent<T>()
        {
            _componentManager.RegisterComponent<T>();
        }
        public void AddComponent<T>(Entity entity, T component)
        {
            _componentManager.AddComponent(entity, component);

            var signature = _entityManager.GetSignature(entity);
            signature.AddComponent(_componentManager.GetComponentType<T>());
            _entityManager.SetSignature(entity, signature);

            // notify other managers
            _eventManager.Notify(new OnEntitySignatureChangedEvent(entity, signature));
        }
        public void RemoveComponent<T>(Entity entity)
        {
            _componentManager.RemoveComponent<T>(entity);

            var signature = _entityManager.GetSignature(entity);
            signature.RemoveComponent(_componentManager.GetComponentType<T>());
            _entityManager.SetSignature(entity, signature);

            _eventManager.Notify(new OnEntitySignatureChangedEvent(entity, signature));
        }
        public ref T GetComponent<T>(Entity entity)
        {
            return ref _componentManager.GetComponent<T>(entity);
        }
        public ComponentType GetComponentType<T>()
        {
            return _componentManager.GetComponentType<T>();
        }
        public bool HasComponent<T>(Entity entity)
        {
            return _componentManager.HasComponent<T>(entity);
        }
        #endregion

        #region System methods
        public T RegisterSystem<T>() where T : Systems.System, new()
        {
            var system = new T();
            system.Coordinator = this;
            return _systemManager.RegisterSystem<T>();
        }
        public void SetSystemSignature<T>(Signature signature)
        {
            _systemManager.SetSignature<T>(signature);
        }
        #endregion
    }
}
