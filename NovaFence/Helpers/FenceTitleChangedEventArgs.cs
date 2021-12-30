using System;

namespace NovaFence.Helpers
{
    internal class FenceTitleChangedEventArgs : EventArgs
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }
}