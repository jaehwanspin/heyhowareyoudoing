using System;

namespace AppSystemSimulator.Model.Common
{
    public class AnalysisInfo
    {
        [Utility.SerializableField]
        public UInt32 ScenarioId;
        [Utility.SerializableField]
        public UInt32 TransmitCompleteRate;
        [Utility.SerializableField]
        public UInt32 TransmitLatencyMs;
        [Utility.SerializableField]
        public UInt32 TotalTransmitPacketCount;
        [Utility.SerializableField]
        public UInt32 TotalReceivePacketCount;
        [Utility.SerializableField]
        public UInt32 TotalTransmitPacketBytes;
        [Utility.SerializableField]
        public UInt32 TotalReceivePacketBytes;
        [Utility.SerializableField]
        public UInt32 TransmitThroughputInKbps;
        [Utility.SerializableField]
        public UInt32 ReceiveThroughputInKbps;
        [Utility.SerializableField]
        public UInt32 PacketDropRate;

        [Utility.SerializableField]
        public UInt32 FrameSizeInBytes;
        [Utility.SerializableField]
        public UInt32 BitrateInKbitps;
        [Utility.SerializableField]
        public UInt32 FrameLengthInMsec;
        [Utility.SerializableField]
        public UInt32 Resolution;
        [Utility.SerializableField]
        public UInt32 FrameRate;
        [Utility.SerializableField]
        public UInt32 InitialTransmitLatency;
        [Utility.SerializableField]
        public UInt32 OutOfOrderFramesCount;
        [Utility.SerializableField]
        public UInt32 FrameIntervalInMsec;
        [Utility.SerializableField]
        public UInt32 ReceiveBitrateInKbitps;


        public AnalysisInfo()
        {

        }
    }

}