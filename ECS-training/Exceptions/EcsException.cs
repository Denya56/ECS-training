using System;

namespace ESC_training.Exceptions
{
    public class EcsException : Exception
    {
        public EcsException(string message) : base(message) { }
    }

    public class EntityLimitExceededException : EcsException
    {
        public EntityLimitExceededException() : base("Too many entities in existence.") { }
    }

    public class EntityOutOfRangeException : EcsException
    {
        public EntityOutOfRangeException(int id) : base($"Entity {id} is out of range.") { }
    }

    public class ComponentAlreadyRegisteredException : EcsException
    {
        public ComponentAlreadyRegisteredException(Type type)
            : base($"Component of type {type.Name} is already registered.") { }
    }

    public class ComponentNotRegisteredException : EcsException
    {
        public ComponentNotRegisteredException(Type type)
            : base($"Component of type {type.Name} is not registered.") { }
    }

    public class ComponentAlreadyExistsException : EcsException
    {
        public ComponentAlreadyExistsException(Type type, int entityId)
            : base($"Component of type {type.Name} already exists on Entity {entityId}.") { }
    }

    public class ComponentNotFoundException : EcsException
    {
        public ComponentNotFoundException(Type type, int entityId)
            : base($"Component of type {type.Name} does not exist on Entity {entityId}.") { }
    }

    public class ComponentLimitExceededException : EcsException
    {
        public ComponentLimitExceededException(int max)
            : base($"Cannot register more than {max} components.") { }
    }

    public class SystemAlreadyRegisteredException : EcsException
    {
        public SystemAlreadyRegisteredException(Type type)
            : base($"System {type.Name} is already registered.") { }
    }

    public class SystemNotRegisteredException : EcsException
    {
        public SystemNotRegisteredException(Type type)
            : base($"System {type.Name} is not registered.") { }
    }
}
