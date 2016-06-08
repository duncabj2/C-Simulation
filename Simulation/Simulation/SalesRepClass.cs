using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    public class SalesRepClass
    {
        //VARS\\
        public bool isFree;
        public ECallType repType { get; set; }

        //CONSTRUCTOR\\
        public SalesRepClass(ECallType repType)
        {
            this.isFree = false;
            this.repType = repType;
        }
    }
}