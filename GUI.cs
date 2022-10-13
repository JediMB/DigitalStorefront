using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using static ConsoleGUI.GUI;

namespace ConsoleGUI
{
    [SupportedOSPlatform("windows")]

    public static partial class GUI
    {
        private static readonly int _guiWidth = 128;
        private static readonly int _guiHeight = 48;
        private static int _logEntries = 0;
        private const ConsoleColor _guiColor = ConsoleColor.Gray;
        private const ConsoleColor _guiTextColor = ConsoleColor.Black;

        private const string _lineH = "─═";
        private const string _lineV = "│║";
        private const string _cornerTL = "┌╔├╠┬╦┼╬";
        private const string _cornerTR = "┐╗┤╣┬╦┼╬";
        private const string _cornerBL = "└╚├╠┴╩┼╬";
        private const string _cornerBR = "┘╝┤╣┴╩┼╬";
        //private const string _block = "■█▀▄";
        //private const string _halftone = "░▒▓";
        //private const string _triangleT = "▵▴△▲";
        //private const string _triangleB = "▿▾▽▼";
        //private const string _triangleL = "◃◂◁◀";
        //private const string _triangleR = "▹▸▷▶";
        //private const string _triangleTL = "◸◤";
        //private const string _triangleTR = "◹◥";
        //private const string _triangleBL = "◺◣";
        //private const string _triangleBR = "◿◢";


        public enum BorderStyle
        {
            Single,
            Double
        }

        public enum EdgeStyle
        {
            None = 0,
            VerticalJunction = 2,
            HorizontalJunction = 4,
            Crossing = 6
        }

        public enum CornerStyle
        {
            Corner = 0,
            VerticalJunction = 2,
            HorizontalJunction = 4,
            Crossing = 6
        }

        public enum ZigzagStyle
        {
            Regular = 0,
            Reversed = 1
        }

        public static int GetGUIWidth { get => _guiWidth; }
        public static int GetGUIHeight { get => _guiHeight; }

        public static void Initialize(string windowTitle)
        {
            Console.BufferWidth = GetGUIWidth;
            Console.BufferHeight = GetGUIHeight + 1;
            Console.WindowWidth = GetGUIWidth;
            Console.WindowHeight = GetGUIHeight + 1;

            DisableResize();

            Console.CursorVisible = false;
            Console.Title = windowTitle;
            Console.BackgroundColor = _guiColor;
            Console.ForegroundColor = _guiTextColor;
            Console.Clear();
        }

        static void DisableResize()
        {
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
                PrintInfo($"DeleteMenu(..) says: '{DeleteMenu(sysMenu, 0xF000, 0x00000000)}'");
        }

        [DllImport("user32.dll")]
        private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool pRevert);
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        
        public static void PrintInfo(string output)
        {
            if (errorLog.Text == string.Empty)
                errorLog.Text = $"({_logEntries:00}) : {output}";
            else
                errorLog.Text = $"({_logEntries:00}) : {output}\n{errorLog.Text}";

            errorLog.Render();
            _logEntries++;
        }

        private static void CharAtPosition(char output, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(output);
        }

        public static void DrawLineZigzag(int startX, int y, int width, BorderStyle borderStyle = 0,
            ZigzagStyle zigzagStyle = ZigzagStyle.Regular, bool straightEdge = false, ConsoleColor? bgColor = null, ConsoleColor? textColor = null)
        {
            try
            {
                // Throw an exception if a parameter value would lead to drawing outside the console buffer...
                if (startX < 0 || startX > GetGUIWidth) throw new ArgumentOutOfRangeException(nameof(startX), "Origin point is beyond buffer bounds.");
                if (y < 0 || y+1 > GetGUIHeight) throw new ArgumentOutOfRangeException(nameof(y), "Y position is beyond buffer bounds.");
                if (startX + width > GetGUIWidth) throw new ArgumentOutOfRangeException(nameof(startX) + "', '" + nameof(width), "Too long.");
                // ...or if the specified size is too small for the element to be drawn properly
                if (width < 2 /*4*/) throw new ArgumentOutOfRangeException(nameof(width), "Length can't be less than 2.");
                //if (evenWidth % 2 != 0) throw new ArgumentException("Length must be an even number.", nameof(evenWidth));

                y++; // Push GUI down from line 0

                Console.BackgroundColor = bgColor ?? _guiColor;
                Console.ForegroundColor = textColor ?? _guiTextColor;

                int numberOfZags = width / 4;
                int remainder = width % 4;      // can be 0, 1, 2, or *3* (standard)

                string[] fragment = {
                    $"{_cornerTL[(int)borderStyle]}{_cornerTR[(int)borderStyle]}",
                    $"{_cornerBL[(int)borderStyle]}{_cornerBR[(int)borderStyle]}"
                };
                string line = string.Empty;

                for (int i = 0; i < width-1; i+=2)
                {
                    line += fragment[0 + (int)zigzagStyle];
                }

                if (straightEdge)
                    line = _lineH[(int)borderStyle] + line[1..(line.Length-1)] + _lineH[(int)borderStyle];

                Console.SetCursorPosition(startX, y + (int)zigzagStyle);
                Console.Write(line);

                line = string.Empty;

                for (int i = 0; i < width-3; i+=2)
                {
                    line += fragment[1 - (int)zigzagStyle];
                }
                Console.SetCursorPosition(startX + 1, y + 1 - (int)zigzagStyle);
                Console.WriteLine(line);
            }
            catch (ArgumentException ex)
            {
                PrintInfo("GUI.DrawLine error: " + ex.Message);
            }
        }

        public static void DrawLine(int startX, int y, int width, BorderStyle borderStyle = 0,
            EdgeStyle edgeStyleLeft = EdgeStyle.None, EdgeStyle edgeStyleRight = EdgeStyle.None,
            ConsoleColor? bgColor = null, ConsoleColor? textColor = null)
        {
            try
            {
                // Throw an exception if a parameter value would lead to drawing outside the console buffer...
                if (startX < 0 || startX > GetGUIWidth) throw new ArgumentOutOfRangeException(nameof(startX), "Origin point is beyond buffer bounds.");
                if (y < 0 || y > GetGUIHeight) throw new ArgumentOutOfRangeException(nameof(y), "Y position is beyond buffer bounds.");
                if (startX + width > GetGUIWidth) throw new ArgumentOutOfRangeException(nameof(startX) + "', '" + nameof(width), "Too long.");
                // ...or if the specified size is too small for the element to be drawn properly
                if (width < 3) throw new ArgumentOutOfRangeException(nameof(width), "Length can't be less than 3 (<3).");

                y++; // Push GUI down from line 0

                Console.BackgroundColor = bgColor ?? _guiColor;
                Console.ForegroundColor = textColor ?? _guiTextColor;

                if (edgeStyleLeft == EdgeStyle.None)
                    CharAtPosition(_lineH[(int)borderStyle], startX, y);
                else
                    CharAtPosition(_cornerTL[(int)borderStyle + (int)edgeStyleLeft], startX, y);

                for (int i = startX + 1; i < startX + width - 1; i++)
                    Console.Write(_lineH[(int)borderStyle]);

                if (edgeStyleRight == EdgeStyle.None)
                    Console.Write(_lineH[(int)borderStyle]);
                else
                    Console.Write(_cornerTR[(int)borderStyle + (int)edgeStyleRight]);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                PrintInfo("GUI.DrawLine error: " + ex.Message);
            }
        }

        public static void DrawColumnZigzag(int x, int startY, int evenHeight, BorderStyle borderStyle = 0,
            ZigzagStyle zigzagStyle = ZigzagStyle.Regular, bool straightEdge = false, ConsoleColor ? bgColor = null, ConsoleColor? textColor = null)
        {
            try
            {
                // Throw an exception if a parameter value would lead to drawing outside the console buffer...
                if (startY < 0 || startY > GetGUIHeight) throw new ArgumentOutOfRangeException(nameof(startY), "Origin point is beyond buffer bounds.");
                if (x < 0 || x > GetGUIWidth) throw new ArgumentOutOfRangeException(nameof(x), "X position is beyond buffer bounds.");
                if (startY + evenHeight > GetGUIHeight) throw new ArgumentOutOfRangeException(nameof(startY) + "', '" + nameof(evenHeight), "Too long.");
                // ...or if the specified size is too small for the element to be drawn properly
                if (evenHeight < 4) throw new ArgumentOutOfRangeException(nameof(evenHeight), "Length can't be less than 3 (<3).");
                if (evenHeight % 2 != 0) throw new ArgumentException("Height must be an even number.", nameof(evenHeight));

                startY++; // Push GUI down from line 0

                Console.BackgroundColor = bgColor ?? _guiColor;
                Console.ForegroundColor = textColor ?? _guiTextColor;

                string[] fragment = {
                    $"{_cornerTL[(int)borderStyle]}{_cornerBR[(int)borderStyle]}",
                    $"{_cornerBL[(int)borderStyle]}{_cornerTR[(int)borderStyle]}"
                };

                CharAtPosition(straightEdge ? _lineV[(int)borderStyle] : fragment[0 + (int)zigzagStyle][0 + (int)zigzagStyle], x + (int)zigzagStyle, startY);

                for (int i = startY + 1; i < startY + evenHeight -1; i+=2)
                {
                    Console.SetCursorPosition(x, i);
                    Console.Write(fragment[1 - (int)zigzagStyle]);
                    Console.SetCursorPosition(x, i+1);
                    Console.Write(fragment[0 + (int)zigzagStyle]);
                }

                CharAtPosition(straightEdge ? _lineV[(int)borderStyle] : fragment[1 - (int)zigzagStyle][0 + (int)zigzagStyle], x + (int)zigzagStyle, startY + evenHeight - 1);
            }
            catch (ArgumentException ex)
            {
                PrintInfo("GUI.DrawColumn error: " + ex.Message);
            }

        }

        public static void DrawColumn(int x, int startY, int height, BorderStyle borderStyle = 0,
            EdgeStyle edgeStyleTop = EdgeStyle.None, EdgeStyle edgeStyleBottom = EdgeStyle.None,
            ConsoleColor? bgColor = null, ConsoleColor? textColor = null)
        {
            try
            {
                // Throw an exception if a parameter value would lead to drawing outside the console buffer...
                if (startY < 0 || startY > GetGUIHeight) throw new ArgumentOutOfRangeException(nameof(startY), "Origin point is beyond buffer bounds.");
                if (x < 0 || x > GetGUIWidth) throw new ArgumentOutOfRangeException(nameof(x), "X position is beyond buffer bounds.");
                if (startY + height > GetGUIHeight) throw new ArgumentOutOfRangeException(nameof(startY) + "', '" + nameof(height), "Too long.");
                // ...or if the specified size is too small for the element to be drawn properly
                if (height < 3) throw new ArgumentOutOfRangeException(nameof(height), "Length can't be less than 3 (<3).");

                startY++; // Push GUI down from line 0

                Console.BackgroundColor = bgColor ?? _guiColor;
                Console.ForegroundColor = textColor ?? _guiTextColor;

                if (edgeStyleTop == EdgeStyle.None)
                    CharAtPosition(_lineV[(int)borderStyle], x, startY);
                else
                    CharAtPosition(_cornerTL[(int)borderStyle + (int)edgeStyleTop], x, startY);
                
                for (int i = startY + 1; i < startY + height - 1; i++)
                    CharAtPosition(_lineV[(int)borderStyle], x, i);

                if (edgeStyleBottom == EdgeStyle.None)
                    CharAtPosition(_lineV[(int)borderStyle], x, startY + height - 1);
                else
                    CharAtPosition(_cornerBL[(int)borderStyle + (int)edgeStyleBottom], x, startY + height - 1);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                PrintInfo("GUI.DrawColumn error: " + ex.Message);
            }
        }

        public static void DrawBox(int left, int top, int width, int height, BorderStyle borderStyle = 0,
            CornerStyle cornerStyleTL = 0, CornerStyle cornerStyleTR = 0, CornerStyle cornerStyleBL = 0, CornerStyle cornerStyleBR = 0,
            ConsoleColor? bgColor = null, ConsoleColor? textColor = null)
        {
            try
            {
                // Throw an exception if a parameter value would lead to drawing outside the console buffer...
                if (left < 0 || left > GetGUIWidth) throw new ArgumentOutOfRangeException(nameof(left), "Origin point is beyond horizontal buffer bounds.");
                if (top < 0 || top > GetGUIHeight) throw new ArgumentOutOfRangeException(nameof(top), "Origin point is beyond vertical buffer bounds.");
                if (left + width > GetGUIWidth) throw new ArgumentOutOfRangeException(nameof(left) + "', '" + nameof(width), "Too wide.");
                if (top + height > GetGUIHeight) throw new ArgumentOutOfRangeException(nameof(top) + "', '" + nameof(height), "Too tall.");
                // ...or if the specified size is too small for the element to be drawn properly
                if (width < 2 || height < 2) throw new ArgumentOutOfRangeException(nameof(width) + ", " + nameof(height), "Can't be smaller than 2 by 2.");

                top++; // Push GUI down from line 0

                Console.BackgroundColor = bgColor ?? _guiColor;
                Console.ForegroundColor = textColor ?? _guiTextColor;

                // Construct the horizontal lines here so you don't have to do it multiple times
                string backgroundFiller = " ";
                string horizontalLine = string.Empty;
                string horizontalChar = _lineH[(int)borderStyle].ToString();

                for (int i = left + 1; i < left + width - 1; i++)
                {
                    backgroundFiller += " ";
                    horizontalLine += horizontalChar;
                }

                // Write the top line of the box, including corners
                CharAtPosition(_cornerTL[(int)borderStyle + (int)cornerStyleTL], left, top);
                Console.Write(horizontalLine);
                Console.Write(_cornerTR[(int)borderStyle + (int)cornerStyleTR]);

                // Write the vertical lines of the box
                for (int i = top + 1; i < top + height - 1; i++)
                {
                    CharAtPosition(_lineV[(int)borderStyle], left, i);

                    if (bgColor != null)
                    {
                        Console.Write(backgroundFiller);
                    }

                    CharAtPosition(_lineV[(int)borderStyle], left + width - 1, i);
                }

                // Write the bottom line of the box, including corners
                CharAtPosition(_cornerBL[(int)borderStyle + (int)cornerStyleBL], left, top + height - 1);
                Console.Write(horizontalLine);
                Console.Write(_cornerBR[(int)borderStyle + (int)cornerStyleBR]);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                PrintInfo("GUI.DrawBox error: " + ex.Message);
            }
        }
    }
}
