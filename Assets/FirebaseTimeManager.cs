    using UnityEngine;
    using Firebase;
    using Firebase.Database;
    using Firebase.Extensions;
    using System.Collections;

    public class FirebaseTimeManager : MonoBehaviour
    {
        DatabaseReference databaseReference;
        private long gameTime = 120; // Başlangıçta 120 saniye olarak ayarlanmış zaman

        private bool isFirebaseInitialized = false; // Firebase'in başlatılıp başlatılmadığını kontrol etmek için

        private void Start()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

                // Firebase başlatıldı, bu yüzden işareti true yap
                isFirebaseInitialized = true;

                // Zamanı güncellemek için bir coroutine başlat
                StartCoroutine(UpdateTime());

                // Firebase'den zamanı al
                LoadTimeFromFirebase();
            });
        }

        private void UpdateTimeLocally()
        {
            // Zamanı her frame'de güncelleme yerine, bu fonksiyonla güncelliyoruz
            gameTime -= (long)(Time.deltaTime * 1000); // deltaTime'i milisaniyeye çevirip long türüne dönüştürüyoruz
            if (gameTime < 0)
                gameTime = 0;

            Debug.Log("Kalan zaman: " + gameTime.ToString() + " milisaniye");
        }

        private IEnumerator UpdateTime()
        {
            // Firebase başlangıcı tamamlanana kadar bekleyin
            while (!isFirebaseInitialized)
            {
                yield return null;
            }

            while (true)
            {
                // Süreyi Firebase'e güncelle
                SaveTimeToFirebase(gameTime);

                // Her saniyede bir zamanı güncelle
                yield return new WaitForSeconds(1f);

                // Zamanı lokal olarak güncelle
                UpdateTimeLocally();
            }
        }

        private void SaveTimeToFirebase(long gameTime)
        {
            if (databaseReference != null)
            {
                // Firebase Realtime Database'de süreyi güncelle
                databaseReference.Child("gameTime").SetValueAsync(gameTime);
            }
            else
            {
                Debug.LogWarning("databaseReference is null. Firebase may not be initialized yet.");
            }
        }

        private void LoadTimeFromFirebase()
        {
            if (databaseReference != null)
            {
                databaseReference.Child("gameTime").GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsCompleted && !task.IsFaulted && task.Result.Exists)
                    {
                        long loadedTime = (long)task.Result.Value;
                        Debug.Log("Loaded game time from Firebase: " + loadedTime);
                    }
                    else
                    {
                        Debug.LogError("Failed to load game time from Firebase: " + task.Exception);
                    }
                });
            }
            else
            {
                Debug.LogWarning("databaseReference is null. Firebase may not be initialized yet.");
            }
        }
    }
