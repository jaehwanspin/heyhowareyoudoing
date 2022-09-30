using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSystemSimulator.Net
{
    class ProtocolConfigException : System.Exception
    {
        public ProtocolConfigException() :
            base("all protocol size must be (8 X N)")
        {

        }
    }

    class ProtocolFactory
    {
        private ProtocolProvider Prov_;
        int Offset_;

        public ProtocolFactory()
        {
            this.Prov_ = new ProtocolProvider();
        }

        public ProtocolFactory Stx(byte[] fixedValue)
        {
            this.Prov_.Set(ProtocolFieldType.Stx,
                this.Offset_,
                fixedValue.Length * 8,
                new System.Collections.Generic.List<byte[]>() { fixedValue });
            this.Offset_ += fixedValue.Length * 8;
            return this;
        }
        public ProtocolFields Etx(byte[] fixedValue)
        {
            this.Prov_.Set(ProtocolFieldType.Etx,
                this.Offset_,
                fixedValue.Length * 8,
                new System.Collections.Generic.List<byte[]>() { fixedValue });

            this.Offset_ += fixedValue.Length * 8;

            if (this.Offset_ % 8 != 0)
            {
                throw new ProtocolConfigException();
            }

            return this.Prov_.Get();
        }

        public ProtocolFields Finish()
        {
            if (this.Offset_ % 8 != 0)
            {
                throw new ProtocolConfigException();
            }
            return this.Prov_.Get();
        }

        public ProtocolFactory Complex(ProtocolFields fields)
        {
            //fields.Fields
            
            return this;
        }

        public ProtocolFactory SequenceNumber(int sizeInBits)
        {
            this.Prov_.Set(ProtocolFieldType.SequenceNumber,
                this.Offset_,
                sizeInBits);

            this.Offset_ += sizeInBits;

            return this;
        }

        public ProtocolFactory SequenceNumber(System.Type sequenceSizeType)
        {
            int size = 0;

            switch (System.Type.GetTypeCode(sequenceSizeType))
            {
                case System.TypeCode.Byte:
                    size = sizeof(System.Byte) * 8;
                    break;
                case System.TypeCode.UInt16:
                    size = sizeof(System.UInt16) * 8;
                    break;
                case System.TypeCode.UInt32:
                    size = sizeof(System.UInt32) * 8;
                    break;
                case System.TypeCode.UInt64:
                    size = sizeof(System.UInt64) * 8;
                    break;

                default:
                    {
                        throw new Exception("payload size type only allows unsigned integer types");
                    }
            }

            this.Prov_.Set(ProtocolFieldType.PayloadSize,
                this.Offset_,
                size);

            this.Offset_ += size;

            return this;
        }

        public ProtocolFactory PayloadCode(System.Type sequenceSizeType)
        {
            int size = 0;

            switch (System.Type.GetTypeCode(sequenceSizeType))
            {
                case System.TypeCode.Byte:
                    size = sizeof(System.Byte) * 8;
                    break;
                case System.TypeCode.UInt16:
                    size = sizeof(System.UInt16) * 8;
                    break;
                case System.TypeCode.UInt32:
                    size = sizeof(System.UInt32) * 8;
                    break;
                case System.TypeCode.UInt64:
                    size = sizeof(System.UInt64) * 8;
                    break;

                default:
                    {
                        throw new Exception("payload size type only allows unsigned integer types");
                    }
            }

            this.Prov_.Set(ProtocolFieldType.PayloadCode,
                this.Offset_,
                size);

            this.Offset_ += size;

            return this;
        }

        public ProtocolFactory PayloadCode(int sizeInBits)
        {
            this.Prov_.Set(ProtocolFieldType.PayloadCode,
               this.Offset_,
               sizeInBits);

            this.Offset_ += sizeInBits;

            return this;
        }

        public ProtocolFactory Extra(System.Collections.Generic.List<byte[]> allowedValues)
        {
            var maxSize = 0;

            if (allowedValues != null && allowedValues.Count > 0)
            {
                maxSize = allowedValues[0].Length;

                foreach (var value in allowedValues)
                {
                    if (value.Length > maxSize)
                    {
                        maxSize = value.Length;
                    }
                }
            }

            maxSize = maxSize * 8;

            this.Prov_.Set(ProtocolFieldType.PayloadSize,
                this.Offset_,
                maxSize,
                allowedValues);

            this.Offset_ += maxSize;

            return this;
        }

        public ProtocolFactory Extra(int sizeInBits)
        {
            this.Prov_.Set(ProtocolFieldType.SequenceNumber,
                this.Offset_,
                sizeInBits);

            this.Offset_ += sizeInBits;

            return this;
        }

        public ProtocolFactory PayloadSizeType(int sizeInBits)
        {
            this.Prov_.Set(ProtocolFieldType.PayloadSize,
                this.Offset_,
                sizeInBits);

            this.Offset_ += sizeInBits;

            return this;
        }

        public ProtocolFactory Payload()
        {
            this.Prov_.Set(ProtocolFieldType.Payload, this.Offset_, 0);

            return this;
        }

        public ProtocolFactory PayloadSizeType(System.Type payloadSizeType)
        {
            int size = 0;

            switch (System.Type.GetTypeCode(payloadSizeType))
            {
                case System.TypeCode.Byte:
                    size = sizeof(System.Byte) * 8;
                    break;
                case System.TypeCode.UInt16:
                    size = sizeof(System.UInt16) * 8;
                    break;
                case System.TypeCode.UInt32:
                    size = sizeof(System.UInt32) * 8;
                    break;
                case System.TypeCode.UInt64:
                    size = sizeof(System.UInt64) * 8;
                    break;

                default:
                    {
                        throw new Exception("payload size type only allows unsigned integer types");
                    }
            }

            this.Prov_.Set(ProtocolFieldType.PayloadSize,
                this.Offset_,
                size);

            this.Offset_ += size;

            return this;
        }

        //public ProtocolFactory 


       
    }
}
