using ModelPlaneInfo.Entities;
using ModelPlaneInfo.Interfaces;
using System.Text;
using System.Xml;

namespace ModelPlaneInfo.IO
{
    public class XmlFileIoController : IFileIoController
    {
        public string FileExtension
        {
            get { return ".xml"; }
        }

        static void WritePlaneTypes(IEnumerable<PlaneType> collection, XmlWriter writer)
        {

            writer.WriteStartElement("PlaneTypesData");
            foreach (var inst in collection)
            {
                writer.WriteStartElement("PlaneType");
                writer.WriteElementString("Id", inst.Id.ToString());
                writer.WriteElementString("TypeName", inst.Name.ToString());
                writer.WriteElementString("Note", inst.Note?.ToString());
                writer.WriteEndElement();

            }
            writer.WriteEndElement();
        }

        void WriteData(IDataSet dataSet, XmlWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("ModelPlaneInfo");
            WritePlaneTypes(dataSet.PlaneTypes, writer);
            WriteModelsPlane(dataSet.ModelPlanes, writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        public void Save(IDataSet dataSet, string fileName)
        {
            XmlWriterSettings settings = new()
            {
                Encoding = Encoding.Unicode
            };
            XmlWriter writer = null;
            try
            {
                writer = XmlWriter.Create(fileName, settings);
                WriteData(dataSet, writer);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                writer?.Close();
            }
        }

        static void ReadPlaneType(XmlReader reader, ICollection<PlaneType> collection)
        {
            reader.ReadStartElement("PlaneType");

            int Id = reader.ReadElementContentAsInt();
            string parent = reader.ReadElementContentAsString();
            PlaneType inst = new(parent) { Id = Id };
            inst.Note = reader.ReadElementContentAsString();
            collection.Add(inst);
        }

        private static void WriteModelsPlane(IEnumerable<ModelPlane> collection, XmlWriter writer)
        {

            writer.WriteStartElement("ModelPlaneData");
            foreach (var inst in collection)
            {
                writer.WriteStartElement("ModelPlane");
                writer.WriteElementString("Id", inst.Id.ToString());
                writer.WriteElementString("Name", inst.Name.ToString());
                int typeId = inst.Type == null ? 0 : inst.Type.Id;
                writer.WriteElementString("TypeId", typeId.ToString());
                writer.WriteElementString("BeginnYear", inst.BeginnYear.ToString());
                writer.WriteElementString("Used", inst.Used.ToString());
                writer.WriteElementString("Description", inst.Description?.ToString());
                writer.WriteElementString("Note", inst.Note?.ToString());

                writer.WriteEndElement();

            }
            writer.WriteEndElement();
        }

        static void ReadModelPlane(XmlReader reader, IDataSet dataSet)
        {
            ModelPlane inst = new ModelPlane();
            reader.ReadStartElement("ModelPlane");

            inst.Id = reader.ReadElementContentAsInt();
            inst.Name = reader.ReadElementContentAsString();
            int typeId = reader.ReadElementContentAsInt();

            inst.Type = dataSet.PlaneTypes
                .FirstOrDefault(e => e.Id == typeId);
            string s = reader.ReadElementContentAsString();
            inst.BeginnYear = string.IsNullOrEmpty(s)
                ? (int?)null : int.Parse(s);
            inst.Used = reader.ReadElementContentAsString();
            inst.Description = reader.ReadElementContentAsString();
            inst.Note = reader.ReadElementContentAsString();
            dataSet.ModelPlanes.Add(inst);
        }

        public void Load(IDataSet dataSet, string fileName)
        {
            if (!File.Exists(fileName)) return;

            XmlReaderSettings settings = new();
            settings.IgnoreWhitespace = true;
            using (XmlReader reader = XmlReader.Create(fileName, settings))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "PlaneType":
                                ReadPlaneType(reader, dataSet.PlaneTypes);
                                break;
                            case "ModelPlane":
                                ReadModelPlane(reader, dataSet);
                                break;
                        }
                    }
                }
            }
        }
    }

}