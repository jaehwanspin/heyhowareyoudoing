namespace AppSystemSimulator.Net
{
    public class TcpClient : NetService, System.IDisposable
    {
        public delegate void DisconnectedHandler(TcpClient client);
        public event DisconnectedHandler Disconnected;
        private void RaiseDisconnected(TcpClient client)
            => Disconnected?.Invoke(client);

        private System.Timers.Timer ReconnectionTimer_;
        
        public TcpClient() : base(new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                    System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp))
        {
            this.ReconnectionTimer_ = new System.Timers.Timer(3000);
            this.ReconnectionTimer_.Elapsed += this.ReconnectionReadyCallback;
        }


        public delegate void ConnectedHandler(TcpClient client);
        public event ConnectedHandler Connected;
        private void RaiseConnected(TcpClient client)
            => Connected?.Invoke(client);

        public void Initialize(string ipAdress, int port, ProtocolFields protoc)
        {
            base.Initialize(protoc, new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ipAdress), port));
            this.Socket.BeginConnect(this.EndPoint, new System.AsyncCallback(ConnectCallback), this.Socket);
        }


        /// <summary>
        /// 비동기 연결 함수
        /// </summary>
        /// <param name="ar">소켓</param>
        private void ConnectCallback(System.IAsyncResult ar)
        {
            try
            {
                var client = ar.AsyncState as System.Net.Sockets.Socket;

                if (client.Connected == false)
                {
                    client.Close();
                    this.ReconnectionTimer_.Start();
                }
                else
                {
                    bool completed = ar.IsCompleted;
                    if (completed)
                    {
                        client.EndConnect(ar);
                        this.DoRecv();
                        RaiseConnected(this);
                    }
                }
            }
            catch (System.Exception e)
            {
                this.Socket.Close();
                this.ReconnectionTimer_.Start();
            }
        }

        protected override void HandleError()
        {
            this.RaiseDisconnected(this);
            this.Socket.Close();
            this.ReconnectionTimer_.Start();
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
                    HandleError();
                }
            }
            catch (System.Exception e)
            {
                HandleError();
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

        private void ReconnectionReadyCallback(object sender, System.Timers.ElapsedEventArgs args)
        {
            this.ReconnectionTimer_.Stop();
            this.Reconnect();
        }

        private void Reconnect()
        {
            this.Socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                    System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            this.Socket.BeginConnect(this.EndPoint, new System.AsyncCallback(ConnectCallback), this.Socket);
        }

        /// <summary>
        /// 리소스 해제 함수
        /// </summary>
        public void Dispose()
        {
            //dequeueControl = false;
            //state.workSocket.Close();
        }
    }
}
