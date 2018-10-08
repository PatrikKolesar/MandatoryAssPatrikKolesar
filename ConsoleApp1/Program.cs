using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using MandatoryAssignment;

namespace TCPServer
{
    class Program
    {
        private static readonly int PORT = 7;
        static void Main(string[] args)
        {
            IPAddress localAddress = IPAddress.Loopback;
            // ip and port of server
            TcpListener serverSocket = new TcpListener(localAddress, PORT);
            //starting server
            serverSocket.Start();
            Console.WriteLine("TCP Server running on port" + PORT);
            while (true) // server loop keeps it running
            {
                try
                {
                    // waiting for incomming client
                    TcpClient client = serverSocket.AcceptTcpClient();
                    Console.WriteLine("incoming client");
                    // give service to this client
                    Task.Run(() => DoIt(client));
                }
                catch (IOException ex)
                {
                    Console.WriteLine("exception. will continue");

                }

            }
        }
        //read what client wants
        private static void DoIt(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            //read only
            StreamReader reader = new StreamReader(stream);
            //write
            StreamWriter writer = new StreamWriter(stream);
            //end
            while (true)
            {
                string request = reader.ReadLine();
                if (request == "bye") break;
                if (string.IsNullOrEmpty(request)) { break; }
                Console.WriteLine("request: " + request);
                string response = request; //echo

                var splitRequest = request.Split();
                double conversionNumber = double.Parse(splitRequest[1]);
                Console.WriteLine("Converting" + conversionNumber + " to ounces:");

                switch (splitRequest[0])
                {
                    case ("converttoounces"):
                        Class1 Grams = new Class1();
                        double OunceResult = Grams.ConvertionGramToOunce(conversionNumber);
                        Console.WriteLine("Result: " + OunceResult + " ounces");
                        break;
                    case ("converttograms"):
                        Class1 Ounces = new Class1();
                        double GramResult = Ounces.ConvertionOunceToGram(conversionNumber);
                        Console.WriteLine($"Result: " + GramResult + " grams");
                        break;
                }


                Console.WriteLine(response);
                writer.WriteLine(response);
                writer.Flush();

            }
            client.Close();

        }

    }
}
