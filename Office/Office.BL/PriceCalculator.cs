using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Office.BL
{
  public static class PriceCalculator
  {
    public static List<ValidPrice> CalculateValidPrices(List<ProductAgreement> inputList)
    {
      inputList.Sort((x, y) => x.Order.CompareTo(y.Order));

      List<ValidPrice> validPrices = new List<ValidPrice>();

      foreach (ProductAgreement agreement in inputList)
      {
        ValidPrice newPrice = new ValidPrice();
        newPrice.StartDate = agreement.StartDate;
        newPrice.EndDate = agreement.EndDate;
        newPrice.Price = agreement.Price;

        UpdateValidPrices(validPrices, newPrice);
        ConsolidateIntervals(validPrices);
      }

      return validPrices;
    }

    public static void UpdateValidPrices(List<ValidPrice> validPrices, ValidPrice newItem)
    {
      // Premesis: The validPrices List is always sorted by StartDate.
      // We control this by the inserting new items correctly.
      var newItemIndex = 0;

      // Find the interval that contains the new item's start date.
      var intervalContainingNewStartDate = validPrices.Where(v => newItem.StartDate >= v.StartDate).LastOrDefault();

      // Find the interval that contains the new item's end date.
      var intervalContainingNewEndDate = validPrices.Where(v => newItem.EndDate <= v.EndDate).FirstOrDefault();

      // Special case: new item lies in one single old interval.
      if (intervalContainingNewStartDate != null && intervalContainingNewStartDate == intervalContainingNewEndDate)
      {
        // The new item is either splits the old period (or they are identical)
        if (intervalContainingNewStartDate.StartDate == newItem.StartDate && intervalContainingNewStartDate.EndDate == newItem.EndDate)
        {
          // The timespans are identical. Simply update the price.
          intervalContainingNewStartDate.Price = newItem.Price;
          return;
        }
        else
        {
          // ToDo: Handle special case: StartDate or EndDate identical!
          // ...
          // ...

          // Split old interval, insert newItem in between.
          var secondPartOfOldInterval = new ValidPrice();
          secondPartOfOldInterval.StartDate = newItem.EndDate.AddDays(1);
          secondPartOfOldInterval.EndDate = intervalContainingNewEndDate.EndDate;
          secondPartOfOldInterval.Price = intervalContainingNewEndDate.Price;

          // Insert new item after first part of old interval.
          newItemIndex = validPrices.IndexOf(intervalContainingNewStartDate) + 1;
          intervalContainingNewStartDate.EndDate = newItem.StartDate.Subtract(new TimeSpan(1, 0, 0, 0));
          validPrices.Insert(newItemIndex, newItem);

          // Insert Second Part of old interval after newItem.
          validPrices.Insert(newItemIndex + 1, secondPartOfOldInterval);
          return;
        }
      }


      // Default: update existing Start/End dates.            

      if (intervalContainingNewStartDate != null)
      {
        newItemIndex = validPrices.IndexOf(intervalContainingNewStartDate) + 1;
        intervalContainingNewStartDate.EndDate = newItem.StartDate.Subtract(new TimeSpan(1, 0, 0, 0));
      }
      if (intervalContainingNewEndDate != null)
      {
        intervalContainingNewEndDate.StartDate = newItem.EndDate.AddDays(1);
      }

      // Insert new item
      validPrices.Insert(newItemIndex, newItem);
    }

    private static void ConsolidateIntervals(List<ValidPrice> validPrices)
    {
      // ToDo: fix things like

      /*
          Start;Ende;Preis
      01.01.2015 00:00:00;01.01.2015 00:00:00;20 !!!
      02.01.2015 00:00:00;01.03.2015 00:00:00;20 !!!
      02.03.2015 00:00:00;31.05.2015 00:00:00;18
      01.06.2015 00:00:00;01.09.2015 00:00:00;16
      02.09.2015 00:00:00;31.12.2015 00:00:00;18
          */
    }
  }
}
