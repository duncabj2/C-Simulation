using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    class QueueClass
    {
        //Create the Queue list for each call type
        public Queue<EntityClass> stereoWaiting;
        public Queue<EntityClass> otherWaiting;

        //CONSTRUCTOR\\
        public QueueClass()
        {
            //Allocate memory
            stereoWaiting = new Queue<EntityClass>();
            otherWaiting = new Queue<EntityClass>();
        }

        //METHODS\\
        public int totalLengthOfQueue()
        {
            //Return the total length of the queue
            //Includes the length of both other and stereo queues
            int stereoCount = stereoWaiting.Count();
            int otherCount = otherWaiting.Count();
            int totalLength = Convert.ToInt16(stereoCount);

            return totalLength;
        }

        //GET THE NEXT WAITING ENTITY IN THE QUEUE
        public EntityClass getEntityWaiting(ECallType callType)
        {
            EntityClass nextEntity = null;

            if (callType == ECallType.stereo) //If the call is stereo
            {
                //If there is someone waiting
                if (stereoWaiting.Count > 0)
                {
                    nextEntity = stereoWaiting.Dequeue();
                }
            }
            else
            {
                if (otherWaiting.Count > 0)
                {
                    nextEntity = otherWaiting.Dequeue();
                }
            }

            return nextEntity;
        }

        //ADD ENTITY TO THE QUEUE
        public void addToQueue(EntityClass activeEntity)
        {
            //Add the entity to the queue
            if (activeEntity.callType == ECallType.stereo)
            {
                //Add to the stereo queue
                stereoWaiting.Enqueue(activeEntity);
            }
            if (activeEntity.callType == ECallType.other)
            {
                otherWaiting.Enqueue(activeEntity);
            }
        }
    }
}