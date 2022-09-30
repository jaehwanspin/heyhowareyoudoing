namespace AppSystemSimulator.Net
{

    [System.AttributeUsage(System.AttributeTargets.All)]
    class NetPayloadCodeAttribute : System.Attribute
    {
        public byte[] PayloadCode { get; }

        public NetPayloadCodeAttribute(byte[] payloadCode)
        {
            this.PayloadCode = payloadCode;
        }
    }

    //[System.AttributeUsage(System.AttributeTargets.All)]
    //class NetAttribute : System.Attribute
    //{
    //    public byte[][] PayloadCode { get; }

    //    public NetAttributes(System.UInt64 payloadCode)
    //    {
    //        this.PayloadCode = payloadCode;
    //    }
    //}


    [NetPayloadCodeAttribute(null)]
    struct Model
    {

    }
}
