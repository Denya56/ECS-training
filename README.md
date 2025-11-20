# RECS A C# ECS Framework

A simple Entity-Component-System (ECS) framework in C# for learning game architecture, inspired by [Austin Morlan's C++ ECS framework](https://austinmorlan.com/posts/entity_component_system/).

---

## Features

* Struct-only components implementing ```IComponentData```
* Packed component arrays with swap-delete behavior for removals
* Entity lifecycle with explicit alive/dead tracking
* Automatic system signature generation via reflection + ```[RequireComponent]```
* 64-bit signature masks, supporting up to 64 component types
* Unit-tested core components (entities, components, systems, signatures)
* Includes a small Raylib2D demo

---

https://github.com/user-attachments/assets/a3f0c054-438c-496c-afc1-9352339b94d2

---

## Usage Examples

### Register Components

```csharp
coordinator.RegisterComponent<Gravity>();
coordinator.RegisterComponent<RigidBody2D>();
coordinator.RegisterComponent<Transform2D>();
coordinator.RegisterComponent<Renderable>();
coordinator.RegisterComponent<Square>();
coordinator.RegisterComponent<Circle>();
```

### Define System Requirements

Systems declare which components they need using the RequireComponent attribute:
```csharp
[RequireComponent(typeof(Transform2D))]
[RequireComponent(typeof(RigidBody2D))]
public class PhysicsSystem : Systems.System
{
    public void Update(float dt) { /* ... */ }
}
```

The system's signature is generated automatically based on these attributes.

### Register Systems

```csharp
var physicsSystem = coordinator.RegisterSystem<PhysicsSystem>();
var renderSystem  = coordinator.RegisterSystem<RenderingSystem>();
```

### Create Entities and Add Components

```csharp
var entity = coordinator.CreateEntity();

coordinator.AddComponent(entity, new Gravity { Force = new Vector2(0f, 9.8f) });
coordinator.AddComponent(entity, new RigidBody2D { Velocity = Vector2.Zero });
coordinator.AddComponent(entity, new Transform2D { Position = new Vector2(0, 0), Scale = Vector2.One });
coordinator.AddComponent(entity, new Renderable { Color = new Vector4(1f, 0f, 0f, 1f) });
```

### Main Loop

```csharp
while (!Raylib.WindowShouldClose())
{
    float dt = Raylib.GetFrameTime();

    physicsSystem.Update(dt);
    renderingSystem.Update(dt);

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.DarkGray);

    foreach (var command in renderingSystem.Commands)
    {
        if (command.Command == RenderCommand.CommandType.DrawRectangle)
            Raylib.DrawRectangle((int)command.Position.X, (int)command.Position.Y,
                                 command.Size, command.Size,
                                 Raylib.ColorFromNormalized(command.Color));
    }

    Raylib.EndDrawing();
}
```
