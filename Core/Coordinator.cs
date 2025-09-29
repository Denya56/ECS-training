namespace ESC_training.Core
{
    internal class Coordinator
    {
        private ComponentManager _componentManager;
        private EntityManager _entityManager;
        private SystemManager _systemManager;

        public Coordinator()
        {
            _componentManager = new ComponentManager();
            _entityManager = new EntityManager();
            _systemManager = new SystemManager();
        }

        #region Entity methods
        public Entity CreateEntity()
        {
            return _entityManager.CreateEntity();
        }
        public void DestroyEntity(Entity entity)
        {
            _entityManager.DestroyEntity(entity);
            _componentManager.EntityDestroyed(entity);
            _systemManager.EntityDestroyed(entity);
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
    }
}
