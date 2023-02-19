using Common.Interfaces;
using Common.Repositories;
using Common.Repositories.Interfaces;
using ModelPlaneInfo.Data;
using ModelPlaneInfo.Entities;
using ModelPlaneInfo.Repositories.Interfaces;

namespace ModelPlaneInfo.Repositories
{
    public class FileBasedUnitOfWork : UnitOfWork, IInfoUnitOfWork, ITestStorage
    {
        public IRepository<ModelPlane> ModelPlanesRepository { get; private set; }
        public IRepository<PlaneType> PlanesTypeRepository { get; private set; }

        private readonly DataContext dataContext;

        public FileBasedUnitOfWork(DataContext dataContext)
        {
            this.dataContext = dataContext;
            ModelPlanesRepository = new Repository<ModelPlane>(dataContext.ModelPlanes);
            PlanesTypeRepository = new Repository<PlaneType>(dataContext.PlaneTypes);
        }

        public FileBasedUnitOfWork(FileBasedUnitOfWork unitOfWork) { }

        public override void Load()
        {
            dataContext.Load();
        }

        public override void Save()
        {
            dataContext.Save();
        }

        protected override void DoDispose()
        {
            //throw new System.NotImplementedException();
        }

        public void CreateTestingData()
        {
            dataContext.CreateTestData();
        }

    }
}
