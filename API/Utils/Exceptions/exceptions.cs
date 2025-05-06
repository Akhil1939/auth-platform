namespace API.Utils.Exceptions;

public class NotFoundException(string message) : Exception(message)
{
}

public class UnauthorizedAccessException(string message) : Exception(message)
{
}
