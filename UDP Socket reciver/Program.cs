using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UDP_Socket_reciver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the ports you want to listen to seperated by a space");
            string Sports = Console.ReadLine();
            string[] ports = Sports.Split(' ');
            foreach (string port in ports)
            {
                Thread th = new Thread(() => server(Int16.Parse(port)));
                th.Start();
            }

            Console.WriteLine("Press any key to exit");

            Console.ReadKey();
        }
        private static void DataRecived(IAsyncResult ar)
        {
            UdpClient c = (UdpClient)ar.AsyncState;
            IPEndPoint recivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Byte[] recivedBytes = c.EndReceive(ar, ref recivedIpEndPoint);
            //Convert data to ASCII and print in console
            String recivedText = ASCIIEncoding.ASCII.GetString(recivedBytes);
            Console.Write(recivedIpEndPoint + ": " + recivedText + Environment.NewLine);
            c.BeginReceive(DataRecived, ar.AsyncState);
        }
        private static void server(int port)
        {      
            //New instance of the UdpClient class 
            UdpClient reciver = new UdpClient(port);
            Console.WriteLine("Listening on: " + port.ToString());
            //Starting asyncron receiving
            reciver.BeginReceive(DataRecived, reciver);
            //Sends a test message to self to test if the program is working as intended
            using (UdpClient sender = new UdpClient(port + 1)) sender.Send(Encoding.ASCII.GetBytes("This is a test"), 14, "localhost", port);
        }
    }
}
