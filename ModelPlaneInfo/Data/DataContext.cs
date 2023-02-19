using System.Collections.Generic;
using ModelPlaneInfo.Entities;
using Common.Extensions;
using ModelPlaneInfo.Interfaces;
using System.IO;
using ModelPlaneInfo.IO;
using System;

namespace ModelPlaneInfo.Data
{
    [Serializable]
    public partial class DataContext : IDataSet
    {

        readonly List<ModelPlane> modelPlanes = new();
        readonly List<PlaneType> planeTypes = new();

        IFileIoController fileIoController;

        public IFileIoController FileIoController
        {
            get { return fileIoController; }
            set { fileIoController = value; }
        }

        public DataContext(IFileIoController fileIoController)
        {
            this.fileIoController = fileIoController;
        }


        public DataContext(string directory)
        {
            this.Directory = directory;
        }

        // public DataContext(){}

        public ICollection<ModelPlane> ModelPlanes
        {
            get { return modelPlanes; }
        }
        public ICollection<PlaneType> PlaneTypes
        {
            get { return planeTypes; }
        }

        public static string fileName = "ModelPlanesInfo";

        private string directory;

        public string Directory
        {
            get { return directory; }
            set
            {
                directory = value;
                if (!System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                }
            }
        }

        public string Path => System.IO.Path.Combine(directory,
            fileName + fileIoController.FileExtension);

        public void Save()
        {
            fileIoController.Save(this, Path);
        }

        public void Load()
        {
            fileIoController.Load(this, Path);
        }
    }
}