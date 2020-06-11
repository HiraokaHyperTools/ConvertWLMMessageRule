using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertWLMMessageRule.Enums
{
    [Flags]
    public enum TBFilterType
    {
        ReceivedMailBeforeSpamFiltering = 1,
        Manual = 16,
        ReceivedMailAfterSpamFiltering = 32,
        Sent = 64,
        Archived = 128,
        TenMinutes = 256,
    }
}
