﻿using ModernMail.Core.Encoding;
using System.Net.Mail;

namespace ModernMail.Core.Model
{
    public class MailHeader
    {
        public MailHeader(string key, string value)
        {
            this.Key = key;
            this.Value = EncodePrintable(key) ? QuotedPrintable.Inline(value) : value;
        }

        public MailHeader(string key, MailAddress value)
        {
            this.Key = key;
            this.Value = GetAddressString(value);
        }

        public MailHeader(string key, MailAddressCollection value)
        {
            this.Key = key;
            this.Value = "";
            for (int i = 0; i < value.Count; i++)
                this.Value += ((i > 0 ? ", " : "") + GetAddressString(value[i]));
        }

        public string Key { get; private set; }

        public string Value { get; private set; }

        public bool FoldedValue
        {
            get { return Value.Contains(Keyword.CRLF); }
        }

        internal void Append(string value)
        {
            this.Value += value;
        }

        private bool EncodePrintable(string key)
        {
            return HeaderName.Subject == key;
        }

        private string GetAddressString(MailAddress addr)
        {
            var dn = addr.DisplayName;
            dn = (string.IsNullOrWhiteSpace(dn) ? "" :
                QuotedPrintable.Inline(dn) + " ");
            return dn + "<" + addr.Address + ">";
        }
    }
}
