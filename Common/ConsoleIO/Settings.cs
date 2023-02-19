using System.Text;

namespace Common.ConsoleIO
{
    public static class Settings
    {
        public static void SetConsoleParam()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.Title = "ModelPlaneInfo.ConsoleEditor";
            Console.ForegroundColor = ConsoleColor.White;
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
        }
    }
}
