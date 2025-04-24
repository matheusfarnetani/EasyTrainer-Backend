using System;

namespace Application.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, int id)
            : base($"Entity '{entityName}' with ID '{id}' was not found.")
        {
        }

        public EntityNotFoundException(string entityName, string compositeKey)
            : base($"Entity '{entityName}' with composite key '{compositeKey}' was not found.")
        {
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
        }
    }

}
