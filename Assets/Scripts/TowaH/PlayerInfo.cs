namespace TowaH {
    public class PlayerInfo {
        public int Id { get; }
        public int CharacterIndex { get; set; }
        
        public PlayerInfo(int id) {
            Id = id;
            CharacterIndex = 0;
        }
    }
}
