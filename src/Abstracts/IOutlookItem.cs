namespace Jpp.AddIn.MailAssistant.Abstracts
{
    internal interface IOutlookItem : IWrappedObject
    {
        string Id { get; }
        string Description { get; }
        string Subject { get; }
        string Sender { get; }
        string Folder { get; }
        int Size { get; }
    } 
}
