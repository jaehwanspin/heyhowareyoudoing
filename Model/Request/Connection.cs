using System;

namespace AppSystemSimulator.Model.Request
{
    [Utility.SerializableModel(new byte[] { 0x00, 0x00, 0x00, 0x01 })]
    class Connection
    {
        public static readonly byte[] PayloadCode =
           Utility.Model.GetPayloadCode(typeof(Connection)).GetValue();

        [Utility.SerializableField]
        public UInt32 AppsimId;

        public Connection()
        {

        }
    }
}
