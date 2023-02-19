using Common.Interfaces;


namespace ModelPlaneInfo.Entities
{
    [Serializable]
    public partial class PlaneType : IEntity
    {
        public int Id { get; set; }
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
        private string note;

        public PlaneType(string name, string note)
        {
            this.Name = name;
            this.Note = note;
        }

        public PlaneType(string name)
        {
            this.Name = name;
        }

        public PlaneType() { }


    }
}
