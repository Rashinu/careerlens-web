using CareerLens.Domain.Entities;
using CareerLens.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerLens.Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(CareerLensDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        await _dbSet.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), ct);

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct = default) =>
        await _dbSet.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, ct);

    public async Task<bool> ExistsAsync(string email, CancellationToken ct = default) =>
        await _dbSet.AnyAsync(u => u.Email == email.ToLowerInvariant(), ct);
}
