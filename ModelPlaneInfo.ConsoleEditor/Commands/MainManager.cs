using Common.ConsoleIO;
using Common.Interfaces;
using ModelPlaneInfo.ConsoleEditor.Editors;
using ModelPlaneInfo.Data;
using ModelPlaneInfo.Entities;
using ModelPlaneInfo.IO;
using ModelPlaneInfo.Repositories.Interfaces;

namespace ModelPlaneInfo.ConsoleEditor.Commands
{
    class MainManager : CommandManager
    {      
        readonly List<ModelPlane> _modelPlanes;
        readonly List<PlaneType> _planeTypes;

        private readonly ModelsPlaneEditor _modelsPlaneEditor;
        private readonly PlanesTypeEditor _planesTypeEditor;
        private readonly IInfoUnitOfWork _unitOfWork;
        private readonly DataContext _context;

        private static readonly BinarySerializationController _bsIoController = new();
        private static readonly XmlFileIoController _xmlFileIoController = new();
        private static readonly TextFileIoController _textFileIoController = new();

        public MainManager(IInfoUnitOfWork unitOfWork, DataContext dataContext)
        {
            this._context = dataContext;
            this._unitOfWork = unitOfWork;
            _modelsPlaneEditor = new ModelsPlaneEditor(unitOfWork);
            _planesTypeEditor = new PlanesTypeEditor(unitOfWork);

            _modelPlanes = (List<ModelPlane>)unitOfWork.ModelPlanesRepository.GetAll();
            _planeTypes = (List<PlaneType>)unitOfWork.PlanesTypeRepository.GetAll();
        }

        protected override void IniCommandInfoArray()
        {
            commandInfoArray = new CommandInfo[]
            {
            new CommandInfo("Exit", null),
            new CommandInfo("Create test data", CreateTestingData),
            new CommandInfo("Deleting data", Clear),
            new CommandInfo("Detailed information", OutDetails),
            new CommandInfo("Save data", Save),
            new CommandInfo("Editing aircraft data", RunModelsPlaneEditor),
            new CommandInfo("Editing type data", RunPlanesTypeEditor),
            };
        }

        private void Clear()
        {
            _modelPlanes.Clear();
            _planeTypes.Clear();
        }

        private void RunPlanesTypeEditor()
        {
            _planesTypeEditor.Run();
        }

        private void RunModelsPlaneEditor()
        {
            _modelsPlaneEditor.Run();
        }

        private void CreateTestingData()
        {
            if (_unitOfWork is ITestStorage storage)
            {
                storage.CreateTestingData();
            }
            else
            {
                Console.WriteLine("\tFailed to create test data ");
                Console.ReadKey(true);
            }
        }

        private void OutDetails()
        {
            Console.WriteLine();
            _modelsPlaneEditor.OutModelsPlaneData();
            Console.WriteLine();
            _planesTypeEditor.OutTypePlaneData();
            Console.WriteLine("\n\tPress any key to continue");
            Console.ReadKey(true);
        }

        private void Save()
        {
            string[] items = new string[] {"\t1. mobd", "\t2. xml",  "\t3. txt", "\t4. *cancel saving data*" };

            Console.WriteLine("\tChoose a file extension: ");

            while (true)
            {
                try
                {
                    foreach (string item in items) 
                        Console.WriteLine(item);
                    int index = Entering.EnterInt("\n\tChoose one of the options ", 1, 4);

                    switch (index)
                    {
                        case 1:
                            _context.FileIoController = _bsIoController;
                            _bsIoController.FileExtension = ".mobd";
                            break;
                        case 2:
                            _context.FileIoController = _xmlFileIoController;
                            break;
                        case 3:
                            _context.FileIoController = _textFileIoController;
                            break;
                        case 4:
                            return;
                        default:
                            Console.WriteLine("\tNo data saved");
                            Console.ReadKey(true);
                            return;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\tThe selected option does not exist");
                    Console.ReadKey(true);
                }
                _context.Save();
                Console.WriteLine("\n\tData saved successfully");
                Console.ReadKey(true);
                break;


            }
        }

        protected override void PrepareScreen()
        {
            Console.Clear();
            OutStatistics();
        }

        private void OutStatistics()
        {
            Console.WriteLine("\n\tStatistical information:" +
               "\n\t\t{0,-11} {1}\n\t\t{2,-11} {3}",
               "Aircraft models", _modelPlanes.Count,
               "Types of aircraft", _planeTypes.Count);

        }

        #region *** Load data ***
        public void Load()
        {

        Mark:
            if (_modelPlanes.Count != 0 || _planeTypes.Count != 0)
                return;
            if (!File.Exists(@"files\ModelPlanesInfo.mobd") &&
                !File.Exists(@"files\ModelPlanesInfo.xml") &&
                !File.Exists(@"files\ModelPlanesInfo.txt"))
                return;

            Console.Write("\n\tLoad data?\n\t\tPress Esc to cancel the action." +
                "\n\t\tTo confirm, press any other key...");
            if (Console.ReadKey().Key == ConsoleKey.Escape)
                return;
            Console.WriteLine();

            string[] items = Directory.GetFiles("files");
            string[] menuItems = new string[] { "\t1. mobd", "\t2. xml", "\t3. txt", "\t4. *cancel load data*" };
           
            Console.WriteLine("\n\tChoose a file to save data: ");

            while (true)
            {
                try
                {
                    foreach (string item in menuItems)
                        Console.WriteLine(item);

                    int n = Entering.EnterInt("\n\tChoose one of the options ", 1, 4);
                    switch (n)
                    {
                        case 1:
                            _context.FileIoController = _bsIoController;
                            _bsIoController.FileExtension = ".mobd";
                            HelpLoad();
                            return;

                        case 2:
                            _context.FileIoController = _xmlFileIoController;
                            HelpLoad();
                            return;

                        case 3:
                            _context.FileIoController = _textFileIoController;
                            HelpLoad();
                            return;

                        case 4:
                            return;

                        default:
                            Console.WriteLine("\n\tInvalid data file");
                            Console.ReadKey(true);
                            Console.WriteLine();
                            goto Mark;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\tThe selected option does not exist");
                    Console.ReadKey(true);
                    break;
                }
            }
        }
        private void HelpLoad()
        {
            try
            {
                _context.Load();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                return;
            }
            Console.WriteLine();
            Console.WriteLine("\tData uploaded successfully");
            Console.ReadKey(true);
        }
    }
    #endregion
}

