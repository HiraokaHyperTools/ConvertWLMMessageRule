using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertWLMMessageRule.Enums
{
    public enum ActionType
    {
        Delete = 7,

        /// <summary>
        /// ValueType = 31
        /// `work@example.com,kojin@example.com`
        /// </summary>
        Forward = 2,
    }
}
