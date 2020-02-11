using Jpp.AddIn.MailAssistant.Abstracts;
using Jpp.AddIn.MailAssistant.Exceptions;
using Jpp.AddIn.MailAssistant.Wrappers;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jpp.AddIn.MailAssistant.Factories
{
    internal static class OutlookItemFactory
    {
        /*
         * All known Outlook item types
         *
         * https://docs.microsoft.com/en-us/office/vba/outlook/how-to/items-folders-and-stores/outlook-item-objects
         *
         */
        public static IOutlookItem Create(dynamic item)
        {
            switch (item)
            {
                case Outlook.MailItem mailItem:
                    return new MailItemWrapper(mailItem);
                case Outlook.MeetingItem meetingItem:
                    return new MeetingItemWrapper(meetingItem);
                case Outlook.ReportItem reportItem:
                    return new ReportItemWrapper(reportItem);
                case Outlook.AppointmentItem appointmentItem:
                    return new AppointmentItemWrapper(appointmentItem);
                case Outlook.ContactItem contactItem:
                    return new ContactItemWrapper(contactItem);
                case Outlook.DistListItem distListItem:
                    return new DistListItemWrapper(distListItem);
                case Outlook.DocumentItem documentItem:
                    return new DocumentItemWrapper(documentItem);
                case Outlook.JournalItem journalItem:
                    return new JournalItemWrapper(journalItem);
                case Outlook.NoteItem noteItem:
                    return new NoteItemWrapper(noteItem);
                case Outlook.PostItem postItem:
                    return new PostItemWrapper(postItem);
                case Outlook.RemoteItem remoteItem:
                    return new RemoteItemWrapper(remoteItem);
                case Outlook.SharingItem sharingItem:
                    return new SharingItemWrapper(sharingItem);
                case Outlook.StorageItem storageItem:
                    return new StorageItemWrapper(storageItem);
                case Outlook.TaskItem taskItem:
                    return new TaskItemWrapper(taskItem);
                case Outlook.TaskRequestAcceptItem taskRequestAcceptItem:
                    return new TaskRequestAcceptItemWrapper(taskRequestAcceptItem);
                case Outlook.TaskRequestDeclineItem taskRequestDeclineItem:
                    return new TaskRequestDeclineItemWrapper(taskRequestDeclineItem);
                case Outlook.TaskRequestItem taskRequestItem:
                    return new TaskRequestItemWrapper(taskRequestItem);
                case Outlook.TaskRequestUpdateItem taskRequestUpdateItem:
                    return new TaskRequestUpdateItemWrapper(taskRequestUpdateItem);
                default:
                    throw new OutlookItemFactoryException(@"Outlook item type not handled", nameof(item));
            }
        }
    }
}

