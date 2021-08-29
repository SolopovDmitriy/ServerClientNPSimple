using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NPSimple
{
    class Program
    {
        public static void SocketConnect(IPAddress iPAddress)
        {
            IPEndPoint endPoint = new IPEndPoint(iPAddress, 80);
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            string response = "";
            try
            {
                s.Connect(endPoint);
                if (s.Connected)
                {
                    string request = "GET\r\n\r\n";
                    s.Send(Encoding.UTF8.GetBytes(request));

                    int lenResponse = 0;
                    byte[] buffer = new byte[1024];
                    do
                    {
                        lenResponse = s.Receive(buffer);
                        response += Encoding.UTF8.GetString(buffer, 0, lenResponse);
                    } while (lenResponse > 0 );

                    Console.WriteLine("Ответ сервера: -------------------------------------------------------start");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(response);
                    Console.ResetColor();
                    Console.WriteLine("Ответ сервера: -------------------------------------------------------end");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            finally
            {
                s.Shutdown(SocketShutdown.Both);
                s.Close();
            }
        }
        static void Main(string[] args)
        {
            IPAddress[] ips = Dns.GetHostAddresses("google.com");
            foreach (var item in ips)
            {
                /* Byte[] bytes = item.GetAddressBytes();
                 for (int i = 0; i < bytes.Length; i++)
                 {
                     Console.Write(bytes[i] + ".");
                 }*/
                Console.WriteLine(item.ToString());
                Console.WriteLine();
                SocketConnect(item);
            }
        }
    }
}
