using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    public void LunchGame(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
