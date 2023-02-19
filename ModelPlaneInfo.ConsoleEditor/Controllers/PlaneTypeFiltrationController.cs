using Common.ConsoleIO;
using ModelPlaneInfo.Entities;
using System.Text.RegularExpressions;

namespace ModelPlaneInfo.ConsoleEditor.Controllers
{
    public class PlaneTypeFiltrationController
    {
        delegate void Filter(ref IEnumerable<PlaneType> collection);
        readonly Filter emptyFilter = delegate { };
        readonly Filter[] filters = new Filter[Enum.GetValues(typeof(FilterId)).Length];
        enum FilterId { None, ByName };
        private void InitializeFilters()
        {
            for (int i = 0; i < filters.Length; i++)
            {
                filters[i] = emptyFilter;
            }
        }
        class FilterCommandInfo
        {
            public string name;
            public FilterId filterId;
            public Filter? filter;
            public Action? enterCommand;

            public FilterCommandInfo(string name, FilterId filterId, Filter filter, Action enterCommand)
            {
                this.filterId = filterId;
                this.name = name;
                this.filter = filter;
                this.enterCommand = enterCommand;
            }
        }

        FilterCommandInfo[] filterCommandsInfo;

        private void IniFilterCommandsInfo()
        {
            filterCommandsInfo = new FilterCommandInfo[]
            {
                    new FilterCommandInfo("Excellent team selection...", FilterId.None, null, null),
                    new FilterCommandInfo("Remove all existing filters", FilterId.None, null, InitializeFilters),

                    new FilterCommandInfo("The specified aircraft type...", FilterId.ByName, TypeName, EnterTypeName),
                    new FilterCommandInfo("The type starts with...", FilterId.ByName, TypeStartWith, EnterTypeStart),

                    new FilterCommandInfo("Unfilter by type...", FilterId.ByName, emptyFilter, null),
                };
        }

        private string typeName = "";
        private void EnterTypeName()
        {
            typeName = Entering.EnterString("Enter Type ",
               @"\A(Destroyer|Scout|Stormtrooper|Passenger's|Cargo|FirePlane|Sanitary)\z",
                 "Need to enter Destroyer|Scout|Stormtrooper|Passenger's|Cargo|FirePlane|Sanitary",
                 RegexOptions.IgnoreCase);
        }
        private void TypeName(ref IEnumerable<PlaneType> collection)
        {
            collection = collection.Where(e => e.Name.Equals(typeName,
                StringComparison.InvariantCultureIgnoreCase));
        }
        private string nameStart = "";
        private void EnterTypeStart()
        {
            nameStart = Entering.EnterString("Enter the beginning of the type ", 1, 30);
        }
        private void TypeStartWith(ref IEnumerable<PlaneType> collection)
        {
            collection = from e in collection
                         where e.Name.StartsWith(nameStart)
                         select e;
        }
        public PlaneTypeFiltrationController()
        {
            InitializeFilters();
            IniFilterCommandsInfo();
        }
        private void ShowFilterCommandsMenu()
        {
            Console.WriteLine("List of filtering commands: \n\t");
            for (int i = 0; i < filterCommandsInfo.Length; i++)
            {
                Console.WriteLine("\t{0,2} - {1}", i, filterCommandsInfo[i].name);
            }
        }
        public void SelectFilterCommand()
        {
            ShowFilterCommandsMenu();
            Console.WriteLine("");
            int number = Entering.EnterInt("Enter the filter command number", 0, filterCommandsInfo.Length - 1);
            var commandInfo = filterCommandsInfo[number];
            if (commandInfo.filterId != FilterId.None)
            {
                if (commandInfo.filter == emptyFilter)
                {
                    filters[(int)commandInfo.filterId] = emptyFilter;
                }
                else
                {
                    filters[(int)commandInfo.filterId] += commandInfo.filter;
                }
            }
            if (commandInfo.enterCommand != null)
            {
                commandInfo.enterCommand();
            }
        }

        public IEnumerable<PlaneType>ApplyFilters(IEnumerable<PlaneType> initialCollection)
        {
            var selectedInstances = initialCollection;
            foreach (Filter filter in filters)
            {
                filter(ref selectedInstances);
            }
            return selectedInstances;
        }

    }
}
