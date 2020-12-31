using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Login2.Auxiliary.WebAPIRequest
{
    public class FPTApiRequest : IWebApiRequest
    {
        internal static string webApiUrl = ConfigurationManager.AppSettings["FPTApiUrl"];
        internal static string apikey = ConfigurationManager.AppSettings["FPTAPIKey"];

        public string Get(string uri, ParamObject param = null, string jwtToken = null)
        {
            return null;
        }

        public string Post(string uri, object param = null, string jwtToken = null)
        {
            string filePath = param.ToString();
            string url = string.Format("{0}{1}", webApiUrl, uri);
			try
			{
				var httpClient = new HttpClient();
				var fileStream = File.Open(filePath, FileMode.Open);
				var fileInfo = new FileInfo(filePath);
				var content = new MultipartFormDataContent();
				content.Headers.Add("api_key", apikey);
				content.Add(new StreamContent(fileStream), "\"image\"", string.Format("\"{0}\"", "image" + fileInfo.Extension));
				var response =  httpClient.PostAsync(url, content).Result;
				var res =  response.Content.ReadAsStringAsync().Result;
				return res;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
    }
}
