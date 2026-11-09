using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MS_DM;
namespace CY_MS_Eng
{
    public class GeneralApi
    {

        static HttpClient GClient = null;
        static HttpClient BGClient = null;
        static string WebApiUri = "https://api.mouser.com";
        private static HttpClient CreatHttpClient()
        {
           // if (DALCore.CUser == null)
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(WebApiUri);//= new Uri("http://localhost.fiddler:5193/");
                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));
                // if (DALCore.CUser != null) client.DefaultRequestHeaders.Add("Authorization", DALCore.CUser.TTKK);
                return client;
            }
            //else
            //{
            //    if (GClient == null)
            //    {

            //        GClient = new HttpClient();
            //        GClient.BaseAddress = new Uri(SelApp.WebApiUri);
            //        GClient.DefaultRequestHeaders.Accept.Add(
            //           new MediaTypeWithQualityHeaderValue("application/json"));
            //        GClient.DefaultRequestHeaders.Authorization =
            //            new AuthenticationHeaderValue("Bearer", DALCore.CUser.TTKK + ":" + DALCore.CUser.Username);
            //        return GClient;
            //    }
            //    else
            //    {
            //        return GClient;
            //    }
            //}
        }


        public static Task<ApiOut<T>> Get<T>(string Address) where T : class
        {
            return Task<ApiOut<T>>.Factory.StartNew(() =>
            {
                ApiOut<T> resOut = new ApiOut<T>();
                try
                {

                    HttpClient client = CreatHttpClient();
                    var myType = typeof(T);
                   // var Controller = TypeApiUri[myType.Name];
                    var url = "api/" + Address;

                    HttpResponseMessage response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var itemString=response.Content.ReadAsStringAsync().Result;
                        var SelItem = response.Content.ReadAsAsync<T>().Result;
                        resOut.Out = SelItem;
                        resOut.Msg = "1";
                        return resOut;
                    }
                    else
                    {
                        var msg1 = response.Content.ReadAsStringAsync().Result;
                        var msg2 = ("Error Code : " + response.StatusCode + " | Message - " + response.ReasonPhrase);
                        resOut.Msg = string.IsNullOrWhiteSpace(msg1) ? msg2 : msg1;
                       // LogGetter.addmessage(resOut.Msg);
                        return resOut;
                    }
                }
                catch (Exception ex)
                {
                    resOut.Msg = ex.Message;
                    //LogGetter.addmessage(ex);
                    return resOut;
                }
            });
        }

    }
}
