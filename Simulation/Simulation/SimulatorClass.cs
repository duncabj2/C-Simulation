using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Simulation
{
    class SimulatorClass
    {
        CalendarClass mainCalendar;
        QueueClass mainQueue;
        DiceRoller diceRoller;
        SalesTeamClass salesTeam;
        SalesRepClass salesRep;
        EntityClass activeEntity;
        EventClass activeEvent;
        ListBox box;
        ListBox box2;
        Form1 mainForm;
        Label onlyLable;
        Label queueCount;

        public int hangupCount;
        int currentTime;
        int GLOBALENTITYID;
        public int totalQueueLength;

        public SimulatorClass(ListBox listbox, Form1 formyForm, Label label, NumericUpDown MAX_ON_HOLD_UP_DOWN, Label lable2, ListBox listBox2)
        {
            mainCalendar = new CalendarClass();
            //mainEvent = new EventClass(); //Something might be wrong here            
            mainQueue = new QueueClass();
            diceRoller = new DiceRoller();
            salesTeam = new SalesTeamClass();
            box = listbox;
            box2 = listBox2;
            mainForm = formyForm;
            onlyLable = label;
            queueCount = lable2;
            //salesRep = new SalesRepClass();
            hangupCount = 0;
            GLOBALENTITYID = 2;
            currentTime = 0;
            GlobalVars.MAX_ON_HOLD = (int)MAX_ON_HOLD_UP_DOWN.Value;
            totalQueueLength = (int)mainQueue.totalLengthOfQueue();
        }

        public void RunSim()
        {
            activeEvent = mainCalendar.getTopEvent();
            activeEntity = activeEvent.currentEntity;

            while (activeEvent.currentEventType != EEventType.EndSim)
            {
                //update the system time.
                currentTime = activeEvent.schedualedTime;
                if (activeEvent.currentEventType == EEventType.CallArrive)
                {
                    //Process the call.
                    ProcessCallArrive();
                }
                if (activeEvent.currentEventType == EEventType.CompleteIVR)
                {
                    //Process the switch.        
                    CompleteIRV();
                }
                if (activeEvent.currentEventType == EEventType.CompleteServiceCall)
                {
                    //Process the completion of the service call.
                    CompleteServiceCall();
                }
                if (activeEvent.currentEventType == EEventType.EndSim)
                {
                    //End the simulation.
                    EndSim();
                }

                activeEvent = mainCalendar.getTopEvent();
                activeEntity = activeEvent.currentEntity;

                mainCalendar.displayCalendar(box);
                box.Items.Add("-----------------------------");
                onlyLable.Text = "" + hangupCount + " people hung up.";
                queueCount.Text = "" + mainQueue.totalLengthOfQueue();

                //FOR DEBUGGING
                box2.Items.Add("-----------------------------");
                box2.Items.Add("Active Entity: " + activeEvent.currentEntity.entityID);
                box2.Items.Add("Active Event: " + activeEvent.currentEventType);
                box2.Items.Add("Time: " + activeEvent.schedualedTime);
                box2.Items.Add("-----------------------------");

                mainForm.Refresh();
            }
        }

        //****************************************************************
        // PROCESS THE CALL ARRIVE EVENT
        //****************************************************************
        public void ProcessCallArrive()
        {
            if (totalQueueLength < GlobalVars.MAX_ON_HOLD)
            {
                //Continues through the system
                int interval = Convert.ToInt16(diceRoller.DiceRoll() * 0.333);
                int scheduledTime = currentTime + interval;

                EventClass newEvent = new EventClass(activeEntity, EEventType.CompleteIVR, scheduledTime);
                mainCalendar.addEvent(newEvent); //This is the new completeSwitchEvent
            }
            else
            {
                //Too many calls, hangs up
                hangupCount++;
            }

            //CREATES A NEW CALL ARRIVE EVENT
            int newInterval = Convert.ToInt16(diceRoller.DiceRoll() * 0.333);
            int newscheduledTime = currentTime + newInterval;

            //Choose call type. Rule: 16% stereo, rest % is other
            //ran num 1-100 if <=16 streoo
            Random rand = new Random();
            int newRandom = rand.Next(1, 100);

            ECallType newCallType;
            if (newRandom <= 16)
            {
                //Stereo
                newCallType = ECallType.stereo;
            }
            else
            {
                //Other
                newCallType = ECallType.other;
            }

            EntityClass newEntity = new EntityClass(GLOBALENTITYID, newCallType);
            GLOBALENTITYID++;
            EventClass newArriveEvent = new EventClass(newEntity, EEventType.CallArrive, newscheduledTime); //Create new arrivalEvent, Another caller

            //Add to calendar
            mainCalendar.addEvent(newArriveEvent);
        }

        //****************************************************************
        // PROCESS THE CONPLETEIVR EVENT
        //****************************************************************
        public void CompleteIRV()
        {
            //Happens when the call is accepted
            //Get type of call.
            if (activeEntity.callType == ECallType.stereo)
            {
                SalesRepClass freeRep = salesTeam.getFreeRep(ECallType.stereo);
                //Check to see if a sales rep is free.
                if (freeRep == null)
                {
                    //Was not a free person, add them to the approprate queue
                    mainQueue.addToQueue(activeEntity);
                }
                else
                {
                    //Was a free dude                    
                    freeRep.isFree = false;
                    activeEntity.personImTalkingTo = freeRep;

                    int interval = Convert.ToInt16(diceRoller.DiceRoll() * 2);
                    int scheduledTime = currentTime + interval;
                    //New completeService Event
                    EventClass newEvent = new EventClass(activeEntity, EEventType.CompleteServiceCall, scheduledTime);
                    //Add new Event to the calendar
                    mainCalendar.addEvent(newEvent);
                }
            }
            else if (activeEntity.callType == ECallType.other)
            {
                SalesRepClass freeRep = salesTeam.getFreeRep(ECallType.other);
                //Check to see if a sales rep is free.
                if (freeRep == null)
                {
                    //Was not a free person
                    mainQueue.addToQueue(activeEntity);
                }
                else
                {
                    //Was a free dude
                    freeRep.isFree = false; //take that resource
                    activeEntity.personImTalkingTo = freeRep; //Assign it to the active entity

                    int interval = Convert.ToInt16(diceRoller.DiceRoll() * 2);
                    int scheduledTime = currentTime + interval;
                    //New completeService Event
                    EventClass newEvent = new EventClass(activeEntity, EEventType.CompleteServiceCall, scheduledTime);
                    //Add new Event to the calendar
                    mainCalendar.addEvent(newEvent);
                }
            }
        }

        //****************************************************************
        // COMPLETE THE SERVICECALL EVENT
        //****************************************************************
        public void CompleteServiceCall()
        {
            //What happens when they comeplete the service call?
            /*
             *Calls complete
             *Free up that sales rep
             *grab from queue
             */

            //EntityClass newEntity = null;

            if (mainQueue.stereoWaiting.Count == 0) //If there isnt anyone waiting do what?
            {
                Console.WriteLine("Number of People on hold: " + mainQueue.stereoWaiting.Count);
            }
            else
            {
                if (activeEntity.callType == ECallType.stereo) //Someone was in the queue
                {
                    activeEntity.personImTalkingTo.isFree = true; //Frees up the resource
                                                                  //Pretty sure he just dissapares from the system now. He no longer holds up any resources

                    int interval = Convert.ToInt16(diceRoller.DiceRoll() * 2);
                    int scheduledTime = currentTime + interval;
                    //New completeService Event
                    EntityClass newEntity = mainQueue.getEntityWaiting(ECallType.stereo);
                    EventClass newEvent = new EventClass(newEntity, EEventType.CompleteServiceCall, scheduledTime);

                    SalesRepClass freeRep = salesTeam.getFreeRep(ECallType.stereo);
                    newEntity.personImTalkingTo = freeRep;//Assign it to the active entity
                    freeRep.isFree = false;

                    //Add new Event to the calendar
                    mainCalendar.addEvent(newEvent);
                }
            }

            if (mainQueue.otherWaiting.Count == 0)
            {
                Console.WriteLine("Number of people on Hold: " + mainQueue.otherWaiting.Count);
            }
            else
            {
                if (activeEntity.callType == ECallType.stereo) //Someone was in the queue
                {
                    activeEntity.personImTalkingTo.isFree = true; //Frees up the resource
                                                                  //Pretty sure he just dissapares from the system now. He no longer holds up any resources

                    int interval = Convert.ToInt16(diceRoller.DiceRoll() * 2);
                    int scheduledTime = currentTime + interval;
                    //New completeService Event
                    EntityClass newEntity = mainQueue.getEntityWaiting(ECallType.other);
                    EventClass newEvent = new EventClass(newEntity, EEventType.CompleteServiceCall, scheduledTime);

                    SalesRepClass freeRep = salesTeam.getFreeRep(ECallType.stereo);
                    newEntity.personImTalkingTo = freeRep;//Assign it to the active entity
                    freeRep.isFree = false;

                    //Add new Event to the calendar
                    mainCalendar.addEvent(newEvent);
                }

                int interval2 = Convert.ToInt16(diceRoller.DiceRoll() * 2);
                int scheduledTime2 = currentTime + interval2;
                //New completeService Event
                EventClass newEvent2 = new EventClass(activeEntity, EEventType.EndSim, scheduledTime2);
                mainCalendar.addEvent(newEvent2);
            }

            /*if (activeEntity.callType == ECallType.stereo) //Someone was in the queue
            {
                activeEntity.personImTalkingTo.isFree = true; //Frees up the resource
                                                              //Pretty sure he just dissapares from the system now. He no longer holds up any resources

                int interval = Convert.ToInt16(diceRoller.DiceRoll() * 2);
                int scheduledTime = currentTime + interval;
                //New completeService Event
                EntityClass newEntity = mainQueue.getEntityWaiting(ECallType.stereo);
                EventClass newEvent = new EventClass(newEntity, EEventType.CompleteServiceCall, scheduledTime);

                SalesRepClass freeRep = salesTeam.getFreeRep(ECallType.stereo);
                newEntity.personImTalkingTo = freeRep;//Assign it to the active entity
                freeRep.isFree = false;

                //Add new Event to the calendar
                mainCalendar.addEvent(newEvent);
            }
            else
            {
                activeEntity.personImTalkingTo.isFree = true; //Frees up the resource

                int interval = Convert.ToInt16(diceRoller.DiceRoll() * 1);
                int scheduledTime = currentTime + interval;
                //New completeService Event                    
                EntityClass newEntity = mainQueue.getEntityWaiting(ECallType.other);
                EventClass newEvent = new EventClass(newEntity, EEventType.CompleteServiceCall, scheduledTime);

                SalesRepClass freeRep = salesTeam.getFreeRep(ECallType.other);
                newEntity.personImTalkingTo = freeRep; //Assign it to the active entity
                freeRep.isFree = false; //take that resource

                //Add new Event to the calendar
                mainCalendar.addEvent(newEvent);
            }
            //Thats it until the time reaches the EndSim eventTime.*/
        }

        public void EndSim()
        {
            //What happens when the simulation is over?
            //Once the events reach this one simulation is over.
            //Forget the events that occur after this one.
            //Stop the simulation
        }
    }
}
