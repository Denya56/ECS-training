namespace ECS_training.Core
{
    // struct instead of class since signatures fit more as values and not references + cheaper
    public struct Signature
    {
        // accepts max 8 components
        private ulong signature;
        public void AddComponent(int index) => signature |= 1UL << index;
        public void RemoveComponent(int index) => signature &= ~(1UL << index);
        internal void Reset() => signature = 0UL;
        public bool HasComponent(int index) => (signature & (1UL << index)) != 0;
        public bool HasComponents(Signature other) => (signature & other.signature) == other.signature;
    }
}