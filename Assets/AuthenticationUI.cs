using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Auth;

public class AuthenticationUI : MonoBehaviour
{
    public TMP_InputField InputFieldEmail; // TextMeshPro InputField
    public TMP_InputField InputFieldPassword; // TextMeshPro InputField
    public TMP_Text StatusText; // TextMeshPro Text

    [SerializeField]    
    private FirebaseAuth auth;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
        });
    }

    public void OnLoginButtonClicked()
    {
        string email = InputFieldEmail.text;
        string password = InputFieldPassword.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                StatusText.text = "Login failed. Check email and password.";
                return;
            }

            Firebase.Auth.FirebaseUser user = task.Result.User;
            StatusText.text = "Logged in as: " + user.Email;
        });
    }

    public void OnRegisterButtonClicked()
    {
        string email = InputFieldEmail.text;
        string password = InputFieldPassword.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                StatusText.text = "Registration failed. Please try again.";
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result.User;
            StatusText.text = "Registered and logged in as: " + newUser.Email;
        });
    }
}
