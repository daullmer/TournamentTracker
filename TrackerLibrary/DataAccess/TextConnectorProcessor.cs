using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {
        public static string FullFilePath(this string fileName)
        {
            return $"{ ConfigurationManager.AppSettings["filePath"] }\\{ fileName }";
        }

        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        public static List<Prize> ConvertToPrizes(this List<string> lines)
        {
            List<Prize> output = new List<Prize>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                Prize p = new Prize
                {
                    Id = int.Parse(cols[0]),
                    PlaceNumber = int.Parse(cols[1]),
                    PlaceName = cols[2],
                    PrizeAmount = decimal.Parse(cols[3]),
                    PrizePercentage = double.Parse(cols[4])
                };
                output.Add(p);
            }
            return output;
        }

        public static void SaveToPrizeFile(this List<Prize> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (Prize p in models)
            {
                lines.Add($"{ p.Id },{p.PlaceNumber},{p.PlaceName},{p.PrizeAmount},{p.PrizePercentage}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static List<Person> ConvertToPerson (this List<string> lines)
        {
            List<Person> output = new List<Person>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                Person p = new Person
                {
                    Id = int.Parse(cols[0]),
                    FirstName = cols[1],
                    LastName = cols[2],
                    EmailAdress = cols[3]
                };
                output.Add(p);
            }
            return output;
        }

        public static void SaveToPeopleFile(this List<Person> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (Person p in models)
            {
                lines.Add($"{ p.Id },{p.FirstName},{p.LastName},{p.EmailAdress}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }
    }
}
