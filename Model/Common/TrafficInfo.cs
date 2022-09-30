using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSystemSimulator.Model.Common
{

    [Utility.SerializableModel]
    public class TrafficInfo : ViewModelBase
    {
        public int No { get => GetValue<int>(); set => SetValue(value); }
        [Utility.SerializableField]
        public UInt32 ScenarioId { get => GetValue<UInt32>(); set => SetValue(value); }

        [Utility.SerializableField]
        public Byte IsUdp { get => GetValue<Byte>(); set => SetValue(value); }                 // TCP / UDP 여부

        [Utility.SerializableField]
        public UInt16 PortNum { get => GetValue<UInt16>(); set => SetValue(value); }

        [Utility.SerializableField(16)]
        public string SourceIp { get => GetValue<string>(); set => SetValue(value); }            // 송신 장비 IP

        [Utility.SerializableField]
        public UInt32 SourceId { get => GetValue<UInt32>(); set => SetValue(value); }

        [Utility.SerializableField(16)]
        public string DestinationIp { get => GetValue<string>(); set => SetValue(value); }       // 수신 장비 IP

        [Utility.SerializableField]
        public UInt32 DestinationId { get => GetValue<UInt32>(); set => SetValue(value); }

        [Utility.SerializableField]
        public UInt32 DscpCode { get => GetValue<UInt32>(); set => SetValue(value); }

        [Utility.SerializableField]
        public UInt32 MsgCategoty { get => GetValue<UInt32>(); set => SetValue(value); }            // 메시지 대분류

        [Utility.SerializableField]
        public UInt32 SenderAppsimId { get => GetValue<UInt32>(); set => SetValue(value); }        // 송신체계 ID

        [Utility.SerializableField]
        public UInt32 ReceiverAppsimId { get => GetValue<UInt32>(); set => SetValue(value); }      // 수신체계 ID


        [Utility.SerializableField]
        public UInt32 TrafficType;

        [Utility.SerializableField]
        public UInt32 Codec;

        [Utility.SerializableField]
        public UInt32 Resolution;

        [Utility.SerializableField]
        public Single Bitrate;

        [Utility.SerializableField]
        public UInt32 FrameRate;

        [Utility.SerializableField]
        public UInt32 TotalTime;

        [Utility.SerializableField]
        public UInt32 FrameSize;

        [Utility.SerializableField]
        public Single FrameLength;

        [Utility.SerializableField]
        public UInt32 NumFrameTransmission;

        [Utility.SerializableField]
        public UInt32 PttTime;

        [Utility.SerializableField]
        public UInt32 WaitTimeInSeconds;

    }
}
