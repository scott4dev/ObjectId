using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectId2
{
    public class DataCenterAwareIdGenerator2 : MongoDB.Bson.Serialization.IIdGenerator
    {
        public static byte DataCenter { get; set; }


        public static byte[] Pack(byte dataCenter, int timestamp, int machine, short pid, int increment)
        {
            if ((machine & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException("machine", "The machine value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }
            if ((increment & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException("increment", "The increment value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }

            byte[] bytes = new byte[12];
            bytes[0] = dataCenter;
            bytes[1] = (byte)(timestamp >> 24);
            bytes[2] = (byte)(timestamp >> 16);
            bytes[3] = (byte)(timestamp >> 8);
            bytes[4] = (byte)(timestamp);
            bytes[5] = (byte)(machine >> 16);
            bytes[6] = (byte)(machine >> 8);
            bytes[7] = (byte)(machine);
            bytes[8] = (byte)(pid >> 8);
            bytes[9] = (byte)(pid);
            bytes[10] = (byte)(increment >> 16);
            bytes[11] = (byte)(increment >> 8);            
            return bytes;
        }

        public object GenerateId(object container, object document)
        {
            var newId = ObjectId.GenerateNewId();
            var bytes = Pack(DataCenter, newId.Timestamp, DataCenter, newId.Pid, newId.Increment);
            return new ObjectId(bytes).ToString();
        }

        public bool IsEmpty(object id)
        {
            return id == null || (ObjectId)id == ObjectId.Empty;
        }
    }
}
