using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepSystems.Models
{
    public class Calculation
    {
        public string Ethnicity { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public double CreatinLevel { get; set; }

        public double Calculate()
        {
            //eGFR  value  in  ml/min/1.73m2 = 
            //186  x  (Creat/  88.4)-1.154 x  (Age)-0.203 x  (0.742  if  female)  x (1.210 if blackethnicity)

            //const umolToDl = need for conversion 

            double eth = (Ethnicity == "1") ? 1.210 : 1.0;

            double gen = (Gender == "1") ? 0.742 : 1.0;

            return 186 * Math.Pow(CreatinLevel / 88.4, -1.154) * Math.Pow(Age, -0.203) * gen * eth;

        }
    }
}
