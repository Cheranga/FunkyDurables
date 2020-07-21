using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Funky.Durables.Extensions
{
    public static class HttpExtensions
    {
        public static async Task<TModel> GetModel<TModel>(this HttpRequest request) where TModel : class
        {
            try
            {
                var content = await new StreamReader(request.Body).ReadToEndAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return null;
                }

                var model = JsonConvert.DeserializeObject<TModel>(content, new JsonSerializerSettings
                {
                    Error = (sender, args) => args.ErrorContext.Handled = true
                });

                return model;
            }
            catch
            {
                //ignored;
            }

            return null;
        }
    }
}