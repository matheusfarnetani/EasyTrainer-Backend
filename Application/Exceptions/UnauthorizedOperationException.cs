using System;

namespace Application.Exceptions
{
    public class UnauthorizedOperationException(string message) : Exception(message)
    {
    }
}
