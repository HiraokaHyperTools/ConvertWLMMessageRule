using ConvertWLMMessageRule.Enums;
using ConvertWLMMessageRule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertWLMMessageRule.Models
{
    public class WLMMailRule
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }

        public List<WLMCriteria> CriteriaList { get; set; } = new List<WLMCriteria>();
        public List<WLMAction> ActionsList { get; set; } = new List<WLMAction>();
    }
}
