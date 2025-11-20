using Recs.Systems;

namespace ECS_Test
{
    internal class TestSystem : Recs.Systems.ECSSystem
    {
        [RequireComponent] public TestComponent TestComponent;
        protected override void UpdateInternal(float dt)
        {
            throw new NotImplementedException();
        }
    }
}
