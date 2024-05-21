using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using UserProvider.Contexts;
using UserProvider.Entities;
using UserProvider.Models;

namespace UserProvider.Repositories;

public class UserRepository(DataContext context, ILogger<UserRepository> logger) : BaseRepo<AspNetUser>(context, logger)
{
    private readonly DataContext _context = context;
    private readonly ILogger _logger;
}
