using System;
using System.Threading;
using System.Threading.Tasks;
using Raspberry.IO.Components.Converters.Mcp3008;
using Raspberry.IO.Components.Devices.PiFaceDigital;
using Raspberry.IO.Components.Sensors;
using Raspberry.IO.Components.Sensors.Temperature.Tmp36;
using Raspberry.IO.GeneralPurpose;
using UnitsNet;

namespace Themy
{
    class Program
    {

        private static PiFaceDigitalDevice piFace;
        private static bool polling = true;
        private static Task t;

        static void Main(string[] args)
        {
            piFace = new PiFaceDigitalDevice();


            // setup events
            foreach (var ip in piFace.InputPins)
            {
                ip.OnStateChanged += ip_OnStateChanged;
            }

            t = Task.Factory.StartNew(() => PollInputs());

            for (int i = 0; i < 10; i++)
            {
                for (int pin = 0; pin < 8; pin++)
                {
                    piFace.OutputPins[pin].State = true;
                    piFace.UpdatePiFaceOutputPins();
                    Thread.Sleep(500);
                    piFace.OutputPins[pin].State = false;
                    piFace.UpdatePiFaceOutputPins();
                    Thread.Sleep(500);
                }
            }

            //stop polling loop
            polling = false;
            t.Wait();


        }

        /// <summary>
        /// Loop polling the inputs at 200ms intervals
        /// </summary>
        private static void PollInputs()
        {
            while (polling)
            {
                piFace.PollInputPins();
                Thread.Sleep(200);
            }
        }

        /// <summary>
        /// Log any input pin changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ip_OnStateChanged(object sender, InputPinChangedArgs e)
        {
            Console.WriteLine("Pin {0} became {1}", e.pin.Id, e.pin.State);
        }
    }
}
