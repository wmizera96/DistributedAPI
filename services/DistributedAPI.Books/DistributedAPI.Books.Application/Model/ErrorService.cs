namespace DistributedAPI.Books.Application.Model;

public interface IErrorService
{
    Task GenericErrorAsync();
    Task CommonErrorAsync(string name);
}

public class ErrorService : IErrorService
{
    public Task GenericErrorAsync()
    {
        throw new Exception("Generic Error");
    }

    public Task CommonErrorAsync(string name)
    {
        throw BookValidationError.ForInvalidTitle(name);
    }
}