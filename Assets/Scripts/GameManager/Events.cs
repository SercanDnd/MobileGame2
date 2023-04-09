using System;

public static class Events
{
    public static readonly Evt OnGameStart = new Evt();
    public static readonly Evt OnGameWin = new Evt();
    public static readonly Evt OnGameLost = new Evt();
    public static readonly Evt OnGamePause = new Evt();
    public static readonly Evt OnWaveComplete = new Evt();
}

public class Evt
{
    private event Action _action = delegate {  };
    public void Invoke() => _action?.Invoke();
    public void AddListener(Action listener) => _action += listener;
    public void RemoveListener(Action listener) => _action -= listener;
    public void DisableEvent() => _action = null;
}
