using ConvertWLMMessageRule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertWLMMessageRule.Models
{
    public class StringVerifier : IVerifier
    {
        public bool AndValues { get; set; }
        public bool NotContain { get; set; }
        public string[] Keywords { get; set; }
    }
}
