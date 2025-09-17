namespace ESC_training.Entities
{
    internal sealed class Entity
    {
        public int Id { get; }
        public object ComponentManager { get; internal set; }

        internal Entity(int id)
        {
            Id = id;
        }

        public override bool Equals(object? obj) =>
            obj is Entity other && Id == other.Id;

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => $"Entity({Id})";
    }
}
