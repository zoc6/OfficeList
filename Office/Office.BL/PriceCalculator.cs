using System;
using System.Collections.Generic;
using System.Text;

namespace Office.BL
{
    public static class PriceCalculator
    {
        public static List<ValidPrice> CalculateValidPrices(List<ProductAgreement> inputList)
        {
            // ToDo: review if needed!!!
            inputList.Sort((x, y) => x.Order.CompareTo(y.Order));

            List<ValidPrice> validPrices = new List<ValidPrice>();

            foreach (ProductAgreement agreement in inputList)
            {
                ValidPrice validPrice = new ValidPrice();
                validPrice.StartDate = agreement.StartDate;
                validPrice.EndDate = agreement.EndDate;
                validPrice.Price = agreement.Price;

                validPrices.Add(validPrice);

                // ToDo: Update list incl. overlap handling!!!
            }

            return validPrices;
        }
    }
}
