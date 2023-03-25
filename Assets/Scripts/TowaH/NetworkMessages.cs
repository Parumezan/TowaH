using Mirror;

namespace TowaH {
    #region Client
    
    public partial struct ErrorMsg : NetworkMessage {
        public string text;
        public bool causesDisconnect;
    }

    public partial struct LoginSuccessMsg : NetworkMessage {
    }
    
    public partial struct PlayerUpdateCharactersMsg : NetworkMessage {
        public partial struct CharacterPreview {
            public int playerId;
            public int characterIndex;
        }
        public CharacterPreview[] characters;
    }
    
    #endregion
    
    #region Server

    public partial struct LoginMsg : NetworkMessage {
        public string version;
    }

    public partial struct SelectPlayerCharacterMsg : NetworkMessage {
        public int index;
    }
    
    public partial struct PlayerReadyMsg : NetworkMessage {
    }
    
    #endregion
}
