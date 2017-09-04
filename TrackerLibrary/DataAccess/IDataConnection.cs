using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public interface IDataConnection
    {
        Prize CreatePrize(Prize model);
        Person CreatePerson(Person model);
    }
}
