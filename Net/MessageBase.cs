namespace AppSystemSimulator.Net
{
    public class MessageBase
    {
        public System.UInt64 SequenceNumber { get; }
        public byte[] PayloadCode { get; }
        public byte[] PayloadBuffer { get; }


        public MessageBase(byte[] payloadBuffer = null, byte[] payloadCode = null, System.UInt64 sequenceNumber = 0)
        {
            this.PayloadBuffer = payloadBuffer;
            this.PayloadCode = payloadCode;
            this.SequenceNumber = sequenceNumber;
        }


        public Utility.Option<PayloadType> Decerialize<PayloadType>() where PayloadType : new()
        {
            var opt = new Utility.Option<PayloadType>();

            var byteArr = this.PayloadBuffer;
            int offset = 0;
            var payload = new PayloadType();
            DeserializeModel(byteArr, ref offset, payload);
            opt += payload;

            return opt;
        }


        private static bool DeserializeModel(byte[] byteArr, ref int offset, object model)
        {
            var result = false;
            var modelType = model.GetType();

            var fields = System.Array.FindAll(modelType.GetFields(),
                (elem) => (elem.GetCustomAttributes(typeof(Utility.SerializableFieldAttribute), true).Length > 0));

            foreach (var field in fields)
            {
                var fattr =
                    (Utility.SerializableFieldAttribute)field.GetCustomAttributes(typeof(Utility.SerializableFieldAttribute), true)[0];
                var ftype = field.FieldType;
                var constructors = ftype.GetConstructors();

                object newObj = null;

                if (ftype == typeof(string))
                {
                    newObj = "";
                }
                else
                {
                    newObj = System.Activator.CreateInstance(ftype);
                }

                var success = DeserializeField(byteArr, ref offset, fattr, field, ref newObj);

                field.SetValue(model, newObj);

                if (byteArr.Length == offset)
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool DeserializeField(byte[] byteArr,
            ref int offset,
            Utility.SerializableFieldAttribute fattr,
            System.Reflection.FieldInfo fieldInfo,
            ref object fieldValue)
        {
            var result = false;
            var fieldType = fieldValue.GetType();

            if (fieldType.IsPrimitive && fieldType.IsValueType || fieldType.IsEnum)
            {
                DeserializePrimitive(byteArr, ref offset, ref fieldValue);
            }
            else if (fattr != null && fattr.ArraySize > 0)
            {
                if (fieldValue is System.Collections.ICollection)
                {
                    //var tmpVal = (System.Collections.ICollection)fieldValue;
                    var elemType = fieldValue.GetType().GetGenericArguments()[0];
                    int fixedSize = fattr.ArraySize;

                    for (var cnt = 0; cnt < fixedSize; cnt++)
                    {
                        var newObj = System.Activator.CreateInstance(elemType);

                        if (elemType.IsPrimitive && elemType.IsValueType || fieldType.IsEnum)
                        {
                            DeserializePrimitive(byteArr, ref offset, ref newObj);
                        }
                        else if (System.Attribute.GetCustomAttributes(elemType, typeof(Utility.SerializableModelAttribute)) != null &&
                            System.Attribute.GetCustomAttributes(elemType, typeof(Utility.SerializableModelAttribute)).Length > 0)
                        {
                            DeserializeModel(byteArr, ref offset, newObj);
                        }
                        ((System.Collections.IList)fieldValue).Add(newObj);
                    }
                }
                else if (fieldType.IsArray)
                {
                    //var elemType = fieldType.GetElementType();
                }
                else if (fieldType == typeof(string))
                {
                    var tmp = new char[fattr.ArraySize];

                    for (int idx = 0; idx < tmp.Length; idx++)
                    {
                        tmp[idx] = (char)byteArr[offset++];
                    }

                    fieldValue = new string(tmp).Trim();
                }
            }
            else if (fieldValue is System.Collections.ICollection &&
                    fieldValue.GetType().IsGenericType && fieldValue.GetType().GetGenericArguments().Length > 1)
            {
                var tmpVal = (System.Collections.ICollection)fieldValue;
                var elemType = fieldValue.GetType().GetGenericArguments()[0];
                var sizeFieldType = fieldValue.GetType().GetGenericArguments()[1];

                var sizeField = System.Activator.CreateInstance(sizeFieldType);
                DeserializePrimitive(byteArr, ref offset, ref sizeField);
                int len = System.Convert.ToInt32(sizeField);

                for (var cnt = 0; cnt < len; cnt++)
                {
                    var elem = System.Activator.CreateInstance(elemType);

                    if (elemType.IsPrimitive && elemType.IsValueType || fieldType.IsEnum)
                    {
                        DeserializePrimitive(byteArr, ref offset, ref elem);
                        ((System.Collections.IList)fieldValue).Add(result);
                    }
                    else if (System.Attribute.GetCustomAttributes(elem.GetType(), typeof(Utility.SerializableModelAttribute)) != null &&
                        System.Attribute.GetCustomAttributes(elem.GetType(), typeof(Utility.SerializableModelAttribute)).Length > 0)
                    {
                        DeserializeModel(byteArr, ref offset, elem);
                        ((System.Collections.IList)fieldValue).Add(elem);
                    }

                    //SerializeField(ref payloadSize, byteArr, null, elem, fieldInfo);
                }
            }

            return result;
        }



        private static bool DeserializePrimitive(byte[] byteArr, ref int offset, ref object value)
        {
            bool result = true;

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
                        value = byteArr[offset++];
                    }
                    break;
                case System.TypeCode.Int16:
                    {
                        var bs = new byte[2];
                        for (int idx = 0; idx < bs.Length; idx++)
                        {
                            bs[idx] = byteArr[offset++];
                        }
                        value = System.BitConverter.ToInt16(bs, 0);
                    }
                    break;
                case System.TypeCode.UInt16:
                    {
                        var bs = new byte[2];
                        for (int idx = 0; idx < bs.Length; idx++)
                        {
                            bs[idx] = byteArr[offset++];
                        }
                        value = System.BitConverter.ToUInt16(bs, 0);
                    }
                    break;
                case System.TypeCode.Int32:
                    {
                        var bs = new byte[4];
                        for (int idx = 0; idx < bs.Length; idx++)
                        {
                            bs[idx] = byteArr[offset++];
                        }
                        value = System.BitConverter.ToInt32(bs, 0);
                    }
                    break;
                case System.TypeCode.UInt32:
                    {
                        var bs = new byte[4];
                        for (int idx = 0; idx < bs.Length; idx++)
                        {
                            bs[idx] = byteArr[offset++];
                        }
                        value = System.BitConverter.ToUInt32(bs, 0);
                    }
                    break;
                case System.TypeCode.Int64:
                    {
                        var bs = new byte[8];
                        for (int idx = 0; idx < bs.Length; idx++)
                        {
                            bs[idx] = byteArr[offset++];
                        }
                        value = System.BitConverter.ToInt64(bs, 0);
                    }
                    break;
                case System.TypeCode.UInt64:
                    {
                        var bs = new byte[8];
                        for (int idx = 0; idx < bs.Length; idx++)
                        {
                            bs[idx] = byteArr[offset++];
                        }
                        value = System.BitConverter.ToUInt64(bs, 0);
                    }
                    break;
                case System.TypeCode.Single:
                    {
                        var bs = new byte[4];
                        for (int idx = 0; idx < bs.Length; idx++)
                        {
                            bs[idx] = byteArr[offset++];
                        }
                        value = System.BitConverter.ToUInt64(bs, 0);
                    }
                    break;
                case System.TypeCode.Double:
                    {
                        var bs = new byte[8];
                        for (int idx = 0; idx < bs.Length; idx++)
                        {
                            bs[idx] = byteArr[offset++];
                        }
                        value = System.BitConverter.ToUInt64(bs, 0);
                    }
                    break;
                default:
                    {
                        result = false;
                    }
                    break;
            }

            return result;
        }
    }
}
