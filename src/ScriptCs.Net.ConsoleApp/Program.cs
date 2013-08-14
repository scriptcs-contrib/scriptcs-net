namespace ScriptCs.Net.ConsoleApp
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var net = (Net)new NetScriptPack().GetContext();

            var server = net.CreateServer(socket =>
            {
                Console.WriteLine("New connection");

                socket.On(
                    data: data => Console.WriteLine(data.AsString()),
                    close: () => Console.WriteLine("Connection closed"),
                    error: e => Console.WriteLine("Error {0}", e.Message));
            });

            Console.WriteLine("Listening at 127.0.0.1:8080");

            server.Listen(8080, "127.0.0.1").Wait();

            Console.WriteLine("Closing server");

            server.Close();
        }
    }
}