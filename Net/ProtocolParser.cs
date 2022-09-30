namespace AppSystemSimulator.Net
{

    internal struct InspectionBuffer
    {
        public byte[] Buffer { get; set; }

        public int Count { get { return this.Buffer.Length; } }
        public int Offset { get; set; }
        public int Processed { get; set; }

        public Utility.Option<byte[]> PayloadCode { get; set; }

        public System.UInt64 SequenceNumber { get; set; }

        public bool IsBufferAvailable { get { return this.Offset < (this.Buffer.Length - 1); } }

        public System.UInt64 PayloadSize { get; set; }

        public byte[] Payload { get; set; }

        public void PutByte(byte val)
        {
            this.Buffer[this.Offset++] = val;
        }

        public void Clear()
        {
            this.Payload = null;
            this.SequenceNumber = 0;

            this.PayloadSize = 0;

            this.Offset = 0;
            this.Processed = 0;
            this.PayloadCode = new Utility.Option<byte[]>(new Utility.NullOpt());
        }
    }

    //internal struct 

    public class ProtocolParser : IBytePutable
    {
        private bool ReorderSequence_;
        private int Progress_;
        private ProtocolFields Fields_;
        private InspectionBuffer Buffer_;
        private System.Collections.Generic.Queue<MessageBase> Payloads_;

        public bool Complete { get; set; }

        private delegate void ParserCompletionHandle();

        private System.Collections.Generic.Dictionary<System.Type, byte[]> PayloadTypes_;

        public ProtocolParser(ProtocolFields fields, int bufferSize = 65536, bool reorderSequence = false)
        {
            this.Progress_ = 0;
            this.ReorderSequence_ = reorderSequence;
            this.Fields_ = fields;
            this.Payloads_ = new System.Collections.Generic.Queue<MessageBase>();
            this.Buffer_ = new InspectionBuffer() {
                Buffer = new byte[bufferSize],
                Offset = 0,
                PayloadCode = new Utility.Option<byte[]>()
            };
        }

        public bool PutByte(byte val)
        {
            if (this.Buffer_.IsBufferAvailable)
            {
                this.Buffer_.PutByte(val);
            }
            else
            {
                throw new System.Exception("not sufficient buffer size");
            }

            return this.InspectBytes();
        }

        

        //public void RegisterPayload(System.Type payloadType, byte[] payloadCode)
        //{
        //    if (this.PayloadTypes_.Count > 0)
        //    {
        //        foreach (var pl in this.PayloadTypes_)
        //        {
        //            if (pl.Value.Length != payloadCode.Length)
        //            {
        //                throw new System.Exception("invalid payload code length");
        //            }
        //        }
        //    }

        //    this.PayloadTypes_.Add(payloadType, payloadCode);
        //}

        public int GetPayloadCount()
        {
            return this.Payloads_.Count;
        }

        public byte[] GetCurrentPayloadCode()
        {
            return this.Payloads_.Peek().PayloadCode;
        }

        public Utility.Option<Message> GetMessage()
        {
            var result = new Utility.Option<Message>();

            if (this.Payloads_.Count > 0)
            {
                result += new Message(this.Payloads_.Dequeue());
            }

            return result;
        }

        public bool InspectBytes()
        {
            bool result = false;

            int currentSize = this.Buffer_.Offset - this.Buffer_.Processed;

            if (this.Fields_.Fields[Progress_].FieldType == ProtocolFieldType.Payload &&
                this.Buffer_.PayloadSize == 0)
            {
                this.Progress_++;
            }
            
            if ((currentSize * 8) == this.Fields_.Fields[Progress_].Length ||
                (this.Fields_.Fields[Progress_].FieldType == ProtocolFieldType.Payload &&
                (System.UInt64)currentSize == this.Buffer_.PayloadSize))
            {
                switch (this.Fields_.Fields[Progress_].FieldType)
                {
                    case ProtocolFieldType.Etx:
                        {
                            System.Console.WriteLine("ETX");
                            var currentByteArr = new byte[currentSize];
                            System.Array.Copy(this.Buffer_.Buffer, this.Buffer_.Offset - currentSize, currentByteArr, 0, currentSize);
                            foreach (var value in this.Fields_.Fields[Progress_].AllowedValues)
                            {
                                if (System.Linq.Enumerable.SequenceEqual(value, currentByteArr))
                                {
                                    this.Payloads_.Enqueue(
                                        new MessageBase(this.Buffer_.Payload,
                                        this.Buffer_.PayloadCode.GetValue(),
                                        this.Buffer_.SequenceNumber));

                                    this.Progress_ = 0;
                                    this.Buffer_.Clear();
                                    result = true;
                                    break;
                                }
                            }
                        }
                        break;
                    case ProtocolFieldType.Stx:
                        {
                            System.Console.WriteLine("STX");
                            var currentByteArr = new byte[currentSize];
                            System.Array.Copy(this.Buffer_.Buffer, this.Buffer_.Offset - currentSize, currentByteArr, 0, currentSize);
                            foreach (var value in this.Fields_.Fields[Progress_].AllowedValues)
                            {
                                if (System.Linq.Enumerable.SequenceEqual(value, currentByteArr))
                                {
                                    int prevProgress = this.Progress_;
                                    this.Progress_++;
                                    this.Buffer_.Processed += this.Fields_.Fields[prevProgress].Length / 8;
                                    break;
                                }
                            }
                        }
                        break;
                    case ProtocolFieldType.Complex:
                        {
                            System.Console.WriteLine("Complex");
                        }
                        break;
                    case ProtocolFieldType.SequenceNumber:
                        {
                            var currentByteArr = new byte[currentSize];
                            System.Array.Copy(this.Buffer_.Buffer, this.Buffer_.Offset - currentSize, currentByteArr, 0, currentSize);

                            switch (currentByteArr.Length)
                            {
                                case 1:
                                    this.Buffer_.SequenceNumber = (System.UInt64)currentByteArr[1];
                                    break;
                                case 2:
                                    this.Buffer_.SequenceNumber = (System.UInt64)System.BitConverter.ToUInt16(currentByteArr, 0);
                                    break;
                                case 4:
                                    this.Buffer_.SequenceNumber = (System.UInt64)System.BitConverter.ToUInt32(currentByteArr, 0);
                                    break;
                                case 8:
                                    this.Buffer_.SequenceNumber = (System.UInt64)System.BitConverter.ToUInt64(currentByteArr, 0);
                                    break;
                            }

                            int prevProgress = this.Progress_;
                            this.Progress_++;
                            this.Buffer_.Processed += this.Fields_.Fields[prevProgress].Length / 8;

                            this.Buffer_.PayloadCode += currentByteArr;

                            System.Console.WriteLine("SequenceNumber");
                        }
                        break;
                    case ProtocolFieldType.PayloadCode:
                        {
                            var currentByteArr = new byte[currentSize];
                            System.Array.Copy(this.Buffer_.Buffer, this.Buffer_.Offset - currentSize, currentByteArr, 0, currentSize);

                            this.Buffer_.PayloadCode += currentByteArr;

                            int prevProgress = this.Progress_;
                            this.Progress_++;
                            this.Buffer_.Processed += this.Fields_.Fields[prevProgress].Length / 8;

                            System.Console.WriteLine("PayloadCode");
                        }
                        break;
                    case ProtocolFieldType.PayloadSize:
                        {
                            System.UInt64 maxSize = 0;
                            System.Console.WriteLine("PayloadSize");
                            var currentByteArr = new byte[currentSize];
                            System.Array.Copy(this.Buffer_.Buffer, this.Buffer_.Offset - currentSize, currentByteArr, 0, currentSize);
                            switch (currentByteArr.Length)
                            {
                                case 1:
                                    maxSize = (System.UInt64)currentByteArr[1];
                                    break;
                                case 2:
                                    maxSize = (System.UInt64)System.BitConverter.ToUInt16(currentByteArr, 0);
                                    break;
                                case 4:
                                    maxSize = (System.UInt64)System.BitConverter.ToUInt32(currentByteArr, 0);
                                    break;
                                case 8:
                                    maxSize = System.BitConverter.ToUInt64(currentByteArr, 0);
                                    break;
                            }

                            this.Buffer_.PayloadSize = maxSize;

                            int prevProgress = this.Progress_;
                            this.Progress_++;
                            this.Buffer_.Processed += this.Fields_.Fields[prevProgress].Length / 8;

                            System.Console.WriteLine($" : {maxSize}");
                        }
                        break;
                    case ProtocolFieldType.Payload:
                        {
                            var startOffset = this.Buffer_.Offset - currentSize;
                            var currentByteArr = new byte[currentSize];
                            for (int idx = 0; idx < currentByteArr.Length; idx++)
                            {
                                currentByteArr[idx] = this.Buffer_.Buffer[startOffset + idx];
                            }

                            this.Buffer_.Payload = currentByteArr;

                            int prevProgress = this.Progress_;
                            this.Progress_++;
                            this.Buffer_.Processed += currentByteArr.Length;
                        }
                        break;
                    case ProtocolFieldType.Checksum:
                        {
                            System.Console.WriteLine("Checksum");
                        }
                        break;
                    case ProtocolFieldType.Extra:
                        {
                            System.Console.WriteLine("Extra");
                        }
                        break;
                    default:
                        {

                        }
                        break;
                }
            }

            return result;
        }



       

    }
}
