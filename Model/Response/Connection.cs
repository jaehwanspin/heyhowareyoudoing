using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSystemSimulator.Model.Response
{
    [Utility.SerializableModel(new byte[] { 0x00, 0x00, 0x00, 0x01 })]
    class Connection
    {
        public static readonly byte[] PayloadCode =
                Utility.Model.GetPayloadCode(typeof(Connection)).GetValue();

        [Utility.SerializableField]
        public System.Byte Result;

        public Connection()
        {
            
        }
    }
}
