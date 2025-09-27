using ESC_training.Components;
using ESC_training.Core;
using ESC_training.Systems;
using System.Numerics;
using static ESC_training.Config;
using Raylib_cs;
using Transform2D = ESC_training.Components.Transform2D;

var coordinator = new Coordinator();

coordinator.RegisterComponent<Gravity>();
coordinator.RegisterComponent<RigidBody2D>();
coordinator.RegisterComponent<Transform2D>();
coordinator.RegisterComponent<Rendarable>();
coordinator.RegisterComponent<Square>();
coordinator.RegisterComponent<Circle>();

var physicsSystem = coordinator.RegisterSystem<PhysicsSystem>();
physicsSystem.Coordinator = coordinator;

var renderingSystem = coordinator.RegisterSystem<RenderingSystem>();
renderingSystem.Coordinator = coordinator;

var physicsSignature = new Signature();
physicsSignature.AddComponent(coordinator.GetComponentType<Gravity>());
physicsSignature.AddComponent(coordinator.GetComponentType<RigidBody2D>());
physicsSignature.AddComponent(coordinator.GetComponentType<Transform2D>());
coordinator.SetSystemSignature<PhysicsSystem>(physicsSignature);

var renderingSignature = new Signature();
renderingSignature.AddComponent(coordinator.GetComponentType<Rendarable>());
renderingSignature.AddComponent(coordinator.GetComponentType<Transform2D>());
coordinator.SetSystemSignature<RenderingSystem>(renderingSignature);

var entities = new List<Entity>(MAX_ENTITIES);
var rand = new Random();

for (int i = 0; i < MAX_ENTITIES; i++)
{
    var entity = coordinator.CreateEntity();

    coordinator.AddComponent(entity, new Gravity
    {
        Force = new Vector2(0f, 5f + (float)rand.NextDouble() * 10f)   
    });

    coordinator.AddComponent(entity, new RigidBody2D
    {
        Velocity = Vector2.Zero,
        Acceleration = Vector2.Zero
    });

    float scale = (float)(3 + rand.NextDouble() * 2);
    coordinator.AddComponent(entity, new Transform2D
    {
        Position = new Vector2(
            (float)(rand.NextDouble() * 800),
            (float)(-rand.NextDouble() * 200 - 100)
        ),

        Scale = new Vector2(scale, scale)
    });

    coordinator.AddComponent(entity, new Rendarable
    {
        Color = new Vector4(
            (float)rand.NextDouble(),
            (float)rand.NextDouble(),
            (float)rand.NextDouble(),
            1f
        )
    });

    if (rand.NextDouble() > 0.5)
    {
        coordinator.AddComponent(entity, new Square
        {
            SideLength = (int)(5 + rand.NextDouble() * 10)
        });
    }
    else
    {
        coordinator.AddComponent(entity, new Circle
        {
            Radius = (int)(5 + rand.NextDouble() * 10)
        });
    }
    /*Rotation = new Vector3(
            (float)(rand.NextDouble() * 3),
            (float)(rand.NextDouble() * 3),
            (float)(rand.NextDouble() * 3)
        ),*/

    entities.Add(entity);
}

Raylib.InitWindow(800, 600, "Raylib Window");
Raylib.SetTargetFPS(60);

while (!Raylib.WindowShouldClose())
{
    float dt = Raylib.GetFrameTime();
    physicsSystem.Update(dt);
    renderingSystem.Update(dt);
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.RayWhite);

    Raylib.EndDrawing();
}

Raylib.CloseWindow();

// main loop
//bool quit = false;
/*var sw = new System.Diagnostics.Stopwatch();
sw.Start();
float elapsedTime = 0f;
float simulationDuration = 10f;

//var entit1y = coordinator.CreateEntity();

*//*coordinator.DestroyEntity(entities[0]);
var pos = coordinator.GetComponent<Transform>(entities[0]);*//*
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
        var t = coordinator.GetComponent<Transform2D>(entities[i]);
        Console.WriteLine($"{i,-6} {t.Position.X,8:F2} {t.Position.Y,8:F2}");
    }
    Thread.Sleep(16);
}*/
Console.WriteLine("Simulation finished.");