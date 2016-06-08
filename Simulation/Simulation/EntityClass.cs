using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    //PUBLIC VARS\\
    public enum ECallType { stereo, other}

    public class EntityClass
    {
        //Entities are things such as people or the call itself

        public ECallType callType { get; set; }
        public SalesRepClass personImTalkingTo { get; set; }
        public int entityID { get; set; }
        public int startQTime { get; set; }
        public int endQTime { get; set; }
        public int startSystemTime { get; set; }
        public int endSystemTime { get; set; }

        //CONSTRUCTOR\\
        public EntityClass(int entityID, ECallType callType)
        {
            this.callType = callType;
            this.entityID = entityID;
            startQTime = 0;
            startSystemTime = 0;
            endQTime = 0;
            endSystemTime = 0;
        }
    }
}
