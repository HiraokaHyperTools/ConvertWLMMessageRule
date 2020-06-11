using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertWLMMessageRule.Enums
{
    public enum CriteriaType
    {
        Subject = 8,
        Body = 9,
        To = 10,
        Cc = 11,
        From = 12,
        Importance = 13,
        HasAttachment = 14,
        OverKBSize = 15,
        TargetAccount = 19,
        Every = 20,
        ToOrCc = 22,
        Security = 31,
    }
}
