using System.Reflection.Metadata.Ecma335;

namespace ConsoleGUI
{
    public static partial class GUI
    {
        private static List<TextBox> textBoxes = new();
        private static byte textBoxSelection = 0;

        private class TextBox
        {
            private readonly int left;
            private readonly int top;
            private readonly int width;
            private readonly int height;

            private string      textFull;
            private string[]    textFormatted;
            private int startLine;

            public string Text
            {
                set
                {
                    textFull = value;
                    textFormatted = FormatText(textFull, width);
                }

                get => textFull;
            }

            public TextBox(int left, int top, int width, int height, string text = "")
            {
                this.left = left;
                this.top = top+1;
                this.width = width;
                this.height = height;

                this.textFull = text;
                this.textFormatted = FormatText(textFull, width);
                this.startLine = 0;
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
                                
                if (text.Length > width)
                {
                    Queue<string> lines = new();

                    while (text.Length > width)
                    {
                        bool foundLinebreak = false;

                        for (int i = 0; i < width; i++)     // If there's already a linebreak in the string, split the string there...
                        {
                            if (text[i] == '\n' || text[i] == '\r')
                            {
                                if (text[i] == '\r' && text[i + 1] == '\n')
                                    text = text.Remove(i+1, 1);

                                foundLinebreak = true;
                                string line = text[..i];
                                
                                for (int j = line.Length; j < width; j++)
                                {
                                    line += " ";
                                }
                                
                                lines.Enqueue(line);
                                text = text[(i + 1)..];
                                break;
                            }
                        }

                        if (!foundLinebreak)                // ...but if not, split at the last possible blankspace
                        {
                            for (int i = width - 1; i >= 0; i--)
                            {
                                if (text[i] == ' ')
                                {
                                    string line = text[..i];

                                    for (int j = line.Length; j < width; j++)
                                    {
                                        line += " ";
                                    }

                                    lines.Enqueue(line);
                                    text = text[(i + 1)..];
                                    break;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < text.Length; i++)   // Checks the final length of text for linebreaks
                    {                                       // TODO: Figure out if there's a simpler way to do all of these things
                        if (text[i] == '\n' || text[i] == '\r')
                        {
                            if (text[i] == '\r' && text[i + 1] == '\n')
                                text = text.Remove(i + 1, 1);

                            string line = text[..i];

                            for (int j = line.Length; j < width; j++)
                            {
                                line += " ";
                            }

                            lines.Enqueue(line);
                            text = text[(i + 1)..];
                            i = 0;
                        }
                    }

                    for (int i = text.Length; i < width; i++)
                    {
                        text += " ";
                    }

                    lines.Enqueue(text);

                    return lines.ToArray();
                }

                return new string[1] { text };
            }

            //private static string SplitLine()
            //{

            //}

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
                for (int i = 0; i < height; i++)
                {
                    if (i + startLine >= textFormatted.Length)
                        return;

                    Console.SetCursorPosition(left, top + i);
                    Console.Write(textFormatted[i + startLine]);
                }
            }
        }

        public static void CreateTextbox(int left, int top, int width, int height, string text = "")
        {
            TextBox textBox = new(left, top, width, height, text);

            textBox.Render();

            textBoxes.Add(textBox);
        }

        public static void ControlTextboxes()
        {
            ConsoleKey key;

            while ((key = Console.ReadKey(true).Key) != ConsoleKey.Q)
            {
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        textBoxes[textBoxSelection].ScrollUp();
                        break;

                    case ConsoleKey.DownArrow:
                        textBoxes[textBoxSelection].ScrollDown();
                        break;
                }
            }
        }
    }
}
