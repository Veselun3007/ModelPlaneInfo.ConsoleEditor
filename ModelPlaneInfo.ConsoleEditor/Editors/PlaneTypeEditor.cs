using Common.ConsoleIO;
using Common.Extensions;
using ModelPlaneInfo.Entities;
using ModelPlaneInfo.ConsoleEditor.Controllers;
using ModelPlaneInfo.ConsoleEditor.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using ModelPlaneInfo.Repositories.Interfaces;
using System.Xml.Linq;
using System.Collections;
using System.Reflection;
using System.Diagnostics.Metrics;

namespace ModelPlaneInfo.ConsoleEditor.Editors
{
    public partial class PlanesTypeEditor : CommandManager
    {

        private IInfoUnitOfWork unitOfWork;

        IEnumerable<PlaneType> sortedCollection;
        ICollection<PlaneType> collection;


        private CommandInfo[] commandInfoSort;

        public PlanesTypeEditor(IInfoUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            collection = (ICollection<PlaneType>)unitOfWork.PlanesTypeRepository.GetAll();
            sortedCollection = collection;
        }

        protected override void IniCommandInfoArray()
        {
            commandInfoArray = new CommandInfo[] {
                 new CommandInfo("Back...", null),
                 new CommandInfo("Add record...", Add),
                 new CommandInfo("Remove record...", Remove),
                 new CommandInfo("Delete data...", Clear),
                 new CommandInfo("Edit data...", Edit),
                 new CommandInfo("Sort data...", Sort),
                 new CommandInfo("Filter data...", PlaneTypeFiltration),
             };
            commandInfoSort = new CommandInfo[]
            {
                 new CommandInfo("Unselect command", null),
                 new CommandInfo("Id", SortId),
                 new CommandInfo("By Type", SortParent),
            };
        }

        private PlaneTypeFiltrationController planeTypeFiltrationController =
            new PlaneTypeFiltrationController();

        private void PlaneTypeFiltration()
        {
            planeTypeFiltrationController.SelectFilterCommand();
        }

        IEnumerable<PlaneType> selectingCollection;

        private void ApplyFilters()
        {
            selectingCollection = planeTypeFiltrationController.ApplyFilters(sortedCollection);
        }


        protected override void PrepareScreen()
        {
            Console.Clear();
            ApplyFilters();
            OutData();
        }

        void OutData()
        {
            Console.WriteLine("\tList of aircraft types:\n");

            CalculationIndents(out int nameId, out int nameWidth, out int noteWidth);
            OutputHeader(nameId, nameWidth, noteWidth);
            OutputData(nameId, nameWidth, noteWidth);
        }

        void Add()
        {
            int password = Entering.EnterInt("\tEnter the administrator password: ");  //1234
            if (password == 1234)
            {
                PlaneType inst = new();
                try
                {
                    EnteringType(inst);
                    inst.Id = collection.Select(e => e.Id).Max() + 1;
                }
                catch (Exception) { inst.Id = 1; }
                collection.Add(inst);

            }
            else
            {
                Console.WriteLine("\tThe password you entered is not correct." + 
                    "Check that the password is correct");
                Console.ReadKey(true);
            }
        }

        private static void EnteringType(PlaneType inst)
        {
            string name;
            while (true)
            {
                name = Entering.EnterString("\tType of aircraft ");
                if (PlaneType.CheckName(name, out string message))
                {
                    break;
                }
                Console.WriteLine("\t" + message);
            }
            inst.Name = name;

            string note;
            while (true)
            {
                note = Entering.EnterString("\tNote");
                if (PlaneType.CheckNote(note, out string message))
                {
                    break;
                }
                Console.WriteLine("\t" + message);
            }
            inst.Note = note;

        }

        void Clear()
        {
            collection.Clear();
        }

        void Remove()
        {
            int password = Entering.EnterInt("\tEnter the administrator password: ");  //1234
            if (password == 1234)
            {
                if (collection.Count == 0)
                {
                    Console.WriteLine("tNo records have been created yet. Add records, " +
                        "to be able to delete them\nPress any key to continue");
                    Console.ReadKey(true);
                    return;
                }
                try
                {
                    int id = Entering.EnterInt("\tEnter the record number");
                    PlaneType inst = collection.First(e => e.Id == id);
                    collection.Remove(inst);
                }
                catch (Exception)
                {
                    Console.WriteLine("Record with this Id does not exist" + "\nPress any key to continue");
                    Console.ReadKey(true);
                }
            }
            else
            {
                Console.WriteLine("\tThe password you entered is not correct." + 
                    "\nCheck that the password is correct");
                Console.ReadKey(true);
            }
        }
       
        #region *** Edit info ***
        private void Edit()
        {
            if (collection.Count == 0)
            {
                Console.WriteLine("\tNo records, add records to have" +
                    "the ability to edit them\n\tPress any key");
                Console.ReadKey(true);
                return;
            }
            int password = Entering.EnterInt("\tEnter the administrator password: ");  //1234
            if (password == 1234)
            {

                int id = Entering.EnterInt("Enter the Record Id", 1, collection.Select(e => e.Id).Max());
                try
                {
                    EditPlanType(collection.First(e => e.Id == id));
                }
                catch (Exception)
                {
                    Console.WriteLine("\tRecord with this Id does not exist\n\tPress any key");
                    Console.ReadKey(true);
                }
            }
            else
            {
                Console.WriteLine("\tThe password you entered is not correct." +
                  "\n\tCheck that the password is correct");
                Console.ReadKey(true);
            }
        }
        private static void EditPlanType(PlaneType inst)
        {
            while (true)
            {
                try
                {

                    string parent = Entering.EnterString("Aircraft type");
                    if (parent != "") inst.Name = parent;

                    string note = Entering.EnterString("tNote");
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
                Console.WriteLine("\tNo entries, add entries to have " +
                      "the ability to sort them\n\tPress any key");
                Console.ReadKey(true);
                return;
            }
            ShowCommandsMenu(commandInfoSort, "\n Sort by:");
            Command command = EnterCommand(commandInfoSort, "Enter the command number");
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

        private void SortParent()
        {
            sortedCollection = sortedCollection.OrderBy(e => e.Name);
        }
        #endregion

        #region *** Output info ***
        public void OutTypePlaneData()
        {
            Console.WriteLine("\tList of aircraft types:\n");

            CalculationIndents(out int nameId, out int nameWidth, out int noteWidth);
            OutputHeader(nameId, nameWidth, noteWidth);
            OutputData(nameId, nameWidth, noteWidth);
        }
        private void CalculationIndents(out int nameId, out int nameWidth, out int noteWidth)
        {
            nameId = sortedCollection.Max(x => ((dynamic)x).Id.ToString().Length + 2);
            nameWidth = sortedCollection.Max(x => ((dynamic)x).Name.Length);
            noteWidth = sortedCollection.Max(x => ((dynamic)x).Note?.Length);
        }
        private static void OutputHeader(int nameId, int nameWidth, int noteWidth)
        {
            Console.WriteLine("\t" + new string('-', nameId + nameWidth + noteWidth + 10));
            Console.WriteLine("\t| {0,-" + nameId + "} | {1,-" + nameWidth + "} | {2,-" + noteWidth + "} |", "Id", "Type", "Note");
            Console.WriteLine("\t" + new string('-', nameId + nameWidth + noteWidth + 10));
        }
        private void OutputData(int nameId, int nameWidth, int noteWidth)
        {
            foreach (var row in sortedCollection)
            {
                Console.WriteLine("\t| {0,-" + nameId + "} | {1,-" + nameWidth + "} | {2,-" + noteWidth + "} |",
                    ((dynamic)row).Id, ((dynamic)row).Name, ((dynamic)row).Note);
                Console.WriteLine("\t" + new string('-', nameId + nameWidth + noteWidth + 10));
            }
        }
        #endregion
    }
}
