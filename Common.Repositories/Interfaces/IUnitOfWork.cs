namespace Common.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        void Load();
    }
}
