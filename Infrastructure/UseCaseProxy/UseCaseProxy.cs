using taf_server.Domain.Usecase;

namespace taf_server.Infrastructure.UseCaseProxy;

public class UseCaseProxy<T, TRequest, TResponse> where T : IUseCase<TRequest, TResponse>
{
    private readonly T _useCase;

    public UseCaseProxy(T useCase)
    {
        _useCase = useCase;
    }

    public T GetInstance()
    {
        return _useCase;
    }
}