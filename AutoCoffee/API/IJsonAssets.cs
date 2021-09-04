using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCoffee.API
{
    public interface IJsonAssets
    {
        int GetObjectId(string name);
        event EventHandler IdsAssigned;
    }
}
