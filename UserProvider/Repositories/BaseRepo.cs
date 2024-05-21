﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;
using UserProvider.Contexts;
using UserProvider.Factories;
using UserProvider.Models;
using static Grpc.Core.Metadata;

namespace UserProvider.Repositories;

public class BaseRepo<TEntity>(DataContext context, ILogger<BaseRepo<TEntity>> logger) where TEntity : class
{
    private readonly DataContext _context = context;
    private readonly ILogger<BaseRepo<TEntity>> _logger = logger;

    public virtual async Task<ResponseResult> CreateAsync(TEntity entity)
    {
        try
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return ResponseFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR :: UserProvider.BaseRepo.CreateAsync() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
    public virtual async Task<ResponseResult> GetAllAsync()
    {
        try
        {
            IEnumerable<TEntity> list = await _context.Set<TEntity>().ToListAsync();
            if (!list.Any())
                return ResponseFactory.NotFound();
            return ResponseFactory.Ok(list);
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR :: UserProvider.BaseRepo.GetAllAsync() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
    public virtual async Task<ResponseResult> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if(entity == null)
                return ResponseFactory.NotFound();
            return ResponseFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR :: UserProvider.BaseRepo.GetAsync() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
    public virtual async Task<ResponseResult> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity)
    {
        try
        {
            var existsResult = await ExistsAsync(predicate);
            if(existsResult.StatusCode == StatusCode.EXISTS)
            {
                _context.Set<TEntity>().Update(entity);
                await _context.SaveChangesAsync();
                return ResponseFactory.Ok(entity);
            }
            else if(existsResult.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound();
            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR :: UserProvider.BaseRepo.UpdateAsync() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
    public virtual async Task<ResponseResult> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var result = await GetAsync(predicate);
            if(result.StatusCode == StatusCode.OK)
            {
                var entity = (TEntity)result.ContentResult!;
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
                return ResponseFactory.Ok();
            }
            else if (result.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound();
            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR :: UserProvider.BaseRepo.DeleteAsync() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
    public virtual async Task<ResponseResult> ExistsAsync (Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            if (await _context.Set<TEntity>().AnyAsync(predicate))
                return ResponseFactory.Exists();
            return ResponseFactory.NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR :: UserProvider.BaseRepo.ExistsAsync() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
}
