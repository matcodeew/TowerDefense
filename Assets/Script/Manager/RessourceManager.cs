using Unity.VisualScripting;
using UnityEngine;

public static class RessourceManager
{
    [Header("Ressources")]
    public static int CurrentGold { get; private set; } = 0;
    public static int BaseLife { get; private set; } = 30;
    public static int MaxWave { get; private set; } = 30;
    public static int CurrentWave { get; private set; } = 0;
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
        Debug.Log($"Added {goldAdded} gold. Total gold: {CurrentGold}");
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
        Debug.Log($"Lost {goldLost} gold. Total gold: {CurrentGold}");
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
            EventsManager.LevelFinished();
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
