using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class LocalStorage
{
    public static void SaveUser(FirebaseUser user)
    {
        string json = JsonConvert.SerializeObject(user);

        PlayerPrefs.SetString("currentUser", json);
    }

    public static FirebaseUser GetUser()
    {
        string savedJson = PlayerPrefs.GetString("currentUser");
        if (string.IsNullOrEmpty(savedJson)) return null;

        FirebaseUser user = JsonConvert.DeserializeObject<FirebaseUser>(savedJson);
        return user;
    }

    public static void DeleteUser()
    {
        PlayerPrefs.DeleteKey("currentUser");
    }

    public static void SaveGame(GameType game)
    {
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
