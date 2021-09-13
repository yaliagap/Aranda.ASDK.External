using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Aranda.ASDK.External
{
    public class Token
    {
        /// <summary>
        /// Obtiene token por identificador de usuario
        /// </summary>
        /// <param name="userId">identificador de usuario</param>
        /// <returns></returns>
        public string Get(int userId)
        {
            string userIdRequest = "";

            if (userId > 0)
            {
                userIdRequest = userId.ToString();
            }
            else
            {
                //userIdRequest = new Random().Next(1, 100000).ToString();
                userIdRequest = Guid.NewGuid().ToString();
            }
            System.Net.ServicePointManager.SecurityProtocol = Aranda.ASDK.Business.SecurityProtocolTypeExtensions.SystemDefault;

            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://ohmega-auth-qa.mybluemix.net/api/auth/company");
            WebProxy myproxy = new WebProxy("10.250.5.181:8080", false);
            httpWebRequest.Proxy = myproxy;
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";            
            httpWebRequest.KeepAlive = false;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"company_uuid\":\"f9ed58bc-8501-4c0a-b7c8-083a5e8bb182\"," +
                                "\"secret\":\"U2FsdGVkX19SokQOxnabUKVomvEUmuSlgI3XX98eG7Q=\"," +
                              "\"user_id\":\"" + userIdRequest + "\"}";

                streamWriter.Write(json);
            }

            var result = "";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            JObject jsonObject = JObject.Parse(result);

            return jsonObject["token"]["access_token"].ToString();

            //return result;
        }
    }
}