using System;
using Microsoft.Win32;

namespace Jpp.AddIn.MailAssistant
{
    public static class UserSettings
    {
        public static bool IsDialogSnoozed()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\JPP Consulting\\MailAssistant"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("ReportSnooze");
                        if (o != null)
                        {
                            DateTime time = DateTime.Parse(o as string);
                            if ((DateTime.Now - time).TotalMinutes < 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)  
            {
                //TODO: Add logging for this
            }

            return false;
        }
        
        public static void SnoozeDialogUntil(DateTime dateTime, bool autoDelete)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\JPP Consulting\\MailAssistant"))
                {
                    string date = dateTime.ToShortDateString();
                    key.SetValue("ReportSnooze", date, RegistryValueKind.String);
                    key.SetValue("AutoDelete", autoDelete, RegistryValueKind.DWord);
                }
            }
            catch (Exception ex)  
            {
                //TODO: Add logging for this
            }
        }

        public static bool IsAutoDelete()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\JPP Consulting\\MailAssistant"))
                {
                    if (key != null)
                    {
                        return Convert.ToBoolean(key.GetValue("AutoDelete"));
                    }
                    
                }
            }
            catch (Exception ex)
            {
                //TODO: Add logging for this
            }

            return false;
        }
    }
}
