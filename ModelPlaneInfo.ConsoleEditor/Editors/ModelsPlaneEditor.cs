using System.Data;
using ModelPlaneInfo.Entities;
using ModelPlaneInfo.ConsoleEditor.Commands;
using ModelPlaneInfo.ConsoleEditor.Controllers;
using ModelPlaneInfo.Repositories.Interfaces;
using Common.Interfaces;
using Common.Extensions;
using Common.ConsoleIO;

namespace ModelPlaneInfo.ConsoleEditor.Editors
{
    public partial class ModelsPlaneEditor : CommandManager
    {
        private readonly IInfoUnitOfWork unitOfWork;
        private IEnumerable<ModelPlane> sortedCollection;
        readonly ICollection<ModelPlane> collection;
        readonly ICollection<PlaneType> collection_Type;

        public ModelsPlaneEditor(IInfoUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            collection = (ICollection<ModelPlane>)unitOfWork.ModelPlanesRepository.GetAll();
            sortedCollection = collection;
            collection_Type = (ICollection<PlaneType>)unitOfWork.PlanesTypeRepository.GetAll();
        }

        private CommandInfo[] commandInfoSort;

        protected override void IniCommandInfoArray()
        {
            commandInfoArray = new CommandInfo[] {
                new CommandInfo("Back...", null),
                new CommandInfo("Add a record...", Add),
                new CommandInfo("Remove record...", Remove),
                new CommandInfo("Delete data...", Clear),
                new CommandInfo("Edit data...", Edit),
                new CommandInfo("Sort data...", Sort),
                new CommandInfo("Filter data about aircraft models...", ModelPlaneFiltration),
            };
            commandInfoSort = new CommandInfo[]
            {
                new CommandInfo("Unselect command", null),
                new CommandInfo("Id", SortId),
                new CommandInfo("By Name", SortName),
                new CommandInfo("By Type", SortType),
                new CommandInfo("Year of start of operation", SortYear),
            };
        }

        private readonly ModelsPlaneFiltrationController modelsPlaneFiltrationController = new();

        private void ModelPlaneFiltration()
        {
            modelsPlaneFiltrationController.SelectFilterCommand();
        }

        IEnumerable<ModelPlane> selectingCollection;

        public IInfoUnitOfWork UnitOfWork => unitOfWork;

        private void ApplyFilters()
        {
            selectingCollection = modelsPlaneFiltrationController.ApplyFilters(sortedCollection);
        }
        protected override void PrepareScreen()
        {
            Console.Clear();
            ApplyFilters();
            Console.WriteLine(selectingCollection.ToLineList("\n\tAircraft models"));
        }

        void Clear()
        {
            collection.Clear();
        }

        #region *** Create new ***
        void Add()
        {

            ModelPlane inst = new();
            if (UnitOfWork is ITestStorage storage)
            {
                storage.CreateTestingData();
            }

            EnteringData(inst);
            try
            {
                inst.Id = collection.Select(e => e.Id).Max() + 1;
            }
            catch (Exception) { inst.Id = 1; }
            collection.Add(inst);
            collection_Type.Clear();
        }
        void EnteringData(ModelPlane inst)
        {
            EnterName(inst);
            SelectTypePlane(inst);
            EnterBeginYear(inst);
            EnterUsed(inst);
            EnterDescription(inst);
            EnterNote(inst);
        }

        private static void EnterNote(ModelPlane inst)
        {
            string note;
            while (true)
            {
                note = Entering.EnterString("\tNote");
                if (ModelPlane.CheckNote(note, out string message))
                {
                    break;
                }
                Console.WriteLine("\t" + message);
            }
            inst.Note = note;
        }

        private static void EnterDescription(ModelPlane inst)
        {
            string description;
            while (true)
            {
                description = Entering.EnterString("\tDescription");
                if (ModelPlane.CheckDescription(description, out string message))
                {
                    break;
                }
                Console.WriteLine("\t" + message);
            }
            inst.Description = description;
        }

        private static void EnterUsed(ModelPlane inst)
        {
            string used;
            while (true)
            {
                used = Entering.EnterString("\tAvailability in use");
                if (ModelPlane.CheckUsed(used, out string message))
                {
                    break;
                }
                Console.WriteLine("\t" + message);
            }
            inst.Used = used;
        }
        private static void EnterBeginYear(ModelPlane inst)
        {
            int? beginnYear;
            while (true)
            {
                beginnYear = Entering.EnterNullableInt32("\tYear of start of operation");
                if (ModelPlane.CheckBeginnYear(beginnYear, out string message))
                {
                    break;
                }
                Console.WriteLine("\t" + message);
            }
            inst.BeginnYear = beginnYear;
        }
        private void SelectTypePlane(ModelPlane inst)
        {
            PlaneType type;
            while (true)
            {
                Console.WriteLine("\tType of aircraft");
                type = SelectType(inst);
                break;
            }
            inst.Type = type;
        }
        private static void EnterName(ModelPlane inst)
        {
            string name;
            while (true)
            {
                name = Entering.EnterString("\tName of the aircraft ");
                if (ModelPlane.CheckName(name, out string message))
                {
                    break;
                }
                Console.WriteLine("\t" + message);
            }
            inst.Name = name;
        }
        #endregion

        private PlaneType SelectType(ModelPlane inst)
        {
            string[] items = new string[] { "Destroyer", "Scout", "Stormtrooper", "Passenger's", "Cargo", "FirePlane", "Sanitary" };
            while (true)
            {
                int i = 1;
                foreach (string item in items)
                {
                    Console.WriteLine($"\t\t{i}.{item}");
                    i++;
                }
                try
                {
                    int n = Entering.EnterInt("\n\tChoose one of the types");

                    switch (n)
                    {
                        case 1:
                            return collection_Type.First(e => e.Name == "Destroyer");

                        case 2:
                            return collection_Type.First(e => e.Name == "Scout");

                        case 3:
                            return collection_Type.First(e => e.Name == "Stormtrooper");

                        case 4:
                            return collection_Type.First(e => e.Name == "Passenger's");

                        case 5:
                            return collection_Type.First(e => e.Name == "Cargo");

                        case 6:
                            return collection_Type.First(e => e.Name == "FirePlane");

                        case 7:
                            return collection_Type.First(e => e.Name == "Sanitary");

                        default:
                            Console.WriteLine("\tThe selected option does not exist");
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\tIncorrect option entered");
                }
            }
        }

        #region *** Remove ***
        void Remove()
        {
            if (collection.Count == 0)
            {
                Console.WriteLine("No records created yet. Add records, " +
                     "to be able to delete them\nPress any key to continue");
                Console.ReadKey(true);
                return;
            }
            try
            {
                int id = Entering.EnterInt("\tEnter the entry code");
                ModelPlane inst = collection.First(e => e.Id == id);
                collection.Remove(inst);
            }
            catch (Exception)
            {
                Console.WriteLine("Record with this Id does not exist" + "\nPress any key to continue");
                Console.ReadKey(true);
            }
        }
        #endregion

        #region *** Edit ***
        private void Edit()
        {
            if (collection.Count == 0)
            {
                Console.WriteLine("\tNo entries, add entries to have " +
                      "able to edit them\n\tPress any key");
                Console.ReadKey(true);
                return;
            }
            int id = Entering.EnterInt("\tEnter the Record Id", 1, collection.Select(e => e.Id).Max());
            try
            {
                EditModelPlane(collection.First(e => e.Id == id));
            }
            catch (Exception)
            {
                Console.WriteLine("\tRecord with this Id does not exist\n\tPress any key");
                Console.ReadKey(true);
            }
        }
        private void EditModelPlane(ModelPlane inst)
        {
            while (true)
            {
                try
                {
                    string name = Entering.EnterString("\tName of the aircraft ");
                    if (name != "") inst.Name = name;

                    Console.WriteLine("\tAircraft type: \n");
                    inst.Type = SelectType(inst);

                    int? beginnYear = Entering.EnterNullableInt32("\tYear of start of operation");
                    if (beginnYear != null) inst.BeginnYear = beginnYear;

                    string used = Entering.EnterString("\tAvailability in use ");
                    if (used != "") inst.Used = used;

                    string description = Entering.EnterString("\tDescription ");
                    if (description != "") inst.Description = description;

                    string note = Entering.EnterString("\tNote ");
                    if (note != "") inst.Note = note;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\t{0}: {1}", ex.GetType().Name, ex.Message);
                }
            }
        }
        #endregion

        #region *** Sort methods ***
        private void Sort()
        {
            if (collection.Count == 0)
            {
                Console.WriteLine("\tNo records, add records to have" +
                    "the ability to sort them\n\tPress any key");
                Console.ReadKey(true);
                return;
            }
            ShowCommandsMenu(commandInfoSort, "\n\tSort by:");
            Command command = EnterCommand(commandInfoSort, "Enter the team number");
            if (command == null)
            {
                return;
            }
            command();
        }

        private void SortId()
        {
            sortedCollection = sortedCollection.OrderBy(e => e.Id);
        }

        private void SortName()
        {
            sortedCollection = sortedCollection.OrderBy(e => e.Name);
        }

        private void SortType()
        {
            sortedCollection = sortedCollection.OrderBy(e => e.Type.Name);
        }

        private void SortYear()
        {
            sortedCollection = sortedCollection.OrderBy(e => e.BeginnYear);
        }
        #endregion

        public void OutModelsPlaneData()
        {
            Console.WriteLine("List of aircraft:");
            foreach (var obj in sortedCollection)
            {
                Console.WriteLine(obj.ToString());
            }
        }
    }
}
