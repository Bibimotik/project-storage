namespace Storage.Application.Exceptions;

public class ExistsException(string message) : SystemException(message)
{
}