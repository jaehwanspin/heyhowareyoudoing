using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSystemSimulator.Net
{
    class ProtocolProvider
    {
        private ProtocolFields Protoc_;

        public ProtocolProvider()
        {
            this.Protoc_ = null;
        }

        public void Set(ProtocolFieldType type, int offset, int length)
        {
            if (this.Protoc_ == null)
            {
                this.Protoc_ = new ProtocolFields();
            }

            this.Protoc_.Add(new ProtocolFieldInfo(type, offset, length));
        }

        public void Set(ProtocolFieldType type, int offset, int length, System.Collections.Generic.List<byte[]> av)
        {
            if (this.Protoc_ == null)
            {
                this.Protoc_ = new ProtocolFields();
            }

            this.Protoc_.Add(new ProtocolFieldInfo(type, offset, length, av));
        }

        public ProtocolFields Get()
        {
            var protoc = this.Protoc_;
            this.Protoc_ = null;
            return protoc;
        }
    }
}
