using UnityEngine;
using Firebase;
using Firebase.Messaging;


//FCM 토큰 생성 및 삭제
//푸시 알림 recieve
public class FirebaseManager : MonoBehaviour
{
    FirebaseApp _app;

    void Start()
    {

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                _app = FirebaseApp.DefaultInstance;

                FirebaseMessaging.TokenReceived += OnTokenReceived;

                FirebaseMessaging.MessageReceived += OnMessageReceived;
                Debug.Log("Firebase 연결 체크");
            }
            else
            {
                Debug.LogError("[FIREBASE] Could not resolve all dependencies: " + task.Result);
            }
        });


        FirebaseMessaging.GetTokenAsync().ContinueWith(token =>
        {
            Debug.Log("FCM Token : " + token.Result);
            //Managers.Player.SetString(Define.DEVICETOKEN, token.Result);
        });

        
    }

    void OnTokenReceived(object sender, TokenReceivedEventArgs e)
    {
        if (e != null)
        {
            Debug.LogFormat("[FIREBASE] Token: {0}", e.Token);
            Managers.Player.SetString(Define.DEVICETOKEN, e.Token);
        }
    }

    void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        if (e != null && e.Message != null && e.Message.Notification != null)
        {
            Debug.LogFormat("[FIREBASE] From: {0}, Title: {1}, Text: {2}",
                e.Message.From,
                e.Message.Notification.Title,
                e.Message.Notification.Body);
        }
    }

    public void DeleteToken()
    {
        FirebaseMessaging.DeleteTokenAsync().ContinueWith(task=>
        {
            Debug.Log("FCM Token Delete");
        });
    }

    public void GetToken()
    {
        FirebaseMessaging.GetTokenAsync().ContinueWith(token =>
        {
            Debug.Log("FCM Token : " + token.Result);
            //Managers.Player.SetString(Define.DEVICETOKEN, token.Result);
        });
    }
}
