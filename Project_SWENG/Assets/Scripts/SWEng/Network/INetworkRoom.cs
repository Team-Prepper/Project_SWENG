using System;

namespace SWEng.Network
{
    public struct RoomMember
    {
        public string NickName;

        public RoomMember(string name)
        {
            NickName = name;
        }
    }

    public interface INetworkRoom
    {

        public void NewPlayerEnter(RoomMember newMember);
        public void PlayerExit(RoomMember exitMember);

        public void EnterRoom();
        public void LeaveRoom();

    }
}