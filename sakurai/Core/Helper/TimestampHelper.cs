using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sakurai.Interface.IHelper;

namespace sakurai.Core.Helper
{
    public class TimestampHelper : ITimestampHelper
    {
        public string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
