using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YT7G72_HFT_2023241.WpfClient.Logic
{
    public class RestService
    {
        HttpClient client;

        public RestService(string baseurl, string endpoint, string pingableEndpoint = "swagger")
        {
            bool isOk = false;
            do
            {
                isOk = Ping(baseurl + pingableEndpoint);
            } while (isOk == false);
            Init(baseurl);
        }

        private bool Ping(string url)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.DownloadData(url);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Init(string baseurl)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseurl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue
                ("application/json"));
            try
            {
                client.GetAsync("").GetAwaiter().GetResult();
            }
            catch (HttpRequestException)
            {
                throw new ArgumentException("Endpoint is not available!");
            }

        }

        public async Task<List<T>> GetAsync<T>(string endpoint)
        {
            List<T> items = new List<T>();
            HttpResponseMessage response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                items = await response.Content.ReadAsAsync<List<T>>();
            }
            else
            {
                var error = await response.Content.ReadAsAsync<RestExceptionInfo>();
                throw new ArgumentException(error.Msg);
            }
            return items;
        }

        public List<T> Get<T>(string endpoint)
        {
            List<T> items = new List<T>();
            HttpResponseMessage response = client.GetAsync(endpoint).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                items = response.Content.ReadAsAsync<List<T>>().GetAwaiter().GetResult();
            }
            else
            {
                var error = response.Content.ReadAsAsync<RestExceptionInfo>().GetAwaiter().GetResult();
                throw new ArgumentException(error.Msg);
            }
            return items;
        }

        public async Task<T> GetSingleAsync<T>(string endpoint)
        {
            T item = default(T);
            HttpResponseMessage response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                item = await response.Content.ReadAsAsync<T>();
            }
            else
            {
                var error = await response.Content.ReadAsAsync<RestExceptionInfo>();
                throw new ArgumentException(error.Msg);
            }
            return item;
        }

        public T GetSingle<T>(string endpoint)
        {
            T item = default(T);
            HttpResponseMessage response = client.GetAsync(endpoint).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                item = response.Content.ReadAsAsync<T>().GetAwaiter().GetResult();
            }
            else
            {
                var error = response.Content.ReadAsAsync<RestExceptionInfo>().GetAwaiter().GetResult();
                throw new ArgumentException(error.Msg);
            }
            return item;
        }

        public async Task<T> GetAsync<T>(int id, string endpoint)
        {
            T item = default(T);
            HttpResponseMessage response = await client.GetAsync(endpoint + "/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                item = await response.Content.ReadAsAsync<T>();
            }
            else
            {
                var error = await response.Content.ReadAsAsync<RestExceptionInfo>();
                throw new ArgumentException(error.Msg);
            }
            return item;
        }

        public T Get<T>(int id, string endpoint)
        {
            T item = default(T);
            HttpResponseMessage response = client.GetAsync(endpoint + "/" + id.ToString()).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                item = response.Content.ReadAsAsync<T>().GetAwaiter().GetResult();
            }
            else
            {
                var error = response.Content.ReadAsAsync<RestExceptionInfo>().GetAwaiter().GetResult();
                throw new ArgumentException(error.Msg);
            }
            return item;
        }

        public async Task PostAsync<T>(T item, string endpoint)
        {
            HttpResponseMessage response =
                await client.PostAsJsonAsync(endpoint, item);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ValidationErrorResponse>(responseContent);
                    throw new ArgumentException($"{error?.Errors.First().Value[0]}");
                }
                else
                {
                    var error = await response.Content.ReadAsAsync<RestExceptionInfo>();
                    throw new ArgumentException(error.Msg);
                }

            }
            response.EnsureSuccessStatusCode();
        }

        public void Post<T>(T item, string endpoint)
        {
            HttpResponseMessage response =
                client.PostAsJsonAsync(endpoint, item).GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
            {
                var error = response.Content.ReadAsAsync<RestExceptionInfo>().GetAwaiter().GetResult();
                throw new ArgumentException(error.Msg);
            }
            response.EnsureSuccessStatusCode();
        }

        public void Post(string endpoint)
        {
            HttpResponseMessage response =
                client.PostAsync(endpoint, null).GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
            {
                var error = response.Content.ReadAsAsync<RestExceptionInfo>().GetAwaiter().GetResult();
                throw new ArgumentException(error.Msg);
            }
            response.EnsureSuccessStatusCode();
        }


        public async Task DeleteAsync(int id, string endpoint)
        {
            HttpResponseMessage response =
                await client.DeleteAsync(endpoint + "/" + id.ToString());

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsAsync<RestExceptionInfo>();
                throw new ArgumentException(error.Msg);
            }

            response.EnsureSuccessStatusCode();
        }

        public void Delete(int id, string endpoint)
        {
            HttpResponseMessage response =
                client.DeleteAsync(endpoint + "/" + id.ToString()).GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
            {
                var error = response.Content.ReadAsAsync<RestExceptionInfo>().GetAwaiter().GetResult();
                throw new ArgumentException(error.Msg);
            }

            response.EnsureSuccessStatusCode();
        }

        public void Delete(string endpoint)
        {
            HttpResponseMessage response =
                client.DeleteAsync(endpoint).GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new ArgumentException("Invalid arugment(s) provided!");
                }
                var error = response.Content.ReadAsAsync<RestExceptionInfo>().GetAwaiter().GetResult();
                throw new ArgumentException(error.Msg);
            }

            response.EnsureSuccessStatusCode();
        }

        public async Task PutAsync<T>(T item, string endpoint)
        {
            HttpResponseMessage response =
                await client.PutAsJsonAsync(endpoint, item);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();  
                    var error = JsonConvert.DeserializeObject<ValidationErrorResponse>(responseContent);
                    throw new ArgumentException($"{error?.Errors.First().Value[0]}");
                }
                else
                {
                    var error = await response.Content.ReadAsAsync<RestExceptionInfo>();
                    throw new ArgumentException(error.Msg);
                }
               
            }

            response.EnsureSuccessStatusCode();
        }

        public void Put<T>(T item, string endpoint)
        {
            HttpResponseMessage response =
                client.PutAsJsonAsync(endpoint, item).GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
            {
                var error = response.Content.ReadAsAsync<RestExceptionInfo>().GetAwaiter().GetResult();
                throw new ArgumentException(error.Msg);
            }

            response.EnsureSuccessStatusCode();
        }

        public string GetAsString(string endpoint)
        {
            HttpResponseMessage response =
                client.GetAsync(endpoint).GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var error = JsonConvert.DeserializeObject<RestExceptionInfo>(responseContent);
                throw new ArgumentException(error.Msg);
            }

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

    }
}
