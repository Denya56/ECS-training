using ESC_training.Components;
using ESC_training.Core;
using ESC_training.Systems;
using System.Numerics;
using static ESC_training.Config;

var coordinator = new Coordinator();

coordinator.RegisterComponent<Gravity>();
coordinator.RegisterComponent<RigidBody>();
coordinator.RegisterComponent<Transform>();

var physicsSystem = coordinator.RegisterSystem<PhysicsSystem>();
physicsSystem.coordinator = coordinator;

var signature = new Signature();
signature.AddComponent(coordinator.GetComponentType<Gravity>());
signature.AddComponent(coordinator.GetComponentType<RigidBody>());
signature.AddComponent(coordinator.GetComponentType<Transform>());
coordinator.SetSystemSignature<PhysicsSystem>(signature);

var entities = new List<Entity>(MAX_ENTITIES);
var rand = new Random();

for (int i = 0; i < MAX_ENTITIES; i++)
{
    var entity = coordinator.CreateEntity();

    coordinator.AddComponent(entity, new Gravity
    {
        Force = new Vector3(0f, -10f, 0f)
    });

    coordinator.AddComponent(entity, new RigidBody
    {
        Velocity = Vector3.Zero,
        Acceleration = Vector3.Zero
    });

    float scale = (float)(3 + rand.NextDouble() * 2);
    coordinator.AddComponent(entity, new Transform
    {
        Position = new Vector3(
            (float)(rand.NextDouble() * 200 - 100),
            (float)(rand.NextDouble() * 200 - 100),
            (float)(rand.NextDouble() * 200 - 100)
        ),
        Rotation = new Vector3(
            (float)(rand.NextDouble() * 3),
            (float)(rand.NextDouble() * 3),
            (float)(rand.NextDouble() * 3)
        ),
        Scale = new Vector3(scale, scale, scale)
    });

    entities.Add(entity);
}

// main loop
//bool quit = false;
var sw = new System.Diagnostics.Stopwatch();
sw.Start();
float elapsedTime = 0f;
float simulationDuration = 10f;

//var entit1y = coordinator.CreateEntity();

coordinator.DestroyEntity(entities[0]);
var pos = coordinator.GetComponent<Transform>(entities[0]);
while (elapsedTime < simulationDuration)
{
    float dt = (float)sw.Elapsed.TotalSeconds;
    sw.Restart();
    elapsedTime += dt;

    physicsSystem.Update(dt);

    Console.Clear();
    Console.WriteLine($"{"Entity",-6} {"X",8} {"Y",8} {"Z",8}");
    Console.WriteLine(new string('-', 32));

    for (int i = 0; i < Math.Min(10, MAX_ENTITIES); i++)
    {
        var t = coordinator.GetComponent<Transform>(entities[i]);
        Console.WriteLine($"{i,-6} {t.Position.X,8:F2} {t.Position.Y,8:F2} {t.Position.Z,8:F2}");
    }
    Thread.Sleep(16);
}
Console.WriteLine("Simulation finished.");