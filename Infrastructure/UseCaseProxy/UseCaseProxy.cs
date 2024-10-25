using taf_server.Domain.Usecase;

namespace taf_server.Infrastructure.UseCaseProxy;

public class UseCaseProxy<T> where T : IUseCase<object>
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