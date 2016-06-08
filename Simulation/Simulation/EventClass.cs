using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    //VARS\\
    public enum EEventType { CallArrive, CompleteIVR, CompleteServiceCall, EndSim}

    public class EventClass : IComparable
    {
        //VARS\\
        public EntityClass currentEntity { get; set; }
        public EEventType currentEventType { get; set; }
        public int schedualedTime { get; set; }

        //CONSTRUCTOR\\
        public EventClass(EntityClass currentEntity, EEventType newEvent, int schedualedTime)
        {
            this.currentEntity = currentEntity;
            this.currentEventType = newEvent;
            this.schedualedTime = schedualedTime;
        }

        //METHODS\\
        public int CompareTo(object obj)
        {
            EventClass otherEvent = (EventClass)obj; //Cast the object to an event

            int compareResult;// = 0;

            if (this.schedualedTime > otherEvent.schedualedTime)
            {
                compareResult = 1;
            }
            if (this.schedualedTime < otherEvent.schedualedTime)
            {
                compareResult = -1;
            }
            else
            {
                compareResult = 0;
            }

            return compareResult;
        }

        public override string ToString()
        {
            return "Current Entity ID: " + currentEntity.entityID + "\r\n" +
                ", Current Event " + currentEventType.ToString() + "\r\n" +
                ", Schedueled Time: " + schedualedTime;

            //return "Current Entity ID: " + currentEntity.entityID + "Current Event Type: " + currentEventType;
        }
    }
}
