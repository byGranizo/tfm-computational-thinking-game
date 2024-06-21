using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using System.Net;

public class AuthServiceCoroutines : MonoBehaviour
{
    private const string API_KEY = "AIzaSyBik9ns5y_mA64Y36Ub0fUUj32xKjTYrWs";

    private const string REGISTER_URL = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + API_KEY;
    private const string LOGIN_URL = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + API_KEY;

    public void LoginUser(string nickname)
    {
        string username = nickname.ToLower();
        string password = GenerateMD5(username);

        StartCoroutine(LoginRequest(username, password, user =>
        {
            if (user != null)
            {
                Debug.Log("Usuario existe");
                Debug.Log("email: " + user.email);
                LocalStorage.SaveUser(user);
            } else
            {
                Debug.Log("Usuario no existe");
                SignUp(username, password);
            }
        }));
    }

    public void SignUp(string username, string password)
    {
        StartCoroutine(SignUpRequest(username, password, user =>
        {
            if (user != null)
            {
                Debug.Log("Usuario creado");
                Debug.Log("email: " + user.email);
                LocalStorage.SaveUser(user);
            }
            else
            {
                Debug.Log("Usuario no creado");
            }
        }));
    }



    private IEnumerator LoginRequest(string username, string password, System.Action<FirebaseUser> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", username + "@user-only-auth.dev");
        form.AddField("password", password);
        form.AddField("returnSecureToken", "true");

        using (UnityWebRequest request = UnityWebRequest.Post(LOGIN_URL, form))
        {
            yield return request.SendWebRequest();

            if (!string.IsNullOrWhiteSpace(request.error))
            {
                Debug.Log(request.error);
                callback?.Invoke(null);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                FirebaseUser user = JsonUtility.FromJson<FirebaseUser>(jsonResponse);
                callback?.Invoke(user);
            }
        }
    }
    
    private IEnumerator SignUpRequest(string username, string password, System.Action<FirebaseUser> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", username + "@user-only-auth.dev");
        form.AddField("password", password);
        form.AddField("returnSecureToken", "true");

        using (UnityWebRequest request = UnityWebRequest.Post(REGISTER_URL, form))
        {
            yield return request.SendWebRequest();

            if (!string.IsNullOrWhiteSpace(request.error))
            {
                Debug.Log(request.error);
                callback?.Invoke(null);
            } else
            {
                string jsonResponse = request.downloadHandler.text;
                FirebaseUser user = JsonUtility.FromJson<FirebaseUser>(jsonResponse);
                callback?.Invoke(user);
            }
        }
    }


    private string GenerateMD5(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}




// https://www.youtube.com/watch?v=LE5Vd83Ed0I   ---> Firebase API con autenticacion min 35:00 mas o menos
// https://www.youtube.com/watch?v=GXvJ1RddsfQ    ---> Api rest unity

// Firebase unity https://www.youtube.com/watch?v=OvkFsAtMGVY

//https://firebase.google.com/docs/unity/setup?hl=es#desktop-workflow




/*public void GenerateRequest()
    {
        StartCoroutine(ProcessRequest(URL));
    }

    private IEnumerator ProcessRequest(string uri)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();

            if (!string.IsNullOrWhiteSpace(request.error))
            {
                Debug.Log(request.error);
            } else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }*/



    /*
    private IEnumerator GetUserRequest(string username, System.Action<bool> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", username + "@user-only-auth.dev");

        using (UnityWebRequest request = UnityWebRequest.Post(GET_USER_URL, form))
        {
            yield return request.SendWebRequest();

            if (!string.IsNullOrWhiteSpace(request.error))
            {
                Debug.Log(request.error);
                callback?.Invoke(false);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                callback?.Invoke(true);
            }
        }
    }
    
    */









