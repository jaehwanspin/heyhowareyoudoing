using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSystemSimulator.Model.Request
{
    class StopTrafffic
    {
        public static readonly byte[] PayloadCode =
           Utility.Model.GetPayloadCode(typeof(StopTrafffic)).GetValue();

        [Utility.SerializableField]
        public UInt32 AppsimId;
        [Utility.SerializableField]
        public Byte Result;

        public StopTrafffic()
        {

        }
    }
}
