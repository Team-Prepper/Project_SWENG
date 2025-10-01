using EasyH;

namespace SWEng.Network
{
    public class NetworkManager : Singleton<NetworkManager>
    {
        public INetworkSystem System { get; set; }

    }
}