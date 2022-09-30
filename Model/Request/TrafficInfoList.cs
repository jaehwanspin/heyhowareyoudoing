using System;

namespace AppSystemSimulator.Model.Request
{

    [Utility.SerializableModel(PayloadCode = new byte[] { 0x00, 0x00, 0x00, 0x04 })]
    class TrafficInfoList
    {
        public static readonly byte[] PayloadCode =
            Utility.Model.GetPayloadCode(typeof(TrafficInfoList)).GetValue();

        [Utility.SerializableField]
        public UInt32 AppSimId;
        [Utility.SerializableField]
        public Byte Result;

        TrafficInfoList()
        {
            this.AppSimId = 0;
            this.Result = 0;
        }
    }
}
