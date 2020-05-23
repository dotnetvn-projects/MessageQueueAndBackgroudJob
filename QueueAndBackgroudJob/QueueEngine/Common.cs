using System;
using System.Collections.Generic;
using System.Text;

namespace QueueEngine
{
    public class Common
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
