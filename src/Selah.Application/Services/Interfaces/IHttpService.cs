using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Selah.Application.Services.Interfaces
{
  public interface IHttpService
  {
    void AddBearerToken(string token);
    void AddApiKeyHeader(string value);
    void AddRequestHeader(string key, string value);
    Task<(T, HttpResponseMessage)> GetAsync<T>(Uri servicePath);
    Task<(T, HttpResponseMessage)> PostAsync<T>(object data, Uri servicePath);
    Task<(T, HttpResponseMessage)> PutAsync<T>(object data, Uri servicePath);    
    Task<(T, HttpResponseMessage)> DeleteAsync<T>(Uri servicePath);
    Task<(T, HttpResponseMessage)> PostUrlEncodedFormData<T>(Uri servicePath, FormUrlEncodedContent data);

  }
}