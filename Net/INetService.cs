using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSystemSimulator.Net
{
    public abstract class INetService
    {
        protected abstract void DoRecv();
        public abstract void Send(byte[] data);
        protected abstract void HandleError();

    }
}
