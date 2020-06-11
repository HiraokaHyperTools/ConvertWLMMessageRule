using ConvertWLMMessageRule.Enums;
using ConvertWLMMessageRule.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConvertWLMMessageRule.Utils
{
    class TBmsgFilterRulesMaker
    {
        public string DatSample { get; }

        public TBmsgFilterRulesMaker(WLMMailRulesReader reader)
        {
            var writer = new StringWriter();
            writer.WriteLine("version={0}", JsEnc("9"));
            writer.WriteLine("logging={0}", JsEnc("0"));

            foreach (var rule in reader.Rules)
            {
                var set = ToSet(rule);
                writer.WriteLine();
                writer.WriteLine("name={0}", JsEnc(rule.Name));
                writer.WriteLine("enabled={0}", JsEnc(rule.Enabled ? "yes" : "no"));
                writer.WriteLine("type={0}", JsEnc(((int)set.type).ToString()));
                foreach (var oneAction in set.actionSets)
                {
                    writer.WriteLine("action={0}", JsEnc(oneAction.action));
                    if (oneAction.actionValue != null)
                    {
                        writer.WriteLine("actionValue={0}", JsEnc(oneAction.actionValue));
                    }
                }
                writer.WriteLine("condition={0}", JsEnc(set.condition));
            }

            DatSample = writer.ToString();
        }

        class ActionSet
        {
            public string action = null;
            public string actionValue = null;

            public static ActionSet AddBusinessTag => new ActionSet
            {
                action = "AddTag",
                actionValue = "$label2",
            };
        }

        class ActionsAndCondSet
        {
            public List<ActionSet> actionSets = new List<ActionSet>();
            public string condition = "ALL";

            public TBFilterType type { get; set; } = TBFilterType.Manual | TBFilterType.ReceivedMailBeforeSpamFiltering;
        }

        private string EncTextInCondition(string text)
        {
            return text.Replace("\"", "\\\\\"");
        }

        private string JsEnc(string str)
        {
            return "\"" + str.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
        }

        private ActionsAndCondSet ToSet(WLMMailRule rule)
        {
            var set = new ActionsAndCondSet();

            if (rule.CriteriaList.Count == 1)
            {
                var criteria = rule.CriteriaList[0];
                switch (criteria.Type)
                {
                    case CriteriaType.From:
                        set.condition = BuildTBStringCondition("from", criteria.Verifier as StringVerifier);
                        break;
                    case CriteriaType.To:
                        set.condition = BuildTBStringCondition("to", criteria.Verifier as StringVerifier);
                        set.type = TBFilterType.Manual | TBFilterType.Sent;
                        break;
                    case CriteriaType.Cc:
                        set.condition = BuildTBStringCondition("cc", criteria.Verifier as StringVerifier);
                        break;
                    case CriteriaType.Body:
                        set.condition = BuildTBStringCondition("body", criteria.Verifier as StringVerifier);
                        break;
                    case CriteriaType.Subject:
                        set.condition = BuildTBStringCondition("subject", criteria.Verifier as StringVerifier);
                        break;
                    case CriteriaType.ToOrCc:
                        set.condition = BuildTBStringCondition("to or cc", criteria.Verifier as StringVerifier);
                        set.type = TBFilterType.Manual | TBFilterType.Sent;
                        break;
                    case CriteriaType.HasAttachment:
                        set.condition = "AND (has attachment status,is,true)";
                        break;
                }
            }
            foreach (var action in rule.ActionsList)
            {
                switch (action.Type)
                {
                    case ActionType.Delete:
                        set.actionSets.Add(new ActionSet
                        {
                            action = "Delete",
                        });
                        break;

                    case ActionType.Forward:
                        foreach (var mail in (action.Verifier as StringTargetVerifier).Target.Split(','))
                        {
                            set.actionSets.Add(
                                new ActionSet
                                {
                                    action = "Forward",
                                    actionValue = mail
                                }
                            );
                        }
                        break;
                }
            }
            if (!set.actionSets.Any())
            {
                set.actionSets.Add(ActionSet.AddBusinessTag);
            }
            return set;
        }

        private string BuildTBStringCondition(string field, StringVerifier verifier)
        {
            var logic = (verifier.AndValues) ? "AND" : "OR";
            var op = verifier.NotContain ? "doesn't contain" : "contains";
            return string.Join(" ",
                verifier.Keywords
                    .Select(keyword => $"{logic} ({field},{op},{EncTextInCondition(keyword)})")
            );
        }
    }
}
