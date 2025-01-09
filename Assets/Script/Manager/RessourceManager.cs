using UnityEngine;

public class RessourceManager : MonoBehaviour
{
    [Header("Ressources")]
    public static int CurrentGold { get; private set; } = 250;
    public static int BaseLife { get; private set; } = 30;
    public static int MaxWave { get; private set; } = 30;
    public static int CurrentWave { get; private set; } = 0;

    private void Awake() // Mettre les bonnes valeur en fonction du level ou difficulter 
    {
        CurrentGold = 500;
        BaseLife = 30;
        MaxWave = 30;
    }
    /// <summary>
    /// Inflicts damage to the base. Reduces base life by the specified amount.
    /// </summary>
    /// <param name="damageTaken">The amount of damage to apply to the base.</param>
    public static void BaseTakeDamage(int damageTaken)
    {
        if (BaseLife > 0)
        {
            BaseLife -= damageTaken;
            BaseLife = Mathf.Max(0, BaseLife); // Ensures life does not go below zero.
            UiManager.Instance.UpdateLife();
            Debug.Log($"Base took {damageTaken} damage. Remaining life: {BaseLife}");
            if (BaseLife <= 0)
            {
                EventsManager.Defeat();
            }
        }
    }

    /// <summary>
    /// Adds gold to the current gold count.
    /// </summary>
    /// <param name="goldAdded">The amount of gold to add.</param>
    public static void AddGold(int goldAdded)
    {
        CurrentGold += goldAdded;
        UiManager.Instance.UpdateGold();
    }

    /// <summary>
    /// Deducts gold from the current gold count.
    /// </summary>
    /// <param name="goldLost">The amount of gold to deduct.</param>
    public static void LoseGold(int goldLost)
    {
        CurrentGold -= goldLost;
        CurrentGold = Mathf.Max(0, CurrentGold); // Ensures gold does not go below zero.
        UiManager.Instance.UpdateGold();
    }

    /// <summary>
    /// Starts a new wave. Increases the current wave count and resets any wave-related parameters.
    /// </summary>
    public static bool StartNewWave()
    {
        if (CurrentWave < MaxWave)
        {
            CurrentWave++;
            Debug.Log($"Wave {CurrentWave} started!");
            UiManager.Instance.UpdateWave();
            return true;
        }
        else
        {
            EventsManager.Victory();
            return false;
        }
    }

    /// <summary>
    /// Resets all resources to their initial state.
    /// </summary>
    public static void ResetResources()
    {
        CurrentGold = 0;
        BaseLife = 30;
        CurrentWave = 0;
        Debug.Log("Resources reset to initial values.");
    }
}
