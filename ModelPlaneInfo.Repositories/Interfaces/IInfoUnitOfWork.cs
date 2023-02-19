using Common.Repositories.Interfaces;
using ModelPlaneInfo.Entities;

namespace ModelPlaneInfo.Repositories.Interfaces
{
    public interface IInfoUnitOfWork : IUnitOfWork
    {
        IRepository<PlaneType> PlanesTypeRepository { get; }
        IRepository<ModelPlane> ModelPlanesRepository { get; }
    }
}
