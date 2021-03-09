using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        
        public static void SnoozeDialogUntil(DateTime dateTime)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\JPP Consulting\\MailAssistant"))
                {
                    string date = dateTime.ToShortDateString();
                    key.SetValue(date, RegistryValueKind.String);
                }
            }
            catch (Exception ex)  
            {
                //TODO: Add logging for this
            }
        }
    }
}
