using Hotel.Domain.DTOs;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IRepositoryQuery<TQuery, TGetQueryResponse, TQueryParameters>
  where TQuery : IDataTransferObject
  where TGetQueryResponse : IDataTransferObject
{
    public Task<TQuery?> GetByIdAsync(Guid id);
    public Task<IEnumerable<TGetQueryResponse>> GetAsync(TQueryParameters queryParameters);
}

public interface IRepositoryQuery<TQuery, TQueryParameters>
  where TQuery : IDataTransferObject
  where TQueryParameters : IDataTransferObject
{
    public Task<TQuery?> GetByIdAsync(Guid id);
    public Task<IEnumerable<TQuery>> GetAsync(TQueryParameters queryParameters);
}

