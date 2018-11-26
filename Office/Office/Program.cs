using System;
using System.Collections.Generic;
using System.IO;
using Office.BL;

namespace Office
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    throw new ArgumentException("Usage: Office.exe <Fully qualified source file path>");                    
                }

                string sourceFilePath = args[0];

                List<ProductAgreement> inputList = CsvReader.ReadFile(sourceFilePath);

                List<ValidPrice> outputList = PriceCalculator.CalculateValidPrices(inputList);

                string targetFilePath = string.Concat(Path.GetDirectoryName(sourceFilePath), @"\Output.csv");
                CsvWriter.WriteFile(targetFilePath, outputList);

            }
            catch (ArgumentException ex)
            {
                string message = $"Invalid argument exception. {ex.Message}";
                Console.WriteLine(message);
            }
            catch (Exception ex)
            {
                string message = $"An excepction occured. {ex.Message}";
                Console.WriteLine(message);
            }

            Console.WriteLine("Hello World!");
        }
    }
}
