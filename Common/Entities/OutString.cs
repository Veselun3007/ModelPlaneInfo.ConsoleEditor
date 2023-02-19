using Common.Interfaces;

namespace Common.Entities
{
    [Serializable]
    public abstract class OutString : IEntity
    {
        public int Id { get; set; }

        public abstract string? ToMembersString();

        public sealed override string ToString()
        {
            return string.Format($"{Id,5} {ToMembersString()}");
        }
    }
}
