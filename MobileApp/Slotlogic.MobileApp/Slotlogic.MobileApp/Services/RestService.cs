using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Slotlogic.MobileApp.Models.Common;
using Slotlogic.MobileApp.Models.InputData;

namespace Slotlogic.MobileApp.Services
{
    public class RestService
    {
        HttpClient client;

        public RestService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        public async Task<T> PostResponse<T>(object data, string method) where T : class
        {
            try
            {
                string jsonStr = JsonConvert.SerializeObject(data);
                string requestUri = $"{Settings.UrlBase}{method}";
                HttpResponseMessage response = await client.PostAsync(requestUri,
                        new StringContent(jsonStr, Encoding.UTF8, "application/json"));
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);                
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine($"Error function PostResponse(). {exc.Message}");
            }
            return null;
        }

        public async Task<HttpStatusCode> PostResponseToken(object data, string method)
        {
            try
            {
                string jsonStr = JsonConvert.SerializeObject(data);
                string requestUri = $"{Settings.UrlBase}{method}";
                HttpResponseMessage response = await client.PostAsync(requestUri,
                        new StringContent(jsonStr, Encoding.UTF8, "application/json"));
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response.StatusCode;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine($"Error function PostResponse(). {exc.Message}");
            }
            return HttpStatusCode.NotFound;
        }

        public async Task<T> GetResponse<T>(string method, string value) where T : class
        {           
            try
            {
                string requestUri = $"{Settings.UrlBase}{method}{value}";
                HttpResponseMessage response = await client.GetAsync(requestUri);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);    
                }
            }
            catch (Exception exc)
            {
                throw new Exception($"Error function GetResponse(). {exc.Message}");
            }
            return null;
        }
    }
}
