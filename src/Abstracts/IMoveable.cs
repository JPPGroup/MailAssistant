using System;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jpp.AddIn.MailAssistant.Abstracts
{
    internal interface IMoveable : IOutlookItem, IEquatable<IMoveable>
    {
        bool Move(Outlook.Folder folder);
        string RestrictCriteria { get; }
    }
}
