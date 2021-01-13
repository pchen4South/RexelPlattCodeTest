using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using PlattCodingChallenge.Models;

namespace PlattCodingChallenge.Helpers
{
    public class Comparers
    {
        public class PlanetDiameterComparer : IComparer<PlanetDetailsViewModel>
        {
            
            /// Method to determine if a string is a number
            /// returns true if "value" is numeric, otherwise false
            public static bool IsNumeric(string value)
            {
                return int.TryParse(value, out _);
            }

            /// Compares Two Planets by their diameter (larger is first)
            /// if the diameter is not a number (ie "unknown") it is last
            public int Compare(PlanetDetailsViewModel p1, PlanetDetailsViewModel p2)
            {
                const int P1BeforeP2 = -1;
                const int P2BeforeP1 = 1;

                var IsNumeric1 = IsNumeric(p1.Diameter);
                var IsNumeric2 = IsNumeric(p2.Diameter);

                //If both are numeric
                if (IsNumeric1 && IsNumeric2)
                {
                    var i1 = Convert.ToInt32(p1.Diameter);
                    var i2 = Convert.ToInt32(p2.Diameter);

                    if (i1 > i2)
                    {
                        return P1BeforeP2;
                    }

                    if (i1 < i2)
                    {
                        return P2BeforeP1;
                    }

                    return 0;
                }

                //if P1 is numeric and P2 is "unknown"
                if (IsNumeric1)
                {
                    return P1BeforeP2;
                }

                //If P2 is numeric and P1 is "unknown"
                if (IsNumeric2)
                {
                    return P2BeforeP1;
                }

                //If both are "unknown", order by name alphabetically
                return String.Compare(p1.Name, p2.Name, comparisonType: StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
