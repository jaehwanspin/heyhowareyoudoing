namespace AppSystemSimulator.Net
{
    class ProtocolSerializer
    {
        private System.Tuple<Utility.Option<byte[]>, Utility.Option<byte[]>> ProtocolBuffer_;
        private ProtocolFields ProtocFields_;

        int CrcSize_;
        int CrcOffset_;

        int PayloadCodeOffset_;
        int PayloadCodeSize_;

        int PayloadSizeOffset_;
        int PayloadSizeSize_;

        int SequenceNumber_;
        int SequenceNumberOffset_;
        int SequenceNumberSize_;


        public ProtocolSerializer(ProtocolFields protocFields)
        {
            this.CrcSize_ = 0;
            this.CrcOffset_ = 0;
            this.SequenceNumber_ = -1;
            this.ProtocFields_ = protocFields;
            this.PayloadCodeOffset_ = 0;
            this.PayloadCodeSize_ = 0;
            this.ProtocolBuffer_ = this.MakeInitialProtocolBuffer();

        }

        public Utility.Option<byte[]> Serialize<PayloadType>(PayloadType model)
        {
            var result = new Utility.Option<byte[]>();

            do
            {
                var byteArr = new System.Collections.Generic.List<byte>();

                var beforePayload = this.GetBytesBeforePayload();
                if (beforePayload.HasValue())
                {
                    foreach (var b in beforePayload.GetValue())
                    {
                        byteArr.Add(b);
                    }
                }

                var attrs = System.Attribute.GetCustomAttributes(typeof(PayloadType), typeof(Utility.SerializableModelAttribute));

                if (attrs != null && attrs.Length > 0)
                {
                    var modelAttr = (Utility.SerializableModelAttribute)attrs[0];
                    var pc = modelAttr.PayloadCode;

                    if (this.PayloadCodeSize_ != pc.Length)
                    {
                        break;
                    }

                    for (int plcIdx = 0; plcIdx < this.PayloadCodeSize_; plcIdx++)
                    {
                        byteArr[plcIdx + this.PayloadCodeOffset_] = pc[plcIdx];
                    }
                }
                else
                {
                    break;
                }

                if (this.SequenceNumberOffset_ > 0)
                {
                    var seqNum = Utility.Types.Convert(this.SequenceNumber_,
                        Utility.Types.GetPrimitiveTypeFromSize(this.SequenceNumberSize_, true));
                    var seqBytes = System.BitConverter.GetBytes(seqNum);

                    for (int seqIdx = 0; seqIdx < this.SequenceNumberSize_; seqIdx++)
                    {
                        byteArr[seqIdx + this.SequenceNumberOffset_] = seqBytes[seqIdx];
                    }
                }


                int payloadSize = 0;
                byteArr = this.SerializeModel(ref payloadSize, byteArr, model);

                if (payloadSize > 0)
                {
                    byte[] payloadSizeBytes = null;

                    switch (this.PayloadSizeSize_)
                    {
                        case 1:
                            {
                                payloadSizeBytes = System.BitConverter.GetBytes((System.Byte)payloadSize);
                            }
                            break;
                        case 2:
                            {
                                payloadSizeBytes = System.BitConverter.GetBytes((System.UInt16)payloadSize);
                            }
                            break;
                        case 4:
                            {
                                payloadSizeBytes = System.BitConverter.GetBytes((System.UInt32)payloadSize);
                            }
                            break;
                        case 8:
                            {
                                payloadSizeBytes = System.BitConverter.GetBytes((System.UInt64)payloadSize);
                            }
                            break;
                    }

                    for (int payloadSizeCnt = 0; payloadSizeCnt < payloadSizeBytes.Length; ++payloadSizeCnt)
                    {
                        byteArr[this.PayloadSizeOffset_ + payloadSizeCnt] = payloadSizeBytes[payloadSizeCnt];
                    }
                }
                else
                {
                    for (int payloadSizeCnt = 0; payloadSizeCnt < this.PayloadSizeSize_; ++payloadSizeCnt)
                    {
                        byteArr[this.PayloadSizeOffset_ + payloadSizeCnt] = 0;
                    }
                }

                //var seqNumBytes = System.BitConverter.GetBytes()


                if (byteArr == null)
                {
                    break;
                }

                var afterPayload = this.GetBytesAfterPayload();
                if (afterPayload.HasValue())
                {
                    foreach (var b in afterPayload.GetValue())
                    {
                        byteArr.Add(b);
                    }
                }

                result += byteArr.ToArray();
                this.SequenceNumber_++;

            } while (false);

            return result;
        }

        private Utility.Option<byte[]> GetBytesBeforePayload()
        {
            var result = new Utility.Option<byte[]>();

            if (this.ProtocolBuffer_.Item1.HasValue())
            {
                var bytes = new byte[this.ProtocolBuffer_.Item1.GetValue().Length];
                System.Array.Copy(this.ProtocolBuffer_.Item1.GetValue(), bytes, bytes.Length);
                result = new Utility.Option<byte[]>(bytes);
            }

            return result;
        }

        private Utility.Option<byte[]> GetBytesAfterPayload()
        {
            var result = new Utility.Option<byte[]>();

            if (this.ProtocolBuffer_.Item2.HasValue())
            {
                var bytes = new byte[this.ProtocolBuffer_.Item2.GetValue().Length];
                System.Array.Copy(this.ProtocolBuffer_.Item2.GetValue(), bytes, bytes.Length);
                result = new Utility.Option<byte[]>(bytes);
            }

            return result;
        }


        private System.Tuple<Utility.Option<byte[]>, Utility.Option<byte[]>> MakeInitialProtocolBuffer()
        {
            var opt1 = new Utility.Option<byte[]>();
            var opt2 = new Utility.Option<byte[]>();

            int payloadPassed = 0;
            System.Collections.Generic.List<byte>[] bytes =
                { new System.Collections.Generic.List<byte>(), new System.Collections.Generic.List<byte>() };

            foreach (var field in this.ProtocFields_.Fields)
            {
                switch (field.FieldType)
                {
                    case ProtocolFieldType.Stx:
                        {
                            foreach (var b in field.AllowedValues[0])
                            {
                                bytes[payloadPassed].Add(b);
                            }
                        }
                        break;
                    case ProtocolFieldType.Etx:
                        {
                            foreach (var b in field.AllowedValues[0])
                            {
                                bytes[payloadPassed].Add(b);
                            }
                        }
                        break;
                    case ProtocolFieldType.Complex:
                        {
                            for (int byteCnt = 0; byteCnt < field.Length / 8; byteCnt++)
                            {
                                bytes[payloadPassed].Add(0);
                            }
                        }
                        break;
                    case ProtocolFieldType.Extra:
                        {
                            for (int byteCnt = 0; byteCnt < field.Length / 8; byteCnt++)
                            {
                                bytes[payloadPassed].Add(0);
                            }
                        }
                        break;
                    case ProtocolFieldType.PayloadCode:
                        {
                            this.PayloadCodeSize_ = field.Length / 8;
                            this.PayloadCodeOffset_ = bytes[payloadPassed].Count;
                            for (int byteCnt = 0; byteCnt < this.PayloadCodeSize_; byteCnt++)
                            {
                                bytes[payloadPassed].Add(0);
                            }
                        }
                        break;
                    case ProtocolFieldType.SequenceNumber:
                        {
                            this.SequenceNumberSize_ = field.Length / 8;
                            this.SequenceNumberOffset_ = bytes[payloadPassed].Count;
                            for (int byteCnt = 0; byteCnt < field.Length / 8; byteCnt++)
                            {
                                bytes[payloadPassed].Add(0);
                            }
                        }
                        break;
                    case ProtocolFieldType.PayloadSize:
                        {
                            this.PayloadSizeSize_ = field.Length / 8;
                            this.PayloadSizeOffset_ = bytes[payloadPassed].Count;
                            for (int byteCnt = 0; byteCnt < field.Length / 8; byteCnt++)
                            {
                                bytes[payloadPassed].Add(0);
                            }
                        }
                        break;
                    case ProtocolFieldType.Payload:
                        {
                            payloadPassed = 1;
                        }
                        break;
                    case ProtocolFieldType.Checksum:
                        {
                            for (int byteCnt = 0; byteCnt < field.Length / 8; byteCnt++)
                            {
                                bytes[payloadPassed].Add(0);
                            }
                        }
                        break;
                }
            }

            if (bytes[0].Count > 0)
            {
                opt1 += bytes[0].ToArray();
            }
            if (bytes[1].Count > 0)
            {
                opt2 += bytes[1].ToArray();
            }

            var result = new System.Tuple<Utility.Option<byte[]>, Utility.Option<byte[]>>(opt1, opt2);

            return result;
        }

        private System.Collections.Generic.List<byte> SerializeModel<ModelType>(ref int payloadSize, System.Collections.Generic.List<byte> byteArr, ModelType m)
        {
            var fields = System.Array.FindAll(m.GetType().GetFields(),
                (elem) => (elem.GetCustomAttributes(typeof(Utility.SerializableFieldAttribute), true).Length > 0));

            foreach (var field in fields)
            {
                var fattr =
                    (Utility.SerializableFieldAttribute)field.GetCustomAttributes(typeof(Utility.SerializableFieldAttribute), true)[0];
                byteArr = SerializeField(ref payloadSize, byteArr, m, fattr, field);
            }

            return byteArr;
        }

        private System.Collections.Generic.List<byte>
        SerializeField<ModelType>(
            ref int payloadSize,
            System.Collections.Generic.List<byte> byteArr,
            ModelType m,
            Utility.SerializableFieldAttribute fattr,
            System.Reflection.FieldInfo fieldInfo)
        {
            var fieldType = fieldInfo.FieldType;
            var fieldValue = fieldInfo.GetValue(m);

            if (fieldType.IsPrimitive && fieldType.IsValueType || fieldType.IsEnum)
            {
                byteArr = this.SerializePrimitive(ref payloadSize, byteArr, fieldValue);
            }
            else if (fattr != null && fattr.ArraySize > 0)
            {
                if (fieldValue is System.Collections.ICollection)
                {
                    var tmpVal = (System.Collections.ICollection)fieldValue;
                    var elemType = fieldValue.GetType().GetGenericArguments()[0];

                    var restOfCount = fattr.ArraySize - tmpVal.Count;

                    for (var cnt = 0; cnt < tmpVal.Count; cnt++)
                    {
                        var elem = ((System.Collections.IList)fieldValue)[cnt];
                        //var elemType = elem.GetType();

                        if (elemType.IsPrimitive && elemType.IsValueType || fieldType.IsEnum)
                        {
                            byteArr = this.SerializePrimitive(ref payloadSize, byteArr, elem);
                        }
                        else if (System.Attribute.GetCustomAttributes(elem.GetType(), typeof(Utility.SerializableModelAttribute)) != null &&
                            System.Attribute.GetCustomAttributes(elem.GetType(), typeof(Utility.SerializableModelAttribute)).Length > 0)
                        {
                            byteArr = this.SerializeModel(ref payloadSize, byteArr, elem);
                        }

                        //SerializeField(ref payloadSize, byteArr, null, elem, fieldInfo);
                    }

                    for (var cnt = 0; cnt < restOfCount; cnt++)
                    {
                        if (elemType.IsPrimitive && elemType.IsValueType || fieldType.IsEnum)
                        {
                            var elemTypeSize = System.Runtime.InteropServices.Marshal.SizeOf(elemType);
                            for (var elemTypeCnt = 0; elemTypeCnt < elemTypeSize; elemTypeCnt++)
                            {
                                byteArr.Add(0);
                                payloadSize++;
                            }

                        }
                    }
                }
                else if (fieldType.IsArray)
                {
                    var elemType = fieldType.GetElementType();
                }
                else if (fieldType == typeof(string))
                {
                    var fixedString = new char[fattr.ArraySize];
                    var tmp = (fieldValue as string).ToCharArray();

                    System.Array.Copy(tmp, fixedString, tmp.Length);

                    foreach (var ch in fixedString)
                    {
                        byteArr.Add((byte)ch);
                        payloadSize++;
                    }
                }
            }
            else if (
                fieldValue is System.Collections.ICollection &&
                fieldValue.GetType().IsGenericType && fieldValue.GetType().GetGenericArguments().Length > 1)
            {
                var tmpVal = (System.Collections.ICollection)fieldValue;
                var elemType = fieldValue.GetType().GetGenericArguments()[0];
                var sizeFieldType = fieldValue.GetType().GetGenericArguments()[1];
                var collectionCount = Utility.Types.Convert(((System.Collections.ICollection)fieldValue).Count,
                        Utility.Types.GetPrimitiveTypeFromSize(System.Runtime.InteropServices.Marshal.SizeOf(sizeFieldType), true));
                var collectionCountBytes = System.BitConverter.GetBytes(collectionCount);

                for (int seqIdx = 0; seqIdx < ((byte[])collectionCountBytes).Length; seqIdx++)
                {
                    byteArr.Add(collectionCountBytes[seqIdx]);
                    payloadSize++;
                }

                var restOfCount = tmpVal.Count - fattr.ArraySize;

                for (var cnt = 0; cnt < tmpVal.Count; cnt++)
                {
                    var elem = ((System.Collections.IList)fieldValue)[cnt];
                    //var elemType = elem.GetType();

                    if (elemType.IsPrimitive && elemType.IsValueType || fieldType.IsEnum)
                    {
                        byteArr = this.SerializePrimitive(ref payloadSize, byteArr, elem);
                    }
                    else if (System.Attribute.GetCustomAttributes(elem.GetType(), typeof(Utility.SerializableModelAttribute)) != null &&
                        System.Attribute.GetCustomAttributes(elem.GetType(), typeof(Utility.SerializableModelAttribute)).Length > 0)
                    {
                        byteArr = this.SerializeModel(ref payloadSize, byteArr, elem);
                    }

                    //SerializeField(ref payloadSize, byteArr, null, elem, fieldInfo);
                }
            }

            return byteArr;
        }

        private System.Collections.Generic.List<byte>
        SerializePrimitive(
            ref int payloadSize,
            System.Collections.Generic.List<byte> byteArr,
            object value)
        {
            var valueType = value.GetType();
            if (valueType.IsEnum)
            {
                valueType = valueType.GetEnumUnderlyingType();
            }

            switch (System.Type.GetTypeCode(valueType))
            {
                case System.TypeCode.Boolean:
                case System.TypeCode.Byte:
                case System.TypeCode.SByte:
                    {
                        var v = (System.Byte)value;
                        byteArr.Add(v);
                        payloadSize++;
                    }
                    break;
                case System.TypeCode.Int16:
                    {
                        var v = System.BitConverter.GetBytes((System.Int16)value);
                        foreach (var b in v)
                        {
                            byteArr.Add(b);
                            payloadSize++;
                        }
                    }
                    break;
                case System.TypeCode.UInt16:
                    {
                        var v = System.BitConverter.GetBytes((System.UInt16)value);
                        foreach (var b in v)
                        {
                            byteArr.Add(b);
                            payloadSize++;
                        }
                    }
                    break;
                case System.TypeCode.Int32:
                    {
                        var v = System.BitConverter.GetBytes((System.Int32)value);
                        foreach (var b in v)
                        {
                            byteArr.Add(b);
                            payloadSize++;
                        }
                    }
                    break;
                case System.TypeCode.UInt32:
                    {
                        var v = System.BitConverter.GetBytes((System.UInt32)value);
                        foreach (var b in v)
                        {
                            byteArr.Add(b);
                            payloadSize++;
                        }
                    }
                    break;
                case System.TypeCode.Int64:
                    {
                        var v = System.BitConverter.GetBytes((System.Int64)value);
                        foreach (var b in v)
                        {
                            byteArr.Add(b);
                            payloadSize++;
                        }
                    }
                    break;
                case System.TypeCode.UInt64:
                    {
                        var v = System.BitConverter.GetBytes((System.UInt64)value);
                        foreach (var b in v)
                        {
                            byteArr.Add(b);
                            payloadSize++;
                        }
                    }
                    break;
                case System.TypeCode.Single:
                    {
                        var v = System.BitConverter.GetBytes((System.Single)value);
                        foreach (var b in v)
                        {
                            byteArr.Add(b);
                            payloadSize++;
                        }
                    }
                    break;
                case System.TypeCode.Double:
                    {
                        var v = System.BitConverter.GetBytes((System.Double)value);
                        foreach (var b in v)
                        {
                            byteArr.Add(b);
                            payloadSize++;
                        }
                    }
                    break;
            }

            return byteArr;
        }
    }
}