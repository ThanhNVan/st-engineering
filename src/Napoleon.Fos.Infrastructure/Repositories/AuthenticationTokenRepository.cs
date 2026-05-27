using Napoleon.Fos.Core.Repositories;
using Napoleon.Fos.Domain.Entities;
using Napoleon.Fos.Infrastructure.AppDbContexts;
using Napoleon.Shared.Infrastructure.Repositories;

namespace Napoleon.Fos.Infrastructure.Repositories;

class AuthenticationTokenRepository(AppDbContext dbContexts)
    : BaseRepository<AuthenticationToken>(dbContexts), IAuthenticationTokenRepository
{
}
