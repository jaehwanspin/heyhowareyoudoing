using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSystemSimulator.TrafficSimulation
{
    struct NetOption
    {
        public string TargetAppSimulatorIpAddress { get; }

        public int PortNumber { get; }

        public Predefs.TransmissionProtocol TransmissionProtocol { get; }

        public Predefs.TrafficRole ThisAppRole { get; }

        public NetOption(string targetAppIp, int portNumber, Predefs.TransmissionProtocol protocol, Predefs.TrafficRole role)
        {
            this.TargetAppSimulatorIpAddress = targetAppIp;
            this.PortNumber = portNumber;
            this.TransmissionProtocol = protocol;
            this.ThisAppRole = role;
        }
    }
}
