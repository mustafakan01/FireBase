using Firebase;
using Firebase.Auth;
using UnityEngine;
using TMPro;

public class AuthenticationManager : MonoBehaviour
{
    public TMP_InputField emailInputField;  // Email için TMP_InputField kullanıyoruz
    public TMP_InputField passwordInputField;  // Şifre için TMP_InputField kullanıyoruz
    public TMP_Text statusText;  // Durum metni için TMP_Text kullanıyoruz

    private FirebaseAuth auth;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;

            // Firebase Authentication başlatıldıktan sonra devam eden kodlar buraya gelebilir
        });
    }

    public void Login()
    {
        if (emailInputField != null && passwordInputField != null)
        {
            string email = emailInputField.text;
            string password = passwordInputField.text;

            auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
                if (task.IsCanceled || task.IsFaulted)
                {
                    statusText.text = "Giriş başarısız. Email ve şifrenizi kontrol edin.";
                    return;
                }

                Firebase.Auth.FirebaseUser user = task.Result.User;
                statusText.text = "Olarak giriş yaptınız: " + user.Email;
            });
        }
        else
        {
            Debug.LogError("Input alanları atanmamış.");
        }
    }

    public void Register()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                statusText.text = "Kayıt başarısız. Lütfen tekrar deneyin.";
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result.User;
            statusText.text = "Olarak kaydoldunuz ve giriş yaptınız: " + newUser.Email;
        });
    }
}
