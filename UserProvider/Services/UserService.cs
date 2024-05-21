using Microsoft.Extensions.Logging;
using UserProvider.Entities;
using UserProvider.Factories;
using UserProvider.Helpers;
using UserProvider.Models;
using UserProvider.Repositories;

namespace UserProvider.Services;

public class UserService(UserFactory userFactory, ILogger<UserService> logger, UserRepository repo)
{
    private readonly UserFactory _userFactory = userFactory;
    private readonly ILogger<UserService> _logger = logger;
    private readonly UserRepository _repo = repo;


    public async Task<ResponseResult> DeleteUserAsync(AspNetUser user)
    {
        try
        {
            var deleteResult = await _repo.DeleteAsync(x => x.Id == user.Id);
            if (deleteResult.StatusCode == StatusCode.OK)
                return ResponseFactory.Ok();
            else if(deleteResult.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound();
            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR :: UserProvider.UserService.DeleteUserAsync() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
    public async Task<ResponseResult> UpdateUserAsync(AspNetUser user)
    {
        try
        {
            var updateResult = await _repo.UpdateAsync(x => x.Id == user.Id, user);
            if (updateResult.StatusCode == StatusCode.OK)
                return ResponseFactory.Ok((AspNetUser)updateResult.ContentResult!);
            else if(updateResult.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound();
            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR :: UserProvider.UserService.GetOneUserAsync() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
    public async Task<ResponseResult> GetOneUserAsync(string email)
    {
        try
        {
            var result = await _repo.GetAsync(x => x.Email == email);
            if (result.StatusCode == StatusCode.OK)
                return ResponseFactory.Ok((AspNetUser)result.ContentResult!);
            else if (result.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound();
            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR :: UserProvider.UserService.GetOneUserAsync() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
    public async Task<ResponseResult> GetAllUsersAsync()
    {
        try 
        {
            var result = await _repo.GetAllAsync();
            if (result.StatusCode == StatusCode.OK)
                return ResponseFactory.Ok((IEnumerable<AspNetUser>)result.ContentResult!);
            else if (result.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound();
            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR :: UserProvider.UserService.GetAllUsersAsync() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
    public ResponseResult ValidateUserEntity(AspNetUser user)
    {
        try
        {
            var validator = new UserValidator();
            var validationResult = validator.Validate(user);
            if (validationResult.IsValid)
                return ResponseFactory.Ok();
            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR :: UserProvider.UserService.ValidateUserEntity() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
    public ResponseResult BodyChecker(string body)
    {
        var userEntity = _userFactory.PopulateUserEntity(body);
        if (userEntity != null)
            return ResponseFactory.Ok(userEntity);
        return ResponseFactory.Error();
    }
}
