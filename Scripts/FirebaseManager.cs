using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var app = FirebaseApp.DefaultInstance;
            });
    }
}
