using ConvertWLMMessageRule.Models;
using Jint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConvertWLMMessageRule.Utils
{
    class MsgFilterRulesUtil
    {
        public static IEnumerable<MsgFilterRulesDat> GetAll()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var profiles = Path.Combine(appData, "Thunderbird", "Profiles");
            foreach (var profileDir in Directory.GetDirectories(profiles))
            {
                Dictionary<string, object> pref = new Dictionary<string, object>();
                var prefsJs = Path.Combine(profileDir, "prefs.js");
                if (File.Exists(prefsJs))
                {
                    try
                    {
                        var engine = new Engine()
                            .SetValue("user_pref", (Action<string, object>)((key, value) => pref[key] = value))
                            ;

                        engine.Execute(
                            File.ReadAllText(
                                prefsJs
                            )
                        );
                    }
                    catch (Exception)
                    {
                        // ignore
                    }
                }

                Dictionary<string, string> dirToName = new Dictionary<string, string>();

                for (int x = 1; ; x++)
                {
                    if (true
                        && pref.TryGetValue($"mail.server.server{x}.directory", out object serverDirectory)
                        && pref.TryGetValue($"mail.server.server{x}.name", out object serverName)
                        )
                    {
                        dirToName[serverDirectory + ""] = serverName + "";
                    }
                    else
                    {
                        break;
                    }
                }

                var mailDir = Path.Combine(profileDir, "Mail");
                if (Directory.Exists(mailDir))
                {
                    foreach (var accountDir in Directory.GetDirectories(mailDir))
                    {
                        var msgFilterRules = Path.Combine(accountDir, "msgFilterRules.dat");
                        if (File.Exists(msgFilterRules))
                        {
                            var one = new MsgFilterRulesDat();

                            if (dirToName.TryGetValue(accountDir, out string name))
                            {
                                one.AccountName = name;
                            }

                            one.FilePath = msgFilterRules;

                            yield return one;
                        }
                    }
                }
            }
        }
    }
}
