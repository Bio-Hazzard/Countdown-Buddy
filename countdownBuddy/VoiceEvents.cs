using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace countdownBuddy
{
    public class VoiceEvents : IEquatable<VoiceEvents>
    {
        public TimeSpan MsgTime { get; set; }
        public int rate { get; set; }
        public string msg { get; set; }


        //public override string ToString()
        //{
        //    return "ID: " + PartId + "   Name: " + PartName;
        //}
        /*public override bool Equals(object obj)
        {
            if (obj == null) return false;
            VoiceEvents objAsPart = obj as VoiceEvents;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }*/
        /*public override int GetHashCode()
        {
            return PartId;
        }*/
        public bool Equals(VoiceEvents other)
        {
            if (other == null) return false;
            return (this.MsgTime.Equals(other.MsgTime));
        }
        // Should also override == and != operators.

        public int CompareTo(VoiceEvents time)
        {
            //not implemented
            if (time.MsgTime < this.MsgTime)
            {
                return 1;
            } else if (time.MsgTime > this.MsgTime)
            {
                return -1;
            } else
            {
                return 0;
            }
        }

    }

    public class TimeSpanComparer : IComparer<VoiceEvents>
    {
        public int Compare(VoiceEvents x, VoiceEvents y)
        {
            return x.CompareTo(y);

            // ...and y is not null, compare the 
            // lengths of the two strings.
            //

            /*int retval = x.CompareTo(y);

            if (retval != 0)
            {
                // If the strings are not of equal length,
                // the longer string is greater.
                //
                return retval;
            }
            else
            {
                // If the strings are of equal length,
                // sort them with ordinary string comparison.
                //
                return x.CompareTo(y);
            }*/
        }
    }
}
