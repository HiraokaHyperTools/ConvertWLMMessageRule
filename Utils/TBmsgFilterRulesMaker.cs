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
                writer.WriteLine();
                writer.WriteLine("name={0}", JsEnc(rule.Name));
                writer.WriteLine("enabled={0}", JsEnc(rule.Enabled ? "yes" : "no"));
                writer.WriteLine("type={0}", JsEnc("16"));
                writer.WriteLine("action={0}", JsEnc("AddTag"));
                writer.WriteLine("actionValue={0}", JsEnc("$label2"));
                writer.WriteLine("condition={0}", JsEnc(ToTBCondition(rule)));
            }

            DatSample= writer.ToString();
        }

        private string EncTextInCondition(string text)
        {
            return text.Replace("\"", "\\\\\"");
        }

        private string JsEnc(string str)
        {
            return "\"" + str.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
        }

        private string ToTBCondition(WLMMailRule rule)
        {
            if (rule.CriteriaList.Count == 1)
            {
                var criteria = rule.CriteriaList[0];
                switch (criteria.Type)
                {
                    case TargetType.From:
                        return BuildTBStringCondition("from", criteria.Verifier as StringVerifier);
                    case TargetType.To:
                        return BuildTBStringCondition("to", criteria.Verifier as StringVerifier);
                    case TargetType.Cc:
                        return BuildTBStringCondition("cc", criteria.Verifier as StringVerifier);
                    case TargetType.Body:
                        return BuildTBStringCondition("body", criteria.Verifier as StringVerifier);
                    case TargetType.Subject:
                        return BuildTBStringCondition("subject", criteria.Verifier as StringVerifier);
                    case TargetType.ToOrCc:
                        return BuildTBStringCondition("to or cc", criteria.Verifier as StringVerifier);
                }
            }
            return "ALL";
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
