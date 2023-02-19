using Common.Context.StringFormatters;
using ModelPlaneInfo.Entities;
using ModelPlaneInfo.Interfaces;
using System.Text;
using Common.Context.Extensions;

namespace ModelPlaneInfo.IO
{
    public class TextFileIoController : IFileIoController
    {
        public string FileExtension { get; } = ".txt";

        public void Load(IDataSet dataSet, string fileName)
        {
            if (!File.Exists(fileName))
                return;
            StreamReader sr = new(fileName, Encoding.Unicode);
            while (sr.Peek() >= 0)
            {
                ReadRecordData(sr, dataSet);
            }
            sr.Close();
        }
        public void Save(IDataSet dataSet, string fileName)
        {
            if (File.Exists(fileName)) DeleteFile(fileName);
            foreach (var el in dataSet.PlaneTypes)
            {
                string obj = CreatePlaneTypeString(el);
                File.AppendAllText(fileName, obj, Encoding.Unicode);
            }
            foreach (var el in dataSet.ModelPlanes)
            {
                string obj = CreateModelPlaneString(el);
                File.AppendAllText(fileName, obj, Encoding.Unicode);
            }
        }

        public static void DeleteFile(string fileName)
        {
            File.Delete(fileName);
        }

        private static string CreateModelPlaneString(ModelPlane modelPlane)
        {
            return string.Format("Aircraft model:" +
                "\n\tId: {0}" +
                "\n\tName: {1}" +
                "\n\tType of aircraft: {2}" +
                "\n\tYear of start of operation: {3}" +
                "\n\tAvailability in use: {4}" +
                "\n\tDescription: {5}" +
                "\n\tNote: {6}\n",
                modelPlane.Id, modelPlane.Name, modelPlane.Type.Name, modelPlane.BeginnYear, modelPlane.Used,
                CreateAtribute(modelPlane.Description), CreateAtribute(modelPlane.Note));
        }
        private static string CreatePlaneTypeString(PlaneType planeType)
        {
            return string.Format("Types of aircraft:" +
                "\n\tId: {0}" +
                "\n\tAircraft type: {1}" +
                "\n\tDescription: {2}\n",
                planeType.Id, planeType.Name, CreateAtribute(planeType.Note));
        }

        private static string CreateAtribute(string str)
        {
            StringFormatter.Current.LineLength = 50;

            str = string.IsNullOrWhiteSpace(str) ? "" : str.ToIndentedLineBlock(0);
            str = str.Insert(0, "|:");
            str += ":|";
            return str;
        }

        private static string GetValue(string s)
        {
            int pos = s.IndexOf(":");
            return s.Substring(pos + 1).Trim();
        }
        private static string GetValue(StreamReader sr, string openTag, string closeTag)
        {
            StringBuilder sb = new();
            string s = sr.ReadLine();
            int pos = s.IndexOf(":");
            int openTagLength = openTag.Length;
            if (s.IndexOf(openTag) != -1)
            {
                while (true)
                {
                    int pos1 = s.IndexOf(closeTag);
                    int pos2 = pos + 2 + openTagLength;
                    if (pos1 != -1)
                    {
                        sb.Append(s.AsSpan(pos2, pos1 - (pos2)));
                        return sb.ToString();
                    }
                    else
                    {
                        sb.Append(s.AsSpan(pos2));
                        s = sr.ReadLine();
                    }
                    pos = -(2 + openTagLength);
                }
            }
            else
            {
                sb.Append(s.AsSpan(pos + 1).Trim());
                return sb.ToString();
            }
        }

        private void ReadRecordData(StreamReader sr, IDataSet dataSet)
        {
            string s = sr.ReadLine().Trim();
            if (s == "Types of aircraft:")
            {
                dataSet.PlaneTypes.Add(ReadPlaneTypeData(sr, dataSet));
            }
            else if (s == "Aircraft model:")
            {
                dataSet.ModelPlanes.Add(ReadModelPlaneData(sr, dataSet));
            }
            
        }

        private static ModelPlane ReadModelPlaneData(StreamReader sr, IDataSet dataSet)
        {
            ModelPlane modelPlane = new();
            string s;
            string q;
            modelPlane.Id = Convert.ToInt32(GetValue(sr.ReadLine()));
            modelPlane.Name = GetValue(sr.ReadLine());
            q = GetValue(sr.ReadLine());
            modelPlane.Type = dataSet.PlaneTypes.FirstOrDefault(e => e.Name == q);
            s = GetValue(sr.ReadLine());
            modelPlane.BeginnYear = string.IsNullOrEmpty(s) ?
                (int?)null : int.Parse(s);
            modelPlane.Used = GetValue(sr.ReadLine());
            modelPlane.Description = GetValue(sr, "|:", ":|");
            modelPlane.Note = GetValue(sr, "|:", ":|");
            return modelPlane;
        }
        private PlaneType ReadPlaneTypeData(StreamReader sr, IDataSet dataSet)
        {
            PlaneType planeType = new()
            {
                Id = Convert.ToInt32(GetValue(sr.ReadLine())),
                Name = GetValue(sr.ReadLine()),
                Note = GetValue(sr, "|:", ":|")
            };
            return planeType;
        }
    }
}
