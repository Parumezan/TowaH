using Mirror;

namespace TowaH {
    #region Client
    
    public partial struct ErrorMsg : NetworkMessage {
        public string text;
        public bool causesDisconnect;
    }

    public partial struct LoginSuccessMsg : NetworkMessage {
    }
    
    public partial struct CharactersAvailableMsg : NetworkMessage {
        // TODO: Implement
    }
    
    #endregion
    
    #region Server

    public partial struct LoginMsg : NetworkMessage {
        public string version;
    }

    public partial struct EditPlayerUsernameMsg : NetworkMessage {
        public string username;
    }
    
    public partial struct SelectPlayerCharacterMsg : NetworkMessage {
        public string characterId;
    }
    
    public partial struct PlayerReadyMsg : NetworkMessage {
    }
    
    #endregion
}
