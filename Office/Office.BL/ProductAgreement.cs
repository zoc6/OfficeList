using System;

namespace Office.BL
{
    public class ProductAgreement
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Decimal Price { get; set; }
        public int Order { get; set; }
    }
}
