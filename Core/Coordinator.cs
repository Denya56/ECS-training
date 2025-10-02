namespace ESC_training.Core
{
    internal class Coordinator : ISubject
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

        private ComponentManager _componentManager;
        private EntityManager _entityManager;
        private SystemManager _systemManager;

        private Dictionary<Type, List<IObserver<TEvent>>> _observers;
        public Coordinator()
        {
            _componentManager = new ComponentManager();
            _entityManager = new EntityManager();
            _systemManager = new SystemManager();

            Attach(_entityManager);
            Attach(_componentManager);
            Attach(_systemManager);
        }

        #region Entity methods
        public Entity CreateEntity()
        {
            return _entityManager.CreateEntity();
        }
        public void DestroyEntity(Entity entity)
        {
            var @event = new OnEntityDeletedEvent(entity);
            Notify<OnEntityDeletedEvent>(@event);
        }
        #endregion

        #region Component methods
        public void RegisterComponent<T>()
        {
            _componentManager.RegisterComponent<T>();
        }
        public void AddComponent<T>(Entity entity, T component)
        {
            _componentManager.AddComponent<T>(entity, component);

            var signature = _entityManager.GetSignature(entity);
            signature.AddComponent(_componentManager.GetComponentType<T>());
            _entityManager.SetSignature(entity, signature);

            _systemManager.EntitySignatureChanged(entity, signature);
        }
        public void RemoveComponent<T>(Entity entity)
        {
            _componentManager.RemoveComponent<T>(entity);

            var signature = _entityManager.GetSignature(entity);
            signature.RemoveComponent(_componentManager.GetComponentType<T>());
            _entityManager.SetSignature(entity, signature);

            _systemManager.EntitySignatureChanged(entity, signature);
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
            return _systemManager.RegisterSystem<T>();
        }
        public void SetSystemSignature<T>(Signature signature)
        {
            _systemManager.SetSignature<T>(signature);
        }
        #endregion

        public void Notify<TEvent>(TEvent @event)
        {
            foreach (var observer in _observers)
            {
                observer.Value.ForEach(x => x.Update(@event));
            }
        }

        public void Attach<TEvent>(IObserver<TEvent> observer)
        {
            _observers.Add(observer);
        }

        public void Detach<TEvent>(IObserver<TEvent> observer)
        {
            _observers.Remove(observer);
        }
    }
}
