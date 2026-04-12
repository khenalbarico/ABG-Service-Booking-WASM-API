namespace CommonLib1.Models.Authentication;

public class AuthResp
{
    public bool   IsAuthenticated { get; set; }
    public string Uid             { get; set; } = "";
    public string Email           { get; set; } = "";
}

