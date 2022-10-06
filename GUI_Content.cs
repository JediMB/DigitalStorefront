using System.Reflection.Metadata.Ecma335;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleGUI
{
    public static partial class GUI
    {
        private static TextBox errorLog = new(0, 0, _guiWidth, 1, string.Empty, ConsoleColor.Black, ConsoleColor.White);
        private static List<TextBox> textFields = new();
        private static List<TextBox> textBoxes = new();
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

            private ConsoleColor bgColor;
            private ConsoleColor textColor;

            public string Text
            {
                set
                {
                    textFull = value;
                    textFormatted = FormatText(textFull, width);
                }

                get => textFull;
            }

            public TextBox(int left, int top, int width, int height, string text = "", ConsoleColor bgColor = _guiColor, ConsoleColor textColor = _guiTextColor)
            {
                this.left = left;
                this.top = top;
                this.width = width;
                this.height = height;

                this.textFull = text;
                this.textFormatted = FormatText(textFull, width);
                this.startLine = 0;

                this.bgColor = bgColor;
                this.textColor = textColor;
            }

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

            private static void SplitText(Queue<string> lines, string textIn, out string textOut, int maxLength)
            {
                for (int i = 0; i <= maxLength && i < textIn.Length; i++)
                {
                    if (textIn[i] == '\n' || textIn[i] == '\r')             // If there's already a linebreak on the current row, split the string there
                    {
                        if (textIn[i] == '\r' && textIn[i + 1] == '\n')     // If carriage return followed by newline, remove the latter
                            textIn = textIn.Remove(i + 1, 1);

                        string line = textIn[..i];                          // Create the new line of text
                        textIn = textIn[(i + 1)..];                         // Remove that line from the full text

                        for (int j = line.Length; j < maxLength; j++)       // Fill out the string with a trail of blankspaces
                        {
                            line += " ";
                        }

                        lines.Enqueue(line);                                // Put the line in the queue
                        
                        if (textIn.Length > maxLength)                      // If there is more than one more line of text, start over...
                        {
                            textOut = textIn;
                            return;
                        }

                        i = 0;                                              // ...and if there isn't, restart the for-loop to look for more linebreaks
                    }
                }

                // If the remaining text is longer than the textbox and doesn't have any linebreaks, split the string at a blankspace
                if (textIn.Length > maxLength)
                {
                    for (int i = maxLength; i >= 0; i--)
                    {
                        if (textIn[i] == ' ')
                        {
                            string line = textIn[..i];

                            for (int j = line.Length; j < maxLength; j++)
                            {
                                line += " ";
                            }

                            lines.Enqueue(line);
                            textOut = textIn[(i + 1)..];
                            return;
                        }
                    }
                }

                // If the remaining text is shorter than the textbox width and doesn't contain any more linebreak characters...
                for (int i = textIn.Length; i < maxLength; i++)           
                {
                    textIn += " ";      // ...fill out the final line/row with blankspaces to match the box width...
                }

                lines.Enqueue(textIn);  // ...and add it to the queue

                textOut = string.Empty;
            }

            public void ScrollUp(/*byte rows = 1*/)
            {
                if (textFormatted.Length > height)
                {
                    startLine--;

                    if (startLine < 0)
                        startLine = 0;
                }

                Render();
            }

            public void ScrollDown(/*byte rows = 1*/)
            {
                if (textFormatted.Length > height)
                {
                startLine++;

                    if (startLine > (textFormatted.Length - height))
                        startLine = textFormatted.Length - height;
                }

                Render();
            }

            public void Render()
            {
                Console.BackgroundColor = bgColor;
                Console.ForegroundColor = textColor;

                for (int i = 0; i < height; i++)
                {
                    if (i + startLine >= textFormatted.Length)
                        return;

                    Console.SetCursorPosition(left, top + i);
                    Console.Write(textFormatted[i + startLine]);
                }

                Console.BackgroundColor = _guiColor;
                Console.ForegroundColor = _guiTextColor;
            }
        }

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

                    TextBox textBox = new(left, top, width, height, text, bgColor.Value, textColor.Value);

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

        private static void PrevTextbox()
        {
            if (textBoxes.Count != 0)
            {
                textBoxSelection--;

                if (textBoxSelection == byte.MaxValue)
                    textBoxSelection = (byte)(textBoxes.Count - 1);
            }
        }

        private static void NextTextbox()
        {
            if (textBoxes.Count != 0)
            {
                textBoxSelection++;

                if (textBoxSelection >= textBoxes.Count)
                    textBoxSelection = 0;
            }
        }

        public static void ControlTextboxes()
        {
            ConsoleKeyInfo keyInfo;

            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Q)
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Tab:
                        if ((keyInfo.Modifiers & ConsoleModifiers.Shift) != 0)
                        {
                            PrevTextbox();
                            break;
                        }
                        NextTextbox();
                        break;

                    case ConsoleKey.PageUp:
                        errorLog.ScrollUp();
                        break;

                    case ConsoleKey.PageDown:
                        errorLog.ScrollDown();
                        break;

                    case ConsoleKey.UpArrow:
                        if (textBoxes.Count > 0)
                            textBoxes[textBoxSelection].ScrollUp();
                        break;

                    case ConsoleKey.DownArrow:
                        if (textBoxes.Count > 0)
                            textBoxes[textBoxSelection].ScrollDown();
                        break;
                }
            }
        }
    }
}
