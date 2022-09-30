using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSystemSimulator.Model.Response
{
    [Utility.SerializableModel(PayloadCode = new byte[4] { 0x00, 0x00, 0x00, 0x03 })]
    class StartTraffic
    {
        public static readonly byte[] PayloadCode =
            Utility.Model.GetPayloadCode(typeof(StartTraffic)).GetValue();
    }
}
