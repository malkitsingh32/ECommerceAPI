using Application.Handler.Common.Dtos;

namespace Application.Abstraction.Services
{
    public interface IRequestService
    {
        void AssignRequest(ServerRowRequest request);
        IRequestService GetRequestBuilder(ServerRowRequest request);
        string GetFilters();
        string GetSorts();
        int GetPageIndex();
        int GetPageSize();
    }
}
