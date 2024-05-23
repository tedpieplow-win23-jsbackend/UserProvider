using Newtonsoft.Json;
using UserProvider.Entities;
using UserProvider.Models;

namespace UserProvider.Factories;

public class UserFactory
{
    public AspNetUser PopulateUserEntity(string body)
    {
        return JsonConvert.DeserializeObject<AspNetUser>(body)!;
    }
    public AspNetUser PopulateUserEntity(AspNetUser entity, UpdateModel model)
    {
        entity.FirstName = model.FirstName;
        entity.LastName = model.LastName;
        entity.Email = model.Email;
        entity.UserName = model.Email;
        entity.NormalizedEmail = model.Email.ToUpper();
        entity.NormalizedUserName = model.Email.ToUpper();
        return entity;
    }
    public UpdateModel PopulateUpdateModel(string body)
    {
        return JsonConvert.DeserializeObject<UpdateModel>(body)!;
    }
}
