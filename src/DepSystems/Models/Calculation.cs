using System;
using System.ComponentModel.DataAnnotations;
using DepSystems.Enums;

namespace DepSystems.Models
{
    public class Calculation
    {
        public const int MIN_AGE_INCLUSIVE = 18;
        public const int MAX_AGE_INCLUSIVE = 110;

        public const double MIN_CREATINE_INCLUSIVE = 0.0001;
        public const double MAX_CREATINE_INCLUSIVE = 100.0000;

        [Required(ErrorMessage = "Please provide an age")]
        [Range(MIN_AGE_INCLUSIVE, MAX_AGE_INCLUSIVE, ErrorMessage = "Please provide a valid age (18-100)")] // Has to be a const string, cannot use the const ints
        public int Age { get; set; }

        [Required(ErrorMessage = "Please provide your ethnicity")]
        public Ethnicity Ethnicity { get; set; }

        [Required(ErrorMessage = "Please provide your gender")]
        public Gender Gender { get; set; }

        // Add a max/min validation attribute
        [Required(ErrorMessage = "Please provide a creatine value")]
        [Display(Name = "Creatine")]
        [Range(MIN_CREATINE_INCLUSIVE, MAX_CREATINE_INCLUSIVE)]
        public double CreatineLevel { get; set; }

        public Calculation()
        {
        }

        public Calculation(int a, Ethnicity e, Gender g, double cl)
        {
            Age = a;
            Ethnicity = e; 
            Gender = g;
            CreatineLevel = cl;
        }

        public double Calculate()
        {
            //eGFR  value  in  ml/min/1.73m2 = 
            //186  x  (Creat/  88.4)-1.154 x  (Age)-0.203 x  (0.742  if  female)  x (1.210 if blackethnicity)
            //const umolToDl = need for conversion 

            double eth = 1 + ((int)Ethnicity * 0.210);
            double gen = 1 - ((int)Gender * 0.258);

            return 186 * Math.Pow(CreatineLevel / 88.4, -1.154) * Math.Pow(Age, -0.203) * gen * eth;
        }

        public bool IsValid()
        {
            return Age >= MIN_AGE_INCLUSIVE && Age <= MAX_AGE_INCLUSIVE
                && CreatineLevel >= MIN_CREATINE_INCLUSIVE && CreatineLevel <= MAX_CREATINE_INCLUSIVE;
        }

        public static bool IsValidAge(string ageString)
        {
            int age = -1;

            Int32.TryParse(ageString, out age);
            if (age < 0 && age > 130)
            {
                return false;
            }
            return true;
        }

        public static bool IsValidEthnicity(string c)
        {
            if(!c.Equals("B", StringComparison.InvariantCultureIgnoreCase) || !c.Equals("O", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            return true;
        }

        public static bool IsValidGender(string genSring)
        {
            int gen = -1;

            Int32.TryParse(genSring, out gen);
            if (gen != 1 || gen!= 0)
            {
                return false;
            }
            return true;
        }
    }
}
