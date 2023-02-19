using Common.ConsoleIO;
using ModelPlaneInfo.ConsoleEditor.Commands;
using ModelPlaneInfo.Data;
using ModelPlaneInfo.IO;
using ModelPlaneInfo.Repositories;

namespace Common.Extensions
{
    class Program
    {

        static DataContext? dataContext;
        static MainManager? mainManager;
        static FileBasedUnitOfWork? unitOfWork;
        static readonly string directory = "files";
        
        static readonly BinarySerializationController bsIoController = new();       

        static void Main(string[] args)
        {
            
            Settings.SetConsoleParam();
            Console.WriteLine("Implementation of classes for the subject area");

            dataContext = new DataContext(bsIoController)
            {
                Directory = directory
            };

            unitOfWork = new FileBasedUnitOfWork(dataContext);
            mainManager = new MainManager(unitOfWork, dataContext);
            mainManager.Load();
            mainManager.Run();

            Console.ReadKey(true);
        }
    }
}
