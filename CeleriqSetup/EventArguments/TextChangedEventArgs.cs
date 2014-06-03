using System;

namespace CeleriqSetup.EventArguments
{
    public class TextChangedEventArgs : EventArgs
    {
        public TextChangedEventArgs()
        {
        }

        public TextChangedEventArgs(string text)
            : this()
        {
            this.Text = text;
        }

        public string Text { get; set; }
    }
}
