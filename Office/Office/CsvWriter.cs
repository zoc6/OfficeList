using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Office.BL;

namespace Office
{
    class CsvWriter
    {
        public static void WriteFile(string filePath, List<ValidPrice> validPrices)
        {            
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Write header
                writer.WriteLine("Start;Ende;Preis");

                // Write data lines
                foreach (ValidPrice price in validPrices)
                {
                    writer.WriteLine($"{price.StartDate};{price.EndDate};{price.Price}");
                }
            }            
        }
    }
}
