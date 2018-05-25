using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LightStore.DataAccess
{
    public interface IDAO
    {
        string tabelName {get; }
        string keyName {get; }
    }
}