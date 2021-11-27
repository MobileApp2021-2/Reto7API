using System.Collections.Generic;
using System.Threading.Tasks;
using TicTacToeAPI.Logica.Dominio.Dto;

namespace TicTacToeAPI.Persistencia.Repositorio.Interfaz
{
    public interface IBoardRepository
    {
        Task<BoardDto> CreateBoard(BoardDto board);
        Task<bool> DeleteBoard(string boardId);
        BoardDto UpdateBoard(string boardId, BoardDto board);
        BoardDto GetBoard(string boardId);
        List<BoardDto> GetAvailableBoards();
    }
}
