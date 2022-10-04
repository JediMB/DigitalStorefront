using System.Runtime.Versioning;
using ConsoleGUI;
using Humanizer;

namespace Digital_Storefront
{
    [SupportedOSPlatform("windows")]

    internal class Program
    {        
        static void Main(/*string[] args*/)
        {
            GUI.Initialize("Digital Storefront");

            DrawGUI();

            Console.SetCursorPosition(2, 17);
            Console.Write($"The feast is in {DateTime.UtcNow.AddHours(12.001).Humanize()}");

            Console.ReadKey();
        }

        static void DrawGUI()
        {
            GUI.DrawBox(0, 0, GUI.GetGUIWidth, GUI.GetGUIHeight, GUI.BorderStyle.Double);
            GUI.DrawBox(0, GUI.GetGUIHeight - 10, 20, 10, GUI.BorderStyle.Double, GUI.CornerStyle.VerticalJunction, 0, 0, GUI.CornerStyle.HorizontalJunction, ConsoleColor.Black, ConsoleColor.White);
            GUI.DrawBox(2, 2, 21, 11, 0, 0, 0, 0, 0, ConsoleColor.DarkRed);
            GUI.DrawColumn(12, 2, 11, 0, GUI.EdgeStyle.HorizontalJunction, GUI.EdgeStyle.HorizontalJunction, ConsoleColor.DarkRed);
            GUI.DrawLine(2, 7, 11, 0, GUI.EdgeStyle.VerticalJunction, 0, ConsoleColor.DarkRed);
            GUI.DrawLine(12, 7, 11, 0, GUI.EdgeStyle.Crossing, GUI.EdgeStyle.VerticalJunction, ConsoleColor.DarkRed);
            GUI.DrawLine(0, 15, GUI.GetGUIWidth, GUI.BorderStyle.Double);
            GUI.DrawColumn(64, 15, GUI.GetGUIHeight - 15, GUI.BorderStyle.Double);  
        }
    }
}