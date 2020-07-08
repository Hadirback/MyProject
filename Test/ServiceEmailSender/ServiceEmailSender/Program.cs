using EmailSenderLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceEmailSender
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                NotificationManager.SendNotification(NotificationTypes.CertificateProblem, NotificationApplications.SlotService, "CERTIFICATE_HAS_EXPIRED", "20-03-2020", "Current club");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Main error - {ex}");
            }
            Console.ReadLine();
        }
    }
}
