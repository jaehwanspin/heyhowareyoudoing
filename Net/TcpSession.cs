
namespace AppSystemSimulator.Net
{
    public class TcpSession : NetService, System.IDisposable
    {
        private TcpServer ServerRef_;
        private string SessionId_;

        public string SessionId { get { return this.SessionId_; } }

        public delegate void DisconnectedHandler(TcpSession session);
        public event DisconnectedHandler Disconnected;
        private void RaiseDisconnected(TcpSession session)
            => Disconnected?.Invoke(session);

        public TcpSession(TcpServer serverRef, string sessionId, System.Net.Sockets.Socket acceptedSocket) :
            base(acceptedSocket)
        {
            this.ServerRef_ = serverRef;
            this.SessionId_ = sessionId;
        }

        ~TcpSession()
        {
        }

        public void Initialize(ProtocolFields protoc)
        {
            base.Initialize(protoc, this.Socket.RemoteEndPoint);
            this.DoRecv();
        }

        protected override void DoRecv()
        {
            this.Socket.BeginReceive(this.RecvBuffer_, 0,
                            this.RecvBuffer_.Length, 0,
                            new System.AsyncCallback(RecvCallback), this);
        }

        /// <summary>
        /// 지정한 소켓에 송신 함수
        /// </summary>
        /// <param name="packet">패킷</param>
        public override void Send(byte[] data)
        {
            try
            {
                if (this.Socket.Connected == true)
                {
                    this.Socket.BeginSend(data, 0, data.Length, 0, new System.AsyncCallback(SendCallback), this.Socket);
                }
                else
                {
                    this.HandleError();
                }
            }
            catch (System.Exception e)
            {
                this.HandleError();
            }
        }

        /// <summary>
        /// 비동기 송신 함수 
        /// </summary>
        /// <param name="ar">소켓</param>
        private void SendCallback(System.IAsyncResult ar)
        {
            try
            {
                int bytesSent = this.Socket.EndSend(ar);

                if (bytesSent <= 0 || this.Socket.Connected == false)
                {
                    this.HandleError();
                }

                this.RaiseSent(this, bytesSent);
            }
            catch (System.Exception e)
            {
                this.HandleError();
            }
        }

        protected override void HandleError()
        {
            this.RaiseDisconnected(this);
            this.Socket.Close();
        }


        /// <summary>
        /// 리소스 해제 함수
        /// </summary>
        public void Dispose()
        {
            this.RaiseDisconnected(this);
            this.Socket.Close();
        }
    }

}