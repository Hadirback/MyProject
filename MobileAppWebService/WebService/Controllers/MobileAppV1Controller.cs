using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SlotLogic.MobileAppWebService.Models;
using SlotLogic.MobileAppWebService.Models.Common;
using SlotLogic.MobileAppWebService.Models.Enum;
using SlotLogic.MobileAppWebService.Models.InputData;

namespace SlotLogic.MobileAppWebService.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class MobileAppV1Controller : Controller
    {
        private ILogger<MobileAppV1Controller> logger { get; set; }
        private Service service { get; set; }

        public MobileAppV1Controller(IConfiguration configuration, ILogger<MobileAppV1Controller> logger)
        {
            this.logger = logger;
            service = new Service(configuration, logger);
        }

        [Route("[action]")]
        [HttpPost]
        public JsonResult LogInSystem(AuthenticationData user)
        {
            Package<PlayerBalancesInfo> package;

            try
            {
                if (service.IsAnyNullOrEmpty(user))
                {
                    package = new Package<PlayerBalancesInfo>(Status.Failed, new PlayerBalancesInfo());
                    logger.LogWarning(service.GetMessageError(1001, TypeError.Common));
                    return new JsonResult(package);
                }

                PlayerBalancesInfo playerBalancesInfo = new PlayerBalancesInfo();
                String connectionString = service.GetConnectionStringByClubId(user.ClubID);

                if (String.IsNullOrEmpty(connectionString))
                {
                    package = new Package<PlayerBalancesInfo>(Status.Failed, new PlayerBalancesInfo(), 1007, TypeError.User);
                    return new JsonResult(package);
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException ex)
                    {
                        package = new Package<PlayerBalancesInfo>(Status.Failed, new PlayerBalancesInfo(), ex.Number, TypeError.Connection);
                        logger.LogError(ex, service.GetMessageError(ex.Number, TypeError.Connection));
                        return new JsonResult(package);
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "mobile_app_v1_get_player_balances";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@v_card_series", user.Card.CardSeries);
                        command.Parameters.AddWithValue("@v_card_company", user.Card.CardCompany);
                        command.Parameters.AddWithValue("@v_card_number", user.Card.CardNumber);
                        command.Parameters.AddWithValue("@v_card_password", user.Password);

                        SqlParameter returnParameter = command.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        try
                        {
                            using (SqlDataReader dataReader = command.ExecuteReader())
                            {
                                Int32? result = (Int32?)returnParameter.Value;
                                if (!result.HasValue)
                                {
                                    while (dataReader.Read())
                                    {
                                        Int32? flag = !dataReader.IsDBNull(0) ? (Int32?)dataReader.GetInt32(0) : null;

                                        if (!flag.HasValue || flag == 0)
                                            playerBalancesInfo.CashbackActive = false;                      
                                        else
                                            playerBalancesInfo.CashbackActive = true;
                                        

                                        playerBalancesInfo.PlayerFirstName = dataReader.GetString(1);
                                        playerBalancesInfo.PlayerSurname = dataReader.GetString(2);
                                        playerBalancesInfo.PlayerStatus = dataReader.GetString(3);
                                        playerBalancesInfo.PtsBalance = !dataReader.IsDBNull(4) ? (Int32?)dataReader.GetInt32(4) : null;
                                        playerBalancesInfo.CashbackAmount = !dataReader.IsDBNull(5) ? (Decimal?)dataReader.GetDecimal(5) : null;
                                        playerBalancesInfo.Balance = !dataReader.IsDBNull(6) ? (Decimal?)dataReader.GetDecimal(6) : null;

                                        if (!playerBalancesInfo.PtsBalance.HasValue)
                                        {
                                            package = new Package<PlayerBalancesInfo>(Status.Failed, new PlayerBalancesInfo());
                                            logger.LogWarning(service.GetMessageError(1002, TypeError.Inner));
                                            return new JsonResult(package);
                                        }

                                        if (!playerBalancesInfo.CashbackAmount.HasValue)
                                        {
                                            package = new Package<PlayerBalancesInfo>(Status.Failed, new PlayerBalancesInfo());
                                            logger.LogWarning(service.GetMessageError(1003, TypeError.Inner));
                                            return new JsonResult(package);
                                        }

                                        if (!playerBalancesInfo.Balance.HasValue)
                                        {
                                            package = new Package<PlayerBalancesInfo>(Status.Failed, new PlayerBalancesInfo());
                                            logger.LogWarning(service.GetMessageError(1017, TypeError.Inner));
                                            return new JsonResult(package);
                                        }
                                    }
                                }
                                else
                                {
                                    package = new Package<PlayerBalancesInfo>(Status.Failed, new PlayerBalancesInfo(), result.Value, TypeError.User);
                                    logger.LogWarning(service.GetMessageError(result, TypeError.User));
                                    return new JsonResult(package);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            package = new Package<PlayerBalancesInfo>(Status.Failed, new PlayerBalancesInfo());
                            logger.LogError(ex, service.GetMessageError(1012, TypeError.Inner));
                            return new JsonResult(package);
                        }
                    }
                }

                if (String.IsNullOrEmpty(playerBalancesInfo.PlayerFirstName) && String.IsNullOrEmpty(playerBalancesInfo.PlayerSurname))
                {
                    package = new Package<PlayerBalancesInfo>(Status.Failed, new PlayerBalancesInfo());
                    logger.LogWarning(service.GetMessageError(1004, TypeError.Common));
                    return new JsonResult(package);
                }

                package = new Package<PlayerBalancesInfo>(Status.Succeed, playerBalancesInfo);
                return new JsonResult(package);
            }
            catch (Exception ex)
            {
                package = new Package<PlayerBalancesInfo>(Status.Failed, new PlayerBalancesInfo());
                logger.LogError(ex, service.GetMessageError(1001, TypeError.Inner));
                return new JsonResult(package);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public JsonResult GetDrawInfo(AuthenticationData user)
        {
            Package<List<DrawInfo>> package;

            try
            {
                if (service.IsAnyNullOrEmpty(user))
                {
                    package = new Package<List<DrawInfo>>(Status.Failed, new List<DrawInfo>());
                    logger.LogWarning(service.GetMessageError(1001, TypeError.Common));
                    return new JsonResult(package);
                }

                List<DrawInfo> drawsList = new List<DrawInfo>();
                String connectionString = service.GetConnectionStringByClubId(user.ClubID);

                if (String.IsNullOrEmpty(connectionString))
                {
                    package = new Package<List<DrawInfo>>(Status.Failed, new List<DrawInfo>(), 1007, TypeError.User);
                    return new JsonResult(package);
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException ex)
                    {
                        package = new Package<List<DrawInfo>>(Status.Failed, new List<DrawInfo>(), ex.Number, TypeError.Connection);
                        logger.LogError(ex, service.GetMessageError(ex.Number, TypeError.Connection));
                        return new JsonResult(package);
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "mobile_app_v1_get_upcoming_draws";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@v_card_series", user.Card.CardSeries);
                        command.Parameters.AddWithValue("@v_card_company", user.Card.CardCompany);
                        command.Parameters.AddWithValue("@v_card_number", user.Card.CardNumber);
                        command.Parameters.AddWithValue("@v_card_password", user.Password);

                        SqlParameter returnParameter = command.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        try
                        {
                            using (SqlDataReader dataReader = command.ExecuteReader())
                            {
                                Int32? result = (Int32?)returnParameter.Value;

                                if (!result.HasValue)
                                {
                                    while (dataReader.Read())
                                    {
                                        drawsList.Add(new DrawInfo()
                                        {
                                            DrawName = dataReader.GetString(0),
                                            DrawStartDate = !dataReader.IsDBNull(1) ? dataReader.GetDateTime(1).ToString("yyyy-MM-dd") : String.Empty,
                                            DrawEndDate = !dataReader.IsDBNull(2) ? dataReader.GetDateTime(2).ToString("yyyy-MM-dd") : String.Empty,
                                            DrawFinalDate = !dataReader.IsDBNull(3) ? dataReader.GetDateTime(3).ToString("yyyy-MM-dd") : String.Empty,
                                            BonusesTill = !dataReader.IsDBNull(4) ? dataReader.GetDateTime(4).ToString("yyyy-MM-dd") : String.Empty,
                                            BonusesForPeriod = !dataReader.IsDBNull(5) ? (Int32?)dataReader.GetInt32(5) : null,
                                            BonusesForToday = !dataReader.IsDBNull(6) ? (Int32?)dataReader.GetInt32(6) : null,
                                            Coupons = !dataReader.IsDBNull(7) ? (Int32?)dataReader.GetInt32(7) : null
                                        });

                                    }
                                }
                                else
                                {
                                    package = new Package<List<DrawInfo>>(Status.Failed, new List<DrawInfo>(), result.Value, TypeError.User);
                                    logger.LogWarning(service.GetMessageError(result, TypeError.User));
                                    return new JsonResult(package);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            package = new Package<List<DrawInfo>>(Status.Failed, new List<DrawInfo>());
                            logger.LogError(ex, service.GetMessageError(1007, TypeError.Inner));
                            return new JsonResult(package);
                        }
                    }
                }

                package = new Package<List<DrawInfo>>(Status.Succeed, drawsList);
                return new JsonResult(package);
            }
            catch (Exception ex)
            {
                package = new Package<List<DrawInfo>>(Status.Failed, new List<DrawInfo>());
                logger.LogError(ex, service.GetMessageError(1008, TypeError.Inner));
                return new JsonResult(package);
            }
        }

        [Route("[action]/{clubID}/")]
        [HttpGet]
        public JsonResult GetAboutClubInfo(String clubID)
        {
            Package<ClubInfo> package;

            try
            {
                if (String.IsNullOrEmpty(clubID))
                {
                    package = new Package<ClubInfo>(Status.Failed, new ClubInfo());
                    logger.LogWarning(service.GetMessageError(1005, TypeError.Common));
                    return new JsonResult(package);
                }

                String connectionString = service.GetConnectionStringByClubId(clubID);

                if (String.IsNullOrEmpty(connectionString))
                {
                    package = new Package<ClubInfo>(Status.Failed, new ClubInfo(), 1007, TypeError.User);
                    return new JsonResult(package);
                }

                ClubInfo result = new ClubInfo();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException ex)
                    {
                        package = new Package<ClubInfo>(Status.Failed, new ClubInfo(), ex.Number, TypeError.Connection);
                        logger.LogError(ex, service.GetMessageError(ex.Number, TypeError.Connection));
                        return new JsonResult(package);
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "mobile_app_v1_get_club_info";
                        command.CommandType = CommandType.StoredProcedure;

                        try
                        {
                            using (SqlDataReader dataReader = command.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result = new ClubInfo
                                    {
                                        ClubName = dataReader.GetString(0),
                                        AboutClub = dataReader.GetString(1)
                                    };
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            package = new Package<ClubInfo>(Status.Failed, new ClubInfo());
                            logger.LogError(ex, service.GetMessageError(1006, TypeError.Inner));
                            return new JsonResult(package);
                        }
                    }
                }

                if (String.IsNullOrEmpty(result.AboutClub))
                {
                    package = new Package<ClubInfo>(Status.Failed, new ClubInfo());
                    logger.LogWarning(service.GetMessageError(1002, TypeError.Common));
                    return new JsonResult(package);
                }

                if (String.IsNullOrEmpty(result.ClubName))
                {
                    package = new Package<ClubInfo>(Status.Failed, new ClubInfo());
                    logger.LogWarning(service.GetMessageError(1003, TypeError.Common));
                    return new JsonResult(package);
                }

                package = new Package<ClubInfo>(Status.Succeed, result);
                return new JsonResult(package);
            }
            catch (Exception ex)
            {
                package = new Package<ClubInfo>(Status.Failed, new ClubInfo());
                logger.LogError(ex, service.GetMessageError(1009, TypeError.Inner));
                return new JsonResult(package);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult WriteError(Token token)
        {
            try
            {
                if (!String.IsNullOrEmpty(token.Comment))
                {
                    logger.LogError($"{token.Message} - {token.DateTime}. Additional Information: {token.Comment}");
                }
                else
                {
                    logger.LogError(token.Message);
                }
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }

            return new OkResult();
        }

        [Route("[action]")]
        [HttpPost]
        public JsonResult SetCashbackActivate(AuthenticationData user)
        {
            Package<String> package;

            try
            {
                if (service.IsAnyNullOrEmpty(user))
                {
                    package = new Package<String>(Status.Failed, String.Empty);
                    logger.LogWarning(service.GetMessageError(1001, TypeError.Common));

                    return new JsonResult(package);
                }

                String connectionString = service.GetConnectionStringByClubId(user.ClubID);

                if (String.IsNullOrEmpty(connectionString))
                {
                    package = new Package<String>(Status.Failed, String.Empty, 1007, TypeError.User);
                    return new JsonResult(package);
                }

                String isActive = String.Empty;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException ex)
                    {
                        package = new Package<String>(Status.Failed, String.Empty, ex.Number, TypeError.Connection);
                        logger.LogError(ex, service.GetMessageError(ex.Number, TypeError.Connection));
                        return new JsonResult(package);
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "mobile_app_v1_activate_cashback";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@v_card_series", user.Card.CardSeries);
                        command.Parameters.AddWithValue("@v_card_company", user.Card.CardCompany);
                        command.Parameters.AddWithValue("@v_card_number", user.Card.CardNumber);
                        command.Parameters.AddWithValue("@v_card_password", user.Password);

                        SqlParameter returnParameter = command.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        try
                        {
                            isActive = (String)command.ExecuteScalar();

                            Int32? result = (Int32?)returnParameter.Value;
                            if (result.HasValue && result != 0)
                            {
                                package = new Package<String>(Status.Failed, String.Empty, result.Value, TypeError.User);
                                logger.LogWarning(service.GetMessageError(result, TypeError.User));
                                return new JsonResult(package);
                            }
                        }
                        catch (Exception ex)
                        {
                            package = new Package<String>(Status.Failed, String.Empty);
                            logger.LogError(ex, service.GetMessageError(1010, TypeError.Inner));
                            return new JsonResult(package);
                        }
                    }
                }

                if (String.IsNullOrEmpty(isActive))
                {
                    package = new Package<String>(Status.Failed, String.Empty);
                    logger.LogWarning(service.GetMessageError(1006, TypeError.Common));

                    return new JsonResult(package);
                }

                package = new Package<String>(Status.Succeed, isActive);
                return new JsonResult(package);
            }
            catch (Exception ex)
            {
                package = new Package<String>(Status.Failed, String.Empty);
                logger.LogError(ex, service.GetMessageError(1011, TypeError.Inner));
                return new JsonResult(package);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public JsonResult AddCommandPutPaymentToGm(PaymentData values)
        {
            Package<Int32?> package;

            try
            {
                if (service.IsAnyNullOrEmpty(values))
                {
                    package = new Package<Int32?>(Status.Failed, null);
                    logger.LogWarning(service.GetMessageError(1001, TypeError.Common));
                    return new JsonResult(package);
                }

                String connectionString = service.GetConnectionStringByClubId(values.AuthenticationData.ClubID);

                if (String.IsNullOrEmpty(connectionString))
                {
                    package = new Package<Int32?>(Status.Failed, null, 1007, TypeError.User);
                    return new JsonResult(package);
                }

                Int32? cardId = null;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException ex)
                    {
                        package = new Package<Int32?>(Status.Failed, null, ex.Number, TypeError.Connection);
                        logger.LogError(ex, service.GetMessageError(ex.Number, TypeError.Connection));
                        return new JsonResult(package);
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "mobile_app_v1_add_command_put_payment_to_lgm";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@v_card_series", values.AuthenticationData.Card.CardSeries);
                        command.Parameters.AddWithValue("@v_card_company", values.AuthenticationData.Card.CardCompany);
                        command.Parameters.AddWithValue("@v_card_number", values.AuthenticationData.Card.CardNumber);
                        command.Parameters.AddWithValue("@v_card_password", values.AuthenticationData.Password);
                        command.Parameters.AddWithValue("@gm_id", values.GameMachineId);                    
                        command.Parameters.AddWithValue("@amount", values.Amount);

                        SqlParameter returnParameter = command.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        try
                        {
                            cardId = (Int32?)command.ExecuteScalar();

                            Int32? result = (Int32?)returnParameter.Value;
                            if (result.HasValue && result != 0)
                            {
                                package = new Package<Int32?>(Status.Failed, null, result.Value, TypeError.User);
                                logger.LogWarning(service.GetMessageError(result, TypeError.User));
                                return new JsonResult(package);
                            }
                        }
                        catch (Exception ex)
                        {
                            package = new Package<Int32?>(Status.Failed, null);
                            logger.LogError(ex, service.GetMessageError(1014, TypeError.Inner));
                            return new JsonResult(package);
                        }
                    }
                }

                package = new Package<Int32?>(Status.Succeed, cardId);
                return new JsonResult(package);
            }
            catch (Exception ex)
            {
                package = new Package<Int32?>(Status.Failed, null);
                logger.LogError(ex, service.GetMessageError(1013, TypeError.Inner));
                return new JsonResult(package);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public JsonResult GetHistoryPaymentToGmInfo(AuthenticationData values)
        {
            Package<List<PaymentInfo>> package;

            try
            {
                if (service.IsAnyNullOrEmpty(values))
                {
                    package = new Package<List<PaymentInfo>>(Status.Failed, new List<PaymentInfo>());
                    logger.LogWarning(service.GetMessageError(1001, TypeError.Common));
                    return new JsonResult(package);
                }

                String connectionString = service.GetConnectionStringByClubId(values.ClubID);

                if (String.IsNullOrEmpty(connectionString))
                {
                    package = new Package<List<PaymentInfo>>(Status.Failed, new List<PaymentInfo>(), 1007, TypeError.User);
                    return new JsonResult(package);
                }

                List<PaymentInfo> paymentList = new List<PaymentInfo>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException ex)
                    {
                        package = new Package<List<PaymentInfo>>(Status.Failed, new List<PaymentInfo>(), ex.Number, TypeError.Connection);
                        logger.LogError(ex, service.GetMessageError(ex.Number, TypeError.Connection));
                        return new JsonResult(package);
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "mobile_app_v1_get_history_payments_to_lgm";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@v_card_series", values.Card.CardSeries);
                        command.Parameters.AddWithValue("@v_card_company", values.Card.CardCompany);
                        command.Parameters.AddWithValue("@v_card_number", values.Card.CardNumber);
                        command.Parameters.AddWithValue("@v_card_password", values.Password);

                        SqlParameter returnParameter = command.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        try
                        {
                            using (SqlDataReader dataReader = command.ExecuteReader())
                            {
                                Int32? result = (Int32?)returnParameter.Value;
                                if(!result.HasValue)
                                {
                                    while (dataReader.Read())
                                    {
                                        paymentList.Add(new PaymentInfo()
                                        {        
                                            CreationDate = !dataReader.IsDBNull(0) ? dataReader.GetDateTime(0).ToString("dd/MM/yyyy HH:mm:ss") : String.Empty,
                                            CompletionDate = !dataReader.IsDBNull(1) ? dataReader.GetDateTime(1).ToString("dd/MM/yyyy HH:mm:ss") : String.Empty,
                                            CommandType = !dataReader.IsDBNull(2) ? (Int32?)dataReader.GetInt32(2) : null,
                                            Amount = !dataReader.IsDBNull(3) ? (Decimal?)dataReader.GetDecimal(3) : 0,
                                            ChangeAmount = !dataReader.IsDBNull(4) ? (Decimal?)dataReader.GetDecimal(4) : 0,
                                            PaymentStatus = !dataReader.IsDBNull(5) ? (Int32?)dataReader.GetInt32(5) : null,
                                            GmNumber = dataReader.GetString(6)
                                        });
                                    }                             
                                }
                                else
                                {
                                    package = new Package<List<PaymentInfo>>(Status.Failed, new List<PaymentInfo>(), result.Value, TypeError.User);
                                    logger.LogWarning(service.GetMessageError(result, TypeError.User));
                                    return new JsonResult(package);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            package = new Package<List<PaymentInfo>>(Status.Failed, new List<PaymentInfo>());
                            logger.LogError(ex, service.GetMessageError(1015, TypeError.Inner));
                            return new JsonResult(package);
                        }
                    }
                }

                package = new Package<List<PaymentInfo>>(Status.Succeed, paymentList);
                return new JsonResult(package);
            }
            catch (Exception ex)
            {
                package = new Package<List<PaymentInfo>>(Status.Failed, new List<PaymentInfo>());
                logger.LogError(ex, service.GetMessageError(1016, TypeError.Inner));
                return new JsonResult(package);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public JsonResult GetAttachedGmsOfPlayer(AuthenticationData values)
        {
            Package<List<GmBalanceInfo>> package;

            try
            {
                if (service.IsAnyNullOrEmpty(values))
                {
                    package = new Package<List<GmBalanceInfo>>(Status.Failed, new List<GmBalanceInfo>());
                    logger.LogWarning(service.GetMessageError(1001, TypeError.Common));
                    return new JsonResult(package);
                }

                String connectionString = service.GetConnectionStringByClubId(values.ClubID);

                if (String.IsNullOrEmpty(connectionString))
                {
                    package = new Package<List<GmBalanceInfo>>(Status.Failed, new List<GmBalanceInfo>(), 1007, TypeError.User);
                    return new JsonResult(package);
                }

                List<GmBalanceInfo> gmBalanceList = new List<GmBalanceInfo>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException ex)
                    {
                        package = new Package<List<GmBalanceInfo>>(Status.Failed, new List<GmBalanceInfo>(), ex.Number, TypeError.Connection);
                        logger.LogError(ex, service.GetMessageError(ex.Number, TypeError.Connection));
                        return new JsonResult(package);
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "mobile_app_v1_get_attached_lgms_of_player";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@v_card_series", values.Card.CardSeries);
                        command.Parameters.AddWithValue("@v_card_company", values.Card.CardCompany);
                        command.Parameters.AddWithValue("@v_card_number", values.Card.CardNumber);
                        command.Parameters.AddWithValue("@v_card_password", values.Password);

                        SqlParameter returnParameter = command.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        try
                        {
                            using (SqlDataReader dataReader = command.ExecuteReader())
                            {
                                Int32? result = (Int32?)returnParameter.Value;
                                if (!result.HasValue)
                                {
                                    while (dataReader.Read())
                                    {
                                        gmBalanceList.Add(new GmBalanceInfo()
                                        {
                                            GmId = !dataReader.IsDBNull(0) ? (Int32?)dataReader.GetInt32(0) : null,
                                            GmNumber = dataReader.GetString(1),
                                            GmBalance = !dataReader.IsDBNull(2) ? (Decimal?)dataReader.GetDecimal(2) : null,
                                            CardBalance = !dataReader.IsDBNull(3) ? (Decimal?)dataReader.GetDecimal(3) : null
                                        });
                                    }
                                }
                                else
                                {
                                    package = new Package<List<GmBalanceInfo>>(Status.Failed, new List<GmBalanceInfo>(), result.Value, TypeError.User);
                                    logger.LogWarning(service.GetMessageError(result, TypeError.User));
                                    return new JsonResult(package);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            package = new Package<List<GmBalanceInfo>>(Status.Failed, new List<GmBalanceInfo>());
                            logger.LogError(ex, service.GetMessageError(1018, TypeError.Inner));
                            return new JsonResult(package);
                        }
                    }
                }

                package = new Package<List<GmBalanceInfo>>(Status.Succeed, gmBalanceList);
                return new JsonResult(package);
            }
            catch (Exception ex)
            {
                package = new Package<List<GmBalanceInfo>>(Status.Failed, new List<GmBalanceInfo>());
                logger.LogError(ex, service.GetMessageError(1019, TypeError.Inner));
                return new JsonResult(package);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public JsonResult AddCommandAttachToGm(GmData values)
        {
            Package<Int32?> package;

            try
            {
                if (service.IsAnyNullOrEmpty(values))
                {
                    package = new Package<Int32?>(Status.Failed, null);
                    logger.LogWarning(service.GetMessageError(1001, TypeError.Common));
                    return new JsonResult(package);
                }

                String connectionString = service.GetConnectionStringByClubId(values.AuthenticationData.ClubID);

                if (String.IsNullOrEmpty(connectionString))
                {
                    package = new Package<Int32?>(Status.Failed, null, 1007, TypeError.User);
                    return new JsonResult(package);
                }

                Int32? cardId = null;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException ex)
                    {
                        package = new Package<Int32?>(Status.Failed, null, ex.Number, TypeError.Connection);
                        logger.LogError(ex, service.GetMessageError(ex.Number, TypeError.Connection));
                        return new JsonResult(package);
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "mobile_app_v1_add_command_attach_to_lgm";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@v_card_series", values.AuthenticationData.Card.CardSeries);
                        command.Parameters.AddWithValue("@v_card_company", values.AuthenticationData.Card.CardCompany);
                        command.Parameters.AddWithValue("@v_card_number", values.AuthenticationData.Card.CardNumber);
                        command.Parameters.AddWithValue("@v_card_password", values.AuthenticationData.Password);
                        command.Parameters.AddWithValue("@gm_id", values.GameMachineId);

                        SqlParameter returnParameter = command.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        try
                        {
                            cardId = (Int32?)command.ExecuteScalar();

                            Int32? result = (Int32?)returnParameter.Value;
                            if (result.HasValue && result != 0)
                            {
                                package = new Package<Int32?>(Status.Failed, null, result.Value, TypeError.User);
                                logger.LogWarning(service.GetMessageError(result, TypeError.User));
                                return new JsonResult(package);
                            } 
                        }
                        catch (Exception ex)
                        {
                            package = new Package<Int32?>(Status.Failed, null);
                            logger.LogError(ex, service.GetMessageError(1020, TypeError.Inner));
                            return new JsonResult(package);
                        }
                    }
                }

                package = new Package<Int32?>(Status.Succeed, cardId);
                return new JsonResult(package);
            }
            catch (Exception ex)
            {
                package = new Package<Int32?>(Status.Failed, null);
                logger.LogError(ex, service.GetMessageError(1021, TypeError.Inner));
                return new JsonResult(package);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public JsonResult CheckCommandStatus(CheckingStatusOfCommandData values)
        {
            Package<Int32?> package;

            try
            {
                if (service.IsAnyNullOrEmpty(values))
                {
                    package = new Package<Int32?>(Status.Failed, null);
                    logger.LogWarning(service.GetMessageError(1001, TypeError.Common));
                    return new JsonResult(package);
                }

                String connectionString = service.GetConnectionStringByClubId(values.ClubId);

                if (String.IsNullOrEmpty(connectionString))
                {
                    package = new Package<Int32?>(Status.Failed, null, 1007, TypeError.User);
                    return new JsonResult(package);
                }
                Int32? status = null;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException ex)
                    {
                        package = new Package<Int32?>(Status.Failed, null, ex.Number, TypeError.Connection);
                        logger.LogError(ex, service.GetMessageError(ex.Number, TypeError.Connection));
                        return new JsonResult(package);
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "mobile_app_v1_get_status_command";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@gm_id", values.GameMachineId);
                        command.Parameters.AddWithValue("@card_id", values.CardId);
                        command.Parameters.AddWithValue("@command_type", values.CommandType);

                        SqlParameter returnParameter = command.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        Stopwatch SW = new Stopwatch();
                        SW.Start();
                        while(SW.Elapsed.Seconds <= 6)
                        {
                            try
                            {
                                command.ExecuteNonQuery();
                                if (returnParameter.Value != DBNull.Value)
                                {
                                    status = (Int32?)returnParameter.Value;
                                }
                            }
                            catch (Exception ex)
                            {
                                package = new Package<Int32?>(Status.Failed, null);
                                logger.LogError(ex, service.GetMessageError(1022, TypeError.Inner));
                                return new JsonResult(package);
                            }
                            if(!status.Equals(-1) && !status.Equals(2))
                            {
                                break;
                            }
                            Thread.Sleep(2000);
                        }
                        SW.Stop();
                    }
                }

                package = new Package<Int32?>(Status.Succeed, status);
                return new JsonResult(package);
            }
            catch (Exception ex)
            {
                package = new Package<Int32?>(Status.Failed, null);
                logger.LogError(ex, service.GetMessageError(1023, TypeError.Inner));
                return new JsonResult(package);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public JsonResult CheckAttachmentToGm(GmData values)
        {
            Package<Boolean> package;

            try
            {
                if (service.IsAnyNullOrEmpty(values))
                {
                    package = new Package<Boolean>(Status.Failed, false);
                    logger.LogWarning(service.GetMessageError(1001, TypeError.Common));
                    return new JsonResult(package);
                }

                String connectionString = service.GetConnectionStringByClubId(values.AuthenticationData.ClubID);

                if (String.IsNullOrEmpty(connectionString))
                {
                    package = new Package<Boolean>(Status.Failed, false, 1007, TypeError.User);
                    return new JsonResult(package);
                }

                Boolean isAttached = false;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException ex)
                    {
                        package = new Package<Boolean>(Status.Failed, false, ex.Number, TypeError.Connection);
                        logger.LogError(ex, service.GetMessageError(ex.Number, TypeError.Connection));
                        return new JsonResult(package);
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "mobile_app_v1_check_attachment_to_lgm";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@v_card_series", values.AuthenticationData.Card.CardSeries);
                        command.Parameters.AddWithValue("@v_card_company", values.AuthenticationData.Card.CardCompany);
                        command.Parameters.AddWithValue("@v_card_number", values.AuthenticationData.Card.CardNumber);
                        command.Parameters.AddWithValue("@v_card_password", values.AuthenticationData.Password);
                        command.Parameters.AddWithValue("@gm_id", values.GameMachineId);

                        SqlParameter returnParameter = command.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        try
                        {
                            isAttached = ((Int32)command.ExecuteScalar()) == 1 ? true : false;
                            Int32? result = (Int32?)returnParameter.Value;
                            if (result.HasValue && result != 0)
                            {
                                package = new Package<Boolean>(Status.Failed, false, result.Value, TypeError.User);
                                logger.LogWarning(service.GetMessageError(result, TypeError.User));
                                return new JsonResult(package);
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            package = new Package<Boolean>(Status.Failed, false);
                            logger.LogError(ex, service.GetMessageError(1026, TypeError.Inner));
                            return new JsonResult(package);
                        }
                    }
                }

                package = new Package<Boolean>(Status.Succeed, isAttached);
                return new JsonResult(package);
            }
            catch (Exception ex)
            {
                package = new Package<Boolean>(Status.Failed, false);
                logger.LogError(ex, service.GetMessageError(1027, TypeError.Inner));
                return new JsonResult(package);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public JsonResult AddCommandWithdrawToGm(GmData values)
        {
            Package<Int32?> package;

            try
            {
                if (service.IsAnyNullOrEmpty(values))
                {
                    package = new Package<Int32?>(Status.Failed, null);
                    logger.LogWarning(service.GetMessageError(1001, TypeError.Common));
                    return new JsonResult(package);
                }

                String connectionString = service.GetConnectionStringByClubId(values.AuthenticationData.ClubID);

                if (String.IsNullOrEmpty(connectionString))
                {
                    package = new Package<Int32?>(Status.Failed, null, 1007, TypeError.User);
                    return new JsonResult(package);
                }

                Int32? cardId = null;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException ex)
                    {
                        package = new Package<Int32?>(Status.Failed, null, ex.Number, TypeError.Connection);
                        logger.LogError(ex, service.GetMessageError(ex.Number, TypeError.Connection));
                        return new JsonResult(package);
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "mobile_app_v1_add_command_withdraw_payment_to_lgm";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@v_card_series", values.AuthenticationData.Card.CardSeries);
                        command.Parameters.AddWithValue("@v_card_company", values.AuthenticationData.Card.CardCompany);
                        command.Parameters.AddWithValue("@v_card_number", values.AuthenticationData.Card.CardNumber);
                        command.Parameters.AddWithValue("@v_card_password", values.AuthenticationData.Password);
                        command.Parameters.AddWithValue("@gm_id", values.GameMachineId);

                        SqlParameter returnParameter = command.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        try
                        {
                            cardId = (Int32?)command.ExecuteScalar();

                            Int32? result = (Int32?)returnParameter.Value;
                            if (result.HasValue && result != 0)
                            {
                                package = new Package<Int32?>(Status.Failed, null, result.Value, TypeError.User);
                                logger.LogWarning(service.GetMessageError(result, TypeError.User));
                                return new JsonResult(package);
                            }                          
                        }
                        catch (Exception ex)
                        {
                            package = new Package<Int32?>(Status.Failed, null);
                            logger.LogError(ex, service.GetMessageError(1024, TypeError.Inner));
                            return new JsonResult(package);
                        }
                    }
                }

                package = new Package<Int32?>(Status.Succeed, cardId);
                return new JsonResult(package);
            }
            catch (Exception ex)
            {
                package = new Package<Int32?>(Status.Failed, null);
                logger.LogError(ex, service.GetMessageError(1025, TypeError.Inner));
                return new JsonResult(package);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public JsonResult GetCurrentGmBalanceInfo(GmData values)
        {
            Package<GmBalanceInfo> package;

            try
            {
                if (service.IsAnyNullOrEmpty(values))
                {
                    package = new Package<GmBalanceInfo>(Status.Failed, new GmBalanceInfo());
                    logger.LogWarning(service.GetMessageError(1001, TypeError.Common));
                    return new JsonResult(package);
                }

                String connectionString = service.GetConnectionStringByClubId(values.AuthenticationData.ClubID);

                if (String.IsNullOrEmpty(connectionString))
                {
                    package = new Package<GmBalanceInfo>(Status.Failed, new GmBalanceInfo(), 1007, TypeError.User);
                    return new JsonResult(package);
                }

                GmBalanceInfo gmBalance = new GmBalanceInfo();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException ex)
                    {
                        package = new Package<GmBalanceInfo>(Status.Failed, new GmBalanceInfo(), ex.Number, TypeError.Connection);
                        logger.LogError(ex, service.GetMessageError(ex.Number, TypeError.Connection));
                        return new JsonResult(package);
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "mobile_app_v1_get_balance_current_lgm";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@v_card_series", values.AuthenticationData.Card.CardSeries);
                        command.Parameters.AddWithValue("@v_card_company", values.AuthenticationData.Card.CardCompany);
                        command.Parameters.AddWithValue("@v_card_number", values.AuthenticationData.Card.CardNumber);
                        command.Parameters.AddWithValue("@v_card_password", values.AuthenticationData.Password);
                        command.Parameters.AddWithValue("@gm_id", values.GameMachineId);

                        SqlParameter returnParameter = command.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        try
                        {
                            using (SqlDataReader dataReader = command.ExecuteReader())
                            {
                                Int32? result = (Int32?)returnParameter.Value;
                                if (!result.HasValue)
                                {
                                    while (dataReader.Read())
                                    {
                                        gmBalance.GmId = !dataReader.IsDBNull(0) ? (Int32?)dataReader.GetInt32(0) : null;
                                        gmBalance.GmNumber = dataReader.GetString(1);
                                        gmBalance.GmBalance = !dataReader.IsDBNull(2) ? (Decimal?)dataReader.GetDecimal(2) : null;
                                        gmBalance.CardBalance = !dataReader.IsDBNull(3) ? (Decimal?)dataReader.GetDecimal(3) : null;
                                    }
                                }
                                else
                                {
                                    package = new Package<GmBalanceInfo>(Status.Failed, new GmBalanceInfo(), result.Value, TypeError.User);
                                    logger.LogWarning(service.GetMessageError(result, TypeError.User));
                                    return new JsonResult(package);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            package = new Package<GmBalanceInfo>(Status.Failed, new GmBalanceInfo());
                            logger.LogError(ex, service.GetMessageError(1029, TypeError.Inner));
                            return new JsonResult(package);
                        }
                    }
                }

                package = new Package<GmBalanceInfo>(Status.Succeed, gmBalance);
                return new JsonResult(package);
            }
            catch (Exception ex)
            {
                package = new Package<GmBalanceInfo>(Status.Failed, new GmBalanceInfo());
                logger.LogError(ex, service.GetMessageError(1030, TypeError.Inner));
                return new JsonResult(package);
            }
        }
    }
}
