using System;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SlotLogic.MobileAppWebService.Models.Enum;
using SlotLogic.MobileAppWebService.Models.InputData;

namespace SlotLogic.MobileAppWebService
{
    public class Service
    {
        private ILogger logger { get; set; }
        private IConfiguration configuration { get; set; }

        public Service(IConfiguration configuration, ILogger logger)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public String GetMessageError(Int32? errorId, TypeError type)
        {
            String message = configuration.GetSection($"{type.ToString()}:{errorId}").Value;

            if (String.IsNullOrEmpty(message))
            {
                message = $"Unhandled error. {errorId}";
            }

            return message;
        }

        public String GetConnectionStringByClubId(String clubID)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();

            try
            {
                configuration.GetSection($"Clubs:{clubID}").Bind(sqlConnectionStringBuilder);

                if (String.IsNullOrEmpty(sqlConnectionStringBuilder.ConnectionString))
                {
                    logger.LogError(GetMessageError(1007, TypeError.User));

                    return null;
                }
            }
            catch (Exception exc)
            {
                logger.LogError(exc, GetMessageError(1004, TypeError.Inner));

                return String.Empty;
            }

            return sqlConnectionStringBuilder.ConnectionString;
        }

        public bool IsAnyNullOrEmpty(object myObject)
        {
            if (myObject == null)
                return true;
            try
            {
                foreach (PropertyInfo pi in myObject.GetType().GetProperties())
                {
                    if (pi.PropertyType == typeof(string))
                    {
                        string value = (string)pi.GetValue(myObject);
                        if (string.IsNullOrEmpty(value))
                        {
                            return true;
                        }
                    }
                    else if (pi.PropertyType == typeof(Int32?))
                    {
                        Int32? value = (Int32?)pi.GetValue(myObject);
                        if (value == null)
                        {
                            return true;
                        }
                    }
                    else if (pi.PropertyType == typeof(Double?))
                    {
                        Double? value = (Double?)pi.GetValue(myObject);
                        if (value == null)
                        {
                            return true;
                        }
                    }
                    else if (pi.PropertyType == typeof(Decimal?))
                    {
                        Decimal? value = (Decimal?)pi.GetValue(myObject);
                        if (value == null)
                        {
                            return true;
                        }
                    }
                    else if (pi.PropertyType.IsClass)
                    {
                        if (IsAnyNullOrEmpty(pi.GetValue(myObject)))
                            return true;
                    }
                }
            } catch(Exception exc)
            {
                logger.LogError(exc, GetMessageError(1028, TypeError.Inner));
                return true;
            }
            return false;
        }
    }
}
