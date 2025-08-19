namespace com.escapinghats.btl.logic.view
{
    using System;

    public class ViewEventArgs : EventArgs
    {
        public ViewEventTypes Type { get; set; }
        public string Message { get; set; }

        public ViewEventArgs(ViewEventTypes type, string message)
        {
            Type = type;
            Message = message;
        }
    }

    public enum ViewEventTypes
    {
        ViewLoaded,
        ViewError,
        ViewDone,
        ViewUnloaded
    }
}

