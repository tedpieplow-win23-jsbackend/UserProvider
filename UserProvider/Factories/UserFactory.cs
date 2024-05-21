using Newtonsoft.Json;
using UserProvider.Entities;

namespace UserProvider.Factories;

public class UserFactory
{
    public AspNetUser PopulateUserEntity(string body)
    {
        return JsonConvert.DeserializeObject<AspNetUser>(body)!;
    }
}
