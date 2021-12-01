using System.Collections.Generic;
using System.Threading.Tasks;
using TicTacToeAPI.Logica.Dominio;
using TicTacToeAPI.Logica.Dominio.Dto;

namespace TicTacToeAPI.Persistencia.Repositorio.Interfaz
{
    public interface IBoardRepository
    {
        Task<Board> CreateBoard(Board board);
        Task<bool> DeleteBoard(string boardId);
        Board UpdateBoard(string boardId, Board board);
        Board GetBoard(string boardId);
        List<Board> GetAvailableBoards();
    }
}
