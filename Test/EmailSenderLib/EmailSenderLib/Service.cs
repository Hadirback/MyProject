using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EmailSenderLib
{
    public class Service
    {
        private static String CONN_STRING;

        static Service()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connection"];
            CONN_STRING = settings.ConnectionString;
        }

        public static void WriteResponseToLogNtfTable(Int32 recipientId, Int32 notificationTypeId, String text, Int32 code, String applicationName)
        {
            SqlParameter recipientIdParam = CreateParameter("@recipient_id", SqlDbType.Int, recipientId);
            SqlParameter notificationTypeIdParam = CreateParameter("@notification_type_id", SqlDbType.Int, notificationTypeId);
            SqlParameter textParam = CreateParameter("@text", SqlDbType.NVarChar, text);
            SqlParameter codeParam = CreateParameter("@code", SqlDbType.Int, code);
            SqlParameter applicationNameParam = CreateParameter("@application_name", SqlDbType.NVarChar, applicationName);

            using (SqlConnection connection = new SqlConnection(CONN_STRING))
            {
                SqlCommand command = new SqlCommand(Queries.WRITE_RESPONCE_TO_LOG_NTF_TABLE, connection);
                command.Parameters.Add(recipientIdParam);
                command.Parameters.Add(notificationTypeIdParam);
                command.Parameters.Add(textParam);
                command.Parameters.Add(codeParam);
                command.Parameters.Add(applicationNameParam);

                connection.Open();
                int number = command.ExecuteNonQuery();
                Console.WriteLine("Обновлено объектов: {0}", number);
            }
        }

        public static String GetClubName()
        {
            using (SqlConnection connection = new SqlConnection(CONN_STRING))
            {
                SqlCommand command = new SqlCommand(Queries.SELECT_CURRENT_SLOT_CLUB_NAME, connection);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.Read())
                        {
                            return Convert.ToString(reader["name"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception GetClubName - {ex}");
                    }
                }
                return string.Empty;
            }
        }

        public static String GetEventCode(Int32 applicationId, String codeApp)
        {
            SqlParameter applicationIdParam = CreateParameter("@application_id", SqlDbType.Int, applicationId);
            SqlParameter codeAppParam = CreateParameter("@code_app", SqlDbType.NVarChar, codeApp);

            using (SqlConnection connection = new SqlConnection(CONN_STRING))
            {
                SqlCommand command = new SqlCommand(Queries.SELECT_EVENT_CODE, connection);
                command.Parameters.Add(applicationIdParam);
                command.Parameters.Add(codeAppParam);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.Read())
                        {
                            return Convert.ToString(reader["code"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception GetEventCode - {ex}");
                    }
                }
                return String.Empty;
            }
        }

        public static String GetLicenseUid()
        {
            using (SqlConnection connection = new SqlConnection(CONN_STRING))
            {
                SqlCommand command = new SqlCommand(Queries.SELECT_LICENSE_UID, connection);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.Read())
                        {
                            return Convert.ToString(reader["value"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception GetLicenseUid - {ex}");
                    }
                }
                return string.Empty;
            }
        }

        public static IList<Recipient> GetRecipients(String eventCode)
        {
            SqlParameter eventCodeParam = CreateParameter("@event_code", SqlDbType.NVarChar, eventCode);
            IList<Recipient> list = new List<Recipient>();

            using (SqlConnection connection = new SqlConnection(CONN_STRING))
            {
                SqlCommand command = new SqlCommand(Queries.GET_RECIPIENTS_BY_EVENT_CODE, connection);
                command.Parameters.Add(eventCodeParam);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            list.Add(new Recipient()
                            {
                                Id = (Int32)reader["id"],
                                Name = (String)reader["name"],
                                PhoneNumber = reader["phone_number"] != DBNull.Value ? (String)reader["phone_number"] : "",
                                EMail = reader["email"] != DBNull.Value ? (String)reader["email"] : "",
                                DNSName = reader["dns_name"] != DBNull.Value ? (String)reader["dns_name"] : "",
                                LanguageId = reader["language_id"] != DBNull.Value ? (Int32)reader["language_id"] : 1
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception GetRecipients - {ex}");
                    }
                }
                return list;
            }
        }

        public static IDictionary<Int32, String> GetEventStrings(NotificationApplications application, String eventCode)
        {
            IDictionary<Int32, String> eventStrings = new Dictionary<Int32, String>();
            SqlParameter appIdParam = CreateParameter("@application_id", SqlDbType.Int, (Int32)application);
            SqlParameter codeAppParam = CreateParameter("@code_app", SqlDbType.NVarChar, eventCode);

            using (SqlConnection connection = new SqlConnection(CONN_STRING))
            {
                SqlCommand command = new SqlCommand(Queries.GET_EVENT_STRINGS, connection);

                command.Parameters.Add(appIdParam);
                command.Parameters.Add(codeAppParam);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            Int32 languageId = (Int32)reader["language_id"];
                            String text = (String)reader["text"];

                            if (!eventStrings.ContainsKey(languageId))
                            {
                                eventStrings.Add(languageId, text);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception GetEventStrings - {ex}");
                    }
                }
                return eventStrings;
            }
        }

        public static SqlParameter CreateParameter(String name, SqlDbType type, Object value)
        {
            SqlParameter result = new SqlParameter(name, type);
            result.Value = value ?? DBNull.Value;
            return result;
        }
    }
}
