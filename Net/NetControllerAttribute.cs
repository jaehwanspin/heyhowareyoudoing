namespace AppSystemSimulator.Net
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    class NetControllerAttribute : System.Attribute
    {
        byte[] PayloadCode { get; }
        public NetControllerAttribute(byte[] payloadCode)
        {
            this.PayloadCode = payloadCode;
        }
    }
}
