using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmailSender
{
    public class EmailService
    {
        private const String URL = "http://v.te.admin.slotlogic.eu/send_mail.php";

        static LoggerNotification log = new LoggerNotification();

        public static void PreparationDataForSending(Recipient recipient, NotificationTypes notificationType, String text, NotificationApplications application, String eventCode)
        {
            log.Log.Info($"{recipient.EMail}, {notificationType.ToString()} {application.ToString()}, {eventCode}");

            if (string.IsNullOrEmpty(recipient.EMail))
            {
                log.Log.Warn("EMail is empty");
                return;
            }

            Random rnd = new Random();

            EmailData emailData = new EmailData()
            {
                R1 = rnd.Next(1000000, 9999999),
                Sender = recipient.EMail,
                Subject = GetSubject((Int32)application, eventCode),
                Text = text,
                R2 = rnd.Next(1000000, 9999999)
            };

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            String finalText = string.Empty;
            try
            {
                /*
                 We convert the emailData object into JSON, JSON into an array of bytes, 
                 and already this array of bytes is passed to the EncryptStringToBytes_Aes function.
                */
                byte[] encryptedByte = AesEncryption.EncryptStringToBytes_Aes(JsonSerializer.SerializeToUtf8Bytes(emailData, options));
                finalText = Convert.ToBase64String(encryptedByte);
            }
            catch (Exception ex)
            {
                log.Log.Error(ex, $"Failed to use AES encryption. Message - {ex}");
                return;
            }

            Task<String> tasks = RequestSendingEmail(finalText);

            Int32? code = ProcessingResult(tasks.Result);
            if (code != null)
            {
                try
                {
                    // Add the response to the current request for sending to the log table
                    // If the code is 1 (successful), then add the current notification to the table with the "sent" flag.
                    // This is necessary so as not to break the current connectivity of notification events, since many 
                    // notifications are checked for the sending frequency from this table.
                    Service.WriteResponseToLogNtfTable(recipient.Id, (Int32)notificationType, text, (Int32)code, application.ToString());
                }
                catch (Exception ex)
                {
                    log.Log.Error(ex, "Failed to write result to table.");
                }
            }

        }

        // Create an Email Header.
        private static String GetSubject(Int32 applicationId, String eventCode)
        {
            String clubName = Service.GetClubName() ?? "Uknown Club";
            String code = Service.GetEventCode(applicationId, eventCode) ?? "Unknown code";

            return $"{(String.IsNullOrEmpty(clubName) ? "Club" : clubName)}: {code}";
        }

        private static Int32? ProcessingResult(String result)
        {
            if (string.IsNullOrEmpty(result))
            {
                log.Log.Info("The answer came empty.");
                return null;
            }

            if (Int32.TryParse(result, out Int32 code))
            {
                return code;
            }
            else
            {
                log.Log.Warn("Failed to parse response from server.");
                return null;
            }
        }

        public static async Task<String> RequestSendingEmail(String base64Text)
        {
            if (String.IsNullOrEmpty(base64Text))
            {
                log.Log.Warn("base64Text is Empty");
                return string.Empty;
            }

            String uid = "lrvDecDwjkPrAZ5x"; //Service.GetLicenseUid();

            if (String.IsNullOrEmpty(uid))
            {
                log.Log.Warn("Uid is Empty");
                return string.Empty;
            }

            // The base64 text and club id values are passed as a function parameter.
            var parameters = new Dictionary<string, string>
            {
                { "data", base64Text }, { "uid", uid }
            };

            var encodedContent = new FormUrlEncodedContent(parameters);

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(URL, encodedContent);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        log.Log.Info($"HttpStatusCode is not OK. Code - {response.StatusCode}");
                        return string.Empty;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                log.Log.Error(ex, $"No response from server.");
                return string.Empty;
            }
        }
    }

    public class Recipient
    {
        public System.Int32 Id { get; set; }
        public System.String Name { get; set; }
        public System.String PhoneNumber { get; set; }
        public System.String EMail { get; set; }
        public System.String DNSName { get; set; }
        public System.Int32 LanguageId { get; set; }
    }

    public enum NotificationTypes
    {
        Jackpots = 1,
        HardwareProblem = 2,
        ProgramExceptionInApplication = 3,
        BreachOfRegulations = 4,
        NetworkProblem = 5,
        Handpays = 6,
        Administrative = 7,
        ReplicationProblem = 8,
        CertificateProblem = 9,
        CertificateWarning = 10,
        SmibProblem = 11
    }

    public enum NotificationApplications
    {
        SlotService = 1,
        JackpotService = 2,
        GSMModemService = 3,
        BeOwner = 4,
        VideoServer = 5
    }
}
