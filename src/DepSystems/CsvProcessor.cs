﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        public static List<string> GetPatientCredentials(IFormFile formFile, out List<Patient> patients)
        {
            List<string> errorMessages = new List<string>();
            patients = new List<Patient>();

            using (StreamReader streamReader = new StreamReader(formFile.OpenReadStream()))
            {
                int lineCount = 0;
                while(streamReader.Peek() > 0)
                {
                    lineCount++;
                    var line = streamReader.ReadLine();
                    var noWhiteSpaceLine = line.Replace(" ", string.Empty);

                    if(noWhiteSpaceLine.Length == 0)
                    {
                        continue;
                    }

                    var commaSeparatedValues = noWhiteSpaceLine.Split(',');
                    if(commaSeparatedValues.Length == 1)
                    {
                        errorMessages.Add($"Error Importing Patient at Line {lineCount}. \n" +
                            $"No comma found");
                        continue;
                    }
                    if(commaSeparatedValues.Length > 2)
                    {
                        errorMessages.Add($"Error Importing Patient at Line {lineCount}. \n" +
                            $"Too many commas");
                        continue;
                    }

                    foreach(var value in commaSeparatedValues)
                    {
                        if(!value.All(c=> char.IsLetterOrDigit(c)))
                        {
                            errorMessages.Add($"Error Importing Patient at Line {lineCount}. \n " +
                            $"Non AlphaNumeric character found");
                            continue;
                        }
                    }

                    if(!Patient.IsValidNHSNumber(commaSeparatedValues[0]))
                    {
                        errorMessages.Add($"Error Importing Patient at Line {lineCount}. \n " +
                            $"Patient NHS Number is not valid");
                        continue;
                    }

                    if(!Patient.IsValidPassord(commaSeparatedValues[1]))
                    {
                        errorMessages.Add($"Error Importing Patient at Line {lineCount}. \n " +
                            $"Patient Password is not valid");
                        continue;
                    }

                    patients.Add(
                        new Patient
                        {
                            NHSNumber = commaSeparatedValues[0],
                            Password = commaSeparatedValues[1]
                        }
                    );

                }
            }
            return errorMessages;
        }
    }
}
