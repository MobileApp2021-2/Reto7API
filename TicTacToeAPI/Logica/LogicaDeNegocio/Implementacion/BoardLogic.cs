using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicTacToeAPI.Logica.Dominio;
using TicTacToeAPI.Logica.Dominio.Dto;
using TicTacToeAPI.Logica.LogicaDeNegocio.Interfaz;
using TicTacToeAPI.Persistencia.UnidadDeTrabajo.Interfaz;

namespace TicTacToeAPI.Logica.LogicaDeNegocio.Implementacion
{
    public class BoardLogic : IBoardLogic
    {
        private readonly IUnitOfwork _unitOfWork;
        private readonly IMapper _mapper;

        public BoardLogic(IUnitOfwork _unitOfWork, IMapper mapper)
        {
            this._unitOfWork = _unitOfWork;
            _mapper = mapper;
        }

        public async Task<BoardDto> CreateBoard(string firstPlayerId)
        {
            Board board = new Board
            {
                FirstPlayerId = firstPlayerId,
            };
            var response = await _unitOfWork.BoardRepository.CreateBoard(board);
            string groupName = $"grp_{response.Id}";
            response.Group = groupName;
            _ = _unitOfWork.BoardRepository.UpdateBoard(response.Id, response);
            return _mapper.Map<BoardDto>(response);
        }

        public async Task<bool> DeleteBoard(string boardId)
        {
            return await _unitOfWork.BoardRepository.DeleteBoard(boardId);
        }

        public List<BoardDto> GetAvailableBoards()
        {
            List<BoardDto> boards = new List<BoardDto>();
            var boardsTmp = _unitOfWork.BoardRepository.GetAvailableBoards();
            foreach(var board in boardsTmp)
            {
                boards.Add(_mapper.Map<BoardDto>(board));
            }
            return boards;
        }

        public BoardDto GetBoard(string boardId)
        {
            var board = _unitOfWork.BoardRepository.GetBoard(boardId);
            return _mapper.Map<BoardDto>(board);
        }

        public IsAvailableDto IsBoardAvailable(string boardId)
        {
            Board ab = _unitOfWork.BoardRepository.GetBoard(boardId);
            IsAvailableDto isAvailable = new IsAvailableDto
            {
                Available = ab != null ? ab.Available : false
            };
            return isAvailable;
        }

        public BoardDto UpdateBoard(string boardId, BoardDto boardDto)
        {
            var board = _mapper.Map<Board>(boardDto);
            _ = _unitOfWork.BoardRepository.UpdateBoard(boardId, board);
            return boardDto;
        }
    }
}
