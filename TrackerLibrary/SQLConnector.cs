using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public class SQLConnector : IDataConnection
    {
        /// <summary>
        /// Saves a new prize to the database
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the Id</returns>
        public Prize CreatePrize(Prize model)
        {
            model.Id = 1;

            return model;
        }
    }
}
