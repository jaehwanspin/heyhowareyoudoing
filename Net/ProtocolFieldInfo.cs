namespace AppSystemSimulator.Net
{
    public class ProtocolFieldInfo
    {
        private ProtocolFieldType FieldType_;
        private int Offset_;
        private int Length_;
        private System.Collections.Generic.List<byte[]> AllowedValues_;
        public System.Collections.Generic.List<ProtocolFieldInfo> ComplexFields_;

        public bool Empty => this.Length_ == 0;

        public int Length { get { return this.Length_; } set { this.Length_ = value; } }
        public int Offset { get { return this.Offset_; } }
        public System.Collections.Generic.List<byte[]> AllowedValues { get { return this.AllowedValues_; } }
        public ProtocolFieldType FieldType { get { return this.FieldType_; } }

        public System.Collections.Generic.List<ProtocolFieldInfo> ComplexFields { get; }

        public ProtocolFieldInfo(ProtocolFieldType type, int offset, int length, System.Collections.Generic.List<byte[]> av)
        {
            this.ComplexFields_ = new System.Collections.Generic.List<ProtocolFieldInfo>();
            this.FieldType_ = type;
            this.Offset_ = offset;
            this.Length_ = length;
            this.AllowedValues_ = av;
        }

        public ProtocolFieldInfo(ProtocolFieldType type, int offset, int length) :
            this(type, offset, length, null) {}




    }
}
