using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_DM
{
    public class ApiOut<T>
    {
        public T Out { get; set; }
        public string Msg { get; set; }
    }
}
