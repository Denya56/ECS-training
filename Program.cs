// ===== ECS setup =====
using ESC_training.Components;
using ESC_training.Core;
using ESC_training.Entities;
using ESC_training.Systems;
using System.Numerics;
using static ESC_training.Config;

/*
var entityManager = new EntityManager();
var componentManager = new ComponentManager();

// Register components
componentManager.RegisterComponent<Transform>();
componentManager.RegisterComponent<RigidBody>();
componentManager.RegisterComponent<Gravity>();

// Create entities
Entity player = entityManager.CreateEntity();
Entity enemy = entityManager.CreateEntity();

// Add components
componentManager.AddComponent(player, new Transform { Position = new Vector3(0, 0, 0), Rotation = Vector3.Zero, Scale = Vector3.One });
componentManager.AddComponent(player, new RigidBody { Velocity = Vector3.Zero, Acceleration = Vector3.Zero });

componentManager.AddComponent(enemy, new Transform { Position = new Vector3(10, 0, 0), Rotation = Vector3.Zero, Scale = Vector3.One });
componentManager.AddComponent(enemy, new Gravity { Force = new Vector3(0, -9.81f, 0) });

// ===== Access and modify components via ref =====
ref var playerTransform = ref componentManager.GetComponent<Transform>(player);
Console.WriteLine($"Player initial position: {playerTransform.Position}");
playerTransform.Position += new Vector3(1, 0, 0);
Console.WriteLine($"Player new position: {playerTransform.Position}");

ref var enemyGravity = ref componentManager.GetComponent<Gravity>(enemy);
Console.WriteLine($"Enemy gravity force: {enemyGravity.Force}");
enemyGravity.Force += new Vector3(0, -1, 0);
Console.WriteLine($"Enemy new gravity force: {enemyGravity.Force}");

// ===== Entity destruction demo =====
entityManager.DestroyEntity(enemy);
componentManager.EntityDestroyed(enemy);

try
{
    componentManager.GetComponent<Gravity>(enemy);
}
catch (Exception ex)
{
    Console.WriteLine($"Accessing destroyed component throws: {ex.Message}");
}*/
var coordinator = new Coordinator();

// Register components
coordinator.RegisterComponent<Gravity>();
coordinator.RegisterComponent<RigidBody>();
coordinator.RegisterComponent<Transform>();

// Register system
var physicsSystem = coordinator.RegisterSystem<PhysicsSystem>();
physicsSystem.coordinator = coordinator;

// Set system signature
var signature = new Signature();
signature.AddComponent(coordinator.GetComponentType<Gravity>());
signature.AddComponent(coordinator.GetComponentType<RigidBody>());
signature.AddComponent(coordinator.GetComponentType<Transform>());
coordinator.SetSystemSignature<PhysicsSystem>(signature);

// Create entities
var entities = new List<Entity>(MAX_ENTITIES);
var rand = new Random();

for (int i = 0; i < MAX_ENTITIES; i++)
{
    var entity = coordinator.CreateEntity();

    // Add Gravity
    coordinator.AddComponent(entity, new Gravity
    {
        Force = new Vector3(0f, -10f, 0f)
    });

    // Add RigidBody
    coordinator.AddComponent(entity, new RigidBody
    {
        Velocity = Vector3.Zero,
        Acceleration = Vector3.Zero
    });

    // Add Transform
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

// Main loop
//bool quit = false;
var sw = new System.Diagnostics.Stopwatch();
sw.Start();
float elapsedTime = 0f;
float simulationDuration = 10f;


while (elapsedTime < simulationDuration)
{
    float dt = (float)sw.Elapsed.TotalSeconds;
    sw.Restart();
    elapsedTime += dt;

    physicsSystem.Update(dt);

    Console.Clear();
    Console.WriteLine($"{"Entity",-6} {"X",8} {"Y",8} {"Z",8}");
    Console.WriteLine(new string('-', 32));

    // Display first 10 entities in table
    for (int i = 0; i < Math.Min(10, MAX_ENTITIES); i++)
    {
        var t = coordinator.GetComponent<Transform>(entities[i]);
        Console.WriteLine($"{i,-6} {t.Position.X,8:F2} {t.Position.Y,8:F2} {t.Position.Z,8:F2}");
    }
    Thread.Sleep(16);
}
Console.WriteLine("Simulation finished.");