// // Created On: 2025.05.06
// // Create by: althunibat

using Bootcamp.WebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp.WebApi.Api;

public static class UsersApi {
    public static void MapUsersApi(this IEndpointRouteBuilder app)
    {
        app.MapGet("/users", GetUsers ).WithName(nameof(GetUsers)).WithOpenApi();
    }

    private static IAsyncEnumerable<UserDto> GetUsers([FromServices] ApiDbContext db) =>
        db.Users.Select(user => new UserDto {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        }).AsAsyncEnumerable();

    public readonly record struct UserDto {
        public int Id { get; init; }
        public string Email { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        
    } 
}

