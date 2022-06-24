using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Motd
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage:  Motd.exe <ip> <port>");
                Environment.Exit(1);
            }
            else
            {
                try
                {
                    Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);
                    client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);
                    client.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0));
                    string[] Guids = Guid.NewGuid().ToString().ToUpper().Split('-');
                    int TickCount = Environment.TickCount;
                    string Text = $"01{TickCount.ToString("X").PadLeft(16, '0')}00FFFF00FEFEFEFEFDFDFDFD12345678{Guids[2]}{Guids[4]}";
                    byte[] sendBytes = new byte[Text.Length / 2];
                    for (int i = 0; i < Text.Length / 2; i++)
                    {
                        sendBytes[i] = Convert.ToByte(Text.Substring(i * 2, 2), 16);
                    }
                    DateTime StartTime = DateTime.Now;
                    client.SendTo(sendBytes, new IPEndPoint(
                        IPAddress.Parse(args[0]),
                        int.TryParse(args[1], out int j) ? j : -1)
                        );
                    EndPoint point = new IPEndPoint(IPAddress.Any, 0);
                    byte[] buffer = new byte[1024];
                    int length = client.ReceiveFrom(buffer, ref point);
                    string Data = length > 35 ?
                        Encoding.UTF8.GetString(buffer, 35, length - 35) :
                        Encoding.UTF8.GetString(buffer, 0, length);
                    client.Close();
                    Console.WriteLine("[Success]" + Data);
                }
                catch (Exception e)
                {
                    Console.WriteLine("[Failure]" + e.Message);
                    Environment.Exit(1);
                }
            }
        }
    }
}