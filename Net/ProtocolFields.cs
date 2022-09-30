namespace AppSystemSimulator.Net
{
    public class ProtocolFields
    {
        private System.Collections.Generic.List<ProtocolFieldInfo> Fields_;

        public System.Collections.Generic.List<ProtocolFieldInfo> Fields { get { return this.Fields_; } }

        public ProtocolFields()
        {
            this.Fields_ = new System.Collections.Generic.List<ProtocolFieldInfo>();
        }

        public void Add(ProtocolFieldInfo info)
        {
            this.Fields_.Add(info);
        }

        //// offset in bits
        //Utility.Option<System.Tuple<int, int>> GetOffset(ProtocolFieldType type)
        //{
        //    var result = new Utility.Option<System.Tuple<int, int>>();

        //    foreach (var field in this.Fields_)
        //    {
        //        if (field.FieldType == type)
        //        {
        //            result += new System.Tuple<int, int>(field.Offset, field.Length);
        //        }
        //    }

        //    return result;
        //}

        

    }
}
