namespace LayerArchitectureExample.DataAccess.Contracts.Core;

using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public interface IGenericRepository<in TIdType, TDbEntity>
    where TDbEntity : IDbEntity<TIdType>
{
    public Task CreateAsync(TDbEntity entity);

    public Task CreateAsync(TDbEntity entity, IDbConnection connection);

    public Task CreateAsync(TDbEntity entity, IDbConnection connection, IDbTransaction transaction);

    public Task<IEnumerable<TDbEntity>> GetAllAsync();

    public Task<IEnumerable<TDbEntity>> GetAllAsync(IDbConnection connection);

    public Task<IEnumerable<TDbEntity>> GetAllAsync(IDbConnection connection, IDbTransaction transaction);

    public Task<TDbEntity> GetByIdAsync(TIdType id);

    public Task<TDbEntity> GetByIdAsync(TIdType id, IDbConnection connection);

    public Task<TDbEntity> GetByIdAsync(TIdType id, IDbConnection connection, IDbTransaction transaction);

    public Task UpdateAsync(TDbEntity entity);

    public Task UpdateAsync(TDbEntity entity, IDbConnection connection);

    public Task UpdateAsync(TDbEntity entity, IDbConnection connection, IDbTransaction transaction);

    public Task DeleteAsync(TIdType id);

    public Task DeleteAsync(TIdType id, IDbConnection connection);

    public Task DeleteAsync(TIdType id, IDbConnection connection, IDbTransaction transaction);
}
