using System;
using System.ComponentModel.DataAnnotations;
using DepSystems.Enums;

namespace DepSystems.Models
{
    public class Calculation
    {
        /// <summary>
        /// The minimum (inclusive) age that a person can be in order to calculate their eGFR value.
        /// </summary>
        public const int MIN_AGE_INCLUSIVE = 18;

        /// <summary>
        /// The maximum (inclusive) age that a person can be in order to calculate the eGFR value.
        /// </summary>
        public const int MAX_AGE_INCLUSIVE = 110;

        //public const double MIN_CREATINE_INCLUSIVE = 0.0001;
        //public const double MAX_CREATINE_INCLUSIVE = 100.0000;


        /// <summary>
        /// The age of the patient
        /// </summary>
        [Required(ErrorMessage = "Please provide an age")]
        [Range(MIN_AGE_INCLUSIVE, MAX_AGE_INCLUSIVE, ErrorMessage = "Please provide a valid age (18-100)")] // Has to be a const string, cannot use the const ints
        public int Age { get; set; }

        /// <summary>
        /// The ethnicity of the patient
        /// </summary>
        [Required(ErrorMessage = "Please provide your ethnicity")]
        public Ethnicity Ethnicity { get; set; }

        /// <summary>
        /// The gender of the patient
        /// </summary>
        [Required(ErrorMessage = "Please provide your gender")]
        public Gender Gender { get; set; }

        
        /// <summary>
        /// The creatinine level of the patient at the time of calculation
        /// </summary>
        //[Range(MIN_CREATINE_INCLUSIVE, MAX_CREATINE_INCLUSIVE)]
        [Required(ErrorMessage = "Please provide a creatine value")]
        [Display(Name = "Creatinine")]
        public double CreatinineLevel { get; set; }

        /// <summary>
        /// Whether to use mgdl as the creatinine metric
        /// </summary>
        public bool UseMgdl { get; set; }

        /// <summary>
        /// Default constructor needed by .NET mvc for injecting empty models into partial views
        /// </summary>
        public Calculation()
        {
        }

        /// <summary>
        /// Constructor that initialises patient details
        /// </summary>
        /// <param name="a">Patient Age</param>
        /// <param name="e">Patient Ethnicity</param>
        /// <param name="g">Patient Gender</param>
        /// <param name="cl">Patient Creatinine Level</param>
        public Calculation(int a, Ethnicity e, Gender g, double cl)
        {
            Age = a;
            Ethnicity = e; 
            Gender = g;
            CreatinineLevel = cl;
        }

        /// <summary>
        /// Calculates the eGFR value based upon the age, gender, ethnicity and creatinine value
        /// </summary>
        /// <returns>The eGFR result</returns>
        public double Calculate()
        {
            //eGFR  value  in  ml/min/1.73m2 = 
            //186  x  (Creat/  88.4)-1.154 x  (Age)-0.203 x  (0.742  if  female)  x (1.210 if blackethnicity)
            //const umolToDl = need for conversion 

            double eth = 1 + ((int)Ethnicity * 0.210);
            double gen = 1 - ((int)Gender * 0.258);

            double result = 186 * Math.Pow(CreatinineLevel / 88.4, -1.154) * Math.Pow(Age, -0.203) * gen * eth;
            if(UseMgdl)
            {
                result /= 176.3551637279597;
            }
            return Math.Round(result, 2);
        }

        /// <summary>
        /// Evaluates whether the calculation model is valid (the properties have valid values)
        /// </summary>
        /// <returns>True if the age is between the correct range and the creatinine level is more than 0</returns>
        public bool IsValid()
        {
            return Age >= MIN_AGE_INCLUSIVE && Age <= MAX_AGE_INCLUSIVE
                && CreatinineLevel > 0; //= MIN_CREATINE_INCLUSIVE && CreatinineLevel <= MAX_CREATINE_INCLUSIVE;
        }

        /// <summary>
        /// Evaluates whether a given string can represent an age within the valid age range
        /// </summary>
        /// <param name="ageString">A string representing an age</param>
        /// <returns>True if the age is between MIN_AGE_INCLUSIVE and MAX_AGE_INCLUSIVE</returns>
        public static bool IsValidAge(string ageString)
        {
            int age = -1;

            Int32.TryParse(ageString, out age);
            return age >= MIN_AGE_INCLUSIVE && age <= MAX_AGE_INCLUSIVE;
        }

        /// <summary>
        /// Evaluates whether a given string can represent a valid ethnicity.
        /// </summary>
        /// <param name="c">A string representing ethnicity</param>
        /// <returns>True if the string is "B" (black) or "O" (other)</returns>
        public static bool IsValidEthnicity(string c)
        {
            if(!c.Equals("B", StringComparison.InvariantCultureIgnoreCase) || !c.Equals("O", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Evaluates whether a given string can represent a valid gender.
        /// </summary>
        /// <param name="genSring">A string representing gender</param>
        /// <returns>True if the string is "1" (female) or "0" (male)</returns>
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
