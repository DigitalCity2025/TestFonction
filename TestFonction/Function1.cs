using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TestFonction.Models;

namespace TestFonction
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([TimerTrigger("0 */1 * * * * ")]TimerInfo myTimer, ILogger log)
        {
            try
            {
                using HttpClient client = new HttpClient();

                ConfigurationBuilder builder = new ConfigurationBuilder();
                IConfigurationRoot config = builder.Build();

                HttpResponseMessage message = client.PostAsJsonAsync(
                    config["apiUrl"] + "/api/login", new { Username = config["user:username"], Password = config["user:password"] }).Result;
                if (message.IsSuccessStatusCode)
                {
                    string token = message.Content.ReadAsAsync<TokenDTO>().Result.Token;
                    log.LogInformation(token);
                }
                else
                {
                    log.LogError("Error");
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }
            // TODO implement what you want
        }
    }
}
