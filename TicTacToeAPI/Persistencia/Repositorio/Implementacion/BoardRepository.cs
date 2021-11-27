using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using TicTacToeAPI.Logica.Dominio.Dto;
using TicTacToeAPI.Persistencia.Repositorio.Interfaz;
using TicTacToeAPI.Logica.Dominio.Configurtacion.Implementacion;

namespace TicTacToeAPI.Persistencia.Repositorio.Implementacion
{
    public class BoardRepository : IBoardRepository
    {
        private readonly IMongoCollection<BoardDto> _boards;

        public BoardRepository(MongoSettings _mongoSettings)
        {
            var client = new MongoClient(_mongoSettings.ConnectionString);
            var database = client.GetDatabase(_mongoSettings.DatabaseName);
            _boards = database.GetCollection<BoardDto>(_mongoSettings.BooksCollectionName);
        }

        public async Task<BoardDto> CreateBoard(BoardDto board)
        {
            await _boards.InsertOneAsync(board);
            return board;
        }

        public async Task<bool> DeleteBoard(string boardId)
        {
            DeleteResult result = await _boards.DeleteOneAsync(board => board.Id == boardId);
            return result.IsAcknowledged;
        }

        public  List<BoardDto> GetAvailableBoards()
        {
           return _boards.Find(board => board.Available == true).ToList();
        }

        public BoardDto GetBoard(string boardId)
        {
            return _boards.Find<BoardDto>(board => board.Id == boardId).FirstOrDefault();
        }

        public BoardDto UpdateBoard(string boardId, BoardDto board)
        {
            _boards.ReplaceOne(board => board.Id == boardId, board);
            return board;
        }
    }
}
