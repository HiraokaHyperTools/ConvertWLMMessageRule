using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertWLMMessageRule.Extensions
{
    static class RegKeyExtensions
    {
        public static int? GetIntValue(this RegistryKey key, string name)
        {
            var value = key.GetValue(name);
            if (value is int)
            {
                return (int)value;
            }
            return null;
        }

        public static string GetByteaUnicodeStringValue(this RegistryKey key, string name)
        {
            var value = key.GetValue(name);
            if (value is byte[])
            {
                return Encoding.Unicode.GetString((byte[])value);
            }
            return null;
        }
    }
}
