using System.Runtime.Versioning;
using Articles;
using ConsoleGUI;
using Humanizer;

namespace Digital_Storefront
{
    [SupportedOSPlatform("windows")]

    internal class Program
    {        
        static void Main(/*string[] args*/)
        {
            GUI.Initialize(Data.name);

            LoadContent();

            DrawGUI();
            CreateContent();

            GUI.PrintInfo("Left and right arrow keys let you switch between textboxes, and up and down scroll through items.");
            GUI.PrintInfo("Press 'Q' to quit. 'Page Down' and 'Page Up' to scroll through this log. :)");
            GUI.ControlTextboxes();
        }

        //const string numberone = "Ho-oh, oh~\r\nHoo-oh whoa-oh~\r\nYeah yeah-eah-eah\r\n\r\nIf you wanna see some action\r\nGotta be the center of attraction\r\nMake sure that they've got their eyes on you\r\nLike the face that you see on every magazine\r\n\r\nBe the focus of attention\r\nBe the name that everyone must mention\r\nCome out from the shadows, it's your time\r\n'Cause tonight is the night for everyone to see\r\n\r\n(It's natural)\r\nYou know that\r\nThis is where you gotta be\r\nIt must be your destiny\r\n(Sensational)\r\nAnd you believe that\r\nThis is what you've waited for\r\nAnd it's you that they all adore\r\n\r\nBaby, now you feel like number one\r\nShinin' bright for everyone\r\nLivin' out your fantasy\r\nThe brightest star for all to see\r\n\r\nNow you feel like number one\r\nShinin' bright for everyone\r\nLivin' out your fantasy\r\nYou're the brightest star there's ever been\r\n\r\nFeel the heat that's all around you\r\nFlashin' lights and ecstasy surround you\r\nEverybody wants a piece of you\r\nYou're the queen of the scene livin' in a dream\r\n\r\n(It's natural)\r\nYou know that\r\nThis is where you gotta be\r\nIt must be your destiny\r\n(Sensational)\r\nAnd you believe that\r\nThis is what you've waited for\r\nAnd it's you that they all adore\r\n\r\nNow you feel like number one\r\nShinin' bright for everyone\r\nLivin' out your fantasy\r\nThe brightest star for all to see\r\nYeah yeah yeah yeah yeah\r\n\r\nOh, this is what you've waited for\r\nAnd it's you that they all adore\r\n\r\n(Now you feel like number one)\t\t> Now you feel, yeah\r\n(Shinin' bright for everyone)\t\t> For everyone, oh~\r\n(Livin' out your fantasy)\t\t> Livin' out your fantasy\r\n(The brightest star for all to see)\t> The brightest star for all to see\r\n\r\n(Now you feel like number one)\t\t> Now you feel, now you feel like number one\r\n(Shinin' bright for everyone)\r\n(Livin' out your fantasy)\t\t\r\n(The brightest star for all to see)\tYou're the brightest star there's ever be~en, yeah\r\n\r\n\r\n\r\n";
        //const string pussycats = "Josie and the Pussycats\r\nLong tails and ears for hats\r\nGuitars, and sharps and flats\r\nNeat, sweet, a groovy song\r\nYou're invited, come along\r\n\r\nHurry, hurry\r\n\r\nSee you all in Persia, or maybe France\r\nWe could be in India, or perchance\r\nGroove with us in Bangkok, makes no difference\r\nWe're involved with this or that\r\nEverywhere the action's at\r\n\r\nCome along now\r\n\r\nJosie and the Pussycats\r\nNo time for purrs and pats\r\nWon't run when they hear scat\r\nJosie and the Pussycats\r\n\r\n\r\nHurry, hurry\r\n\r\nSee you all in Persia, or maybe France\r\nWe could be in India, or perchance\r\nGroove with us in Bangkok, makes no difference\r\nWe're involved with this or that\r\nEverywhere the action's at\r\n\r\nCome along now\r\n\r\nJosie and the Pussycats\r\nNo time for purrs and pats\r\nWon't run when they hear scat\r\nThere where the plot begins\r\nCome on, watch the good guys win\r\nJosie and the Pussycats\r\nJosie and the Pussycats\r\n\r\n\r\nJosie and the Pussycats\r\nJosie and the Pussycats\r\nJosie and the Pussycats\r\nJosie and the Pussycats\r\n\r\nJosie and the Pussycats\r\nJosie and the Pussycats\r\nJosie and the Pussycats\r\nJosie and the Pussycats\r\n\r\nJosie\r\nJosie\r\nJosie and the Pussycats\r\nJosie and the Pussycats\r\n\r\nJosie and the Pussycats\r\nJosie\r\nJosie and the Pussycats\r\nJosie and the Pussycats\r\n";
        //const string cyborgs = "fukisusabu kaze ga  yoku ni au\r\nku nin no senki to  hito no iu\r\ndaga wareware wa  ai no tame\r\ntatakai wasureta  hito no tame\r\nnamida de wataru chi no taiga\r\nyumemite hashiru shi no kouya\r\nCyborg senshi  taga tame ni tataku\r\nCyborg senshi  taga tame ni tataku\r\n\r\ntomurai no kane ga  yoku ni au\r\njigoku no shisha to  hito no iu\r\ndaga wareware wa  ai no tame\r\ntataki wasureta  hito no tame\r\nyami oiharau  toki no kane\r\nasu no yoake wo  tsugeru kane\r\nCyborg senshi  taga tame ni tataku\r\nCyborg senshi  taga tame ni tataku\r\n\r\ndaga wareware wa  ai no tame\r\ntatakai wasureta  hito no tame\r\nnamida de wataru chi no taiga\r\nyumemite hashiru shi no kouya\r\nCyborg senshi  taga tame ni tataku\r\nCyborg senshi  taga tame ni tataku";

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

        static void DrawGUI()
        {
            GUI.DrawBox(0, 0, GUI.GetGUIWidth, 9, GUI.BorderStyle.Double, 0, 0, 0, 0, ConsoleColor.DarkBlue, ConsoleColor.Yellow);
            GUI.DrawLine(1, 6, GUI.GetGUIWidth - 2, 0, 0, 0, ConsoleColor.DarkBlue, ConsoleColor.Yellow);
            GUI.DrawBox(0, GUI.GetGUIHeight - 6, GUI.GetGUIWidth, 6, GUI.BorderStyle.Double, 0, 0, 0, 0, ConsoleColor.DarkBlue, ConsoleColor.Yellow);
            GUI.DrawLineZigzag(1, 9, GUI.GetGUIWidth-2, GUI.BorderStyle.Single, GUI.ZigzagStyle.Regular, true);
            GUI.DrawColumnZigzag(0, 9, 32, GUI.BorderStyle.Single, GUI.ZigzagStyle.Regular);
            GUI.DrawColumnZigzag(GUI.GetGUIWidth-2, 9, 32, GUI.BorderStyle.Single, GUI.ZigzagStyle.Reversed);
            GUI.DrawLineZigzag(1, 39, GUI.GetGUIWidth-2, GUI.BorderStyle.Single, GUI.ZigzagStyle.Reversed, true);
        }
        
        static void CreateContent()
        {
            GUI.CreateTextbox(2, 1, 76, 5, Data.logo, ConsoleColor.DarkBlue, ConsoleColor.Cyan);
            GUI.CreateTextbox(GUI.GetGUIWidth - 27, 1, 25, 5, Data.name + "\n" + Data.addressStore, ConsoleColor.DarkBlue, ConsoleColor.Yellow);
            GUI.CreateTextbox(27, 7, 75, 1, Data.slogan, ConsoleColor.DarkBlue, ConsoleColor.Yellow);
            GUI.CreateTextbox(GUI.GetGUIWidth - 27, GUI.GetGUIHeight - 5, 25, 4, Data.name + "\n" + Data.addressBilling, ConsoleColor.DarkBlue, ConsoleColor.Yellow);

            GUI.CreateTextbox(2, 11, 62, 2, "MUGS\n" + Data.GetMugsHeaders(), ConsoleColor.DarkGray, ConsoleColor.White);
            GUI.CreateTextbox(2, 13, 62, GUI.GetGUIHeight - 22, Data.GetMugsAsColumns(true, true), null, null, GUI.Interactable.Yes);
            GUI.CreateTextbox(GUI.GetGUIWidth - 64, 11, 62, 2, "T-SHIRTS\n" + Data.GetTShirtsHeaders(), ConsoleColor.DarkGray, ConsoleColor.White);
            GUI.CreateTextbox(GUI.GetGUIWidth - 64, 13, 62, GUI.GetGUIHeight - 22, Data.GetTShirtsAsColumns(true), null, null, GUI.Interactable.Yes);
        }
    }
}