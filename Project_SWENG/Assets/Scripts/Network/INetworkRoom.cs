using System;

public struct RoomMember {
    public string NickName;

    public RoomMember(string name) {
        NickName = name;
    }
}

public interface INetworkRoom {

    public void NewPlayerEnter(RoomMember newMember);
    public void PlayerExit(RoomMember exitMember);

}