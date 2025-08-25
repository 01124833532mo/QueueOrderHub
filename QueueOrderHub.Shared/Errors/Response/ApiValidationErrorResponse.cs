using System.Text.Json;

namespace QueueOrderHub.Shared.Errors.Response
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public required IEnumerable<string> Erroes { get; set; }

        public ApiValidationErrorResponse(string message = null!) : base(400, message)
        {

        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

    }
}
