using System;
using System.ComponentModel.DataAnnotations;
using DepSystems.Enums;

namespace DepSystems.Models
{
    public class Calculation
    {
        [Required]
        public int Age { get; set; }

        [Required]
        public Ethnicity Ethnicity { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        // Add a max/min validation attribute
        public double CreatineLevel { get; set; }

        public double Calculate()
        {
            //eGFR  value  in  ml/min/1.73m2 = 
            //186  x  (Creat/  88.4)-1.154 x  (Age)-0.203 x  (0.742  if  female)  x (1.210 if blackethnicity)
            //const umolToDl = need for conversion 

            double eth = 1 + ((int)Ethnicity * 0.210);
            double gen = 1 - ((int)Gender * 0.258);

            return 186 * Math.Pow(CreatineLevel / 88.4, -1.154) * Math.Pow(Age, -0.203) * gen * eth;
        }
    }
}
