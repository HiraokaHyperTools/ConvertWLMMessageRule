using ConvertWLMMessageRule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertWLMMessageRule.Models
{
    public class IntVerifier : IVerifier
    {
        public int Value { get; set; }
    }
}
