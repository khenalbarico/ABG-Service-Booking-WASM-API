using CommonLib1.Models.Client;

namespace CommonLib1.Models.Admin;

public class ClientServiceRow
{
    public required ClientRequest Request { get; init; }
    public required ClientService Service { get; init; }
}
