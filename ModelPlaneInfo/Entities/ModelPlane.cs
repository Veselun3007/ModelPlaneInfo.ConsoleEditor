using System;
using Common.Interfaces;
using Common.Context.StringFormatters;
using Common.Entities;
using Common.Context.Extensions;

namespace ModelPlaneInfo.Entities
{
    [Serializable]
    public partial class ModelPlane : OutString, IEntity
    {
        public string Name
        {
            get { return name; }
            set
            {
                if (!CheckName(value, out string message))
                {
                    throw new ArgumentException(message);
                }
                name = value;
            }
        }
        public int? BeginnYear
        {
            get { return beginnYear; }
            set
            {
                if (!CheckBeginnYear(value, out string message))
                {
                    throw new ArgumentException(message);
                }
                beginnYear = value;
            }
        }
        public string Used
        {
            get { return used; }
            set
            {
                if (!CheckUsed(value, out string message))
                {
                    throw new ArgumentException(message);
                }
                used = value;
            }
        }
        public PlaneType Type { get => type; set => type = value; }
        public string Description
        {
            get { return description; }
            set
            {
                if (!CheckDescription(value, out string message))
                {
                    throw new ArgumentException(message);
                }
                description = value;
            }
        }
        public string Note
        {
            get { return note; }
            set
            {
                if (!CheckNote(value, out string message))
                {
                    throw new ArgumentException(message);
                }
                note = value;
            }
        }

        private string name;
        private PlaneType type;
        private int? beginnYear;
        private string used;
        private string description;
        private string note;

        public ModelPlane(string name, PlaneType type,
            int? beginnyear, string used, string description, string note)
        {
            Name = name;
            Type = type;
            BeginnYear = beginnyear;
            Used = used;
            Description = description;
            Note = note;
        }
        public ModelPlane(string name, PlaneType type, int? beginnyear, string used)
        {
            Name = name;
            Type = type;
            BeginnYear = beginnyear;
            Used = used;
        }

        public ModelPlane(PlaneType type)
        {
            Type = type;
        }

        public ModelPlane(string name, int? beginnyear, string used)
        {
            Name = name;
            BeginnYear = beginnyear;
            Used = used;
        }
        public ModelPlane() { }

        public override string ToMembersString()
        {
            StringFormatter.Current.LineLength = 60;
            return string.Format("Name: {0, 2}    "
                + "Type: {1, 2}    "
                + "Year of start of operation: {2, 2}    "
                + "Availability in use: {3, 2}    \n"
                + "Description: {4}\n"
                + "Note: {5}\n",
                Name,
                Type == null ? "" : type.Name,
                BeginnYear,
                Used,
                string.IsNullOrWhiteSpace(Description) ? "" : Description.ToIndentedLineBlock(2),
                string.IsNullOrWhiteSpace(Note) ? "" : Note.ToIndentedLineBlock(2)
                );

        }
    }
}
