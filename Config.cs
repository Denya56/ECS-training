global using ComponentType = System.Byte;

namespace ESC_training
{
    // inject from e.g. json for flexibility (runtime-configurable)
    internal static class Config
    {
        public const int MAX_ENTITIES = 5000;
        public const int MAX_COMPONENTS = 8;
    }
}