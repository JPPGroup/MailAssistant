using System;

namespace Jpp.AddIn.MailAssistant.Abstracts
{
    internal interface IWrappedObject : IDisposable
    {
        Type InnerObjectType { get; }
    }
}
