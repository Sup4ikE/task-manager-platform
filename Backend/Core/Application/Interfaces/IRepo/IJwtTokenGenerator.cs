using TaskManager_API.Core.Domain;

namespace TaskManager_API.Core.Application.Interfaces;

public interface IJwtTokenGenerator
{ 
    public string CreateToken(User user);
}