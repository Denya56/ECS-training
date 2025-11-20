namespace Recs.Core
{
    public sealed class Entity
    {
        public int Id { get; }

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
