using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSystemSimulator.Model.Common
{
    [Utility.SerializableModel]
    class TimeSpec
    {
        private static TimeSpec GetNow()
        {
            var result = new TimeSpec();

            return result;
        }

        private static TimeSpec GetUtcNow()
        {
            var result = new TimeSpec();

            return result;
        }

        public TimeSpec Now
        {
            get
            {
                return GetNow();
            }
        }
        public TimeSpec UtcNow
        {
            get
            {
                return GetUtcNow();
            }
        }

        [Utility.SerializableField]
        public System.UInt64 Seconds;

        [Utility.SerializableField]
        public System.UInt64 Nanoseconds;

        public TimeSpec()
        {
            this.Seconds = 0;
            this.Nanoseconds = 0;
        }

        public System.DateTime ToDateTime()
        {
            var result = new System.DateTime();

            var n = System.DateTime.Now;
            

            return result;
        }


    }
}
