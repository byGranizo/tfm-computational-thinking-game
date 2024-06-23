using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Linq;
using UnityEditor.Tilemaps;


public static class DataService
{
  private const string API_KEY = "AIzaSyBik9ns5y_mA64Y36Ub0fUUj32xKjTYrWs";

  //  https://firestore.googleapis.com/v1/projects/tfm-ct-dashboard/databases/(default)/documents/[documento]
  private const string URL = "https://firestore.googleapis.com/v1/projects/tfm-ct-dashboard/databases/(default)/documents/";

  private static HttpClient _client = new HttpClient();


  /* TEST REQUESTS */
  public static async Task TestPostRequest(string labelValue)
  {
    var fields = new Dictionary<string, object>
    {
      ["label"] = new { stringValue = labelValue },
      ["name"] = new { stringValue = "nameValue" }
    };

    await SendPostRequest(fields, "test");
  }

  public static async Task TestPatchRequest(string labelValue)
  {
    var fields = new Dictionary<string, object>
    {
      ["name"] = new { stringValue = "nuevonuevonuevonuevo" }
    };

    await SendPatchRequest(fields, "test", "ZerogRu2ukUC3Pn3ot0b");
  }


  /* POST REQUESTS */
  public static async Task PostTurn(TurnType turnType)
  {
    GameType currentGame = LocalStorage.GetGame();

    var fields = new Dictionary<string, object>
    {
      ["game_id"] = new { stringValue = currentGame.Id },
      ["turn_n"] = new { integerValue = turnType.NTurn },
      ["turn_duration"] = new { doubleValue = turnType.TurnDuration },
    };

    string documentId = await SendPostRequest(fields, "turn");
    Debug.Log("Turn posted: " + documentId);
  }

  public static async Task PostCompletedMission(CardType cardType)
  {
    GameType currentGame = LocalStorage.GetGame();

    var fields = new Dictionary<string, object>
    {
      ["game_id"] = new { stringValue = currentGame.Id },
      ["turn_start"] = new { integerValue = cardType.TurnStart },
      ["turn_end"] = new { integerValue = cardType.TurnEnd },
      ["difficulty"] = new { stringValue = cardType.MissionType.ToString() },
      ["biome"] = new { stringValue = cardType.Biome.ToString() },
      ["date"] = new { timestampValue = DateTime.Now.ToUniversalTime().ToString("o") },
    };

    string documentId = await SendPostRequest(fields, "completed_mission");
    Debug.Log("Mission completed: " + documentId);
  }

  public static async Task<string> PostStartGame(GameType gameType)
  {
    var fields = new Dictionary<string, object>
    {
      ["date_start"] = new { timestampValue = gameType.StartDateTime.ToUniversalTime().ToString("o") },
    };

    string documentId = await SendPostRequest(fields, "game");
    Debug.Log("Game started: " + documentId);

    return documentId;
  }


  /* PATCH REQUESTS */
  public static async Task PatchEndGame(GameType gameType)
  {
    string documentId = gameType.Id;
    Debug.Log("Document ID: " + documentId);

    var fields = new Dictionary<string, object>
    {
      ["date_end"] = new { timestampValue = gameType.EndDateTime.ToUniversalTime().ToString("o") },
      ["n_turns"] = new { integerValue = gameType.NTurns },
      ["n_tiles"] = new { integerValue = gameType.NTiles },
      ["n_missions"] = new { integerValue = gameType.NMissions },
      ["result"] = new { booleanValue = gameType.Result == EndGameUIState.Win },
    };

    await SendPatchRequest(fields, "game", documentId);

    Debug.Log("Game ended: " + documentId);
  }


  /* PRIVATE METHODS */
  private static async Task<string> SendPostRequest(Dictionary<string, object> fields, string collection)
  {
    FirebaseUser user = LocalStorage.GetUser();

    if (user == null)
    {
      Debug.Log("No hay usuario");
      return null;
    }

    var fieldsWithUid = new Dictionary<string, object>(fields)
    {
      ["uid"] = new { stringValue = user.localId }
    };

    var data = new { fields = fieldsWithUid };
    string json = JsonConvert.SerializeObject(data);

    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var request = new HttpRequestMessage()
    {
      RequestUri = new Uri(URL + collection + "?key=" + API_KEY),
      Method = HttpMethod.Post,
      Headers = {
                { HttpRequestHeader.Authorization.ToString(), "Bearer " + user.idToken }
            },
      Content = content
    };

    HttpResponseMessage response = await _client.SendAsync(request);

    if (!response.IsSuccessStatusCode)
    {
      Debug.Log(await response.Content.ReadAsStringAsync());
      return null;
    }
    else
    {
      Debug.Log("Solicitud POST exitosa");
      string responseContent = await response.Content.ReadAsStringAsync();

      var responseJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
      string documentId = responseJson["name"].ToString().Split("/")[^1];

      return documentId;
    }
  }

  private static async Task SendPatchRequest(Dictionary<string, object> fields, string collection, string documentId)
  {
    FirebaseUser user = LocalStorage.GetUser();

    if (user == null)
    {
      Debug.Log("No hay usuario");
      return;
    }

    var data = new { fields };

    string json = JsonConvert.SerializeObject(data);

    var content = new StringContent(json, Encoding.UTF8, "application/json");

    string fieldsToUpdate = string.Join("&", fields.Keys.Select(k => $"updateMask.fieldPaths={k}"));


    Debug.Log(fieldsToUpdate);

    var request = new HttpRequestMessage()
    {
      RequestUri = new Uri($"{URL}{collection}/{documentId}?key={API_KEY}&{fieldsToUpdate}"),
      Method = new HttpMethod("PATCH"),
      Headers = {
          { HttpRequestHeader.Authorization.ToString(), "Bearer " + user.idToken }
        },
      Content = content
    };

    HttpResponseMessage response = await _client.SendAsync(request);

    if (!response.IsSuccessStatusCode)
    {
      Debug.Log(await response.Content.ReadAsStringAsync());
    }
    else
    {
      Debug.Log("Solicitud PATCH exitosa");
      string responseContent = await response.Content.ReadAsStringAsync();
      Debug.Log(responseContent);

      var responseJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
      Debug.Log(responseJson["name"]);
    }
  }
}