using Napoleon.Fos.Application.Extensions;
using Napoleon.Fos.Contract.Customers;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Fos.Domain.Entities;
using Napoleon.Shared.Core.Requests.Commands;

namespace Napoleon.Fos.Application.Customers.Auth.Commands;

public record RegisterCommand(CustomerAuthDto CustomerAuthDto) : ICommand<bool>;

public class RegisterCommandHanlder(IAppUnitOfWork appUnitOfWork) : ICommandHandler<RegisterCommand, bool>
{
    public async Task<bool> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var isExitedEmail = await appUnitOfWork.CustomerRepository.IsAnyAsync(x => x.Email == command.CustomerAuthDto.Email, cancellationToken);
        
        if (isExitedEmail)
            return false;

        var newCustomer = new Customer
        {
            Email = command.CustomerAuthDto.Email,
            Password = command.CustomerAuthDto.Password.ComputeSHA512(),
            Age = 0,
            Name = ""
        };

        await appUnitOfWork.CustomerRepository.AddAndSaveAsync(newCustomer, cancellationToken);

        return true;
    }

    
}

