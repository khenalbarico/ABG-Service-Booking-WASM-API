namespace CommonLib1.Models.States;

public sealed class AdminAuthState
{
    public bool   IsAuthenticated { get; set; }
    public string Email           { get; set; } = "";
}
