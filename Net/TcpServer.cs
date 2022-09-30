using System.Linq;

namespace AppSystemSimulator.Net
{
    /// <summary>
    /// TCP 1:N 통신 서버
    /// </summary>
    public class TcpServer : NetService, System.IDisposable
    {
        private System.Collections.Generic.Dictionary<string, TcpSession> Sessions_;

        public Utility.Option<TcpSession> ReleaseSession(string sessionId)
        {
            var result = new Utility.Option<TcpSession>();
            result += this.Sessions_[sessionId];
            if (result.HasValue())
            {
                this.Sessions_.Remove(sessionId);
            }

            return result;
        }

        public Utility.Option<TcpSession> GetSession(string sessionId)
        {
            var result = new Utility.Option<TcpSession>();
            result += this.Sessions_[sessionId];

            return result;
        }

        public TcpServer() :
            base(new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp))
        {
            this.Sessions_ = new System.Collections.Generic.Dictionary<string, TcpSession>();
        }

        ~TcpServer()
        {
        }

        /// <summary>
        /// 허용 이벤트 생성
        /// </summary>
        public delegate void AcceptHandler(TcpSession session);
        public event AcceptHandler Accepted;
        private void RaiseAccepted(TcpSession session)
            => Accepted?.Invoke(session);

        /// <summary>
        /// 소켓을 열어 서버를 시작하는 함수
        /// </summary>
        /// <typeparam name="T">헤더 구조체</typeparam>
        /// <param name="port">포트 번호</param>
        /// <param name="isHeaderInDataSize">DataSize에 HeaderSize가 포함되어 있는지 확인 (기본값 : HeaderSize 포함 안됨) </param>
        /// <param name="isRecivedDataInHeader">Recived 함수에서 받는 packet에 Header가 포함할지 설정 (기본값 : packet에 Headre 포함) </param>
        //public void Initialize<T>(int port, bool isHeaderInDataSize = false, bool isRecivedDataInHeader = true)
        public void Initialize(int port, ProtocolFields protoc)
        {
            base.Initialize(protoc, new System.Net.IPEndPoint(System.Net.IPAddress.Any, port));
            this.Socket.Bind(this.EndPoint);
            this.Socket.Listen(20);
            this.Socket.BeginAccept(new System.AsyncCallback(AcceptedCallback), this.Socket);
        }

        /// <summary>
        /// 비동기 허용 함수
        /// </summary>
        /// <param name="ar">StateObject</param>
        private void AcceptedCallback(System.IAsyncResult ar)
        {
            try
            {
                var acceptedSocket = this.Socket.EndAccept(ar);
                var sessionId = System.Guid.NewGuid().ToString();
                var newSession = new TcpSession(this, sessionId, acceptedSocket);

                this.Sessions_.Add(sessionId, newSession);

                this.RaiseAccepted(newSession);

                this.Socket.BeginAccept(new System.AsyncCallback(AcceptedCallback), this.Socket);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Accept error");
            }
        }

        /// <summary>
        /// 리소스 해제 함수
        /// </summary>
        public void Dispose()
        {
            this.Socket.Close();
        }
    }
}
