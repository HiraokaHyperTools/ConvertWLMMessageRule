using ConvertWLMMessageRule.Enums;
using ConvertWLMMessageRule.Extensions;
using ConvertWLMMessageRule.Models;
using ConvertWLMMessageRule.Utils;
using Jint;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ConvertWLMMessageRule
{
    public partial class Form1 : Form
    {
        private WLMMailRulesReader sourceRules;

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

            this.sourceRules = reader;

            var maker = new TBmsgFilterRulesMaker(reader);

            textBox1.Text = maker.DatSample.ToString();
        }


        private void reloadBtn_Click(object sender, EventArgs e)
        {
            Reload();
        }


        private void openBtn_DropDownOpening(object sender, EventArgs e)
        {
            openBtn.DropDownItems.Clear();

            //C:\Users\USER\AppData\Roaming\Thunderbird\Profiles\3cm5xixi.default\Mail\f\msgFilterRules.dat
            foreach (var pair in MsgFilterRulesUtil.GetAll())
            {
                var tsi = openBtn.DropDownItems.Add($"{pair.FilePath} ({pair.AccountName})", null, OpenIt);
                tsi.Name = pair.FilePath;
            }
        }

        private void OpenIt(object sender, EventArgs e)
        {
            var menu = (ToolStripMenuItem)sender;
            Process.Start("notepad.exe", "\"" + menu.Name + "\"");
        }

        private void debugMenu_Click(object sender, EventArgs e)
        {
            var form = new Form();
            TreeView tree = new TreeView();
            Walk(tree.Nodes, sourceRules);
            tree.Dock = DockStyle.Fill;
            tree.ExpandAll();
            form.Text = "sourceRules";
            form.Controls.Add(tree);
            form.Show(this);
        }

        private void Walk(TreeNodeCollection nodes, object source)
        {
            if (source == null)
            {
                return;
            }

            if (source is System.Collections.ICollection)
            {
                foreach (var one in (System.Collections.ICollection)source)
                {
                    Walk(nodes, one);
                }
                return;
            }
            var type = source.GetType();
            foreach (var prop in type.GetProperties())
            {
                if (false
                    || prop.PropertyType == typeof(string)
                    || typeof(ValueType).IsAssignableFrom(prop.PropertyType)
                )
                {
                    var subNode = nodes.Add(prop.Name + " = " + prop.GetValue(source, null));
                    continue;
                }
                {
                    var subNode = nodes.Add(prop.Name);
                    try
                    {
                        //public class List<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
                        //public sealed class String : IComparable, ICloneable, IConvertible, IComparable<String>, IEnumerable<char>, IEnumerable, IEquatable<String>
                        var subAny = prop.GetValue(source, null);
                        Walk(subNode.Nodes, subAny);
                    }
                    catch (TargetParameterCountException)
                    {
                        // ignore
                    }
                }
            }
        }
    }
}
