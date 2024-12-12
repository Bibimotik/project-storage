namespace Storage.Application.Exceptions;

public class NotFoundException(string message) : SystemException(message)
{
}
