using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoundMastery.DataAccess.Contexts;
using SoundMastery.Domain;

namespace SoundMastery.DataAccess.Services.Common;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class, IHasId
{
    private readonly SoundMasteryContext _context;

    public GenericRepository(SoundMasteryContext context)
    {
        _context = context;
    }

    public Task<TEntity> Get(int id)
    {
        return _context.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == id);
    }

    public Task<TEntity> Get<T>(int id, Expression<Func<TEntity, T>> includable = null)
    {
        var set = _context.Set<TEntity>();

        if (includable != null)
        {
            set.Include(includable);
        }

        return set.SingleOrDefaultAsync(x => x.Id == id);
    }

    public Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
    {
        return _context.Set<TEntity>().Where(filter).SingleOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<TEntity>> Find(Expression<Func<TEntity, bool>> filter)
    {
        return await _context.Set<TEntity>().Where(filter).ToListAsync();
    }

    public async Task<IReadOnlyCollection<TEntity>> Find<T>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, T>> includable = null)
    {
        var set = _context.Set<TEntity>();

        if (includable != null)
        {
            set.Include(includable);
        }

        return await set.Where(filter).ToListAsync();
    }

    public async Task<TEntity> Create(TEntity entity)
    {
        var result = await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task Delete(int id)
    {
        var entity = await Get(id);
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        return _context.SaveChangesAsync();
    }
}