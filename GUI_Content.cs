namespace ConsoleGUI
{
    public static partial class GUI
    {
        private static List<TextBox> textBoxes = new List<TextBox>();

        private struct TextBox
        {
            private readonly int left;
            private readonly int top;
            private readonly int width;
            private readonly int height;

            private string      textFull;
            private string[]    textFormatted;
            private int textVisibilityStart;

            public TextBox(int left, int top, int width, int height, string text = "")
            {
                this.left = left;
                this.top = top+1;
                this.width = width;
                this.height = height;

                this.textFull = text;
                this.textFormatted = FormatText(textFull, width);
                this.textVisibilityStart = 0;
            }

            private static string[] FormatText(string text, int width)
            {
                if (string.IsNullOrWhiteSpace(text))
                    return new string[] { "" };
                
                if (text.Length > width)
                {
                    Queue<string> lines = new Queue<string>();

                    while (text.Length > width)
                    {
                        bool foundLinebreak = false;

                        for (int i = 0; i < width; i++)
                        {
                            if (text[i] == '\n')
                            {
                                foundLinebreak = true;
                                lines.Enqueue(text.Substring(0, i));
                                text = text.Substring(i + 1);
                                break;
                            }
                        }

                        if (!foundLinebreak)
                        {
                            for (int i = width - 1; i >= 0; i--)
                            {
                                if (text[i] == ' ')
                                {
                                    lines.Enqueue(text.Substring(0, i));
                                    text = text.Substring(i + 1);
                                    break;
                                }
                            }
                        }
                    }

                    lines.Enqueue(text);

                    return lines.ToArray();
                }

                return new string[1] { text };
            }

            public void Render()
            {
                for (int i = 0; i < height; i++)
                {
                    if (i + textVisibilityStart >= textFormatted.Length)
                        return;

                    Console.SetCursorPosition(left, top + i);
                    Console.Write(textFormatted[i + textVisibilityStart]);
                }
            }
        }

        public static void CreateTextbox(int left, int top, int width, int height, string text = "")
        {
            TextBox textBox = new TextBox(left, top, width, height, text);

            textBox.Render();

            textBoxes.Add(textBox);
        }
    }
}
