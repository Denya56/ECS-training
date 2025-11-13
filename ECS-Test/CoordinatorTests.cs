using ECS_training.Core;
using ECS_training.Exceptions;
using ECS_training.Systems;

namespace ECS_Test
{
    public class CoordinatorTests
    {
        private const int MaxEntities = ECS_training.EcsConfig.MAX_ENTITIES;
        private readonly Coordinator _coordinator;

        public CoordinatorTests()
        {
            _coordinator = new Coordinator();
        }
        #region entity tests
        [Fact]
        public void CreateEntity_ReturnsUniqueEntities_UntilLimit()
        {
            var entities = new HashSet<int>();
            for (int i = 0; i < MaxEntities; i++)
            {
                var entity = _coordinator.CreateEntity();
                Assert.DoesNotContain(entity.Id, entities);
                entities.Add(entity.Id);
            }
        }
        [Fact]
        public void CreateEntity_Throws_WhenLimitExceeded()
        {
            for (int i = 0; i < MaxEntities; i++)
            {
                _coordinator.CreateEntity();
            }
            Assert.Throws<EntityLimitExceededException>(_coordinator.CreateEntity);
        }
        // assuming ecs recycles ids of destroyed entities 
        [Fact]
        public void DestroyEntity_EnablesReuse()
        {
            var createdEntities = new List<Entity>();
            for (int i = 0; i < MaxEntities; i++)
            {
                createdEntities.Add(_coordinator.CreateEntity());
            }
            var toDestroy = createdEntities[0];
            _coordinator.DestroyEntity(toDestroy);
            var reusedEntity = _coordinator.CreateEntity();

            Assert.Equal(toDestroy.Id, reusedEntity.Id);
        }
        [Fact]
        public void GetEntitySignature_ReturnsDefaultSignature_Initially()
        {
            var entity = _coordinator.CreateEntity();
            var signature = _coordinator.GetEntitySignature(entity);
            Assert.False(signature.HasComponent(0));
        }
        [Fact]
        public void DestroyEntity_Throws_WhenEntityDoesNotExist()
        {
            var entity = _coordinator.CreateEntity();
            _coordinator.DestroyEntity(entity);
            Assert.Throws<EntityNotAliveException>(() => _coordinator.DestroyEntity(entity));
        }
        [Fact]
        public void GetEntitySignature_Throws_WhenEntityDoesNotExist()
        {
            var entity = _coordinator.CreateEntity();
            _coordinator.DestroyEntity(entity);
            Assert.Throws<EntityNotAliveException>(() => _coordinator.GetEntitySignature(entity));
        }
        #endregion
        #region component tests
        [Fact]
        public void AddComponent_UpdatesSignature()
        {
            var entity = _coordinator.CreateEntity();
            _coordinator.RegisterComponent<TestComponent>();
            _coordinator.AddComponent(entity, new TestComponent { Value = 69 });

            var signature = _coordinator.GetEntitySignature(entity);
            var componentType = _coordinator.GetComponentType<TestComponent>();
            Assert.True(signature.HasComponent(componentType));
        }
        [Fact]
        public void RemoveComponent_UpdatesSignature()
        {
            var entity = _coordinator.CreateEntity();
            _coordinator.RegisterComponent<TestComponent>();
            _coordinator.AddComponent(entity, new TestComponent { Value = 69 });
            _coordinator.RemoveComponent<TestComponent>(entity);

            var signature = _coordinator.GetEntitySignature(entity);
            var componentType = _coordinator.GetComponentType<TestComponent>();
            Assert.False(signature.HasComponent(componentType));
        }
        [Fact]
        public void HasComponent_ReturnsTrueAfterAdd_FalseAfterRemove()
        {
            var entity = _coordinator.CreateEntity();
            _coordinator.RegisterComponent<TestComponent>();
            Assert.False(_coordinator.HasComponent<TestComponent>(entity));
            _coordinator.AddComponent(entity, new TestComponent { Value = 69 });
            Assert.True(_coordinator.HasComponent<TestComponent>(entity));
            _coordinator.RemoveComponent<TestComponent>(entity);
            Assert.False(_coordinator.HasComponent<TestComponent>(entity));
        }
        [Fact]
        public void GetComponent_ReturnsCorrectValue()
        {
            var entity = _coordinator.CreateEntity();
            _coordinator.RegisterComponent<TestComponent>();
            _coordinator.AddComponent(entity, new TestComponent { Value = 69 });
            var value = _coordinator.GetComponent<TestComponent>(entity).Value;
            Assert.Equal(69, value);
        }
        [Fact]
        public void GetComponent_Throws_IfComponentNotRegistered()
        {
            var entity = _coordinator.CreateEntity();
            Assert.Throws<ComponentNotRegisteredException>(() => _coordinator.GetComponent<TestComponent>(entity));
        }
        [Fact]
        public void AddComponent_Throws_IfTypeNotRegistered()
        {
            var entity = _coordinator.CreateEntity();
            Assert.Throws<ComponentNotRegisteredException>(() => _coordinator.AddComponent(entity, new TestComponent { Value = 69 }));
        }
        [Fact]
        public void AddComponent_Throws_IfAlreadyExists()
        {
            var entity = _coordinator.CreateEntity();
            _coordinator.RegisterComponent<TestComponent>();
            _coordinator.AddComponent(entity, new TestComponent { Value = 69 });
            Assert.Throws<ComponentAlreadyExistsException>(() => _coordinator.AddComponent(entity, new TestComponent { Value = 13 }));
        }
        [Fact]
        public void RemoveComponent_Throws_IfNotPresent()
        {
            var entity = _coordinator.CreateEntity();
            _coordinator.RegisterComponent<TestComponent>();
            Assert.Throws<ComponentNotFoundException>(() => _coordinator.RemoveComponent<TestComponent>(entity));
        }
        [Fact]
        public void RemoveComponent_Throws_WhenComponentNotRegistered()
        {
            var entity = _coordinator.CreateEntity();
            Assert.Throws<ComponentNotRegisteredException>(() => _coordinator.RemoveComponent<TestComponent>(entity));
        }
        #endregion
        #region system tests
        [Fact]
        public void RegisterSystem_ReturnsSameInstanceOnMultipleCalls()
        {
            var system1 = _coordinator.RegisterSystem<RenderingSystem>();
            Assert.Throws<SystemAlreadyRegisteredException>(_coordinator.RegisterSystem<RenderingSystem>);
        }
        [Fact]
        public void RegisterSystem_SetsCoordinatorReference()
        {
            var system = _coordinator.RegisterSystem<RenderingSystem>();
            Assert.Equal(_coordinator, system.Coordinator);
        }
        [Fact]
        public void SetSystemSignature_Throws_IfSystemNotRegistered()
        {
            var sig = new Signature();
            Assert.Throws<SystemNotRegisteredException>(() => _coordinator.SetSystemSignature<RenderingSystem>(sig));
        }
        #endregion
        [Fact]
        public void EntitySignatureChange_UpdatesSystemEntityList()
        {
            _coordinator.RegisterComponent<TestComponent>();
            var system = _coordinator.RegisterSystem<PhysicsSystem>();

            var signature = new Signature();
            signature.AddComponent(_coordinator.GetComponentType<TestComponent>());
            _coordinator.SetSystemSignature<PhysicsSystem>(signature);

            var entity = _coordinator.CreateEntity();
            _coordinator.AddComponent(entity, new TestComponent { Value = 69 });

            Assert.Contains(entity, system.Entities);
        }
        [Fact]
        public void RemoveComponent_CleansUpComponentStorage()
        {
            var entity = _coordinator.CreateEntity();
            _coordinator.RegisterComponent<TestComponent>();
            _coordinator.AddComponent(entity, new TestComponent { Value = 69});
            _coordinator.RemoveComponent<TestComponent>(entity);

            Assert.Throws<ComponentNotFoundException>(() => _coordinator.GetComponent<TestComponent>(entity));
        }
    }
}