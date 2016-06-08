using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    class SalesTeamClass
    {
        //List of Sales Reps for both stereo and other
        List<SalesRepClass> stereoRep;
        List<SalesRepClass> otherRep;

        //CONSTRUCTOR\\
        public SalesTeamClass()
        {
            //Allocate space for the lists
            stereoRep = new List<SalesRepClass>();
            otherRep = new List<SalesRepClass>();

            //Statically create Sales Rep
            SalesRepClass salesRep = new SalesRepClass(ECallType.stereo);
            //Add to the Stereo List
            stereoRep.Add(salesRep);

            //Rinse and repeat
            SalesRepClass salesRep2 = new SalesRepClass(ECallType.other);
            SalesRepClass salesRep3 = new SalesRepClass(ECallType.other);
            //Add to the Stereo List
            otherRep.Add(salesRep2);
            //otherRep.Add(salesRep3);
        }

        //METHODS\\
        public SalesRepClass getFreeRep(ECallType callType)
        {
            if (callType == ECallType.stereo)
            {
                foreach (SalesRepClass item in stereoRep)
                {
                    if (item.isFree == true)
                    {
                        //If there is a free stereo rep, return them.
                        return item;
                    }
                }
                
            }
            if (callType == ECallType.other)
            {
                foreach (SalesRepClass item in otherRep)
                {
                    if (item.isFree == true)
                    {
                        //If there is a free other rep, return them.
                        return item;
                    }
                }

            }

            //If couldnt find free rep
            return null;
        }
    }
}