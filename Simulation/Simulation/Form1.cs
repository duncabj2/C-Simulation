using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulation
{
    public partial class Form1 : Form
    {
        //VARIABLES (Declearation)\\
        SimulatorClass simClass;
        QueueClass queueClass;
        CalendarClass mainCalendar;
        SalesRepClass salesRep;

        EntityClass activeEntity;
        EventClass activeEvent;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //What happens when the form is loaded
            simClass = new SimulatorClass(listBox1, this, label1, numericUpDown1, label3, listBox2);
            //RENAME THE FUCKING OBJECTS NOT JUST LISTBOX1
            //Don't be lazy.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Button on will run the simulation
            simClass.RunSim();
            
        } 

        private void button2_Click(object sender, EventArgs e)
        {
            //Button Two is for testing
            CalendarClass newCalendar = new CalendarClass();

            EntityClass newEntity = new EntityClass(0, ECallType.stereo);
            EventClass newCallEvent = new EventClass(newEntity, EEventType.CallArrive, 0);
            newCalendar.addEvent(newCallEvent);

            /*EntityClass newEntity1 = new EntityClass(1, ECallType.other);
            EventClass newCallEvent1 = new EventClass(newEntity1, EEventType.CompleteServiceCall, 100);
            newCalendar.addEvent(newCallEvent1);

            EntityClass newEntity2 = new EntityClass(2, ECallType.other);
            EventClass newCallEvent2 = new EventClass(newEntity2, EEventType.CompleteIVR, 50);
            newCalendar.addEvent(newCallEvent2);

            newCalendar.displayCalendar(listBox1);*/

            //VARS
            /*activeEvent = mainCalendar.getTopEvent();
            activeEntity = activeEvent.currentEntity;
            int currentTime = 0;

            //While Loop to see what the simulation will load (call Types)
            while (activeEvent.currentEventType != EEventType.EndSim)
            {
                //Update the System Time
                currentTime = activeEvent.schedualedTime;
                //If conditions (UPDATE TO CASE STATEMENTS)
                if (activeEvent.currentEventType == EEventType.CallArrive)
                {
                    //Process the call
                    simClass.ProcessCallArrive();
                    //Console.WriteLine("ONE");
                }
                if (activeEvent.currentEventType == EEventType.CompleteIVR)
                {
                    //Process the Switch
                    simClass.CompleteIVR();
                    Console.WriteLine("TWO");
                }
                if (activeEvent.currentEventType == EEventType.CompleteServiceCall)
                {
                    //Process the Completed call
                    simClass.CompleteServiceCall();
                    Console.WriteLine("THREE");
                }
                if (activeEvent.currentEventType == EEventType.EndSim)
                {
                    //End the Simulation
                    simClass.EndSim();
                    Console.WriteLine("FOUR");
                }
            }*/
            newCalendar.displayCalendar(listBox1);
        }
    }
}
