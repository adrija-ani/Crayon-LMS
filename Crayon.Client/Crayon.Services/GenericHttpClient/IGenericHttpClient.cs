using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
namespace Crayon.Services.GenericHttpClients
{
    public interface IGenericHttpClient
    {
        Task<T> GetAsync<T>(string url);
        Task<T> PostAsAsync<T>(string url,dynamic dynamicdata);
        Task<T> PutAsAsync<T>(string url,dynamic dynamicdata);
        Task<T> DeleteAsAsync<T>(string url);

    }


}
