using ECS_training.Systems;

namespace ECS_Test
{
    internal class TestSystem : ECS_training.Systems.ECSSystem
    {
        [RequireComponent] public TestComponent TestComponent;
        protected override void UpdateInternal(float dt)
        {
            throw new NotImplementedException();
        }
    }
}
