using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSystemSimulator.Net
{
    public enum ProtocolFieldType
    {
        Stx,
        Etx,
        Complex,
        Extra,
        PayloadCode,
        SequenceNumber,
        PayloadSize,
        Payload,
        Checksum
    }
}
