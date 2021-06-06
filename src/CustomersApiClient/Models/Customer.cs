namespace CustomersApiClient.Models
{
    public record Customer(
        string CustomerId,
        string FirstName,
        string LastName
    );
}
