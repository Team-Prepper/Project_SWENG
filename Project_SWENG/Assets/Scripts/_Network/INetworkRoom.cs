using System;
using System.Collections.Generic;

public struct RoomMember {
    public string NickName;

    public RoomMember(string name) {
        NickName = name;
    }
}

public interface RoomObserver {
    public void Renewal();
}

public class RoomUnsubscriber : IDisposable {
    private readonly ISet<RoomObserver> _observers;
    private readonly RoomObserver _observer;

    public RoomUnsubscriber(ISet<RoomObserver> observers, RoomObserver observer)
    {
        _observers = observers;
        _observer = observer;
    }

    public void Dispose()
    {
        if (_observers.Contains(_observer))
            _observers.Remove(_observer);
    }
}

public interface INetworkRoom : IGameSetting {

    public void NewPlayerEnter(RoomMember newMember);
    public void PlayerExit(RoomMember exitMember);

    public bool StartGame();

    public IDisposable Subscribe(RoomObserver observer);
}