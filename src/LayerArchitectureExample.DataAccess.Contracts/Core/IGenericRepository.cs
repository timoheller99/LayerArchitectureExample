namespace LayerArchitectureExample.DataAccess.Contracts.Core;

using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public interface IGenericRepository<in TIdType, TDbModel>
    where TDbModel : IDbModel<TIdType>
{
    public Task CreateAsync(TDbModel entity);

    public Task CreateAsync(TDbModel entity, IDbConnection connection);

    public Task CreateAsync(TDbModel entity, IDbConnection connection, IDbTransaction transaction);

    public Task<IEnumerable<TDbModel>> GetAllAsync();

    public Task<IEnumerable<TDbModel>> GetAllAsync(IDbConnection connection);

    public Task<IEnumerable<TDbModel>> GetAllAsync(IDbConnection connection, IDbTransaction transaction);

    public Task<TDbModel> GetByIdAsync(TIdType id);

    public Task<TDbModel> GetByIdAsync(TIdType id, IDbConnection connection);

    public Task<TDbModel> GetByIdAsync(TIdType id, IDbConnection connection, IDbTransaction transaction);

    public Task UpdateAsync(TDbModel entity);

    public Task UpdateAsync(TDbModel entity, IDbConnection connection);

    public Task UpdateAsync(TDbModel entity, IDbConnection connection, IDbTransaction transaction);

    public Task DeleteAsync(TIdType id);

    public Task DeleteAsync(TIdType id, IDbConnection connection);

    public Task DeleteAsync(TIdType id, IDbConnection connection, IDbTransaction transaction);
}
