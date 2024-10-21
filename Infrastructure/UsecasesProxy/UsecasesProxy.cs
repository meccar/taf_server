namespace Infrastructure.UsecasesProxy;
public class UseCaseProxy<T>
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