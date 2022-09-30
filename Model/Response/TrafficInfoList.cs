using System;
using NetCol = AppSystemSimulator.Utility.Collections.Generic;

namespace AppSystemSimulator.Model.Response
{
    [Utility.SerializableModel(PayloadCode = new byte[4] { 0x00, 0x00, 0x00, 0x04 })]
    public class TrafficInfoList
    {
        public static readonly byte[] PayloadCode =
           Utility.Model.GetPayloadCode(typeof(TrafficInfoList)).GetValue();

        [Utility.SerializableField]
        public NetCol.List<Common.TrafficInfo, System.UInt16> List;

        public TrafficInfoList()
        {
            this.List = new NetCol.List<Common.TrafficInfo, System.UInt16>();
        }
    }
}
