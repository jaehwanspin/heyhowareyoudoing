using System.Linq;

namespace AppSystemSimulator.Net
{
    class UdpTransceiver : NetService, System.IDisposable
    {
        private System.Net.EndPoint MyEndpoint_;

        public UdpTransceiver() :
            base(new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                        System.Net.Sockets.SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp))
        {
            
        }

        public void Initialize(int myPort, string ipAdress, int port, ProtocolFields protoc)
        {
            base.Initialize(protoc, new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ipAdress), port));
            this.MyEndpoint_ = new System.Net.IPEndPoint(System.Net.IPAddress.Any, myPort);
            this.Socket.Bind(this.MyEndpoint_);
        }

        protected override void HandleError()
        {
            this.Socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                        System.Net.Sockets.SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);
            DoRecv();
        }

        protected override void DoRecv()
        {
            this.Socket.BeginReceiveFrom(
                RecvBuffer_,
                0,
                RecvBuffer_.Length,
                System.Net.Sockets.SocketFlags.None,
                ref this.ServiceEndpoint_,
                new System.AsyncCallback(RecvCallback),
                this.Socket);
        }

        public void Send(byte[] data)
        {
            try
            {
                this.Socket.BeginSendTo(
                    data,
                    0,
                    data.Length,
                    0,
                    this.ServiceEndpoint_,
                    new System.AsyncCallback(SendCallback),
                    this.Socket);
                
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
        }

        private void SendCallback(System.IAsyncResult ar)
        {
            try
            {
                int bytesSent = this.Socket.EndSend(ar);
                this.RaiseSent(this, bytesSent);

                if (bytesSent <= 0 || this.Socket.Connected == false)
                {
                    HandleError();
                }
            }
            catch (System.Exception e)
            {
                HandleError();
            }
        }

        /// <summary>
        /// 리소스 해제 함수
        /// </summary>
        public void Dispose()
        {
        }
    }
}
