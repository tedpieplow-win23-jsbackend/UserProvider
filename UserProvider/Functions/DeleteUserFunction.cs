using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using UserProvider.Entities;
using UserProvider.Models;
using UserProvider.Services;

namespace UserProvider.Functions
{
    public class DeleteUserFunction(ILogger<DeleteUserFunction> logger, UserService userService)
    {
        private readonly ILogger<DeleteUserFunction> _logger = logger;
        private readonly UserService _userService = userService;

        [Function("DeleteUserFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            try
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();
                if (body != null)
                {
                    var entityResult = _userService.BodyChecker(body);
                    if(entityResult.StatusCode == StatusCode.OK)
                    {
                        var entity = (AspNetUser)entityResult.ContentResult!;
                        var deleteResult = await _userService.DeleteUserAsync(entity);
                        if (deleteResult.StatusCode == StatusCode.OK)
                            return new OkResult();
                        else if (deleteResult.StatusCode == StatusCode.NOT_FOUND)
                            return new NotFoundResult();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"ERROR :: UserProvider.DeleteUserFunction.Run() : {ex.Message}");
            }
            return new BadRequestResult();
        }
    }
}
