namespace ESC_training.Core
{
    // struct instead of class since signatures fit more as values and not references + cheaper
    internal struct Signature
    {
        private byte signature;

        public void AddComponent(int index) => signature |= (byte)(1 << index);
        public void RemoveComponent(int index) => signature &= (byte)~(1 << index);
        public void Reset() => signature = 0;
        public bool HasComponent(int index) => (signature & (byte)(1 << index)) != 0;
        public bool HasComponents(Signature other) => (signature & other.signature) == other.signature;
    }
}