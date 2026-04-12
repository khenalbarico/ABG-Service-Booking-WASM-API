using Firebase.Auth;

namespace ToolsLib1.FirebaseTools;

public interface IToolFirebaseAuth
{
    Task<UserCredential> SignInAsync(
        string email,
        string password);
    Task SignUpAsync(
        string email,
        string password);
    void SignOut();
}