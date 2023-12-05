using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SoundMastery.Domain;

namespace SoundMastery.DataAccess.Services.Common;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity> Get(int id);

    Task<IReadOnlyCollection<TEntity>> Find(Expression<Func<TEntity, bool>> filter);

    Task<IReadOnlyCollection<TEntity>> Find<T>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, T>> includable = null);

    Task<TEntity> Create(TEntity course);

    Task Delete(int id);

    Task Update(TEntity product);
}