namespace MiniTrello.Domain.Services
{
    public interface IRegisterValidator<T>
    {
        string Validate(T model);
    }
}