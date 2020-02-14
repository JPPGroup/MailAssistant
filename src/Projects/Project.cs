using System;

namespace Jpp.AddIn.MailAssistant.Projects
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Office { get; set; }
        public string Lead { get; set; }
        public string Discipline { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string ExternalId { get; set; }
        public string Grouping { get; set; }
        public string Folder { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}
