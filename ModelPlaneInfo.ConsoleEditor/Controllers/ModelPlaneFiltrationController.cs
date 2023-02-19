using Common.ConsoleIO;
using ModelPlaneInfo.Entities;
using System.Text.RegularExpressions;

namespace ModelPlaneInfo.ConsoleEditor.Controllers
{
    public class ModelsPlaneFiltrationController
    {

        delegate void Filter(ref IEnumerable<ModelPlane> collection);
        readonly Filter emptyFilter = delegate { };
        readonly Filter[] filters = new Filter[Enum.GetValues(typeof(FilterId)).Length];
        enum FilterId { None, ByName, ByType, ByBeginnYear, ByUsed };
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

                    new FilterCommandInfo("The name starts with...", FilterId.ByName, NameStartWith, EnterNameStart),
                    new FilterCommandInfo("The title contains...", FilterId.ByName, NameContains, EnterNameSubstring),
                    new FilterCommandInfo("The specified aircraft type...", FilterId.ByType, TypeName, EnterTypeName),
                    new FilterCommandInfo("The type starts with...", FilterId.ByName, TypeStartWith, EnterTypeStart),
                    new FilterCommandInfo("The year of the start of operation is indicated", FilterId.ByBeginnYear, BeginnYearUnknown, null ),
                    new FilterCommandInfo("Availability in use is indicated...", FilterId.ByUsed, UsedUnknown, null),

                    new FilterCommandInfo("Unfilter by name...", FilterId.ByName, emptyFilter, null),
                    new FilterCommandInfo("Cancel filtering by year of operation...", FilterId.ByBeginnYear, emptyFilter, null),
                    new FilterCommandInfo("Cancel filtering according to availability in operation", FilterId.ByUsed, emptyFilter, null),
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

        private void TypeName(ref IEnumerable<ModelPlane> collection)
        {
            collection = collection.Where(e => e.Type.Name.Equals(typeName,
                StringComparison.InvariantCultureIgnoreCase));
        }

        private string typeStart = "";

        private void EnterTypeStart()
        {
            typeStart = Entering.EnterString("Enter the beginning of the type ", 1, 30);
        }

        private void TypeStartWith(ref IEnumerable<ModelPlane> collection)
        {
            collection = from e in collection
                         where e.Type.Name.StartsWith(typeStart)
                         select e;
        }

        private void BeginnYearUnknown(ref IEnumerable<ModelPlane> collection)
        {
            collection = from e in collection
                         where e.BeginnYear != null
                         select e;
        }

        private void UsedUnknown(ref IEnumerable<ModelPlane> collection)
        {
            collection = from e in collection
                         where e.Used != ""
                         select e;
        }


        private string nameStart = "";
        private void EnterNameStart()
        {
            nameStart = Entering.EnterString("Enter the beginning of the name ", 1, 30);
        }

        private void NameStartWith(ref IEnumerable<ModelPlane> collection)
        {
            collection = collection.Where(e => e.Name.StartsWith(nameStart,
                StringComparison.InvariantCultureIgnoreCase));
        }

        private string nameSubstring = "";

        private void EnterNameSubstring()
        {
            nameSubstring = Entering.EnterString("Enter a fragment of the name ", 1, 30);
        }
        private void NameContains(ref IEnumerable<ModelPlane> collection)
        {
            collection = collection.Where(e => e.Name.IndexOf(nameSubstring,
                StringComparison.InvariantCultureIgnoreCase) >= 0);
        }

        public ModelsPlaneFiltrationController()
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

        public IEnumerable<ModelPlane> ApplyFilters(IEnumerable<ModelPlane> initialCollection)
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
