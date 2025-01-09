using UnityEngine;
using UnityEngine.SceneManagement;

public class WinAndDefeat : MonoBehaviour
{

    #region Event
    private void OnEnable()
    {
        EventsManager.OnLevelFinished += Victory;
        EventsManager.OnDefeated += Defeat;
    }
    private void OnDisable()
    {
        EventsManager.OnLevelFinished -= Victory;
        EventsManager.OnDefeated -= Defeat;
    }
    #endregion
    [ContextMenu("Victory")]
    public void Victory()
    {
        SceneManager.LoadScene("Victory");
    }
    [ContextMenu("Defeat")]
    public void Defeat()
    {
        SceneManager.LoadScene("Defeat");
    }
}
