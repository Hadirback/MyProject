using Translate = Slotlogic.MobileApp.Localization.Resources;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;
using Slotlogic.MobileApp.Models.InputData;
using System.Diagnostics;
using System.Net;
using Slotlogic.MobileApp.Models.Common;
using System.Collections.ObjectModel;
using Slotlogic.MobileApp.Models.OutputData;
using Slotlogic.MobileApp.Pages;
using Slotlogic.MobileApp.Models.Enum;
using System.Collections.Generic;
using Slotlogic.MobileApp.Models.Common.PBI;
using Slotlogic.MobileApp.Models.Common.Main;
using System.Globalization;
using System.Text;

namespace Slotlogic.MobileApp.Services
{
    internal static class Service
    {
        public static bool IsCorrectCardNumber(string cardNumber)
        {
            try
            {
                Regex regex = new Regex(@"^\w{2}-\w{2}-(\d?){6}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (regex.IsMatch(cardNumber))
                    return true;
                else
                    return false;
            }
            catch(Exception e)
            {
                Debug.WriteLine($"Error in Service.IsCorrectCardNumber function {e}");
                return false;
            }
        }

        public static CardInfo SplitCardNumber(string cardNumber)
        {
            CardInfo card;
            string[] parts = cardNumber.Split(new char[] { '-' });
            if (string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]) || string.IsNullOrEmpty(parts[2]))
                return null;

            if (Int32.TryParse(parts[2], out int num))
                card = new CardInfo(parts[0], parts[1], num);
            else return null;

            return card;
        }

        public static bool IsConnectedToInternet(string address)
        {
            return IsConnectedToInternet(address, 1);
        }

        private static bool IsConnectedToInternet(string hostNameOrAddress, int ttl)
        {
            Ping pinger = null;
            try
            {
                if (ttl >= 10)
                    return false;

                pinger = new Ping();
                PingOptions pingerOptions = new PingOptions(ttl, true);
                int timeout = 5000;
                byte[] buffer = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                PingReply reply = default(PingReply);

                reply = pinger.Send(hostNameOrAddress, timeout, buffer, pingerOptions);

                if (reply.Status == IPStatus.Success)
                    return true;
                else if (reply.Status == IPStatus.TtlExpired)
                {
                    bool tempResult = false;
                    tempResult = IsConnectedToInternet(hostNameOrAddress, ttl + 1);
                    return tempResult;
                }
                else
                {
                    return false;
                }
            }
            catch (PingException ex)
            {
                return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }
        }

        internal async static Task<List<DrawsInfo>> UpdateDrawsList(Page page)
        {
            try
            {
                Package<ModelDraws> jsonOutput = await App.RestService.PostResponse<Package<ModelDraws>>(BaseMDPage.session, Settings.DrawsInfo);
                if (jsonOutput == null)
                {
                    WriteToLog($"Failed to get data on update Draws page. (jsonOutput == null)", page: page);
                    return new List<DrawsInfo>();
                }

                if (jsonOutput.Status == Statuses.Succeed)
                {
                    return jsonOutput.Data.DrawsList;
                }
                else
                {
                    WriteToLog($"Failed to get data on update Draws page.", page: page);
                    return new List<DrawsInfo>();
                }
            }
            catch(Exception exc)
            {
                WriteToLog($"Failed to get data on update Draws page.", exc, page);
                return new List<DrawsInfo>();
            }
        }

        public async static Task ErrorDisplay(string message, Page page)
        {
            await page.DisplayAlert($"{Translate.TrTextError}", message, $"{Translate.TrTextOK}");
        }

        public async static void WriteToLog(string message, Exception exc = null, Page page = null)
        {
            try
            {
                Token token = (exc == null) ? new Token(message) : new Token(message, exc);
                HttpStatusCode statusCode = await App.RestService.PostResponseToken(token, Settings.WriteError);
                if(exc == null)
                    Debug.WriteLine($"Text:{message} HttpStatusCode:{statusCode}.");
                else
                    Debug.WriteLine($"Text:{message} HttpStatusCode:{statusCode}. Error message:{exc.Message}");

                if (page != null)
                {
                    await page.DisplayAlert($"{Translate.TrTextError}", $"{Translate.TrMainPageError}", $"{Translate.TrTextOK}");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error in Service.WriteToLog function {e}");
            }
        }

        internal async static Task<ResourcesDataPlayer> UpdateResourcesDataPlayer(Page page)
        {
            try
            {
                Package<ModelResourcesDataPlayer> jsonOutput = await App.RestService.PostResponse<Package<ModelResourcesDataPlayer>>(BaseMDPage.session, Settings.ResourcesDataPlayer);
                if (jsonOutput == null)
                {
                    WriteToLog($"Failed to get data on update pbi page. (jsonOutput == null)", page: page);
                    return null;
                }

                if (jsonOutput.Status == Statuses.Succeed)
                {
                    return jsonOutput.Data.PBIData;
                }
                else
                {
                    WriteToLog($"Failed to get data on update pbi page.", page: page);
                    return null;
                }
            }
            catch (Exception exc)
            {
                WriteToLog($"Failed to get data on update pbi page.", exc, page);
                return null;
            }
        }
    }   
}
