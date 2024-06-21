using UnityEngine;
using UnityEngine.SceneManagement;
public static class CustomSceneManager
{
    public static void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
    public static void LoadMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }
}