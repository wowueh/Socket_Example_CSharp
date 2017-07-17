using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace MyServer
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
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), PORT_NUMBER);

                Console.WriteLine("dang cho client ket noi...");

                // tao ra server
                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // server lang nghe ket noi tu client
                server.Bind(iep);
                server.Listen(10);

                Socket client = server.Accept();
                Console.WriteLine("Chap nhan ket noi tu: " + client.RemoteEndPoint.ToString());

                byte[] data = new byte[BUFFER_SIZE];
                String result = "";
                while (true)
                {
                    // nhan du lieu tu client
                    int rec = client.Receive(data);
                    // chuyen ve string
                    string command = encoding.GetString(data, 0, rec);
                    Console.WriteLine("Client: " + command);
                    if (command.Equals("gethostname"))
                    {
                        // lay ten server
                        IPEndPoint endPoint = (IPEndPoint)client.RemoteEndPoint;
                        IPAddress ipAddress = endPoint.Address;
                        IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
                        result = hostEntry.HostName;
                    }
                    else if (command.Equals("gethosttime"))
                    {
                        // lay thoi gian 
                        result = DateTime.Now.ToString("h:mm:ss tt");
                    }
                    else if (command.Equals("gethostdate"))
                    {
                        // lay ngay
                        result = DateTime.Now.ToString("dd:MM:yyyy");
                    }
                    else if (command.Equals("quit"))
                    {
                        // dong
                        server.Close();
                        client.Close();
                        break;
                    }
                    else
                    {
                        result = "khong phai lenh dung";
                    }
                    // tra ve ket qua cho client
                    client.Send(encoding.GetBytes(result));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }
    }
}