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

            //Console.SetCursorPosition(2, 17);
            GUI.ControlTextboxes();

            //Console.ReadKey(true);
        }

        const string test = "Ho-oh, oh~\r\nHoo-oh whoa-oh~\r\nYeah yeah-eah-eah\r\n\r\nIf you wanna see some action\r\nGotta be the center of attraction\r\nMake sure that they've got their eyes on you\r\nLike the face that you see on every magazine\r\n\r\nBe the focus of attention\r\nBe the name that everyone must mention\r\nCome out from the shadows, it's your time\r\n'Cause tonight is the night for everyone to see\r\n\r\n(It's natural)\r\nYou know that\r\nThis is where you gotta be\r\nIt must be your destiny\r\n(Sensational)\r\nAnd you believe that\r\nThis is what you've waited for\r\nAnd it's you that they all adore\r\n\r\nBaby, now you feel like number one\r\nShinin' bright for everyone\r\nLivin' out your fantasy\r\nThe brightest star for all to see\r\n\r\nNow you feel like number one\r\nShinin' bright for everyone\r\nLivin' out your fantasy\r\nYou're the brightest star there's ever been\r\n\r\nFeel the heat that's all around you\r\nFlashin' lights and ecstasy surround you\r\nEverybody wants a piece of you\r\nYou're the queen of the scene livin' in a dream\r\n\r\n(It's natural)\r\nYou know that\r\nThis is where you gotta be\r\nIt must be your destiny\r\n(Sensational)\r\nAnd you believe that\r\nThis is what you've waited for\r\nAnd it's you that they all adore\r\n\r\nNow you feel like number one\r\nShinin' bright for everyone\r\nLivin' out your fantasy\r\nThe brightest star for all to see\r\nYeah yeah yeah yeah yeah\r\n\r\nOh, this is what you've waited for\r\nAnd it's you that they all adore\r\n\r\n(Now you feel like number one)\t\t> Now you feel, yeah\r\n(Shinin' bright for everyone)\t\t> For everyone, oh~\r\n(Livin' out your fantasy)\t\t> Livin' out your fantasy\r\n(The brightest star for all to see)\t> The brightest star for all to see\r\n\r\n(Now you feel like number one)\t\t> Now you feel, now you feel like number one\r\n(Shinin' bright for everyone)\r\n(Livin' out your fantasy)\t\t\r\n(The brightest star for all to see)\tYou're the brightest star there's ever be~en, yeah\r\n\r\n\r\n\r\n";

        static void DrawGUI()
        {
            GUI.DrawBox(0, 0, GUI.GetGUIWidth, GUI.GetGUIHeight, GUI.BorderStyle.Double);

            GUI.DrawBox(1, 1, 30, 19, GUI.BorderStyle.Double);
            GUI.CreateTextbox(2, 2, 28, 17, test);


            //GUI.DrawBox(0, GUI.GetGUIHeight - 10, 20, 10, GUI.BorderStyle.Double, GUI.CornerStyle.VerticalJunction, 0, 0, GUI.CornerStyle.HorizontalJunction, ConsoleColor.Black, ConsoleColor.White);
            //GUI.DrawBox(2, 2, 21, 11, 0, 0, 0, 0, 0, ConsoleColor.DarkRed);
            //GUI.DrawColumn(12, 2, 11, 0, GUI.EdgeStyle.HorizontalJunction, GUI.EdgeStyle.HorizontalJunction, ConsoleColor.DarkRed);
            //GUI.DrawLine(2, 7, 11, 0, GUI.EdgeStyle.VerticalJunction, 0, ConsoleColor.DarkRed);
            //GUI.DrawLine(12, 7, 11, 0, GUI.EdgeStyle.Crossing, GUI.EdgeStyle.VerticalJunction, ConsoleColor.DarkRed);
            //GUI.DrawLine(0, 15, GUI.GetGUIWidth, GUI.BorderStyle.Double);
            //GUI.DrawColumn(64, 15, GUI.GetGUIHeight - 15, GUI.BorderStyle.Double);  
        }
    }
}