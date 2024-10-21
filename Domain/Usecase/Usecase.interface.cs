namespace Domain.Usecase;
public interface IUseCase<TModel>
{
    Task<TModel> Execute(params object[] args);
}