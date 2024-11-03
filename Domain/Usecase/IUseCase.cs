namespace Domain.Usecase;

public interface IUseCase<in TRequest, TResponse>
{
    Task<TResponse> Execute(TRequest request);
}