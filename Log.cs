namespace BumsBot
{
    public class Log
    {
        public static void Show(string Game, string Account, string Message, ConsoleColor Color)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"[{Game}] ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"[{Account}] ");
            Console.ForegroundColor = Color;
            Console.WriteLine(Message);
            Console.ResetColor();
        }
    }
}