using System;

public static class EventManager
{
    public static event Action OnPlayerHit;
    public static event Action OnGameOver;
    public static event Action<int> OnWaveChanged;

    public static void TriggerPlayerHit() => OnPlayerHit?.Invoke();
    public static void TriggerGameOver() => OnGameOver?.Invoke();
    public static void TriggerWaveChanged(int wave) => OnWaveChanged?.Invoke(wave);
}