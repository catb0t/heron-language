﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace HeronEdit
{
    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        public delegate void Proc();

        #region useful Regular expressions
        public static Regex RegexXmlOpenTag = new Regex(@"<\s*(\w)+", RegexOptions.Compiled);
        public static Regex RegexXmlCloseTag = new Regex(@"<\s*/\s*(\w)+", RegexOptions.Compiled);
        public static Regex RegexTags = new Regex(@"<[^>]*>", RegexOptions.Compiled);
        #endregion

        #region Regex extensions
        public static string CapturedString(this Match self, int n)
        {
            if (self.Groups.Count <= n + 1) return "";
            return self.Groups[n + 1].Value;
        }
        public static string CapturedString(this Match self)
        {
            return CapturedString(self, 0);
        }
        public static bool AtBegin(this Match self)
        {
            return self.Index == 0;
        }
        public static bool AtEnd(this Match self, String s)
        {
            return self.Index + self.Length == s.Length;
        }
        #endregion 

        #region TextBoxBase and RichTextBox extensions
        public static void ColorText(this RichTextBox self, int n, int len, Color c)
        {
            ColorText(self, new Range(n, len), c);
        }

        public static void ColorText(this RichTextBox self, Range r, Color c)
        {
            self.SelectAndApply(r, () => { self.SelectionColor = c; });
        }

        public static Range SelectedRange(this TextBoxBase self)
        {
            return new Range(self.SelectionStart, self.SelectionLength);
        }

        public static void SelectText(this TextBoxBase self, Range range)
        {
            self.SelectionStart = range.Begin;
            self.SelectionLength = range.Length;
        }

        public static int SelectionEnd(this TextBoxBase self)
        {
            return self.SelectionStart + self.SelectionLength;
        }

        public static void ExpandSelectionLeftTo(this TextBoxBase self, int n)
        {
            Range r = self.SelectedRange();
            r.ExpandLeftTo(n);
            SelectText(self, r);
        }

        public static void ExpandSelectionRightTo(this TextBoxBase self, int n)
        {
            Range r = self.SelectedRange();
            r.ExpandRightTo(n);
            SelectText(self, r);
        }

        public static void SelectAndApply(this RichTextBox self, Range r, Proc p)
        {
            Range old = SelectedRange(self);
            try
            {
                SelectText(self, r);
                p();
            }
            finally
            {
                SelectText(self, old);
            }
        }

        public static void Apply(this RichTextBox self, Proc p)
        {
            Range old = SelectedRange(self);
            try
            {
                p();
            }
            finally
            {
                SelectText(self, old);
            }
        }

        public static void SelectText(this TextBoxBase self, int n, int len)
        {
            self.SelectionStart = n;
            self.SelectionLength = len;
        }

        public static void SetTextColor(this RichTextBox self, Color c, Range r)
        {
            SelectAndApply(self, r, () => self.SelectionColor = c);
        }

        public static string GetText(this TextBoxBase self, int begin, int len)
        {
            return GetText(self, new Range(begin, len));
        }

        public static string GetText(this TextBoxBase self, Range r)
        {
            return self.Text.Substring(r);
        }

        public static int SelectionLineIndex(this TextBoxBase self)
        {
            int line, pos;
            GetSelectionLineAndPos(self, out line, out pos);
            return line;
        }

        public static int SelectionLinePos(this TextBoxBase self)
        {
            int line, pos;
            GetSelectionLineAndPos(self, out line, out pos);
            return pos;
        }

        public static void GetLineAndPosFromIndex(this TextBoxBase self, int index, out int line, out int pos)
        {
            line = self.GetLineFromCharIndex(index);
            int linepos = self.GetFirstCharIndexFromLine(line);
            pos = index - linepos;
        }

        public static void GetSelectionLineAndPos(this TextBoxBase self, out int line, out int pos)
        {
            GetLineAndPosFromIndex(self, self.SelectionStart, out line, out pos);
        }

        /*
        public static void WavyUnderline(this RichTextBox self)
        {
            string rtf = self.SelectedText;
            self.SelectionBackColor = Color.YellowGreen;
            //rtf = rtf.StripRtfHeader();
            //rtf = @"\ulwave " + rtf + @"\ulwave0 ";            
            //self.SelectedRtf = rtf;
        }

        public static void WavyUnderline(this RichTextBox self, Range r)
        {
            self.SelectAndApply(r, () => { self.WavyUnderline(); });  
        }

        public static void ClearWavyUnderlines(this RichTextBox self)
        {
            string rtf = self.Rtf;
            if (rtf.IndexOf(@"\ulwave") >= 0)
                return;
            rtf = rtf.Replace(@"\ulwave ", "");
            rtf = rtf.Replace(@"\ulwave0 ", "");
            self.Rtf = rtf;
        }*/

        public static int GetCharIndexFromLineAndPosition(this RichTextBox self, int line, int pos)
        {
            return self.GetFirstCharIndexFromLine(line) + pos;
        }
        #endregion

        #region other control extensions
        public static void SelectBestMatch(this ComboBox self, string s)
        {
            int n = self.FindString(s);
            if (n > 0)
            {
                self.SelectedIndex = n;
                return;
            }
            
            int cnt = self.Items.Count;
            if (cnt > 0)
                SelectBestMatchBetween(self, s, 0, cnt - 1);
        }

        public static void SelectBestMatchBetween(this ComboBox self, string s, int a, int b)
        {
            if (b - a < 0)
                throw new ArgumentException("'b' must be greater than 'a'");

            if (b - a == 0)
            {
                self.SelectedIndex = a;
            }
            else if (b - a == 1)
            {
                if (self.GetItemText(b).CompareTo(s) <= 0)
                {
                    self.SelectedIndex = b;
                }
                else
                {
                    self.SelectedIndex = a;
                }
            }
            else 
            {
                int n = a + ((b - a) / 2);
                string tmp = self.Items[n].ToString();
                if (tmp.CompareTo(s) < 0)
                {
                    SelectBestMatchBetween(self, s, n, b);
                }
                else 
                {
                    SelectBestMatchBetween(self, s, a, n);
                }
            }
        }
        #endregion

        #region char extensions
        public static bool IsWSpace(this Char self)
        {
            return Char.IsWhiteSpace(self);
        }
        public static bool IsLetter(this Char self)
        {
            return Char.IsLetter(self);
        }
        #endregion 

        #region string extensions
        public static String Substring(this String self, Range range)
        {
            return self.Substring(range.Begin, range.Length);
        }

        public static String CompressWSpace(this String self)
        {
            self = self.Trim();
            if (self.Length == 0) return self;
            Regex ws = new Regex(@"\s+", RegexOptions.Singleline | RegexOptions.Compiled);
            return ws.Replace(self, " ");
        }

        public static String CompressWSpaceKeepingNewLines(this String self)
        {
            if (self.Length == 0) return self;

            Regex wspaceWithNewline = new Regex(@"[ \t\r]*\n[ \t\r]*", RegexOptions.Singleline | RegexOptions.Compiled);
            self = wspaceWithNewline.Replace(self, "\n");

            Regex wspaceWithoutNewline = new Regex(@"[ \t\r]+", RegexOptions.Singleline | RegexOptions.Compiled);
            self = wspaceWithoutNewline.Replace(self, " ");
            
            return self;
        }

        public static int IndexOf(this String self, Predicate<Char> pred, int n)
        {
            while (n < self.Length)
            {
                if (pred(self[n]))
                    return n;
                ++n;
            }
            return -1;
        }

        public static int IndexOfPrev(this String self, char c, int n)
        {
            return IndexOfPrev(self, (Char x) => x == c, n); 
        }

        public static int IndexOfPrev(this String self, Predicate<Char> pred, int n)
        {
            --n;
            while (n >= 0)
            {
                if (pred(self[n]))
                    return n;
                --n;
            }
            return -1;
        }

        public static String Until(this String self, int n)
        {
            return self.Substring(0, n);
        }

        public static bool IsAtTag(this String self, int n)
        {
            if (n < 0 || n >= self.Length - 2)
                return false;
            return self[n] == '<';
        }

        public static bool IsAtOpenTag(this String self, int n)
        {
            if (!IsAtTag(self, n))
                return false;
            if (!self[n + 1].IsLetter())
                return false;
            int m = self.IndexOf('>', n + 1);
            if (m < 0)
                return false;
            return self[m - 1] != '/';
        }

        public static bool IsAtCloseTag(this String self, int n)
        {
            if (!IsAtTag(self, n))
                return false;
            return self[n + 1] == '/';
        }

        public static String OpenTagAt(this String self, int n)
        {
            if (!IsAtOpenTag(self, n))
                return "";
            return WordAt(self, n + 1);
        }

        public static String CloseTagAt(this String self, int n)
        {
            if (!IsAtCloseTag(self, n))
                return "";
            return WordAt(self, n + 2);
        }

        public static String WordAt(this String self, int n)
        {
            if (n < 0 || n >= self.Length)
                return "";
            if (!self[n].IsLetter())
                return "";
            int a = n;
            while (a > 0 && self[a - 1].IsLetter())
                --a;
            int b = n;
            while (b < self.Length && self[b].IsLetter())
                ++b;
            return self.Substring(a, b - a);
        }

        public static bool AtString(this String self, string sub, int n)
        {
            if (n > self.Length - sub.Length)
                return false;
            for (int i=0; i < sub.Length; ++i)
                if (self[n + i] != sub[i])
                    return false;
            return true;
        }
        #endregion

        #region matching extensions
        public static bool MatchesAtBegin(this string self, Regex regex)
        {
            Match m = regex.Match(self);
            return m != null && m.AtBegin();
        }
        public static bool MatchesAtEnd(this string self, Regex regex)
        {
            Match m = self.LastMatch(regex);
            return m != null && m.AtEnd(self);
        }
        public static Match LastMatch(this string self, Regex regex)
        {
            MatchCollection c = regex.Matches(self);
            if (c.Count == 0) return null;
            return c[c.Count - 1];
        }
        public static Match MatchBefore(this string self, Regex regex, int n)
        {
            return LastMatch(self.Until(n), regex); 
        }

        public static Match MatchAfter(this string self, Regex regex, int n)
        {
            return regex.Match(self.Substring(n));
        }

        public static Match MatchWithin(this string self, Regex regex, Range r)
        {
            return regex.Match(self.Substring(r));
        }

        public static Match MatchWithin(this string self, Regex regex, int n, int len)
        {
            return MatchWithin(self, regex, new Range(n, len));
        }        
        #endregion

        #region XML tag parsing text extensions
        public static Match MatchXmlOpenTag(this string self, int n)
        {
            return self.MatchAfter(RegexXmlOpenTag, n);
        }       

        public static int IndexOfTagBeginBefore(this string self, int n)
        {
            while (n > 0 && self[n] != '<') --n;
            return n;
        }

        public static int IndexOfTagEndAfter(this string self, int n)
        {
            while (n > 0 && self[n] != '>') n++;
            return n;
        }

        public static String GetXmlAround(this string self, int n)
        {
            int a = self.IndexOfTagBeginBefore(n);
            int b = self.IndexOfTagEndAfter(n);
            return self.Substring(a, b - a);
        }

        public static bool IsXmlElement(this string self, ref string tag)
        {
            Match m = RegexXmlOpenTag.Match(self);
            if (m == null) return false;
            tag = m.CapturedString();
            if (tag == null || tag.Length == 0) return false;
            Regex closeTag = new Regex(@"<\s*/\s*" + tag + @"\s*>");
            return self.MatchesAtEnd(closeTag);            
        }

        public static int IndexOfPreviousTagBegin(this string s, int n)
        {
            if (s.Length == 0)
                return -1;

            while (n >= 0)
            {
                if (s[n] == '<')
                    return n;
                --n;
            }
            return -1;
        }

        public static int IndexOfNextTagEnd(this string s, int n)
        {
            while (n < s.Length - 1)
            {
                if (s[n] == '>')
                    return n + 1;
                ++n;
            }
            return -1;
        }

        public static string GetSurroundingTaggedText(this string s, int n)
        {
            int begin = s.IndexOfPreviousTagBegin(n);
            int end = s.IndexOfNextTagEnd(n);
            if (begin < 0 || end < 0 || end <= begin)
                return "";
            return s.Substring(begin, end - begin);                
        }

        public static String StripTags(this String self)
        {
            return RegexTags.Replace(self, "");
        }

        public static String AddTags(this String self, string tag)
        {
            return "<" + tag + ">" + self + "</" + tag + ">";
        }

        public static bool InsideOfTag(this string self, int n)
        {
            --n;
            while (n >= 0)
            {
                if (self[n] == '<')
                    return true;
                else if (self[n] == '>')
                    return false;
                else
                    --n;
            }
            return false;
        }
        #endregion 

        #region XML extensions
        public static int GetNestingLevel(this XmlNode e)
        {
            if (e.ParentNode != null)
                return 1 + GetNestingLevel(e.ParentNode);
            else
                return 0;
        }

        public static bool HasTextChildren(this XmlNode n)
        {
            foreach (XmlNode child in n.ChildNodes)
                if (child is XmlText)
                    return true;
            return false;
        }

        public static IEnumerable<XmlElement> ChildElements(this XmlElement e)
        {
            foreach (XmlNode node in e.ChildNodes)
            {
                if (node is XmlElement)
                    yield return node as XmlElement;
            }
        }

        public static XmlElement GetChildElement(this XmlElement e, string tag)
        {
            return e.GetElementsByTagName(tag)[0] as XmlElement;
        }

        public static void AddAttribute(this XmlElement e, string key, string value)
        {
            XmlAttribute attr = e.OwnerDocument.CreateAttribute(key);
            attr.Value = value;
            e.Attributes.Append(attr);
        }

        public static XmlElement FirstChildElement(this XmlNode n)
        {
            if (n.FirstChild is XmlElement)
                return n.FirstChild as XmlElement;
            else
                return n.FirstChild.NextElement();
        }

        public static XmlElement NextElement(this XmlNode n)
        {
            n = n.NextSibling;
            while (n != null)
            {
                if (n is XmlElement)
                    return n as XmlElement;
                n = n.NextSibling;
            }
            return null;
        }

        public static XmlElement GetAncestorElement(this XmlNode e, string name)
        {
            e = e.ParentNode;
            while (e != null)
            {
                if (e is XmlElement && e.Name == name)
                    return e as XmlElement;
                e = e.ParentNode;
            }
            return null;
        }

        public static XmlElement AddElement(this XmlNode n, string name)
        {
            XmlElement r = n.OwnerDocument.CreateElement(name);
            n.AppendChild(r);
            return r;
        }

        public static IEnumerable<XmlNode> InnerNodes(this XmlNode n)
        {
            foreach (XmlNode child in n.ChildNodes)
            {
                if (!(child is XmlAttribute))
                    yield return child;                    
            }
        }

        public static int MaxNesting(this XmlNode n)
        {
            if (!n.HasChildNodes)
                return 0;
            int max = 0;
            foreach (XmlNode child in n.ChildNodes)
                if (child.MaxNesting() > max)
                    max = child.MaxNesting();
            return max + 1;
        }

        public static bool ShouldIndent(this XmlNode n)
        {
            return n is XmlElement && (n.MaxNesting() > 1 || n.ChildNodes.Count > 1);
        }

        public static bool HasTextNodes(this XmlNode self)
        {
            foreach (XmlNode child in self.ChildNodes)
                if (child is XmlText)
                    return true;
            return false;
        }

        /*
        public static void Reformat(this XmlNode n, string indent)
        {
            if (n is XmlText)
            {
                string s = n.InnerText.Trim().CompressWSpaceKeepingNewLines();

                // Indent the new lines 
                s = s.Replace("\n", "\n" + indent);
                
                if (n.PreviousSibling != null)
                    s = " " + s;

                if (n.NextSibling != null)
                    s = s + " ";

                n.InnerText = s;
            }
            else if (n is XmlElement)
            {
                XmlElement e = n as XmlElement;

                // Concatenate the contents of each child node
                StringBuilder sb = new StringBuilder();
                foreach (XmlNode child in e.InnerNodes())
                {
                    if (child.ShouldIndent())
                    {

                    }
                    string s = child.OuterXml;
                    sb.Append("\n" + indent);
                    sb.Append(s);
                }

                    e.InnerXml = sb.ToString() + "\n" + indent;
                }
                else if (innerNodes.Count == 1)
                {
                    if (innerNodes[0] is XmlText)
                    {
                        if (innerNodes[0].InnerXml.Contains('\n'))
                        {
                            e.InnerXml = "\n  " + indent + innerNodes[0].OuterXml + "\n" + indent;
                        }
                    }
                    else if (innerNodes[0] is XmlElement)
                    {
                        if (innerNodes[0].ShouldIndent())
                        {
                            e.InnerXml = "\n  " + indent + innerNodes[0].OuterXml + "\n" + indent;
                        }
                    }
                }
            }
        }

        public static string PrettyPrint(this XmlDocument doc)
        {
            foreach (XmlElement e in doc.
        }
         * */

        public static string PrettyPrint(this XmlDocument doc)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.NewLineChars = "\n";
            settings.NewLineHandling = NewLineHandling.Replace;
            settings.OmitXmlDeclaration = true;
            XmlWriter writer = XmlWriter.Create(sb, settings);
            doc.Save(writer);
            writer.Close();
            return sb.ToString();
        }
        #endregion
    }

    /// <summary>
    /// Represents an integer range, for example when dealing with sub-strings.
    /// </summary>
    public class Range
    {
        int begin;
        int length;

        public int Begin 
        {
            get { return begin; }
            set { if (value < 0) value = 0;  begin = value; }
        }

        public int Length
        {
            get { return length; }
            set { if (value < 0) value = 0; length = value; }
        }

        public int End
        {
            get { return begin + length; }
            set { if (value < Begin) value = Begin; Length = value - Begin; }
        }

        public Range(int begin, int len)
        {
            Begin = begin;
            Length = len;
        }

        public void Clear()
        {
            Begin = 0;
            Length = 0;
        }

        public String Apply(String s)
        {
            return s.Substring(Begin, Length);
        }

        public void ExpandTo(int n)
        {
            if (n >= Begin)
            {
                End = n;
            }
            else if (n < Begin)
            {
                Begin = n;
            }
        }

        public void ExpandLeftTo(int n)
        {
            int end = End;
            if (n > end)
            {
                Begin = n;
                Length = 0;
            }
            else
            {
                Begin = n;
                End = end;
            }
        }

        public void ExpandRightTo(int n)
        {
            if (n < Begin)
            {
                Begin = n;
                Length = 0;
            }
            else
            {
                End = n;
            }
        }

        public void Set(int begin, int length)
        {
            Begin = begin;
            Length = length;
        }
    }
}
