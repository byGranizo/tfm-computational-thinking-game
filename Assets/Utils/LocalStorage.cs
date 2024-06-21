using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LocalStorage
{
    public static void SaveUser(FirebaseUser user)
    {
        string json = JsonConvert.SerializeObject(user);

        PlayerPrefs.SetString("currentUser", json);
    }

    public static FirebaseUser GetUser()
    {
        string savedJson = PlayerPrefs.GetString("currentUser");
        FirebaseUser user = JsonConvert.DeserializeObject<FirebaseUser>(savedJson);

        return user;
    }

    public static void SaveGame(GameType game)
    {
        //string json = JsonUtility.ToJson(game);
        string json = JsonConvert.SerializeObject(game);

        PlayerPrefs.SetString("currentGame", json);
    }

    public static GameType GetGame()
    {
        string savedJson = PlayerPrefs.GetString("currentGame");
        GameType game = JsonConvert.DeserializeObject<GameType>(savedJson);

        return game;
    }

    public static void DeleteGame()
    {
        PlayerPrefs.DeleteKey("currentGame");
    }
}
