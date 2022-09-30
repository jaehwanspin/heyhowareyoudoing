using System;
using NetCol = AppSystemSimulator.Utility.Collections.Generic;

namespace AppSystemSimulator.Model.Response
{
    [Utility.SerializableModel(PayloadCode = new byte[4] { 0x00, 0x00, 0x00, 0x05 })]
    class TrafficAnalysisList
    {
        public static readonly byte[] PayloadCode =
           Utility.Model.GetPayloadCode(typeof(TrafficAnalysisList)).GetValue();

        [Utility.SerializableField]
        public NetCol.List<Common.AnalysisInfo, UInt16> List;

        public TrafficAnalysisList()
        {
            this.List = new NetCol.List<Common.AnalysisInfo, UInt16>();
        }
    }
}
