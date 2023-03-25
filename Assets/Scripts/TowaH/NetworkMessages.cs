using Mirror;

namespace TowaH {
    #region Client
    
    public partial struct ErrorMsg : NetworkMessage {
        public string text;
        public bool causesDisconnect;
    }

    public partial struct LoginSuccessMsg : NetworkMessage {
    }
    
    public partial struct PlayerCharactersMsg : NetworkMessage {
        public partial struct CharacterPreview {
            public int playerId;
            public int characterIndex;
        }
        public CharacterPreview[] characters;
    }
    
    public partial struct PlayerCharacterAuthorityMsg : NetworkMessage {
        public int playerId;
    }
    
    public partial struct StartGameMsg : NetworkMessage {
    }
    
    public partial struct EndGameMsg : NetworkMessage {
    }
    
    #endregion
    
    #region Server

    public partial struct LoginMsg : NetworkMessage {
        public string version;
    }
    
    public partial struct PlayerJoinedMsg : NetworkMessage {
        public int playerId;
    }
    
    public partial struct PlayerLeftMsg : NetworkMessage {
        public int playerId;
    }

    public partial struct SelectPlayerCharacterMsg : NetworkMessage {
        public int playerId;
        public int characterIndex;
    }
    
    public partial struct PlayerReadyMsg : NetworkMessage {
    }

    #endregion
}
