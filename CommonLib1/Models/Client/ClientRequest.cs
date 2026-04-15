using CommonLib1.Models.PolicyForms;
using static CommonLib1.Models.Constants;

namespace CommonLib1.Models.Client;

public sealed class ClientRequest
{
    public ClientInformation   ClientInformation { get; set; } = new();
    public List<ClientService> ClientServices    { get; set; } = [];
    public ConsentModel        ClientConsent     { get; set; } = new();
    public ClientStatus        Status            { get; set; } = ClientStatus.Pending;
}
