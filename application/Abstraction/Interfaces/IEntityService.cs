using application.MVVM.Model;

using CSharpFunctionalExtensions;

namespace application.Abstraction.Interfaces;

public interface IEntityService
{
	Task<Result> Login(string email, string password);
	Task<Result<EntityModel>> Registration(EntityModel model);
}	