using EasyH;

namespace SWEng.Chat {

    public class ChatManager : Singleton<ChatManager>
    {
        public IChatSystem System { get; set; }
            = new ChatSystem();
    }
}