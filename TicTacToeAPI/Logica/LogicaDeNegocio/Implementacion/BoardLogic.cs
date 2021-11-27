using System.Collections.Generic;
using System.Threading.Tasks;
using TicTacToeAPI.Logica.Dominio.Dto;
using TicTacToeAPI.Logica.LogicaDeNegocio.Interfaz;
using TicTacToeAPI.Persistencia.UnidadDeTrabajo.Interfaz;

namespace TicTacToeAPI.Logica.LogicaDeNegocio.Implementacion
{
    public class BoardLogic : IBoardLogic
    {
        private readonly IUnitOfwork _unitOfWork;

        public BoardLogic(IUnitOfwork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

        public async Task<BoardDto> CreateBoard(string firstPlayerId)
        {
            BoardDto board = new BoardDto
            {
                FirstPlayerId = firstPlayerId,
                CurrentGame = new char[9]{ '1','2','3','4','5','6','7','8','9'}
            };
            var response = await _unitOfWork.BoardRepository.CreateBoard(board);
            return response;
        }

        public async Task<bool> DeleteBoard(string boardId)
        {
            return await _unitOfWork.BoardRepository.DeleteBoard(boardId);
        }

        public List<BoardDto> GetAvailableBoards()
        {
            return _unitOfWork.BoardRepository.GetAvailableBoards();
        }

        public BoardDto GetBoard(string boardId)
        {
            return _unitOfWork.BoardRepository.GetBoard(boardId);
        }

        public bool IsBoardAvailable(string boardId)
        {
            return _unitOfWork.BoardRepository.GetBoard(boardId).Available;
        }

        public BoardDto UpdateBoard(string boardId, BoardDto board)
        {
            return _unitOfWork.BoardRepository.UpdateBoard(boardId, board);
        }
    }
}
