using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Buffers;
using System.IO;
using CsvHelper;

namespace DepSystems.Models
{
    public class FileHandler
    {
        private string filePath { get; set; }
        private string fileExtension { get; set; }
        
        public void ReadFromCSVFile(string path)
        {
            filePath = path;
            fileExtension = Path.GetExtension(Path.GetFileName(filePath));

            if (FileExists(path))
            {
                if (!IsCsv(fileExtension))
                {
                    //"The file you're trying to upload is not a csv file."
                }
                else
                {
                    TextReader textReader = new StreamReader(filePath);
                    CsvReader csvReader = new CsvReader(textReader);
                    var records = csvReader.GetRecords<Patient>();
                }
            }
        }

        public bool FileExists(string file)
        {
            if (!File.Exists(file))
            {
                //"The file you're trying to upload does not exist."
                return false;
            }
            else { return true; }
        }

        public bool IsCsv(string extension)
        {
            if (!extension.Equals("csv"))
            {
                //"The file you're trying to upload is not a csv file."
                return false;
            }
            else { return true; }
        }
    }
}
