global using ComponentType = System.Byte;

namespace ESC_training
{
    // inject from e.g. json for flexibility (runtime-configurable)
    internal static class Config
    {
        public const int MAX_ENTITIES = 10000;
        public const int MAX_COMPONENTS = 8;
        public const int WINDOW_WIDTH = 1920;
        public const int WINDOW_HEIGHT = 1080;
    }
}