using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    class Program
    {
        static string[] messages = 
        {
            "ok",
            "hello",
            "bad"
        };


        static string GetRandomMessage()
        {
            Random random = new Random();
            return messages[random.Next(messages.Length)]; // 0, 1, 2 
        }


        static void Main(string[] args)
        {
            Socket s = null;
            try
            {
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                IPEndPoint ep = new IPEndPoint(ip, 45001);
                 s = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                Console.WriteLine("Сервер запущен----------------------------------------------------------");
                s.Bind(ep);
                s.Listen(10);

                Socket ns = null;
                while (true)
                {
                    ns = s.Accept();
                    
                    Console.WriteLine("Конечная точка клиента: " + ns.RemoteEndPoint.ToString());
                    // Console.WriteLine(ns.LocalEndPoint);

                    /*обработка инфы от клиента-----------------------start*/
                    byte[] bytes = new byte[1024];
                    int byteLen = ns.Receive(bytes);

                    string clientRequest = Encoding.UTF8.GetString(bytes, 0, byteLen);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Запрос клиента: {clientRequest}") ;
                    Console.ResetColor();
                    /*обработка инфы от клиента-----------------------end*/


                    /**/
                    string prediction = GetRandomMessage();
                    /**/

                    ns.Send(Encoding.UTF8.GetBytes($"Поздравляю, вот ваше случайное предсказание: {prediction }"));
                    ns.Shutdown(SocketShutdown.Both);
                    ns.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                s.Shutdown(SocketShutdown.Both);
                s.Close();
            }
           
        }
    }
}
