using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class AuthService
{
    private const string API_KEY = "AIzaSyBik9ns5y_mA64Y36Ub0fUUj32xKjTYrWs";

    private const string REGISTER_URL = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + API_KEY;
    private const string LOGIN_URL = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + API_KEY;

    private static HttpClient _client = new HttpClient();

    public static async Task<FirebaseUser> LoginUser(string nickname)
    {
        //string username = nickname.ToLower();
        string username = nickname.Replace(" ", "").ToLower();
        string password = GenerateMD5(username);

        FirebaseUser userLogin = await LoginRequest(username, password);
        if (userLogin != null)
        {
            Debug.Log("Usuario existe");
            Debug.Log("email: " + userLogin.email);

            return userLogin;
        }

        Debug.Log("Usuario no existe");

        FirebaseUser userRegister = await SignUpRequest(username, password);
        if (userRegister != null)
        {
            Debug.Log("Usuario creado");
            Debug.Log("email: " + userRegister.email);

            return userRegister;
        }

        Debug.Log("Usuario no creado");
        return null;
    }

    private static async Task<FirebaseUser> LoginRequest(string username, string password)
    {
        FormUrlEncodedContent form = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("email", username + "@user-only-auth.dev"),
            new KeyValuePair<string, string>("password", password),
            new KeyValuePair<string, string>("returnSecureToken", "true")
        });

        HttpResponseMessage response = await _client.PostAsync(LOGIN_URL, form);
        if (!response.IsSuccessStatusCode)
        {
            Debug.Log(await response.Content.ReadAsStringAsync());
            return null;
        }
        else
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            FirebaseUser user = JsonUtility.FromJson<FirebaseUser>(jsonResponse);
            return user;
        }
    }

    private static async Task<FirebaseUser> SignUpRequest(string username, string password)
    {
        FormUrlEncodedContent form = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("email", username + "@user-only-auth.dev"),
            new KeyValuePair<string, string>("password", password),
            new KeyValuePair<string, string>("returnSecureToken", "true")
        });

        HttpResponseMessage response = await _client.PostAsync(REGISTER_URL, form);
        if (!response.IsSuccessStatusCode)
        {
            Debug.Log(await response.Content.ReadAsStringAsync());
            return null;
        }
        else
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            FirebaseUser user = JsonUtility.FromJson<FirebaseUser>(jsonResponse);
            return user;
        }
    }

    private static string GenerateMD5(string input)
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