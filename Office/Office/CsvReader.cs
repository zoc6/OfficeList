using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Office.BL;

namespace Office
{
    static class CsvReader
    {
        private static int startColumn = -1;
        private static int endColumn = -1;
        private static int priceColumn = -1;
        private static int orderColumn = -1;

        public static List<ProductAgreement> ReadFile(string filePath)
        {
            List<ProductAgreement> productAgreements;

            using (StreamReader reader = new StreamReader(filePath))
            {                            
                InitializeColumns(reader);
                productAgreements = ReadLines(reader);
            }

            return productAgreements;
        }

        private static void InitializeColumns(StreamReader reader)
        {
            string header = reader.ReadLine();

            string[] columns = header.Split(';');

            if (columns.Length != 4)
            {
                throw new InvalidDataException("Invalid source file: header must contain 4 columns.");
            }

            for (int i = 0; i < columns.Length; i++)
            {
                switch (columns[i])
                {
                    case "Start": startColumn = i; break;
                    case "Ende": endColumn = i; break;
                    case "Preis": priceColumn = i; break;
                    case "Reihenfolge": orderColumn = i; break;
                }
            }

            if (!((startColumn >= 0 && startColumn < 4) && (endColumn >= 0 && endColumn < 4) && (priceColumn >= 0 && priceColumn < 4) && (orderColumn >= 0 && orderColumn < 4)))
            {
                throw new InvalidDataException("Invalid source file: invalid column headers.");
            }
        }

        private static List<ProductAgreement> ReadLines(StreamReader reader)
        {
            List<ProductAgreement> productAgreements = new List<ProductAgreement>();
            int lineCount = 1;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] columns = line.Split(';');

                if (columns.Length != 4)
                {
                    throw new InvalidDataException($"Invalid source file: Line {lineCount} does not contain 4 columns.");
                }

                productAgreements.Add(new ProductAgreement
                {
                    StartDate = DateTime.Parse(columns[startColumn]),
                    EndDate = DateTime.Parse(columns[endColumn]),
                    Price = Decimal.Parse(columns[priceColumn]),
                    Order = int.Parse(columns[orderColumn])
                });

                lineCount++;
            }

            return productAgreements;
        }
    }
}
