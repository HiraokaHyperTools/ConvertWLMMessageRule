using ConvertWLMMessageRule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertWLMMessageRule.Models
{
    public class StringTargetVerifier : IVerifier
    {
        public string Target { get; set; }
    }
}
