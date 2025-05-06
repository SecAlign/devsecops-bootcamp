// // Created On: 2025.05.06
// // Create by: althunibat

namespace Bootcamp.WebApi.Data.Model;

public class User {
    public int Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public uint RowVersion { get; init; }
}