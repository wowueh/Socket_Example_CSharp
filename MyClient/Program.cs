using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace MyClient
{
    class Program
    {
        private const int BUFFER_SIZE = 1024;
        private const int PORT_NUMBER = 7826;

        static ASCIIEncoding encoding = new ASCIIEncoding();

        public static void Main()
        {
            try
            {
                // IPAddress address = IPAddress.Parse("127.0.0.1");
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), PORT_NUMBER);

                // tao ra client
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // client connect den cong
                client.Connect(iep);

                String command = "";
                while (!command.Equals("quit"))
                {
                    // cho nhap lenh
                    command = Console.ReadLine();
                    // gui lenh
                    client.Send(encoding.GetBytes(command));

                    byte[] data = new byte[BUFFER_SIZE];
                    int rec = client.Receive(data);
                    Console.WriteLine("server: " + encoding.GetString(data, 0, rec));
                }

                client.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }
    }
}