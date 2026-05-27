using Napoleon.Fos.Application.Extensions;
using Napoleon.Fos.Contract.Customers;
using Napoleon.Fos.Core.Settings;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Fos.Domain.Entities;
using Napoleon.Shared.Core.Requests.Commands;

namespace Napoleon.Fos.Application.Customers.Auth.Commands;

public record LoginCommand(CustomerAuthDto CustomerAuthDto) : ICommand<string>;

public class LoginCommandHandler(IAppUnitOfWork appUnitOfWork, JwtSettings jwtSettings) : ICommandHandler<LoginCommand, string>
{
    public async Task<string> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var query = appUnitOfWork.CustomerRepository.GetQueryable()
            .Where(x => x.Email.ToLower() == command.CustomerAuthDto.Email.ToLower() && x.Password.ToLower() == command.CustomerAuthDto.Password.ComputeSHA512().ToLower())
            .Select(x => new CustomerDto(x.Id, x.Name, x.Age, x.Email));            

        var customerData = await appUnitOfWork.CustomerRepository.GetSingleTypeAsync(query, cancellationToken);

        if (customerData is null)
            return string.Empty;

        var token = customerData.GetCustomerAccessToken(jwtSettings);

        var authenTokenEntity = new AuthenticationToken
        {
            Id = token.Id,
            CustomerId = customerData.Id!.Value,
            Token = token.Token,
            ValidTill = DateTimeOffset.Now.AddDays(jwtSettings.ValidationLengthInDays)
        };

        var result = await appUnitOfWork.AuthenticationTokenRepository.AddAndSaveAsync(authenTokenEntity, cancellationToken);

        return result.Token;
    }
}
