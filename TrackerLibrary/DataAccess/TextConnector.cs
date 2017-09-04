using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;
using System.Linq;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "Prizes.csv";
        private const string PeopleFile = "People.csv";

        /// <summary>
        /// Saves a new prize to the textfile
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the Id</returns>
        public Prize CreatePrize(Prize model)
        {
            //load the text file and convert the text to List<Prize>
            List<Prize> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizes();

            // find the max id

            int currentId = 1;
            if (prizes.Count > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }

            // add the new record with the new id (max + 1)
            model.Id = currentId;
            prizes.Add(model);

            // convert the prizes to a list<string>
            // save the list<string> to the text file
            prizes.SaveToPrizeFile(PrizesFile);

            //return the model
            return model;
        }

        public Person CreatePerson(Person model)
        {
            List<Person> people = PeopleFile.FullFilePath().LoadFile().ConvertToPerson();

            int currentId = 1;
            if(people.Count > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            people.Add(model);

            people.SaveToPeopleFile(PeopleFile);

            return model;
        }
    }
}
