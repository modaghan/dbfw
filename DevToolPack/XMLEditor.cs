using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DevToolPack
{
    public partial class XmlEditor : UserControl
    {
        #region Instance Variables

        private static Color specialCharColor = Color.Blue;   //  Color for special characters
        private static Color escapeColor = Color.Orchid;      //  Color for escape sequences
        private static Color elementColor = Color.DarkRed;    //  Color for Xml elements
        private static Color attributeColor = Color.Red;      //  Color for Xml attributes
        private static Color valueColor = Color.DarkBlue;     //  Color for attribute values
        private static Color commentColor = Color.DarkGreen;  //  Color for Xml comments

        private bool allowXmlFormatting = true;               //  Whether to do Xml formatting when text changes

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public XmlEditor()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Set or get the text of the xml editor.
        /// </summary>
        public override string Text
        {
            set
            {
                xmlTextBox.Text = value;
            }
            get
            {
                return xmlTextBox.Text;
            }
        }

        /// <summary>
        /// Tells whether to format the editor's Xml or not.
        /// </summary>
        public bool AllowXmlFormatting
        {
            set
            {
                allowXmlFormatting = value;
            }
            get
            {
                return allowXmlFormatting;
            }
        }

        /// <summary>
        /// Whether to allow the user to change text.
        /// </summary>
        public bool ReadOnly
        {
            set
            {
                this.xmlTextBox.ReadOnly = value;
            }
            get
            {
                return this.xmlTextBox.ReadOnly;
            }
        }

        #endregion

        #region UI Events

        /// <summary>
        /// Format Xml when text changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xmlTextBox_TextChanged(object sender, EventArgs e)
        {
            if (allowXmlFormatting)
            {
                FormatXml(this.xmlTextBox);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Format Xml in the passed rich text box.
        /// </summary>
        /// <param name="xmlEditor"></param>
        public static void FormatXml(RichTextBox xmlEditor)
        {
            //  Stop redrawing
            RichTextDrawing.StopRedraw(xmlEditor);

            //  Tokenize the Xml string
            List<XmlToken> tokens = XmlTokenizer.Tokenize(xmlEditor.Text);
            foreach (XmlToken token in tokens)
            {
                xmlEditor.Select(token.Index, token.Text.Length);
                switch (token.Type)
                {
                    case XmlTokenType.None:
                        xmlEditor.SelectionColor = xmlEditor.ForeColor;
                        break;
                    case XmlTokenType.SpecialChar:
                        xmlEditor.SelectionColor = specialCharColor;
                        break;
                    case XmlTokenType.Escape:
                        xmlEditor.SelectionColor = escapeColor;
                        break;
                    case XmlTokenType.Element:
                        xmlEditor.SelectionColor = elementColor;
                        break;
                    case XmlTokenType.Attribute:
                        xmlEditor.SelectionColor = attributeColor;
                        break;
                    case XmlTokenType.Value:
                        xmlEditor.SelectionColor = valueColor;
                        break;
                    case XmlTokenType.Comment:
                        xmlEditor.SelectionColor = commentColor;
                        break;
                }
            }

            //  Sample code to show that the perf problem is a RichTexBox problem
            //string content = xmlEditor.Text;
            //Random gen = new Random();
            //for (int i = 0; i < content.Length; i++)
            //{
            //    xmlEditor.Select(i, 1);
            //    Color c = Color.FromArgb(gen.Next(256), gen.Next(256), gen.Next(256));
            //    xmlEditor.SelectionColor = c;
            //}

            //  Resume redraw
            RichTextDrawing.RestoreRedraw(xmlEditor);
        }

        #endregion

    }

    #region Helper Class

    /// <summary>
    /// Helper class to change colors on a RichTextBox without flickering.
    /// </summary>
    public class RichTextDrawing
    {
        private static int lastSelection;

        [DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hWndLock);

        public static void StopRedraw(RichTextBox richTextBox)
        {
            LockWindowUpdate(richTextBox.Handle);

            //  Save the last location 
            lastSelection = richTextBox.SelectionStart;

            // Refresh colors
            richTextBox.SelectAll();
            richTextBox.SelectionColor = richTextBox.ForeColor;
        }

        public static void RestoreRedraw(RichTextBox richTextBox)
        {
            LockWindowUpdate(IntPtr.Zero);

            //  Restore selection and color state
            richTextBox.SelectionStart = lastSelection;
            richTextBox.SelectionLength = 0;
            richTextBox.SelectionColor = richTextBox.ForeColor;
        }
    }

    #endregion


    #region Enums

    /// <summary>
    /// The type of the token can be a special character like < / ? ! >
    /// or an escape sequence like &quote; or a comment like <!-- comment -->
    /// or an element, attribute, or value.
    /// </summary>
    public enum XmlTokenType
    {
        None,
        SpecialChar,
        Escape,
        Comment,
        Element,
        Attribute,
        Value
    }

    #endregion

    /// <summary>
    /// Represents an Xml token with a specific type, index in the parsed string
    /// and text.
    /// </summary>
    public class XmlToken
    {
        #region Instance Variables

        private string text;            //  Token Text
        private int index;              //  Token Index
        private XmlTokenType type;      //  Token Type

        #endregion

        #region Constructors

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public XmlToken() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text"></param>
        /// <param name="index"></param>
        /// <param name="type"></param>
        public XmlToken(string text, int index, XmlTokenType type)
        {
            this.text = text;
            this.index = index;
            this.type = type;
        }

        #endregion

        #region Properties

        public string Text
        {
            set
            {
                this.text = value;
            }
            get
            {
                return this.text;
            }
        }

        public int Index
        {
            set
            {
                this.index = value;
            }
            get
            {
                return this.index;
            }
        }

        public XmlTokenType Type
        {
            set
            {
                this.type = value;
            }
            get
            {
                return this.type;
            }
        }

        #endregion
    }

    /// <summary>
    /// Class responsible for splitting Xml text into tokens.
    /// </summary>
    public class XmlTokenizer
    {
        #region Public Static Methods

        /// <summary>
        /// Split the passed string into Xml tokens.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <remarks>Looping char by char is more efficient than regular expressions</remarks>
        public static List<XmlToken> Tokenize(string str)
        {
            return LoopTokenize(str);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Iterate over characters one by one to tokenize the Xml string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static List<XmlToken> LoopTokenize(string str)
        {
            //  Temp variables to build up the current token
            List<char> currentTokenText = new List<char>();

            //  Represents the list of tokens to be returned
            List<XmlToken> tokens = new List<XmlToken>();

            //  Represents the index of the first character in the token
            int tokenIndex = 0;

            bool isStartTag = false;
            bool isComment = false;
            bool isQuote = false;
            bool isAttribute = false;

            for (int i = 0; i < str.Length; i++)
            {
                //  Get the current character
                char c = str[i];

                //  Skip the "ZERO WIDTH NO-BREAK SPACE" character resulting from encoding
                if (c == 65279)
                {
                    continue;
                }

                //  Handle the escape sequence case
                if (c == '&')
                {
                    if (currentTokenText.Count > 0)
                    {
                        XmlToken token = new XmlToken();
                        token.Index = tokenIndex;
                        token.Text = new string(currentTokenText.ToArray());
                        tokens.Add(token);
                        currentTokenText.Clear();

                        //  Determine token type
                        if (isQuote)
                        {
                            token.Type = XmlTokenType.Value;
                        }
                        else if (isComment)
                        {
                            token.Type = XmlTokenType.Comment;
                        }
                        else if (isStartTag)
                        {
                            if (isAttribute)
                            {
                                token.Type = XmlTokenType.Attribute;
                            }
                            else
                            {
                                token.Type = XmlTokenType.Element;
                            }
                        }
                        else
                        {
                            token.Type = XmlTokenType.None;
                        }
                    }

                    currentTokenText.Add('&');

                    XmlToken escapeToken = new XmlToken();
                    escapeToken.Type = XmlTokenType.Escape;
                    escapeToken.Index = i;
                    i++;

                    while (i < str.Length && char.IsLetterOrDigit(str[i]))
                    {
                        currentTokenText.Add(str[i]);
                        i++;
                    }

                    if (i < str.Length && c == ';')
                    {
                        currentTokenText.Add(';');
                        i++;
                    }

                    escapeToken.Text = new string(currentTokenText.ToArray());
                    currentTokenText.Clear();
                    tokens.Add(escapeToken);
                    continue;
                }

                //  Only if the character is not between "" that is in a tag
                if (!isQuote)
                {
                    //  Only if the character is not in a comment
                    if (!isComment)
                    {
                        //  Only if we already have a start tag
                        if (isStartTag)
                        {
                            if (char.IsLetterOrDigit(c))
                            {
                                //  We're starting to build up a token, so save its index
                                if (currentTokenText.Count == 0)
                                {
                                    tokenIndex = i;
                                }

                                currentTokenText.Add(c);
                            }
                            else
                            {
                                //  Add the previous token that could be an element or an attribute
                                if (currentTokenText.Count > 0)
                                {
                                    XmlToken token = new XmlToken();
                                    token.Text = new string(currentTokenText.ToArray());
                                    currentTokenText.Clear();
                                    token.Index = tokenIndex;

                                    if (isAttribute)
                                    {
                                        token.Type = XmlTokenType.Attribute;
                                    }
                                    else
                                    {
                                        token.Type = XmlTokenType.Element;
                                    }

                                    tokens.Add(token);
                                }

                                //  Check if we have something like <!-- to flag that we have a comment
                                if (c == '-')
                                {
                                    if (i - 2 >= 0 && i + 1 < str.Length)
                                    {
                                        if (str[i - 2] == '<' && str[i - 1] == '!' && str[i + 1] == '-')
                                        {
                                            isStartTag = false;
                                            isComment = true;
                                            i += 1;
                                        }
                                    }
                                }
                                //  Check if our start tag is now closed
                                else if (c == '>')
                                {
                                    isStartTag = false;
                                    isAttribute = false;
                                }
                                //  We hit another start tag
                                else if (c == '<')
                                {
                                    isAttribute = false;
                                }
                                //  We're starting a quote
                                else if (c == '"')
                                {
                                    isQuote = true;
                                }

                                //  Check if we now have an attribute
                                if (char.IsWhiteSpace(c))
                                {
                                    isAttribute = true;
                                }
                                else
                                {
                                    tokens.Add(new XmlToken(c.ToString(), i, XmlTokenType.SpecialChar));
                                }
                            }
                        }
                        //  If we didn't have a start tag, check if we now have one
                        else
                        {
                            if (c == '<')
                            {
                                if (currentTokenText.Count > 0)
                                {
                                    XmlToken token = new XmlToken();
                                    token.Index = tokenIndex;
                                    token.Text = new string(currentTokenText.ToArray());
                                    token.Type = XmlTokenType.None;
                                    tokens.Add(token);
                                    currentTokenText.Clear();
                                }

                                isStartTag = true;
                                tokens.Add(new XmlToken("<", i, XmlTokenType.SpecialChar));
                            }
                            else
                            {
                                if (currentTokenText.Count == 0)
                                {
                                    tokenIndex = i;
                                }

                                currentTokenText.Add(c);
                            }
                        }
                    }
                    //  In case we have a comment
                    else
                    {
                        //  We're starting to build up a token, so save its index
                        if (currentTokenText.Count == 0)
                        {
                            tokenIndex = i;
                        }

                        currentTokenText.Add(c);

                        //  Check if we have something like --> to see if we're closing a comment
                        //  or if we're at the end
                        if (i + 2 < str.Length)
                        {
                            if (c == '-')
                            {
                                if (str[i + 1] == '-' && str[i + 2] == '>')
                                {
                                    isComment = false;
                                    i += 2;
                                }
                            }
                        }
                        else
                        {
                            isComment = false;
                        }

                        if (!isComment)
                        {
                            XmlToken token = new XmlToken();
                            token.Type = XmlTokenType.Comment;
                            token.Index = tokenIndex;
                            token.Text = new string(currentTokenText.ToArray());
                            tokens.Add(token);
                            currentTokenText.Clear();
                        }
                    }
                }
                //  In case we have a quote
                else
                {
                    //  We're starting to build up a token, so save its index
                    if (currentTokenText.Count == 0)
                    {
                        tokenIndex = i;
                    }

                    //  Check if we no longer have a quote
                    if (c == '"')
                    {
                        isQuote = false;
                        XmlToken token = new XmlToken();
                        token.Type = XmlTokenType.Value;
                        token.Index = tokenIndex;
                        token.Text = new string(currentTokenText.ToArray());
                        tokens.Add(token);
                        currentTokenText.Clear();
                    }
                    else
                    {
                        currentTokenText.Add(c);
                    }
                }
            }

            //  Handle the last element
            if (currentTokenText.Count > 0)
            {
                XmlToken token = new XmlToken();
                token.Index = tokenIndex;
                token.Text = new string(currentTokenText.ToArray());
                tokens.Add(token);
                currentTokenText.Clear();

                //  Determine token type
                if (isQuote)
                {
                    token.Type = XmlTokenType.Value;
                }
                else if (isComment)
                {
                    token.Type = XmlTokenType.Comment;
                }
                else if (isStartTag)
                {
                    if (isAttribute)
                    {
                        token.Type = XmlTokenType.Attribute;
                    }
                    else
                    {
                        token.Type = XmlTokenType.Element;
                    }
                }
                else
                {
                    token.Type = XmlTokenType.None;
                }
            }

            return tokens;
        }

        #endregion
    }
}
