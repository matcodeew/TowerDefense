using UnityEngine;

public class UiAnimation : MonoBehaviour
{
    public static UiAnimation Instance;
    [SerializeField] private Animator WaveIndication;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void StopWaveButtonAnim()
    {
        WaveIndication.SetBool("isActive", true);
    }
    public void StartWaveButtonAnim()
    {
        WaveIndication.SetBool("isActive", false);
    }
}
