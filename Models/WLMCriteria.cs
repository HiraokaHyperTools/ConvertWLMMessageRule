using ConvertWLMMessageRule.Enums;
using ConvertWLMMessageRule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertWLMMessageRule.Models
{
    public class WLMCriteria
    {
        public TargetType Type { get; set; }
        public IVerifier Verifier { get; set; }
        public bool ThisAndNext { get; set; }
    }
}
