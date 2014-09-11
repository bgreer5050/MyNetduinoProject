using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NetduinoApplication1
{
    public class Program
    {
        public static void Main()
        {
            //// write your code here

            //bool x = true;

            OutputPort port8 = new OutputPort(Pins.GPIO_PIN_D8, true);
            OutputPort port7 = new OutputPort(Pins.GPIO_PIN_D7, false);
            //OutputPort led = new OutputPort(Pins.ONBOARD_LED, false);

            ////while(x)
            ////{
            ////    led.Write(true);
            ////    Thread.Sleep(3000);
            ////    led.Write(false);
            ////    Thread.Sleep(3000);

            ////}
            //Thread.Sleep(Timeout.Infinite);

            Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].EnableDhcp();
            WebServer webServer = new WebServer();
            webServer.ListenForRequest();
        }

    }
}
