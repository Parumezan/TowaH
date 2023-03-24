using Mirror;

namespace TowaH {
    public partial struct ErrorMsg : NetworkMessage {
        public string text;
        public bool causesDisconnect;
    }

    public partial struct LoginSuccessMsg : NetworkMessage {
    }

    public partial struct LoginMsg : NetworkMessage {
        public string version;
    }
}
