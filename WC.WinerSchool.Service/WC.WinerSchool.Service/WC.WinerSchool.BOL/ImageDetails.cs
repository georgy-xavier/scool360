using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WC.WinerSchool.BOL
{
    public class ImageDetails
    {
        public int Id;
        public String FileType;
        public int UserId;
        public byte[] Image;
        public long SyncDate;
    }
}
