using System;
using System.Collections.Generic;
using System.Threading;

namespace EmailSenderLib
{
    public class NotificationManager
    {
        public static void SendNotification(NotificationTypes notificationType, NotificationApplications application, System.String eventCode,
                                     params System.Object[] parameters)
        {
            IList<Recipient> recipients = Service.GetRecipients(eventCode);
            if (recipients.Count == 0)
            {
                return;
            }

            IDictionary<System.Int32, System.String> eventStrings = Service.GetEventStrings(application, eventCode);

            foreach (Recipient recipient in recipients)
            {
                if (recipient.EMail != "mail.evgeny.filippov@gmail.com")
                {
                    continue;
                }

                System.String text;
                if (eventStrings.ContainsKey(recipient.LanguageId))
                {
                    text = eventStrings[recipient.LanguageId];
                }
                else if (eventStrings.ContainsKey(1))
                {
                    text = eventStrings[1];
                }
                else
                {
                    text = eventCode;
                }

                text = System.String.Format(text, parameters);

                try
                {
                    new Thread(() => EmailService.PreparationDataForSending(recipient, notificationType, text, application, eventCode)).Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SendNotification error - {ex}");
                }
            }
        }
    }
}
