using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using TicTacToeAPI.Logica.Dominio.Dto;
using TicTacToeAPI.Persistencia.Repositorio.Interfaz;
using TicTacToeAPI.Logica.Dominio.Configurtacion.Implementacion;
using TicTacToeAPI.Logica.Dominio;
using AutoMapper;

namespace TicTacToeAPI.Persistencia.Repositorio.Implementacion
{
    public class BoardRepository : IBoardRepository
    {
        private readonly IMongoCollection<Board> _boards;

        public BoardRepository(MongoSettings _mongoSettings)
        {
            var client = new MongoClient(_mongoSettings.ConnectionString);
            var database = client.GetDatabase(_mongoSettings.DatabaseName);
            _boards = database.GetCollection<Board>(_mongoSettings.BooksCollectionName);
        }

        public async Task<Board> CreateBoard(Board board)
        {
            await _boards.InsertOneAsync(board);
            return board;
        }

        public async Task<bool> DeleteBoard(string boardId)
        {
            DeleteResult result = await _boards.DeleteOneAsync(board => board.Id == boardId);
            return result.IsAcknowledged;
        }

        public List<Board> GetAvailableBoards()
        {
            return _boards.Find(board => board.Available == true).ToList();
        }

        public Board GetBoard(string boardId)
        {
            return _boards.Find<Board>(board => board.Id == boardId).FirstOrDefault();
        }

        public Board UpdateBoard(string boardId, Board board)
        {
            _boards.ReplaceOne(board => board.Id == boardId, board);
            return board;
        }
    }
}
