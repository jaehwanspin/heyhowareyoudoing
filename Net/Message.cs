
namespace AppSystemSimulator.Net
{
    public class Message : MessageBase
    {
        public Message(MessageBase baseVal) :
            base(baseVal.PayloadBuffer, baseVal.PayloadCode, baseVal.SequenceNumber)
        {
        }
    }
}
