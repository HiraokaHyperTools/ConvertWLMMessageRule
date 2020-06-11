using ConvertWLMMessageRule.Enums;
using ConvertWLMMessageRule.Extensions;
using ConvertWLMMessageRule.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConvertWLMMessageRule.Utils
{
    public class WLMMailRulesReader
    {
        public List<WLMMailRule> Rules { get; } = new List<WLMMailRule>();

        private static Regex spaceSplitter = new Regex("\\s+");

        public WLMMailRulesReader()
        {
            using (var mailKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows Live Mail\Rules\Mail", false))
            {
                if (mailKey != null)
                {
                    var OrderValue = "" + mailKey.GetValue("Order");
                    foreach (var order in spaceSplitter.Split(OrderValue))
                    {
                        using (var orderKey = mailKey.OpenSubKey(order, false))
                        {
                            if (orderKey != null)
                            {

                                var Name = "" + orderKey.GetValue("Name");
                                var Enabled = ("" + orderKey.GetValue("Enabled")) == "1";

                                var rule = new WLMMailRule
                                {
                                    Name = Name,
                                    Enabled = Enabled,
                                };
                                Rules.Add(rule);

                                using (var criteriaKey = orderKey.OpenSubKey("Criteria", false))
                                {
                                    if (criteriaKey != null)
                                    {
                                        var criteriaOrderValue = "" + criteriaKey.GetValue("Order");
                                        foreach (var criteriaOrder in spaceSplitter.Split(criteriaOrderValue))
                                        {
                                            using (var criteriaOrderKey = criteriaKey.OpenSubKey(criteriaOrder, false))
                                            {
                                                if (criteriaOrderKey != null)
                                                {
                                                    var Logic = criteriaOrderKey.GetIntValue("Logic") ?? 0;
                                                    var Type = criteriaOrderKey.GetIntValue("Type") ?? 0;

                                                    var criteria = new WLMCriteria
                                                    {
                                                        ThisAndNext = 0 != (2 & Logic),
                                                        Type = (TargetType)Type,
                                                    };
                                                    rule.CriteriaList.Add(criteria);

                                                    var Flags = criteriaOrderKey.GetIntValue("Flags") ?? 0;
                                                    var ValueType = criteriaOrderKey.GetIntValue("ValueType") ?? 0;
                                                    var StringValue = criteriaOrderKey.GetByteaUnicodeStringValue("Value") ?? "";
                                                    var IntValue = criteriaOrderKey.GetIntValue("Value") ?? 0;

                                                    switch (ValueType)
                                                    {
                                                        case 19:
                                                            {
                                                                criteria.Verifier = new IntVerifier
                                                                {
                                                                    Value = IntValue,
                                                                };
                                                                break;
                                                            }
                                                        case 31:
                                                            {
                                                                criteria.Verifier = new AccountVerifier
                                                                {
                                                                    Account = StringValue,
                                                                };
                                                                break;
                                                            }
                                                        case 65:
                                                            {
                                                                criteria.Verifier = new StringVerifier
                                                                {
                                                                    AndValues = 0 != (2 & Flags),
                                                                    NotContain = 0 != (1 & Flags),
                                                                    Keywords = StringValue.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries),
                                                                };
                                                                break;
                                                            }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
