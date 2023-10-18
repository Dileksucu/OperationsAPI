namespace OperationsAPI.Models
{
    public record UpdateOperationPatchCommand(string? FullName, string? Email, long? Phone, string? Address)
    {
    }
}
