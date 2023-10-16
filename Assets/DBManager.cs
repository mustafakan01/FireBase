using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;

public class DBManager : MonoBehaviour
{
   public DatabaseReference usersRef;
   public InputField usernameInput,passwordInput;
    void Start()
    {
        StartCoroutine(Initialization());      
    }

    private IEnumerator Initialization()
    {
        var task=FirebaseApp.CheckAndFixDependenciesAsync();
        while(!task.IsCompleted)
        {
            yield return null;
        }

        if (task.IsCanceled || task.IsFaulted)
        {
            Debug.LogError("DataBase Error:" + task.Exception);
        }

        var dependencyStatus=task.Result;
        if (dependencyStatus==DependencyStatus.Available)
        {
            usersRef=FirebaseDatabase.DefaultInstance.GetReference("Users");
            Debug.Log("init completed");
        }
        
        else
        {
            Debug.LogError("DataBase Error");
        }
    }

    public void SaveUser()
    {
        string username=usernameInput.text;
        string password=passwordInput.text;

        Dictionary<string,object> user=new Dictionary<string, object>();
        user["username"]=username;
        user["password"]=password;

        string key=usersRef.Push().Key;

        usersRef.Child(key).UpdateChildrenAsync(user);

    }



}
