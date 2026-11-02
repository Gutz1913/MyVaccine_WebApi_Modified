using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using System.Linq.Expressions;

namespace MyVaccine.WebApi.Repositories.Implementations;

public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
{
    private readonly MyVaccineAppDbContext _context;

    public BaseRepository(MyVaccineAppDbContext context)
    {
        _context = context;
    }

    public async Task Add(T entity)
    {
        var UpdatedAt = entity.GetType().GetProperty("UpdatedAt");
        if (UpdatedAt != null) entity.GetType().GetProperty("UpdatedAt").SetValue(entity, DateTime.UtcNow);

        var CreatedAt = entity.GetType().GetProperty("CreatedAt");
        if (CreatedAt != null) entity.GetType().GetProperty("CreatedAt").SetValue(entity, DateTime.UtcNow);

        await _context.AddAsync(entity);
        _context.Entry(entity).State = EntityState.Added;
        await _context.SaveChangesAsync();
    }

    public async Task AddRange(List<T> entity)
    {
        entity = entity.Select(x =>
        {
            if (x.GetType().GetProperty("UpdatedAt") != null) x.GetType().GetProperty("UpdatedAt").SetValue(x, DateTime.UtcNow);
            if (x.GetType().GetProperty("CreatedAt") != null) x.GetType().GetProperty("CreatedAt").SetValue(x, DateTime.UtcNow);
            return x;
        }).ToList();

        _context.AddRange(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        var UpdatedAt = entity.GetType().GetProperty("UpdatedAt");
        if (UpdatedAt != null) entity.GetType().GetProperty("UpdatedAt").SetValue(entity, DateTime.UtcNow);
        // _context.Entry(entity).State = EntityState.Modified;

        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRange(List<T> entity)
    {
        entity = entity.Select(x =>
        {
            if (x.GetType().GetProperty("UpdatedAt") != null) x.GetType().GetProperty("UpdatedAt").SetValue(x, DateTime.UtcNow);
            return x;
        }).ToList();

        _context.UpdateRange(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRange(List<T> entity)
    {
        _context.RemoveRange(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Patch(T entity)
    {
        var entry = _context.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            // Assuming that the entity has an Id property
            var key = entity.GetType().GetProperty("Id").GetValue(entity, null);
            var originalEntity = await _context.Set<T>().FindAsync(key);

            // Atach the original entity and set values from the incoming entity
            entry = _context.Entry(originalEntity);
            entry.CurrentValues.SetValues(entity);
        }

        // Update the UpdatedAt property if it exists
        var updatedAtProperty = entity.GetType().GetProperty("UpdatedAt");
        if (updatedAtProperty != null)
        {
            updatedAtProperty.SetValue(entity, DateTime.UtcNow);
            entry.Property("UpdatedAt").IsModified = true;
        }

        // Get a list of properties that hav been modified
        var changedProperties = entry.Properties
            .Where(p => p.IsModified)
            .Select(p => p.Metadata.Name);

        // Ensure that only the changed properties will be updated
        foreach (var name in changedProperties)
        {
            entry.Property(name).IsModified = true;
        }

        // Save changes
        await _context.SaveChangesAsync();
    }

    public async Task PatchRange(List<T> entities)
    {
        foreach (var entity in entities)
        {
            var entry = _context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                // Assuming that the entity has an Id property
                var key = entity.GetType().GetProperty("Id").GetValue(entity, null);
                var originalEntity = await _context.Set<T>().FindAsync(key);

                // Attach the original entity and set values from the incoming entity
                entry = _context.Entry(originalEntity);
                entry.CurrentValues.SetValues(entity);
            }

            // Update the UpdatedAt property if it exists
            var updatedAtProperty = entity.GetType().GetProperty("UpdatedAt");
            if (updatedAtProperty != null)
            {
                updatedAtProperty.SetValue(entity, DateTime.UtcNow);
                entry.Property("UpdatedAt").IsModified = true;
            }

            // Get a list of properties that hav been modified
            var changedProperties = entry.Properties
                .Where(p => p.IsModified)
                .Select(p => p.Metadata.Name);

            // Ensure that only the changed properties will be updated
            foreach (var name in changedProperties)
            {
                entry.Property(name).IsModified = true;
            }
        }

        // Save Changes
        await _context.SaveChangesAsync();
    }

    public IQueryable<T> GetAll()
    {
        var entitySet = _context.Set<T>();
        return entitySet.AsQueryable();
    }

    public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
    {
        return GetAll().Where(predicate);
    }

    public IQueryable<T> FindByAsNoTracking(Expression<Func<T, bool>> predicate)
    {
        return GetAll().AsNoTracking().Where(predicate);
    }
}