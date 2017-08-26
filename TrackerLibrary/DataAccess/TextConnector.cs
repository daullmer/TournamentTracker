using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        // TODO - Make Method work
        /// <summary>
        /// Saves a new prize to the textfile
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
