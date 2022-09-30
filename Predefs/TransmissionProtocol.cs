using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSystemSimulator.Predefs
{
    public enum TransmissionProtocol : System.Byte
    {
        Tcp = 0,
        Udp,
        Rudp,
        Quic
    }
}
