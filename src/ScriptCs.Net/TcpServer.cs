namespace ScriptCs.Net
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    public class TcpServer
    {
        private readonly Action<EmmiterTcpClient> onConnection;

        private CancellationTokenSource cts;

        private TcpListener serverSocket;

        private bool closed;

        public TcpServer(Action<EmmiterTcpClient> onConnection)
        {
            this.onConnection = onConnection;
            this.closed = false;
        }

        public async Task Listen(int port, string address)
        {
            var bytes = address.Split('.').Select(byte.Parse).ToArray();

            var localEndpoint = new IPEndPoint(new IPAddress(bytes), port);

            this.serverSocket = new TcpListener(localEndpoint);

            this.serverSocket.Start();

            while (!this.closed)
            {
                var clientSocket = await this.serverSocket.AcceptTcpClientAsync();

                var emmiterSocket = new EmmiterTcpClient(clientSocket);

                emmiterSocket.Run();

                this.onConnection(emmiterSocket);
            }
        }

        public void Close()
        {
            this.serverSocket.Stop();
            this.closed = true;
        }
    }
}