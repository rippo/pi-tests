using System;
using Raspberry.IO.GeneralPurpose;

namespace Blinky
{
    class Program
    {
        //private static bool ledIsOn = false;

        static void Main(string[] args)
        {



            // Here we create a variable to address a specific pin for output
            // There are two different ways of numbering pins--the physical numbering, and the CPU number
            // "P1Pinxx" refers to the physical numbering, and ranges from P1Pin01-P1Pin40
            var led1 = ConnectorPin.P1Pin07.Output();

            led1.StatusChangedAction += StatusChangedAction;

            // Here we create a connection to the pin we instantiated above
            var connection = new GpioConnection(led1);

            for (var i = 0; i < 100; i++)
            {
                // Toggle() switches the high/low (on/off) status of the pin
                connection.Toggle(led1);
                System.Threading.Thread.Sleep(250);
            }





            connection.Close();

        }

        private static void StatusChangedAction(bool state)
        {
            Console.WriteLine(state ? "On" : "Off");
            //ledIsOn = state;
        }
    }
}
