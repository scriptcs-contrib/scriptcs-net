namespace ScriptCs.Net
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    using ScriptCs.Contracts;

    public class Net : IScriptPackContext
    {
        private const string DefaultConnectAddress = "127.0.0.1";

        public TcpServer CreateServer(Action<EmmiterTcpClient> connectionCallback)
        {
            return new TcpServer(connectionCallback);
        }

        public EmmiterTcpClient Connect(int port, string address = DefaultConnectAddress, Action onConnect = null)
        {
            var client = new TcpClient();

            var socket = new EmmiterTcpClient(client);
            client.ConnectAsync(new IPAddress(address.GetIpAddressBytes()), port).ContinueWith(t =>
                {
                    if (onConnect != null)
                    {
                        onConnect();
                    }

                    socket.Run();
                });

            return socket;
        }
    }
}
