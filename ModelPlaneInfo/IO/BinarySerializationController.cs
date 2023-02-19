using ModelPlaneInfo.Interfaces;

namespace ModelPlaneInfo.IO
{
    [Serializable]
    public class BinarySerializationController :
        GenericBinarySerializationController<IDataSet>, IFileIoController
    {
        public void Load(IDataSet dataSet, string fileName)
        {
            IDataSet newDataSet = Load(fileName);
            if (newDataSet == null)
                return;
            foreach (var el in newDataSet.PlaneTypes)
            {
                dataSet.PlaneTypes.Add(el);
            }
            foreach (var el in newDataSet.ModelPlanes)
            {
                dataSet.ModelPlanes.Add(el);
            }
        }
    }
}