using Digital_Storefront;
using System.Reflection.Metadata.Ecma335;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleGUI
{
    public static partial class GUI
    {
        private static readonly TextBox errorLog = new(0, 0, _guiWidth, 1, -1, string.Empty, ConsoleColor.Black, ConsoleColor.White);
        private static readonly TextBox cartNumber = new(3, GUI.GetGUIHeight - 4, 2, 1, -1, "00", ConsoleColor.Yellow, ConsoleColor.DarkBlue);
        private static readonly List<TextBox> textFields = new();
        private static readonly List<TextBox> textBoxes = new();
        private static byte textBoxSelection = 0;

        public enum Interactable
        {
            No,
            Yes
        }

        private class TextBox
        {
            private readonly int left;
            private readonly int top;
            private readonly int width;
            private readonly int height;

            private string      textFull;
            private string[]    textFormatted;
            private int startLine;
            private int selectedLine;

            private readonly ConsoleColor bgColor;
            private readonly ConsoleColor textColor;
            private readonly ConsoleColor inactiveColor;

            /// <summary>
            /// Gets and sets the text of a textbox, and also formats the text to properly fit into the box.
            /// </summary>
            public string Text
            {
                set
                {
                    textFull = value;
                    textFormatted = FormatText(textFull, width);
                    Render();
                }

                get => textFull;
            }

            public TextBox(int left, int top, int width, int height, int selectedLine, string text = "", ConsoleColor bgColor = _guiColor, ConsoleColor textColor = _guiTextColor, ConsoleColor inactiveColor = _guiInactiveColor)
            {
                this.left = left;
                this.top = top;
                this.width = width;
                this.height = height;

                this.textFull = text;
                this.textFormatted = FormatText(textFull, width);
                this.startLine = 0;
                this.selectedLine = selectedLine;

                this.bgColor = bgColor;
                this.textColor = textColor;
                this.inactiveColor = inactiveColor;
            }

            /// <summary>
            /// Handles formatting of a text string to fit within a block of the specified width.
            /// </summary>
            private static string[] FormatText(string text, int width)
            {
                if (string.IsNullOrWhiteSpace(text))
                    return new string[] { "" };

                text = text.Replace('\t', ' ');

                for (int i = text.Length-1; i >= 0; i--)    // Removes any trailing linebreaks
                {
                    if (text[i] != '\r' && text[i] != '\n')
                    {
                        text = text[..(i+1)];
                        break;
                    }

                }
                
                Queue<string> lines = new();

                while (text != string.Empty)                         // Split the text into lines that fit into the textbox
                    SplitText(lines, text, out text, width);
                                
                return lines.ToArray();
            }

            /// <summary>
            /// Identifies at what point to split a line in order to fit it into a textbox.
            /// </summary>
            private static void SplitText(Queue<string> lines, string textIn, out string textOut, int maxLength)
            {
                for (int i = 0; i <= maxLength && i < textIn.Length; i++)
                {
                    if (textIn[i] == '\n' || textIn[i] == '\r')             // If there's already a linebreak on the current row, split the string there
                    {
                        if (textIn[i] == '\r' && textIn[i + 1] == '\n')     // If carriage return followed by newline, remove the latter
                            textIn = textIn.Remove(i + 1, 1);

                        lines.Enqueue(textIn[..i].PadRight(maxLength));     // Put the new line in the queue
                        textIn = textIn[(i + 1)..];                         // Remove that line from the full text
                                                                        
                        if (textIn.Length > maxLength)                      // If there is more than one more line of text, start over...
                        {
                            textOut = textIn;
                            return;
                        }

                        i = 0;                                              // ...and if there isn't, restart the for-loop to look for more linebreaks
                    }
                }

                // If the remaining text is longer than the textbox and doesn't have any linebreaks, split the string at a dash or blankspace
                if (textIn.Length > maxLength)
                {
                    for (int i = maxLength; i >= 0; i--)
                    {
                        if (i != maxLength && textIn[i] == '-')
                        {
                            lines.Enqueue(textIn[..(i + 1)].PadRight(maxLength));
                            textOut = textIn[(i + 1)..];
                            return;
                        }

                        if (textIn[i] == ' ')
                        {
                            lines.Enqueue(textIn[..i].PadRight(maxLength));
                            textOut = textIn[(i + 1)..];
                            return;
                        }
                    }
                }

                lines.Enqueue(textIn.PadRight(maxLength));  // If the remaining, linebreak-free text fits in the textbox, add it to the queue

                textOut = string.Empty;
            }

            /// <summary>
            /// Moves the selection one line up in the current textbox. Scrolls up if necessary.
            /// </summary>
            public void PrevLine()
            {
                if (selectedLine > 0)
                    selectedLine--;
                else
                    ScrollUp();

                Render();
            }

            /// <summary>
            /// Moves the selection one line down in the current textbox. Scrolls down if necessary.
            /// </summary>
            public void NextLine()
            {
                if (selectedLine < height - 1)
                    selectedLine++;
                else
                    ScrollDown();

                Render();
            }


            /// <summary>
            /// Handles scrolling a textbox upwards.
            /// </summary>
            public void ScrollUp()
            {
                if (textFormatted.Length > height)
                {
                    startLine--;

                    if (startLine < 0)
                        startLine = 0;
                }
            }

            /// <summary>
            /// Handles scrolling a textbox downwards.
            /// </summary>
            public void ScrollDown()
            {
                if (textFormatted.Length > height)
                {
                startLine++;

                    if (startLine > (textFormatted.Length - height))
                        startLine = textFormatted.Length - height;
                }
            }


            /// <summary>
            /// Handles 'selection' of lines in a textbox, which currently means adding the items to your 'cart'.
            /// </summary>
            public void Select()
            {
                if (textFormatted[startLine + selectedLine][0] != '✓')
                {
                    textFormatted[startLine + selectedLine] = '✓' + textFormatted[startLine + selectedLine][1..];

                    Data.ItemsInCart++;

                    if (Data.ItemsInCart > 99)
                        Data.ItemsInCart = 99;

                    cartNumber.Text = $"{Data.ItemsInCart:00}";
                    Render();
                }
            }


            /// <summary>
            /// Renders all the visible text lines in a textbox
            /// </summary>
            public void Render()
            {
                Console.BackgroundColor = bgColor;
                Console.ForegroundColor = textColor;

                // Calculations used for moving the scrollbar
                float scrollPercentage = ((float)startLine + height / 2) / (float)textFormatted.Length;
                int scrollbarPosition = 1 + (int)((height - 2) * scrollPercentage);

                for (int i = 0; i < height; i++)
                {
                    if (i + startLine >= textFormatted.Length)
                        return;

                    // This handles showing what line you have marked
                    if (GUI.textBoxes.Count != 0 && i == selectedLine) {
                        if (this == textBoxes[GUI.textBoxSelection])    // Different colors depending on if it's the active textbox or not
                            (Console.BackgroundColor, Console.ForegroundColor) = (textColor, bgColor);
                        else
                            (Console.BackgroundColor, Console.ForegroundColor) = (inactiveColor, textColor);
                    }

                    Console.SetCursorPosition(left, top + i);
                    Console.Write(textFormatted[i + startLine]);

                    // This scrollbar code was hacked in at the very end. Would have preferred to have it more robustly implemented in the textbox code, but... time.
                    if (selectedLine != -1)
                    {
                        if (i == 0 && startLine > 0)
                        {
                            Console.BackgroundColor = bgColor;
                            Console.ForegroundColor = textColor;
                            Console.CursorLeft--;
                            Console.Write("▲");
                        }
                        else if (i == scrollbarPosition || startLine == 0 || startLine == textFormatted.Length - height)
                        {
                            if (startLine == 0)
                                Console.CursorTop = top + 1;
                            else if (startLine == textFormatted.Length - height)
                                Console.CursorTop = (top + height) - 2;

                            Console.CursorLeft--;
                            Console.BackgroundColor = bgColor;
                            Console.ForegroundColor = textColor;
                            Console.Write("█");
                        }
                        else if (i == height - 1 && startLine + height < textFormatted.Length)
                        {
                            Console.BackgroundColor = bgColor;
                            Console.ForegroundColor = textColor;
                            Console.CursorLeft--;
                            Console.Write("▼");
                        }
                    }

                    // Restores console colors in case they've been changed by line selection code
                    Console.BackgroundColor = bgColor;
                    Console.ForegroundColor = textColor;
                }
            }
        }

        /// <summary>
        /// Creates a textbox and adds it to the textBoxes or textFields lists, depending on if it's set as interactable or not
        /// </summary>
        public static void CreateTextbox(int left, int top, int width, int height, string text = "",
            ConsoleColor? bgColor = null, ConsoleColor? textColor = null, Interactable interactable = Interactable.No)
        {
            if (textBoxes.Count < byte.MaxValue || interactable == Interactable.No)
            {
                try
                {
                    // Throw an exception if a parameter value would lead to drawing outside the console buffer...
                    if (left < 0 || left > GetGUIWidth) throw new ArgumentOutOfRangeException(nameof(left), "Origin point is beyond horizontal buffer bounds.");
                    if (top < 0 || top > GetGUIHeight) throw new ArgumentOutOfRangeException(nameof(top), "Origin point is beyond vertical buffer bounds.");
                    if (left + width > GetGUIWidth) throw new ArgumentOutOfRangeException(nameof(left) + "', '" + nameof(width), "Too wide.");
                    if (top + height > GetGUIHeight) throw new ArgumentOutOfRangeException(nameof(top) + "', '" + nameof(height), "Too tall.");

                    top++;  // Push GUI down from row 0

                    // Assigns default colors if none are provided
                    bgColor ??= _guiColor;
                    textColor ??= _guiTextColor;
                    int selectedLine = (interactable == Interactable.Yes ? 0 : -1);

                    TextBox textBox = new(left, top, width, height, selectedLine ,text, bgColor.Value, textColor.Value);

                    textBox.Render();

                    if (interactable == Interactable.No)
                    {
                        textFields.Add(textBox);
                        return;
                    }

                    textBoxes.Add(textBox);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    PrintInfo("GUI.CreateTextbox error: " + ex.Message);
                }
            }
        }


        /// <summary>
        /// Switches focus and controls to the previous item in the textBoxes list.
        /// </summary>
        private static void PrevTextbox()
        {
            if (textBoxes.Count != 0)
            {
                textBoxSelection--;

                if (textBoxSelection == byte.MaxValue)
                    textBoxSelection = (byte)(textBoxes.Count - 1);

                foreach (TextBox textBox in textBoxes)
                    textBox.Render();
            }
        }

        /// <summary>
        /// Switches focus and controls to the next item in the textBoxes list.
        /// </summary>
        private static void NextTextbox()
        {
            if (textBoxes.Count != 0)
            {
                textBoxSelection++;

                if (textBoxSelection >= textBoxes.Count)
                    textBoxSelection = 0;

                foreach (TextBox textBox in textBoxes)
                    textBox.Render();
            }
        }

        /// <summary>
        /// Handles all user input, which is primarily about manipulating textboxes.
        /// </summary>
        public static void ControlTextboxes()
        {
            ConsoleKeyInfo keyInfo;

            if (textBoxes.Count > 0)
                textBoxes[0].Render();

            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Q)
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.PageUp:
                        errorLog.ScrollUp();
                        errorLog.Render();
                        break;

                    case ConsoleKey.PageDown:
                        errorLog.ScrollDown();
                        errorLog.Render();
                        break;

                    case ConsoleKey.UpArrow:
                        if (textBoxes.Count > 0)
                            textBoxes[textBoxSelection].PrevLine();
                        break;

                    case ConsoleKey.DownArrow:
                        if (textBoxes.Count > 0)
                            textBoxes[textBoxSelection].NextLine();
                        break;

                    case ConsoleKey.LeftArrow:
                        PrevTextbox();
                        break;

                    case ConsoleKey.RightArrow:
                        NextTextbox();
                        break;

                    case ConsoleKey.Enter:
                        if (textBoxes.Count > 0)
                            textBoxes[textBoxSelection].Select();
                        break;
                }
            }
        }
    }
}
