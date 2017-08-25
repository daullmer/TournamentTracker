using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public interface IDataConnection
    {
        Prize CreatePrize(Prize model);
    }
}
