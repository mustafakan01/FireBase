using UnityEngine;
using TMPro;

public class SignUpUI : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public FirebaseAuthManager authManager;

    public void SignUpButtonClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        authManager.SignUp(email, password);
    }
}
