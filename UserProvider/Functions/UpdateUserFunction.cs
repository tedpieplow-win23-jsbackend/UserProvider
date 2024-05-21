using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using UserProvider.Entities;
using UserProvider.Models;
using UserProvider.Services;

namespace UserProvider.Functions
{
    public class UpdateUserFunction(ILogger<UpdateUserFunction> logger, UserService userService)
    {
        private readonly ILogger<UpdateUserFunction> _logger = logger;
        private readonly UserService _userService = userService;

        [Function("UpdateUserFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            try
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();
                if(body != null)
                {
                    var entityResult = _userService.BodyChecker(body);
                    if(entityResult.StatusCode == StatusCode.OK) 
                    {
                        var updateResult = await _userService.UpdateUserAsync((AspNetUser)entityResult.ContentResult!);
                        if (updateResult.StatusCode == StatusCode.OK)
                            return new OkObjectResult((AspNetUser)updateResult.ContentResult!);
                        else if (updateResult.StatusCode == StatusCode.NOT_FOUND)
                            return new NotFoundResult();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"ERROR :: UserProvider.UpdateUserFunction.Run() : {ex.Message}");
            }
            return new BadRequestResult();
        }
    }
}
