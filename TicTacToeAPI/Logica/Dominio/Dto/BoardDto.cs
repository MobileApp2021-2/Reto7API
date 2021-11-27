using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicTacToeAPI.Logica.Dominio.Dto
{
    public class BoardDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Group { get; set; }

        [BsonRepresentation(BsonType.Boolean)]
        public bool Available { get; set; } = true;
        public string FirstPlayerId { get; set; }
        public string SecondPlayerId { get; set; }
        public int Ties { get; set; } = 0;
        public int FirstPlayerWins { get; set; } = 0;
        public int SecondPlayerWins { get; set; } = 0;
        public int GamesPlayed { get; set; }
        public char[] CurrentGame { get; set; }
    }
}
