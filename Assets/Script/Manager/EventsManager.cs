using UnityEngine.Events;

public static class EventsManager
{
    public static event UnityAction<S_Enemy, int> OnWaveStart;
    public static event UnityAction OnLevelFinished;
    public static event UnityAction OnDefeated;

    public static void Victory()
    {
        OnLevelFinished?.Invoke();
    }
    public static void Defeat()
    {
        OnDefeated?.Invoke();
    }
    public static void StartNewWave(S_Enemy enemy, int quantity)
    {
        OnWaveStart?.Invoke(enemy, quantity);
    }
}
