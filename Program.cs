using System.Runtime.Versioning;
using Articles;
using ConsoleGUI;

namespace Digital_Storefront
{
    [SupportedOSPlatform("windows")]

    internal class Program
    {        
        static void Main()
        {
            GUI.Initialize(Data.name);

            LoadContent();

            DrawGUI();
            CreateContent();

            GUI.PrintInfo("Press enter/return to 'add' an item to your cart.");
            GUI.PrintInfo("Left and right arrow keys let you switch between textboxes, and up and down scroll through items.");
            GUI.PrintInfo("Press 'Q' to quit. 'Page Down' and 'Page Up' to scroll through this log. :)");
            GUI.ControlTextboxes();
        }

        /// <summary>
        /// Loads article data (mugs and t-shirts) from their respective files, or generates new data if necessary
        /// </summary>
        static void LoadContent()
        {
            if (!Data.LoadMugsFromFile())
            {
                GUI.PrintInfo($"{Data.mugsFilename} not found.");
                if (Data.GenerateMugs())
                    GUI.PrintInfo("Mugs generated from prints data.");
            }

            if (!Data.LoadTShirtsFromFile())
            {
                GUI.PrintInfo($"{Data.tshirtsFilename} not found.");
                if (Data.GenerateTShirts())
                    GUI.PrintInfo("T-Shirts generated from prints data.");
            }
        }

        /// <summary>
        /// Draws non-updating GUI elements such as boxes, lines, and columns.
        /// </summary>
        static void DrawGUI()
        {
            GUI.DrawBox(0, 0, GUI.GetGUIWidth, 9, GUI.BorderStyle.Double, 0, 0, 0, 0, ConsoleColor.DarkBlue, ConsoleColor.Yellow);
            GUI.DrawLine(1, 6, GUI.GetGUIWidth - 2, 0, 0, 0, ConsoleColor.DarkBlue, ConsoleColor.Yellow);
            GUI.DrawBox(0, GUI.GetGUIHeight - 7, GUI.GetGUIWidth, 7, GUI.BorderStyle.Double, 0, 0, 0, 0, ConsoleColor.DarkBlue, ConsoleColor.Yellow);
            GUI.DrawLineZigzag(1, 9, GUI.GetGUIWidth-2, GUI.BorderStyle.Single, true, false, ConsoleColor.DarkRed, ConsoleColor.White);
            GUI.DrawLineZigzag(1, 39, GUI.GetGUIWidth-2, GUI.BorderStyle.Single, true, true, ConsoleColor.DarkRed, ConsoleColor.White);
            GUI.DrawColumnZigzag(0, 9, 32, GUI.BorderStyle.Single, false, false, ConsoleColor.DarkRed, ConsoleColor.White);
            GUI.DrawColumnZigzag(GUI.GetGUIWidth-3, 9, 32, GUI.BorderStyle.Single, false, true, ConsoleColor.DarkRed, ConsoleColor.White);
        }

        /// <summary>
        /// Creates static text fields and interactable, scrolling text boxes on top of previously drawn GUI elements.
        /// </summary>
        static void CreateContent()
        {
            GUI.CreateTextbox(2, 1, 76, 5, Data.logo, ConsoleColor.DarkBlue, ConsoleColor.Cyan);
            GUI.CreateTextbox(GUI.GetGUIWidth - 27, 1, 25, 5, Data.name + "\n" + Data.addressStore, ConsoleColor.DarkBlue, ConsoleColor.Yellow);
            GUI.CreateTextbox(27, 7, 75, 1, Data.slogan, ConsoleColor.DarkBlue, ConsoleColor.Yellow);
            GUI.CreateTextbox(GUI.GetGUIWidth - 27, GUI.GetGUIHeight - 6, 25, 5, "BILLING ADDRESS:" + "\n" + Data.name + "\n" + Data.addressBilling, ConsoleColor.DarkBlue, ConsoleColor.Yellow);
            GUI.CreateTextbox(3, GUI.GetGUIHeight - 5, 2, 1, $"{Data.ItemsInCart:00}", ConsoleColor.Yellow, ConsoleColor.DarkBlue);
            GUI.CreateTextbox(6, GUI.GetGUIHeight - 5, 18, 1, "items in your cart", ConsoleColor.DarkBlue, ConsoleColor.Yellow);
            GUI.CreateTextbox(2, GUI.GetGUIHeight - 3, 30, 1, Data.copyright, ConsoleColor.DarkBlue, ConsoleColor.Yellow);

            GUI.CreateTextbox(3, 11, 61, 2, " MUGS\n" + Data.GetMugsHeaders(), ConsoleColor.DarkRed, ConsoleColor.White);
            GUI.CreateTextbox(3, 13, 61, GUI.GetGUIHeight - 22, Data.GetMugsAsColumns(true, true), null, null, GUI.Interactivity.ScrollAndSelect);
            GUI.CreateTextbox(GUI.GetGUIWidth - 64, 11, 61, 2, " T-SHIRTS\n" + Data.GetTShirtsHeaders(), ConsoleColor.DarkRed, ConsoleColor.White);
            GUI.CreateTextbox(GUI.GetGUIWidth - 64, 13, 61, GUI.GetGUIHeight - 22, Data.GetTShirtsAsColumns(true), null, null, GUI.Interactivity.ScrollOnly);
        }
    }
}