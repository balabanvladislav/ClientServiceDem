using Microsoft.AspNetCore.Builder;

namespace Features.Abstract;

public interface IEndpoint
{
    void MapEndpoint(WebApplication app);
}