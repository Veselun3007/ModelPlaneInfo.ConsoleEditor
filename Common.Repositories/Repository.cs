using Common.Interfaces;
using Common.Repositories.Interfaces;

namespace Common.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {

        protected internal readonly ICollection<T> collection;

        public Repository(ICollection<T> collection)
        {

            this.collection = collection;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return collection;
        }

        public virtual T GetById(int id)
        {
            return collection.First(e => e.Id == id);
        }

        public virtual void Add(T item)
        {
            collection.Add(item);
        }

        public virtual void Update(T item)
        {
            throw new NotSupportedException();
        }

        public virtual void Delete(T item)
        {
            if (item != null && collection.Contains(item))
                collection.Remove(item);
        }

    }
}
