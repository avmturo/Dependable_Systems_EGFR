using DepSystems.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DepSystems.Models
{
    public class ListCalculations
    {
        public String NHSNumber { get; set; }


        [Required(ErrorMessage = "Please provide an age")]
        [Range(1, 130, ErrorMessage = "Please provide a valid age (1-130)")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Please provide your ethnicity")]
        public Ethnicity Ethnicity { get; set; }

        [Required(ErrorMessage = "Please provide your gender")]
        public Gender Gender { get; set; }

        // Add a max/min validation attribute
        [Required(ErrorMessage = "Please provide a creatine value")]
        [Display(Name = "Creatine")]
        public double CreatineLevel { get; set; }

        public double Value { get; set; }

        public ListCalculations()
        {
        }

        public ListCalculations(String nhs, int a, Ethnicity e, Gender g, double cl)
        {
            NHSNumber = nhs;
            Age = a;
            Ethnicity = e;
            Gender = g;
            CreatineLevel = cl;

            double eth = 1 + ((int)Ethnicity * 0.210);
            double gen = 1 - ((int)Gender * 0.258);

            Value = 186 * Math.Pow(CreatineLevel / 88.4, -1.154) * Math.Pow(Age, -0.203) * gen * eth;
        }


        public double Calculate()
        {
            //eGFR  value  in  ml/min/1.73m2 = 
            //186  x  (Creat/  88.4)-1.154 x  (Age)-0.203 x  (0.742  if  female)  x (1.210 if blackethnicity)
            //const umolToDl = need for conversion 

            double eth = 1 + ((int)Ethnicity * 0.210);
            double gen = 1 - ((int)Gender * 0.258);

            return 186 * Math.Pow(CreatineLevel / 88.4, -1.154) * Math.Pow(Age, -0.203) * gen * eth;
            List<int> test = new List<int>(); 
        }
    }
}
