using AIMatchWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIMatchWeb.Business.Interfaces
{
    public interface IAuthGptBusiness
    {
        Task<TOutput> PostHttpRequestAuthApiGpt<TInput, TOutput>(TInput model) where TOutput : CommonPropertyResponseDto, new();
        Task<string> PostHttpRequestAuthApiGpt (AuthGptRequestDto model);
    }
}
