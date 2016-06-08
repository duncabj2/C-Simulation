using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    public class DiceRoller
    {
        Random rGen = new Random();

        public int DiceRoll()
        {
            //Random number between 1,2,3,4,5,6 Just like a dice.
            int minutes = rGen.Next(1, 7) + rGen.Next(1, 7);
            int seconds = minutes * 60;

            return seconds;

            //Could be better with the random numbers. Distrubution issue.
        }

        public DiceRoller()
        {
            //Constructor
        }
    }
}