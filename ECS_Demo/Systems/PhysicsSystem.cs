using ECS_Demo.Components;
using Recs.Core;
using Recs.Systems;


namespace ECS_Demo.Systems
{
    public class PhysicsSystem : ECSSystem
    {
        [RequireComponent] public Gravity Gravity;
        [RequireComponent] public RigidBody2D RigidBody2D;
        [RequireComponent] public Transform2D Transform2D;
        protected override void UpdateInternal(float dt)
        {
            foreach (Entity entity in entities)
            {
                ref var rigidBody = ref Coordinator.Instance.GetComponent<RigidBody2D>(entity);
                ref var transform = ref Coordinator.Instance.GetComponent<Transform2D>(entity);
                var gravity = Coordinator.Instance.GetComponent<Gravity>(entity);

                transform.Position += rigidBody.Velocity * dt;
                rigidBody.Velocity += gravity.Force * dt;
            }
        }
    }
}
