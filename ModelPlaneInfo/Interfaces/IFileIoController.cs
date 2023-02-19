using ModelPlaneInfo.Data;

namespace ModelPlaneInfo.Interfaces
{
    public interface IFileIoController
    {
        string FileExtension { get; }

        void Load(IDataSet dataSet, string fileName);
        void Save(IDataSet dataSet, string fileName);
    }
}
