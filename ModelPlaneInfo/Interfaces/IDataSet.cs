using ModelPlaneInfo.Entities;
using System.Collections.Generic;

namespace ModelPlaneInfo.Interfaces
{
    public interface IDataSet
    {
        ICollection<ModelPlane> ModelPlanes { get; }
        ICollection<PlaneType> PlaneTypes { get; }
    }
}
