using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    class EventCompare : IComparer<EventClass>
    {
        public int Compare(EventClass firstEvent, EventClass secondEvent)
        {
            if (firstEvent == null)
            {
                if (secondEvent == null)
                {
                    //Both events are null, they are equal
                    return 0;
                }
                else
                {
                    //If firstEvent is null and secondEvent is not, secondEvent is greater
                    return -1;
                }
            }
            else
            {
                //If firstEvent is not null
                if (secondEvent == null)
                {
                    return 1;
                }
                else
                {
                    int retval = firstEvent.schedualedTime.CompareTo(secondEvent.schedualedTime);
                    if (retval != 0)
                    {
                        //if the times are not of equal length,
                        //The longer number is greater
                        return retval;
                    }
                    else
                    {
                        //If the times are the same, sort them
                        return firstEvent.CompareTo(secondEvent);
                    }
                }
            }
        }
    }
}
