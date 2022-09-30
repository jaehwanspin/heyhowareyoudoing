using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSystemSimulator.Net
{
    class NetworkObjectBase
    {
        protected byte[] PayloadBuffer_;
        
        public NetworkObjectBase(int bufferSize)
        {
            this.PayloadBuffer_ = new byte[bufferSize];
        }
    }
}
