using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;

public class GameState : MonoBehaviour
{
    // singleton
    public static GameState Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) { Destroy(this); return; }
        Instance = this;

        DontDestroyOnLoad(this);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public async Task SubmitNickname(string nickname)
    {
        Debug.Log("Nickname submitted " + nickname);
        FirebaseUser user = await AuthService.LoginUser(nickname);
        LocalStorage.SaveUser(user);
    }

    public void SubmitCompletedMission(CardType cardType)
    {
        _ = DataService.PostCompletedMission(cardType);
    }

    public void SubmitTurn(TurnType turnType)
    {
        _ = DataService.PostTurn(turnType);
    }

    public void SubmitStartGame()
    {
        GameType gameType = new GameType();

        _ = SubmitStartGame_aux(gameType);
    }

    private async Task SubmitStartGame_aux(GameType gameType)
    {
        string gameId = await DataService.PostStartGame(gameType);
        gameType.Id = gameId;
        LocalStorage.SaveGame(gameType);
    }

    public void SubmitEndGame(int nTurns, int nTiles, int nMissions, EndGameUIState result)
    {
        GameType currentGame = LocalStorage.GetGame();
        currentGame.EndGame(nTurns, nTiles, nMissions, result);
        _ = SubmitEndGame_aux(currentGame);
    }

    private async Task SubmitEndGame_aux(GameType gameType)
    {
        await DataService.PatchEndGame(gameType);
        LocalStorage.DeleteGame();
    }
}
