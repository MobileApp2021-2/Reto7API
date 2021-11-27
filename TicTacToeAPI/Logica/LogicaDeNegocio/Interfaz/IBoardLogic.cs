using System.Collections.Generic;
using System.Threading.Tasks;
using TicTacToeAPI.Logica.Dominio.Dto;

namespace TicTacToeAPI.Logica.LogicaDeNegocio.Interfaz
{
    public interface IBoardLogic
    {
        Task<BoardDto> CreateBoard(string firstPlayerId);
        Task<bool> DeleteBoard(string boardId);
        BoardDto UpdateBoard(string boardId, BoardDto board);
        BoardDto GetBoard(string boardId);
        List<BoardDto> GetAvailableBoards();
        bool IsBoardAvailable(string boardId);
    }
}
