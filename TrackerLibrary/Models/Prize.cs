using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    public class Prize
    {
        public int Id { get; set; }
        public int PlaceNumber { get; set; }
        public string PlaceName { get; set; }
        public decimal PrizeAmount { get; set; }
        public double PrizePercentage { get; set; }

        public Prize()
        {

        }

        public Prize(string placeNumber, string placeName, string prizeAmount, string prizePercentage)
        {
            PlaceName = placeName;

            int.TryParse(placeNumber, out int pn);
            PlaceNumber = pn;

            decimal.TryParse(prizeAmount, out decimal pa);
            PrizeAmount = pa;

            double.TryParse(prizePercentage, out double pp);
            PrizePercentage = pp;
        }

        public override string ToString() => $"{PlaceNumber}. Platz - {PlaceName}";
    }
}
