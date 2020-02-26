namespace Jpp.AddIn.MailAssistant.OutputReports
{
    public class ItemProperties
    {
        public string Description { get; }
        public string Source { get; }
        public string Destination { get; }
        public ItemStatus Status { get; set; } = ItemStatus.NotSet;

        public ItemProperties(string description, string source, string destination)
        {
            Description = description;
            Source = source;
            Destination = destination;
        }
    }
}
