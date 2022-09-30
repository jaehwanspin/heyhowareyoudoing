using System.Linq;

namespace AppSystemSimulator.Net
{
    /// <summary>
    /// 네트워크 서비스 공통 베이스 클래스
    /// </summary>
    public class NetService : INetService
    {
        /// <summary>
        /// 네트워크 서비스 소켓
        /// </summary>
        protected System.Net.Sockets.Socket ServiceSocket_;
        protected System.Net.EndPoint ServiceEndpoint_;
        public System.Net.Sockets.Socket Socket { 
            get { return this.ServiceSocket_; }
            set { this.ServiceSocket_ = value; }
        }
        public System.Net.EndPoint EndPoint { get { return this.ServiceEndpoint_; } }

        /// <summary>
        /// 응용계층 프로토콜 파서
        /// </summary>
        private ProtocolParser ProtocolParser_;

        public NetService(System.Net.Sockets.Socket serviceSocket)
        {
            this.ServiceSocket_ = serviceSocket;
        }

        protected void Initialize(ProtocolFields protocFields, System.Net.EndPoint serviceEndPoint)
        {
            this.ServiceEndpoint_ = serviceEndPoint;
            this.ProtocolParser_ = new ProtocolParser(protocFields);
        }

        public delegate void SentHandler(NetService service, int bytesSent);
        public event SentHandler Sent;
        protected void RaiseSent(NetService service, int bytesSent)
            => Sent?.Invoke(service, bytesSent);

        protected byte[] RecvBuffer_ = new byte[ushort.MaxValue];
        public delegate void RecvdHandler(NetService service, Net.Message msg);
        public event RecvdHandler Recvd;
        protected void RaiseRecvd(NetService service, Net.Message msg)
            => Recvd?.Invoke(service, msg);

        protected void RecvCallback(System.IAsyncResult ar)
        {
            try
            {
                int bytesRead = this.ServiceSocket_.EndReceive(ar);

                if (bytesRead > 0)
                {
                    for (int bufCount = 0; bufCount < bytesRead; bufCount++)
                    {
                        if (this.ProtocolParser_.PutByte(this.RecvBuffer_[bufCount]) == true)
                        {
                            var pl = this.ProtocolParser_.GetMessage();
                            if (pl.HasValue())
                            {
                                this.RaiseRecvd(this, pl.GetValue());
                            }
                        }
                    }

                    this.DoRecv();
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

        protected override void DoRecv()
        {
            this.ServiceSocket_.BeginReceive(this.RecvBuffer_, 0,
                            this.RecvBuffer_.Length, 0,
                            new System.AsyncCallback(RecvCallback), this.ServiceSocket_);
        }

        public override void Send(byte[] data)
        {
            throw new System.NotImplementedException();
        }

        protected override void HandleError()
        {
            throw new System.NotImplementedException();
        }

    }
}