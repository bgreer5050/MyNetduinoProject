using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Net.Sockets;
using System.Text;

namespace NetduinoApplication1
{
    public class WebServer : IDisposable
    {
        private Socket socket = null;
        //open connection to onbaord led so we can blink it with every request
        private OutputPort led = new OutputPort(Pins.ONBOARD_LED, false);
        public WebServer()
        {
           
                //Initialize Socket class
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //Request and bind to an IP from DHCP server
                //IPAddress address = new IPAddress(167823217);
            
                socket.Bind(new IPEndPoint(IPAddress.Any, 80));
               // socket.Bind(new IPEndPoint(address, 80));

                //Debug print our IP address
                Debug.Print(Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].IPAddress);
                var x = Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].IPAddress.ToString();
                //Start listen for web requests
                socket.Listen(10);
                ListenForRequest();
          
        }

        public void ListenForRequest()
        {
            socket.ReceiveTimeout = 500;

            while (true)
            {


                using (Socket clientSocket = socket.Accept())
                {

                    //Get clients IP
                    IPEndPoint clientIP = clientSocket.RemoteEndPoint as IPEndPoint;
                    EndPoint clientEndPoint = clientSocket.RemoteEndPoint;
                    //int byteCount = cSocket.Available;
                    int bytesReceived = clientSocket.Available;
                    var buffer = new byte[1024];
                        if (clientSocket.Receive(buffer, 1024, SocketFlags.None)>0)
                        {
                            //Get request
                            //byte[] buffer = new byte[bytesReceived];
                            int byteCount = clientSocket.Receive(buffer, bytesReceived, SocketFlags.None);
                            string request = new string(Encoding.UTF8.GetChars(buffer));
                            Debug.Print(request);
                            //Compose a response
                            string response = @"<center>";
                
                            response += @"<img src=http://www.seeedstudio.com/depot/images/product/Netduino%20plus2.jpg />";
                            response += @"<br />";
                            response += @"<hr />";

                            response += @"<h3 style='background-color:yellow'>Apollo has detected that the 15500 drawbench has been down for 22 minutes.  Please log the reason
                            to restart your machine</h3>";

                            response += @"<div class='btn-group btn-group-lg'>";

                            response += @"<button type='button' class='btn btn-primary btn-sm'style='margin:5px';>Tooling</button>";
                            response += @"<button type='button' class='btn btn-primary btn-sm'style='margin:5px';>Maintenance</button>";
                            response += @"<button type='button' class='btn btn-primary btn-sm'style='margin:5px';>Break</button>";
                            response += @"<button type='button' class='btn btn-primary btn-sm'style='margin:5px';>Start Up</button>";
                            response += @"</div>";

                            //*****************************************Radio*******************************************************
                            response += @"<div>";
                            response += @"<div class='radio'>";
                            response += @"<label>";
                            response += @"<input type='radio' name='rdoDown' value='Tooling'>";
                            response += "Tooling";
                            response += @"</label>";
                            response += @"</div>";


                            response += @"<div class='radio'>";
                            response += @"<label>";
                            response += @"<input type='radio' name='rdoDown' value='Maintenance'>";
                            response += "Maintenance";
                            response += @"</label>";
                            response += @"</div>";


                            response += @"<div class='radio'>";
                            response += @"<label>";
                            response += @"<input type='radio' name='rdoDown' value='Break'>";
                            response += "Break";
                            response += @"</label>";
                            response += @"</div>";


                            response += @"<div class='radio'>";
                            response += @"<label>";
                            response += @"<input type='radio' name='rdoDown' value='Start Up'>";
                            response += "Start Up";
                            response += @"</label>";

                            response += @"</div>";
                            response += @"</div>";

                            response += @"</center>";
                            //*******************************************End Radio***************************************************
                            response += @"<link rel='stylesheet' href='//ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/themes/smoothness/jquery-ui.css' />";
                            response += @"<link rel='stylesheet' href='//maxcdn.bootstrapcdn.com/bootstrap/3.3.0/css/bootstrap.min.css' />";

                            response += @"<script src='//ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js'></script>";
                            response += @"<script src='//ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/jquery-ui.min.js'></script>";
                            response += @"<script src='//maxcdn.bootstrapcdn.com/bootstrap/3.3.0/js/bootstrap.min.js'></script>";

                            
                                                        

                            string header = "HTTP/1.0 200 OK\r\nContent-Type: image; charset=utf-8\r\nContent-Length: " + response.Length.ToString() + "\r\nConnection: close\r\n\r\n";
                             header = "HTTP/1.0 200 OK\r\nContent-Type: text/html; charset=utf-8\r\nContent-Length: " + response.Length.ToString() + "\r\nConnection: close\r\n\r\n";

                            clientSocket.Send(Encoding.UTF8.GetBytes(header), header.Length, SocketFlags.None);
                            clientSocket.Send(Encoding.UTF8.GetBytes(response), response.Length, SocketFlags.None);
                            //Blink the onboard LED
                            led.Write(true);
                            Thread.Sleep(150);
                            led.Write(false);
                        }
                    
                }
            }
        }
        #region IDisposable Members
        ~WebServer()
        {
            Dispose();
        }
        public void Dispose()
        {
            if (socket != null)
                socket.Close();
        }
        #endregion
    }
}
