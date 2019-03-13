using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DepSystems.Enums;
using DepSystems.Models;
using Microsoft.AspNetCore.Http;

namespace DepSystems
{
    public static class CsvProcessor
    {
        public static bool IsCsv(IFormFile formFile)
        {
            const string extension = ".csv";
            const int extensionLength = 4; //".csv".Length
            /*const int requiredLength = extensionLength + 1; */

            var length = formFile.FileName.Length;

            return length > extensionLength 
                && formFile.FileName.Substring(length - extensionLength) == extension;
        }

        public static List<Patient> ReadPatients(IFormFile formFile, List<string> errorMessages)
        {
            List<Patient> patients = new List<Patient>();

            ParseFile(formFile, (patientLine, lineNumber) =>
            {
                ParsePatientLine(patientLine, lineNumber, errorMessages, patients);
            });
            return patients;
        }

        public static List<string> GetClinicianCredentials(IFormFile formFile, out List<Clinician> clinicians)
        {
            List<string> errorMessages = new List<string>();
            clinicians = new List<Clinician>();

            using (StreamReader streamReader = new StreamReader(formFile.OpenReadStream()))
            {
                int lineCount = 0;
                while (streamReader.Peek() > 0)
                {
                    // Track the line count for error messages
                    lineCount++;
                    var line = streamReader.ReadLine();
                    // TODO: Move the replacement of whitespace around
                    var noWhiteSpaceLine = line.Replace(" ", string.Empty);

                    // If the line was only white spaces, the string will now be of length 0
                    if (noWhiteSpaceLine.Length == 0)
                    {
                        continue;
                    }

                    var commaSeparatedValues = noWhiteSpaceLine.Split(',');
                    if (commaSeparatedValues.Length == 1)
                    {
                        errorMessages.Add($"Error Importing Clinician at Line {lineCount}. \n" +
                            $"No comma found");
                        continue;
                    }
                    if (commaSeparatedValues.Length > 2)
                    {
                        errorMessages.Add($"Error Importing Clinician at Line {lineCount}. \n" +
                            $"Too many commas");
                        continue;
                    }

                    bool valid = true;
                    foreach (var value in commaSeparatedValues)
                    {
                        if (!value.All(c => char.IsLetterOrDigit(c)))
                        {
                            errorMessages.Add($"Error Importing Clinician at Line {lineCount}. \n " +
                            $"Non AlphaNumeric character found");
                            valid = false;
                            break;
                        }
                    }

                    if (!valid)
                    {
                        continue;
                    }

                    if (!Clinician.IsValidHCPId(commaSeparatedValues[0]))
                    {
                        errorMessages.Add($"Error Importing Clinician at Line {lineCount}. \n " +
                            $"Clinician HCP ID is not valid");
                        continue;
                    }

                    if (!Clinician.IsValidPassword(commaSeparatedValues[1]))
                    {
                        errorMessages.Add($"Error Importing Clinician at Line {lineCount}. \n " +
                            $"Clinician Password is not valid");
                        continue;
                    }

                    clinicians.Add(
                        new Clinician
                        {
                            HCPId = commaSeparatedValues[0],
                            ClinicianPassword = commaSeparatedValues[1]
                        }
                    );

                }
            }

            return errorMessages;
        }

        public static List<string> ReadBatchPatientData(IFormFile formFile, out List<ListCalculations> batchPatients)
        {
            List<string> errorMessages = new List<string>();
            batchPatients = new List<ListCalculations>();

            using (StreamReader streamReader = new StreamReader(formFile.OpenReadStream()))
            {
                int ethnicityInt = 0;
                int age = 0;
                Ethnicity ethnicity = new Ethnicity();
                Gender gender = new Gender();
                int lineCount = 0;
                while (streamReader.Peek() > 0)
                {
                    lineCount++;
                    var line = streamReader.ReadLine();
                    var noWhiteSpaceLine = line.Replace(" ", string.Empty);

                    if (noWhiteSpaceLine.Length == 0)
                    {
                        continue;
                    }

                    var commaSeparatedValues = noWhiteSpaceLine.Split(',');
                    if (commaSeparatedValues.Length == 1)
                    {
                        errorMessages.Add($"Error Importing Patient at Line {lineCount}. \n" +
                            $"No comma found");
                        continue;
                    }
                    if (commaSeparatedValues.Length > 5)
                    {
                        errorMessages.Add($"Error Importing Patient at Line {lineCount}. \n" +
                            $"Too many commas");
                        continue;
                    }

                    bool valid = true;
                    foreach (var value in commaSeparatedValues)
                    {
                        //if (!value.All(c => char.IsLetterOrDigit(c)))
                        //{
                        //    errorMessages.Add($"Error Importing Patient at Line {lineCount}. \n " +
                        //    $"Non AlphaNumeric character found");
                        //   valid = false;
                        //    break;
                        //}
                    }

                    //if (!valid)
                    //{
                    //    continue;
                    //}

                    //if (!Patient.IsValidNHSNumber(commaSeparatedValues[0]))
                    //{
                    //    errorMessages.Add($"Error Importing Patient at Line {lineCount}. \n " +
                    //        $"Patient NHS Number is not valid");
                    //    continue;
                    //}

                    //if (!Calculation.IsValidEthnicity(commaSeparatedValues[2]))
                    //{
                        //errorMessages.Add($"Error Importing Patient at Line {lineCount}. \n " +
                        //    $"Patient Ethnicity is not valid, use B or O");
                        if (commaSeparatedValues[2].Equals("B", StringComparison.InvariantCultureIgnoreCase))
                        {
                             ethnicityInt = 1;
                        }
                    else
                    {
                        ethnicityInt = 0;
                    }
                        ethnicity = (Ethnicity)ethnicityInt;
                        //continue;

                    //}
                    //if (!Calculation.IsValidAge(commaSeparatedValues[3]))
                    //{
                        //errorMessages.Add($"Error Importing Patient at Line {lineCount}. \n " +
                           // $"Patient Age is invalid");
                        
                        Int32.TryParse(commaSeparatedValues[4], out age);
                        //continue;
                    //}
                    //if (!Calculation.IsValidGender(commaSeparatedValues[1]))
                    //{
                    //    errorMessages.Add($"Error Importing Patient at Line {lineCount}. \n " +
                    //        $"Patient Age is invalid");
                        int gen = 0;
                        Int32.TryParse(commaSeparatedValues[1], out gen);
                        gender = (Gender)gen;
                        //continue;
                    //}
                    double creatineLevel;
                    Double.TryParse(commaSeparatedValues[4], out creatineLevel);
                    ListCalculations addCalculation = new ListCalculations(commaSeparatedValues[0], age, ethnicity, gender, creatineLevel);
                    batchPatients.Add(addCalculation);
                      
               }
            }
            return errorMessages;
        }

        private static void ParseFile(IFormFile formFile, Action<string, int> lineParser)
        {
            using (StreamReader streamReader = new StreamReader(formFile.OpenReadStream()))
            {
                int lineCount = 0;

                // While there is a character to read
                while (streamReader.Peek() > 0)
                {
                    ++lineCount; // Track the line count
                    string line = streamReader.ReadLine();
                    lineParser(line, lineCount);
                }
            }
        }

        private static void ParsePatientLine(string patientLine, int lineNumber, List<string> errorMessages, List<Patient> patients)
        {
            string patientLineWithoutWhiteSpace = patientLine.Replace(" ", string.Empty);

            if (patientLineWithoutWhiteSpace.Length > 0)
            {
                string[] CSVs = patientLineWithoutWhiteSpace.Split(',');

                if (CSVs.Length == 1)
                {
                    errorMessages.Add($"Error Importing Patient at Line {lineNumber}. \n" +
                        $"No comma found");
                    return;
                }

                if (CSVs.Length > 2)
                {
                    errorMessages.Add($"Error Importing Patient at Line {lineNumber}. \n" +
                        $"Too many commas");
                    return;
                }

                if (!Patient.IsValidNHSNumber(CSVs[0]))
                {
                    errorMessages.Add($"Error Importing Patient at Line {lineNumber}. \n " +
                        $"Patient NHS Number is not valid");
                    return;
                }

                if (!Patient.IsValidPassord(CSVs[1]))
                {
                    errorMessages.Add($"Error Importing Patient at Line {lineNumber}. \n " +
                        $"Patient Password is not valid");
                    return;
                }

                patients.Add(
                    new Patient
                    {
                        NHSNumber = CSVs[0],
                        Password = CSVs[1]
                    }
                );
            }
        }
    }
}
