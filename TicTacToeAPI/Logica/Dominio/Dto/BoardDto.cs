namespace TicTacToeAPI.Logica.Dominio.Dto
{
    public class BoardDto
    {
        public string Id { get; set; }
        public string Group { get; set; }
        public bool Available { get; set; } = true;
        public string FirstPlayerId { get; set; }
        public string SecondPlayerId { get; set; }
        public int Ties { get; set; } = 0;
        public int FirstPlayerWins { get; set; } = 0;
        public int SecondPlayerWins { get; set; } = 0;
        public int GamesPlayed { get; set; }
    }
}
