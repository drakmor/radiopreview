using System;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;

namespace RadioPreview
{

    class Program
    {
        static async void RewriteGPIO(string hostName, string gpioPort)
        {
            while (true)
            {
                Console.Write("Connecting to {0}:93, GPIO port {1}...", hostName, gpioPort);
                try
                {
                    using (var tcp = new TcpClient())
                    {
                        await tcp.ConnectAsync(hostName, 93);
                        Console.WriteLine("Done.");

                        using (var stream = tcp.GetStream())
                        using (var reader = new StreamReader(stream, Encoding.ASCII))
                        using (var writer = new StreamWriter(stream, Encoding.ASCII))
                        {
                            writer.NewLine = "\n";
                            writer.AutoFlush = true;
                            writer.WriteLine("IP");
                            writer.WriteLine("VER");
                            writer.WriteLine("ADD GPO");
                            while (true && tcp.Connected)
                            {
                                string data = await reader.ReadLineAsync();
                                if (data != null)
                                {
                                    Console.WriteLine(data);
                                    string[] parts = data.Split(' ');
                                    if (parts[0] == "GPO" && parts[1] == gpioPort)
                                    {
                                        await writer.WriteLineAsync("GPI " + gpioPort + " xx" + parts[2].Substring(3, 1));
                                    }
                                }
                                else
                                {
                                    break;
                                }   
                            }
                        }
                        tcp.Close();
                        Console.WriteLine("Connection lost.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Thread.Sleep(1000);
            }

        }

        static void Main(string[] args)
        {

            if (args.Length < 2) {
                Console.WriteLine("Usage: radiopreview.exe host-to-connect gpio-port-number");
                return;
            }
            RewriteGPIO(args[0], args[1]);
            while (true) { Thread.Sleep(1000); }
        }
    }
}
