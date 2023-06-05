using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartClient();
        }

        private static void StartClient()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, 12345);

            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sender.Connect(remoteEndPoint);
                
                Console.WriteLine($"Socket connected to {sender.RemoteEndPoint}");

                byte[] msg = Encoding.ASCII.GetBytes("This is a test message. <EOF>");
                int bytesSent = sender.Send(msg);

                int bufferSize = 1024;
                byte[] bytes = new byte[bufferSize];

                int bytesReceived = sender.Receive(bytes);

                Console.WriteLine($"Response received = {Encoding.ASCII.GetString(bytes, 0, bytesReceived)}");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
        }
    }
}