using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using UserProvider.Entities;
using UserProvider.Models;
using UserProvider.Services;

namespace UserProvider.Functions;

public class GetUsersFunction(ILogger<GetUsersFunction> logger, UserService userService)
{
    private readonly UserService _userService = userService;
    private readonly ILogger<GetUsersFunction> _logger = logger;

    [Function("GetUsersFunction")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
		try
		{
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            if (body != null)
            {
                var testResult = _userService.BodyChecker(body);
                if(testResult.StatusCode == StatusCode.OK)
                {
                    var entity = (AspNetUser)testResult.ContentResult!;
                    var validationResult = _userService.ValidateUserEntity(entity);
                    if(validationResult.StatusCode == StatusCode.OK)
                    {
                        var getResult = await _userService.GetOneUserAsync(entity.Email!);
                        if(getResult.StatusCode == StatusCode.OK)
                            return new OkObjectResult((AspNetUser)getResult.ContentResult!);
                        else if(getResult.StatusCode == StatusCode.NOT_FOUND)
                            return new NotFoundResult();
                    }
                }
                else if(testResult.StatusCode == StatusCode.ERROR)
                {
                    var getAllResult = await _userService.GetAllUsersAsync();
                    if(getAllResult.StatusCode == StatusCode.OK)
                        return new OkObjectResult((IEnumerable<AspNetUser>)getAllResult.ContentResult!);
                    else if(getAllResult.StatusCode == StatusCode.NOT_FOUND)
                        return new NotFoundResult();
                }
            }
		}
		catch (Exception ex)
		{
            _logger.LogDebug($"ERROR :: UserProvider.GetUserFunction.Run() : {ex.Message}");
		}
        return new BadRequestResult();
    }
}
