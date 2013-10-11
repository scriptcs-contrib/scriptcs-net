namespace ScriptCs.Net
{
    using System;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    public class EmmiterTcpClient
    {
        private const int BufferSize = 4096;

        private readonly TcpClient socket;

        private Action<byte[]> onData;

        private Action onClose;

        private Action<Exception> onError;

        private NetworkStream stream;

        private CancellationTokenSource cts;

        public EmmiterTcpClient(TcpClient socket)
        {
            this.socket = socket;
            this.cts = new CancellationTokenSource();
        }

        public async void Run()
        {
            try
            {
                var buffer = new byte[BufferSize];

                this.stream = this.socket.GetStream();

                int bytesRead = await this.stream.ReadAsync(buffer, 0, BufferSize, this.cts.Token);

                while (bytesRead != 0 && !this.cts.Token.IsCancellationRequested)
                {
                    await this.stream.FlushAsync(this.cts.Token);

                    if (this.onData != null)
                    {
                        var bytes = buffer.TakeWhile((b, i) => i < bytesRead).ToArray();
                        this.onData.Invoke(bytes);
                    }

                    bytesRead = await this.stream.ReadAsync(buffer, 0, 4096, this.cts.Token);
                }

                if (this.onClose != null)
                {
                    this.onClose.Invoke();
                }
            }
            catch (ObjectDisposedException)
            {
                if (!this.cts.IsCancellationRequested)
                {
                    throw;
                }
                
                // otherwise, the error is expected since the socket is closed
                // source: http://social.msdn.microsoft.com/Forums/vstudio/en-US/588d25b5-75df-483b-a658-7da9cb547b69/cancel-beginread?forum=netfxbcl
            } 
            catch (Exception e)
            {
                if (this.onError == null)
                {
                    throw;
                }

                this.onError(e);
            }
            finally
            {
                this.Close();
            }
        }

        public void On(Action<byte[]> data = null, Action close = null, Action<Exception> error = null)
        {
            this.onData = data;
            this.onClose = close;
            this.onError = error;
        }

        public async Task WriteAsync(byte[] buffer)
        {
            await this.stream.WriteAsync(buffer, 0, buffer.Length, this.cts.Token);
        }

        public void Close()
        {
            if (this.cts.Token.IsCancellationRequested)
            {
                return;
            }

            this.cts.Token.Register(
                () =>
                {
                    this.socket.Close();

                    if (this.stream != null)
                    {
                        this.stream.Close();
                    }
                });

            this.cts.Cancel();
        }
    }
}