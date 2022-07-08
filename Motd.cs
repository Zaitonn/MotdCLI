using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Motd
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket client;
            IPAddress ip = null;
            int port = 0;
            if (args.Length != 3 && args.Length != 2)
            {
                Console.WriteLine($"Usage:  {AppDomain.CurrentDomain.FriendlyName} <udp|tcp> <ip> [port]");
                Environment.Exit(1);
            }
            else
            {
                try
                {
                    if (!new Regex(
                        @"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))")
                        .IsMatch(args[1]))
                    {
                        IPAddress[] IPs = Dns.GetHostAddresses(args[1]);
                        ip = IPs[0];
                    }
                    else
                    {
                        ip = IPAddress.Parse(args[1]);
                    }
                    if (args.Length == 3)
                    {
                        port = int.TryParse(args[2], out int j) ? j > 0 && j < 65536 ? j : 0 : 0;
                    }
                    else if (args.Length == 2)
                    {
                        port = args[0].ToLower() == "udp" ? 19132 : args[0].ToLower() == "tcp" ? 25565 : 0;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(1);
                }
            }
            if (args[0].ToLower() == "udp")
            {
                try
                {
                    client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);
                    client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);
                    client.Bind(new IPEndPoint(IPAddress.Any, 0));
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
                        ip,
                        port)
                        );
                    EndPoint point = new IPEndPoint(IPAddress.Any, 0);
                    byte[] buffer = new byte[1024];
                    int length = client.ReceiveFrom(buffer, ref point);
                    string Data = length > 35 ?
                        Encoding.UTF8.GetString(buffer, 35, length - 35) :
                        Encoding.UTF8.GetString(buffer, 0, length);
                    client.Close();
                    Console.WriteLine(Data);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(1);
                }
            }
            else if (args[0].ToLower() == "tcp")
            {
                try
                {
                    client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);
                    client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);
                    client.Connect(
                        new IPEndPoint(
                            ip,
                            port
                            )
                        );
                    client.Send(
                        new byte[] { 6, 0, 0, 0, 0x63, 0xdd, 1, 1, 0 }
                        );
                    byte[] buffer = new byte[1024 * 1024];
                    int length = client.Receive(buffer);
                    string Data = length>5?
                        Encoding.UTF8.GetString(buffer, 5, length-5):
                        Encoding.UTF8.GetString(buffer, 0, length);
                    client.Close();
                    Console.WriteLine(Data);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(1);
                }
            }
        }
    }
}