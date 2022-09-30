using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSystemSimulator.TrafficSimulation
{
    class NetworkServiceInitializer
    {
        Net.NetService NetServiceRef_;

        public NetworkServiceInitializer(Net.NetService netService)
        { this.NetServiceRef_ = netService; }

        public void Initialize(NetOption opt)
        {
            switch (opt.TransmissionProtocol)
            {
                case Predefs.TransmissionProtocol.Tcp:
                    {
                        switch (opt.ThisAppRole)
                        {
                            case Predefs.TrafficRole.Sender:
                                {
                                    (this.NetServiceRef_ as Net.TcpServer).Initialize(opt.PortNumber,
                                        CreateNetProtocol(opt));
                                    
                                }
                                break;
                            case Predefs.TrafficRole.Receiver:
                                {
                                    (this.NetServiceRef_ as Net.TcpClient).Initialize(opt.TargetAppSimulatorIpAddress,
                                        opt.PortNumber, CreateNetProtocol(opt));
                                }
                                break;
                        }
                    }
                    break;
                case Predefs.TransmissionProtocol.Udp:
                    {
                        (this.NetServiceRef_ as Net.UdpTransceiver).Initialize(opt.PortNumber,
                            opt.TargetAppSimulatorIpAddress, opt.PortNumber, CreateNetProtocol(opt));
                    }
                    break;
            }
        }

        private static Net.ProtocolFields CreateNetProtocol(NetOption opt)
        {
            Net.ProtocolFields result = null;
            switch (opt.TransmissionProtocol)
            {
                case Predefs.TransmissionProtocol.Tcp:
                    {
                        result = new Net.ProtocolFactory().
                            Stx(new byte[] { (byte)'h', (byte)'e', (byte)'y', (byte)'!' }).
                            SequenceNumber(16).
                            PayloadCode(32).
                            PayloadSizeType(32).
                            Payload().
                            Etx(new byte[] { (byte)'b', (byte)'y', (byte)'e', (byte)'!' });
                    }
                    break;
                case Predefs.TransmissionProtocol.Udp:
                    {
                        result = new Net.ProtocolFactory().
                            Stx(new byte[] { (byte)'h', (byte)'e', (byte)'y', (byte)'!' }).
                            PayloadCode(32).
                            PayloadSizeType(32).
                            Payload().
                            Etx(new byte[] { (byte)'b', (byte)'y', (byte)'e', (byte)'!' });
                    }
                    break;
            }

            return result;
        }
    }
}
