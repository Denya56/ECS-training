global using ComponentType = System.Byte;

namespace Recs
{
    // inject from e.g. json for flexibility (runtime-configurable)
    public static class EcsConfig
    {
        public const int MAX_ENTITIES = 10000;
        public const int MAX_COMPONENTS = 64;

        /*public int MaxEntities { get; }
        public int MaxComponents { get; }

        public EcsConfig(int maxEntities = MAX_ENTITIES, int maxComponents = MAX_COMPONENTS)
        {
            MaxEntities = maxEntities;
            MaxComponents = maxComponents;
        }*/
    }
}