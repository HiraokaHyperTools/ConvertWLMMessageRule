using ConvertWLMMessageRule.Enums;
using ConvertWLMMessageRule.Extensions;
using ConvertWLMMessageRule.Models;
using ConvertWLMMessageRule.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ConvertWLMMessageRule
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Reload();
        }

        void Reload()
        {
            var reader = new WLMMailRulesReader();

            var maker = new TBmsgFilterRulesMaker(reader);

            textBox1.Text = maker.DatSample.ToString();
        }


        private void reloadBtn_Click(object sender, EventArgs e)
        {
            Reload();
        }

        IEnumerable<string> GetAllMsgFilterRules()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var profiles = Path.Combine(appData, "Thunderbird", "Profiles");
            foreach (var profileDir in Directory.GetDirectories(profiles))
            {
                var mailDir = Path.Combine(profileDir, "Mail");
                if (Directory.Exists(mailDir))
                {
                    foreach (var accountDir in Directory.GetDirectories(mailDir))
                    {
                        var msgFilterRules = Path.Combine(accountDir, "msgFilterRules.dat");
                        if (File.Exists(msgFilterRules))
                        {
                            yield return msgFilterRules;
                        }
                    }
                }
            }
        }

        private void openBtn_DropDownOpening(object sender, EventArgs e)
        {
            openBtn.DropDownItems.Clear();

            //C:\Users\USER\AppData\Roaming\Thunderbird\Profiles\3cm5xixi.default\Mail\f\msgFilterRules.dat
            foreach (var file in GetAllMsgFilterRules())
            {
                openBtn.DropDownItems.Add(file, null, OpenIt);
            }
        }

        private void OpenIt(object sender, EventArgs e)
        {
            var menu = (ToolStripMenuItem)sender;
            Process.Start("notepad.exe", "\"" + menu.Text + "\"");
        }
    }
}
