namespace QueueOrderHub.Shared.Errors.Models
{
    public class NotFoundExeption : ApplicationException
    {
        public NotFoundExeption(string name, object key) : base($"{name} with : ({key} is not Found)")
        {

        }
    }
}
