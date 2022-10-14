using Articles;
using System.Collections;
using System.Runtime.InteropServices;

namespace Digital_Storefront
{
    public static class Data
    {
        public const string logo =
            "    /////   //        ////    /////  //   ////   /////   //   /////  //////\n" +
            "   //  //  //       //  //  //      //  //  //  //  //  //  //        //\n" +
            "  /////   //       //////  // ///  //  //////  /////   //   ////     //\n" +
            " //      //       //  //  //  //  //  //  //  // //   //      //    //\n" +
            "//      ///////  //  //   ////   //  //  //  //  //  //  /////     //";
        public const string name = "Plagiarist Printings Inc.";
        public const string slogan = "For when you really want a coffee cup with someone else's stolen art on it.";
        public const string addressStore = "1337 Ave Avenue\nYour Town Here, XX\n35199-0753\nFantasyland";
        public const string addressBilling = "PO Box 1984\nSome Other Town, XX\nFantasyland";
        public const string copyright = "(c) Copyright 2022, M Berntson";
        private static int itemsInCart = 0;

        public static int ItemsInCart { get => itemsInCart; set => itemsInCart = value; }

        public const string printsFilename = @"Data\Prints.txt";
        public const string mugsFilename = @"Data\Mugs.txt";
        public const string tshirtsFilename = @"Data\T-shirts.txt";

        private static readonly Random random = new();

        private static string[] prints = Array.Empty<string>();
        private static readonly List<TShirt> tshirts = new();
        private static readonly List<Mug> mugs = new();
        private static (
            (int Print, int Type, int Total) Mugs,
            (int Print, int Size, int Fabric, int Total) TShirts,
            int Price
            ) ColumnLengths;
        private const int columnLengthAverageScore = 6;
        private const int columnPadding = 2;

        /// <summary>
        /// Generates a formatted header row for the mug list. Was a late addition and might not hold up to environmental changes.
        /// </summary>
        public static string GetMugsHeaders()
        {
            int extraPadding = 0;

            if (ColumnLengths.Mugs.Total == 0 || ColumnLengths.TShirts.Total == 0)
                ColumnLengthSummation();

            if (ColumnLengths.Mugs.Total < ColumnLengths.TShirts.Total)
                extraPadding = ColumnLengths.TShirts.Total - ColumnLengths.Mugs.Total;

            return " PRINT".PadRight(ColumnLengths.Mugs.Print + columnPadding) +
                " TYPE".PadRight(ColumnLengths.Mugs.Type + columnPadding) +
                "RATING".PadLeft(extraPadding + columnPadding + columnLengthAverageScore + 1) +
                "PRICE".PadLeft(ColumnLengths.Price + columnPadding);
        }

        /// <summary>
        /// Generates a formatted header row for the t-shirt list. Was a late addition and might not hold up to environmental changes.
        /// </summary>
        public static string GetTShirtsHeaders()
        {
            int extraPadding = 0;

            if (ColumnLengths.Mugs.Total == 0 || ColumnLengths.TShirts.Total == 0)
                ColumnLengthSummation();

            if (ColumnLengths.TShirts.Total < ColumnLengths.Mugs.Total)
                extraPadding = ColumnLengths.Mugs.Total - ColumnLengths.TShirts.Total;

            return " PRINT".PadRight(ColumnLengths.TShirts.Print + columnPadding) +
                " FABRIC".PadRight(ColumnLengths.TShirts.Fabric + columnPadding) +
                " SIZE".PadRight(ColumnLengths.TShirts.Size + columnPadding) +
                "RATING".PadLeft(extraPadding + columnPadding + columnLengthAverageScore + 1) +
                "PRICE".PadLeft(ColumnLengths.Price + columnPadding);
        }

        /// <summary>
        /// Generates a string of mugs formatted into columns, for insertion into a scrollable textbox.
        /// </summary>
        public static string GetMugsAsColumns(bool sort = false, bool ascending = false)
        {
            int extraPadding = 0;

            if (ColumnLengths.Mugs.Total == 0 || ColumnLengths.TShirts.Total == 0)
                ColumnLengthSummation();

            if (ColumnLengths.Mugs.Total < ColumnLengths.TShirts.Total)
                extraPadding = ColumnLengths.TShirts.Total - ColumnLengths.Mugs.Total;

            List<Mug> sortableList = new(mugs);

            if (sort)
            {
                if (ascending)
                    sortableList.Sort();
                else
                    sortableList.Sort(Mug.CompareByAvgScoreDescending());
            }

            string data = string.Empty;

            foreach (Mug mug in sortableList)
            {
                data += mug.ToString(
                    ColumnLengths.Mugs.Print,
                    ColumnLengths.Price,
                    ColumnLengths.Mugs.Type,
                    columnLengthAverageScore, columnPadding, extraPadding) + "\n";
            }

            return data;
        }

        /// <summary>
        /// Generates a string of t-shirts formatted into columns, for insertion into a scrollable textbox.
        /// </summary>
        public static string GetTShirtsAsColumns(bool sort = false, bool ascending = false)
        {
            int extraPadding = 0;

            if(ColumnLengths.TShirts.Total == 0 || ColumnLengths.Mugs.Total == 0)
                ColumnLengthSummation();

            if (ColumnLengths.TShirts.Total < ColumnLengths.Mugs.Total)
                extraPadding = ColumnLengths.Mugs.Total - ColumnLengths.TShirts.Total;

            List<TShirt> sortableList = new(tshirts);

            if (sort)
            {
                if (ascending)
                    sortableList.Sort();
                else
                    sortableList.Sort(Mug.CompareByAvgScoreDescending());
            }

            string data = string.Empty;

            foreach (TShirt tshirt in sortableList)
            {
                data += tshirt.ToString(
                    ColumnLengths.TShirts.Print,
                    ColumnLengths.Price,
                    ColumnLengths.TShirts.Size,
                    ColumnLengths.TShirts.Fabric,
                    columnLengthAverageScore, columnPadding, extraPadding) + "\n";
            }

            return data;
        }

        /// <summary>
        /// Generates a series of mug variants for each available print
        /// </summary>
        public static bool GenerateMugs()
        {
            LoadPrintsFromFile();

            if (mugs.Count == 0 && prints.Length > 0)
            {
                foreach (string print in prints)    // For each print...
                {
                    int mugTypeMax = (int)Enum.GetValues(typeof(Mug.Types)).Cast<Mug.Types>().Max();

                    for (int typeIndex = 0; typeIndex <= mugTypeMax; typeIndex++)   // ...generate one mug of every type...
                    {
                        decimal price = 50.0m + 7.5m * typeIndex;   // ...with a base price and a type modifier...

                        float avgScore = 0;
                        int numberOfReviews = random.Next(20 + 1);

                        for (int scores = 0; scores < numberOfReviews; scores++)
                            avgScore += random.Next(10 + 1);

                        if (numberOfReviews != 0)           // ...and an average score (0-10) from up to twenty randomly generated ones
                            avgScore /= numberOfReviews;

                        mugs.Add(new Mug(print, price, (Mug.Types)typeIndex, avgScore));


                        // Generates column data later used for formatting
                        if (print.Length > ColumnLengths.Mugs.Print)
                            ColumnLengths.Mugs.Print = print.Length;
                        
                        if ($"{price:C2}".Length > ColumnLengths.Price)
                            ColumnLengths.Price = $"{price:C2}".Length;

                        if ($"{(Mug.Types)typeIndex}".Length > ColumnLengths.Mugs.Type)
                            ColumnLengths.Mugs.Type = $"{(Mug.Types)typeIndex}".Length;
                    }
                }

                using StreamWriter writer = new(mugsFilename);

                foreach (Mug mug in mugs)
                {
                    writer.WriteLine(mug.Print + ";" + mug.Price + ";" + mug.Type + ";" + mug.AverageReviewScore);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Generates a series of t-shirt variants for each available print
        /// </summary>
        public static bool GenerateTShirts()
        {
            LoadPrintsFromFile();

            if (tshirts.Count == 0 && prints.Length > 0)
            {
                foreach (string print in prints)    // For each print...
                {
                    int tshirtFabricMax = (int)Enum.GetValues(typeof(TShirt.Fabrics)).Cast<TShirt.Fabrics>().Max();
                    int tshirtSizeMax = (int)Enum.GetValues(typeof(TShirt.Sizes)).Cast<TShirt.Sizes>().Max();

                    for (int fabricIndex = 0; fabricIndex <= tshirtFabricMax; fabricIndex++)    // ...generate one t-shirt of every combination of fabric...
                    {
                        for (int sizeIndex = 0; sizeIndex <= tshirtSizeMax; sizeIndex++)        // ...and size...
                        {
                            decimal price = (150.0m + 10m*sizeIndex);   // ...with a base price and size/fabric modifiers...
                            price += price * (fabricIndex / 2);

                            float avgScore = 0;
                            int numberOfReviews = random.Next(20 + 1);

                            for (int scores = 0; scores < numberOfReviews; scores++)
                                avgScore += random.Next(10 + 1);

                            if (numberOfReviews != 0)           // ...and an average score (0-10) from up to twenty randomly generated ones
                                avgScore /= numberOfReviews;

                            tshirts.Add(new TShirt(print, price, (TShirt.Sizes)sizeIndex, (TShirt.Fabrics)fabricIndex, avgScore));


                            // Generates column data later used for formatting
                            if (print.Length > ColumnLengths.TShirts.Print)
                                ColumnLengths.TShirts.Print = print.Length;

                            if ($"{price:C2}".Length > ColumnLengths.Price)
                                ColumnLengths.Price = $"{price:C2}".Length;

                            if ($"{(TShirt.Sizes)sizeIndex}".Length > ColumnLengths.TShirts.Size)
                                ColumnLengths.TShirts.Size = $"{(TShirt.Sizes)sizeIndex}".Length;

                            if ($"{(TShirt.Fabrics)fabricIndex}".Length > ColumnLengths.TShirts.Fabric)
                                ColumnLengths.TShirts.Fabric = $"{(TShirt.Fabrics)fabricIndex}".Length;
                        }
                    }
                }

                using StreamWriter writer = new(tshirtsFilename);

                foreach (TShirt tshirt in tshirts)
                {
                    writer.WriteLine(tshirt.Print + ";" + tshirt.Price + ";" + tshirt.Size + ";" + tshirt.Fabric + ";" + tshirt.AverageReviewScore);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads all the prints from the included text file
        /// </summary>
        private static bool LoadPrintsFromFile()
        {
            if (prints.Length == 0)
            {
                if (!File.Exists(printsFilename))
                    throw new FileNotFoundException("File not found in Data.LoadPrintsFromFile()", printsFilename);

                prints = File.ReadAllLines(printsFilename);

                if (prints.Length == 0)
                    throw new FormatException($"{printsFilename} is empty");

                for (int i = 0; i < prints.Length; i++)
                {
                    if (prints[i] == string.Empty)
                    {
                        prints = Array.Empty<string>();
                        throw new FormatException($"Line {i+1} in {printsFilename} is empty.");
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// If it exists, loads all the mugs from the generated text file. Otherwise returns false;
        /// </summary>
        public static bool LoadMugsFromFile()
        {
            if (File.Exists(mugsFilename))
            {
                using StreamReader reader = new(mugsFilename);
                int lineNumber = 0;
                string line = string.Empty;
                while ((line = reader.ReadLine() ?? string.Empty) != string.Empty)
                {
                    lineNumber++;
                    string[] substrings; //List<string> values;

                    try
                    {
                        substrings = line.Split(';');  //SplitLine(line);

                        for (int substringIndex = 0; substringIndex < substrings.Length; substringIndex++)
                        {
                            if (string.IsNullOrEmpty(substrings[substringIndex]))
                                throw new FormatException($"Value {substringIndex} missing.");
                        }

                        if (substrings.Length != 4)
                            throw new FormatException($"{substrings.Length} values found (expected 4).");
                    }
                    catch (FormatException ex)
                    {
                        throw new FormatException($"Malformed string in {mugsFilename} on line {lineNumber}. " + ex.Message, ex);
                    }

                    string print = substrings[0];

                    if (!decimal.TryParse(substrings[1], out decimal price))
                        throw new FormatException($"Malformed string in {mugsFilename} on line {lineNumber}. Value 2 is not a decimal.");

                    if (!Enum.TryParse<Mug.Types>(substrings[2], out Mug.Types type))
                        throw new FormatException($"Malformed string in {mugsFilename} on line {lineNumber}. Value 3 is not a mug type.");

                    if (!float.TryParse(substrings[3], out float avgScore))
                        throw new FormatException($"Malformed string in {mugsFilename} on line {lineNumber}. Value 4 is not a float.");

                    mugs.Add(new Mug(print, price, type, avgScore));


                    // Generates column data later used for formatting
                    if (print.Length > ColumnLengths.Mugs.Print)
                        ColumnLengths.Mugs.Print = print.Length;

                    if ($"{price:C2}".Length > ColumnLengths.Price)
                        ColumnLengths.Price = $"{price:C2}".Length;

                    if ($"{type}".Length > ColumnLengths.Mugs.Type)
                        ColumnLengths.Mugs.Type = $"{type}".Length;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// If it exists, loads all the t-shirts from the generated text file. Otherwise returns false;
        /// </summary>
        public static bool LoadTShirtsFromFile()
        {
            if (File.Exists(tshirtsFilename))
            {
                using StreamReader reader = new(tshirtsFilename);
                int lineNumber = 0;
                string line = string.Empty;
                while ((line = reader.ReadLine() ?? string.Empty) != string.Empty)
                {
                    lineNumber++;
                    string[] substrings; //List<string> values;

                    try
                    {
                        substrings = line.Split(';');  //SplitLine(line);

                        for (int substringIndex = 0; substringIndex < substrings.Length; substringIndex++)
                        {
                            if (string.IsNullOrEmpty(substrings[substringIndex]))
                                throw new FormatException($"Value {substringIndex} missing.");
                        }

                        if (substrings.Length != 5)
                            throw new FormatException($"{substrings.Length} values found (expected 5).");
                    }
                    catch (FormatException ex)
                    {
                        throw new FormatException($"Malformed string in {tshirtsFilename} on line {lineNumber}. " + ex.Message, ex);
                    }

                    string print = substrings[0];

                    if (!decimal.TryParse(substrings[1], out decimal price))
                        throw new FormatException($"Malformed string in {tshirtsFilename} on line {lineNumber}. Value 2 is not a decimal.");

                    if (!Enum.TryParse<TShirt.Sizes>(substrings[2], true, out TShirt.Sizes size))
                        throw new FormatException($"Malformed string in {tshirtsFilename} on line {lineNumber}. Value 3 is not a t-shirt size.");

                    if (!Enum.TryParse<TShirt.Fabrics>(substrings[3], true, out TShirt.Fabrics fabric))
                        throw new FormatException($"Malformed string in {tshirtsFilename} on line {lineNumber}. Value 4 is not a fabric type.");

                    if (!float.TryParse(substrings[4], out float avgScore))
                        throw new FormatException($"Malformed string in {tshirtsFilename} on line {lineNumber}. Value 5 is not a float.");

                    tshirts.Add(new TShirt(print, price, size, fabric, avgScore));


                    // Generates column data later used for formatting
                    if (print.Length > ColumnLengths.TShirts.Print)
                        ColumnLengths.TShirts.Print = print.Length;

                    if ($"{price:C2}".Length > ColumnLengths.Price)
                        ColumnLengths.Price = $"{price:C2}".Length;
                    
                    if ($"{size}".Length > ColumnLengths.TShirts.Size)
                        ColumnLengths.TShirts.Size = $"{size}".Length;

                    if ($"{fabric}".Length > ColumnLengths.TShirts.Fabric)
                        ColumnLengths.TShirts.Fabric = $"{fabric}".Length;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds upp the total row lengths from columns and padding. Used to give the mug and t-shirt lists equal width through extra padding.
        /// </summary>
        private static void ColumnLengthSummation()
        {
            ColumnLengths.Mugs.Total =
                    ColumnLengths.Mugs.Print + columnPadding +
                    ColumnLengths.Mugs.Type + columnPadding +
                    columnLengthAverageScore + columnPadding +
                    ColumnLengths.Price + columnPadding;

            ColumnLengths.TShirts.Total = ColumnLengths.TShirts.Print + columnPadding +
                ColumnLengths.TShirts.Fabric + columnPadding +
                ColumnLengths.TShirts.Size + columnPadding +
                columnLengthAverageScore + columnPadding +
                ColumnLengths.Price + columnPadding;
        }
    }
}
