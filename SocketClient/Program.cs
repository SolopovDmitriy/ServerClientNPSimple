using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketClient("127.0.0.1", 45001, "Случайное предсказание");
        }
        public static void SocketClient(string ipAdress, int port, string request)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAdress), port);
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string response = "";
            try
            {
                s.Connect(endPoint);
                if (s.Connected)
                {
                    s.Send(Encoding.UTF8.GetBytes(request));
                    int LenResponse = 0;
                    byte[] buffer = new byte[1024];
                    do
                    {
                        LenResponse = s.Receive(buffer);
                        response += Encoding.UTF8.GetString(buffer, 0, LenResponse);
                    } while (LenResponse > 0);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Ответ сервера ---------------------------------- Начало ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(response);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Ответ сервера ---------------------------------- Конец ");
                    Console.ResetColor();
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
            Console.ReadKey();
        }
    }
}
