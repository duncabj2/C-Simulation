using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Simulation
{
    public class CalendarClass
    {
        List<EventClass> eventList { get; set; }
        public int totalLengthInQueue;

        //CONSTRUCTOR\\
        public CalendarClass()
        {
            //Envoke the list (Actually create it)
            eventList = new List<EventClass>();

            //Static first event/entity
            EntityClass firstEntity = new EntityClass(1, ECallType.stereo);
            EventClass firstEvent = new EventClass(firstEntity, EEventType.CallArrive, 0);
            addEvent(firstEvent);

            //Static Last Event/Entity
            EntityClass lastEntity = new EntityClass(1, ECallType.stereo);
            EventClass lastEvent = new EventClass(lastEntity, EEventType.EndSim, 10800);
            //addEvent(lastEvent);
        } 

        //METHODS\\
        public void addEvent(EventClass newEvent)
        {
            //Add the new event to the event list
            eventList.Add(newEvent);
            SortEvents();
        }

        public void SortEvents()
        {
            //Sort the Events by Schedualed Time.
            //eventList.Sort();
            //IComparable on the Events
            EventCompare eventComapre = new EventCompare();
            eventList.Sort(eventComapre);
        }

        public EventClass getTopEvent()
        {
            //Get the next Event in the list
            EventClass topEvent = eventList.First();
            //Remove the event from the list so you dont use the same event over and over
            eventList.RemoveAt(0); 
            //*******************\\
           //Can see an issue here\\

            //Return the top event
            return topEvent;
        }

        public void displayCalendar(ListBox displayBox)
        {
            foreach (EventClass item in eventList)
            {
                //Display to the Screen
                displayBox.Items.Add(item.ToString());
            }
        }
    }
}