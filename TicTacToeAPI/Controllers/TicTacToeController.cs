using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicTacToeAPI.Logica.LogicaDeNegocio.Interfaz;

namespace TicTacToeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TicTacToeController : ControllerBase
    {
        private readonly IBoardLogic _boardLogic;

        public TicTacToeController(IBoardLogic _boardLogic)
        {
            this._boardLogic = _boardLogic;
        }

        [HttpGet("GetAvailableBoards")]
        public IActionResult GetAvailableBoards()
        {
            return Ok(_boardLogic.GetAvailableBoards());
        }


        [HttpGet("IsBoardAvailable/{boardId}")]
        public IActionResult IsBoardAvailable(string boardId)
        {
            return Ok(_boardLogic.IsBoardAvailable(boardId));
        }

        [HttpPost("CreateBoard/{firstPlayerId}")]
        public async Task<IActionResult> CreateBoard(string firstPlayerId)
        {
            var response = await _boardLogic.CreateBoard(firstPlayerId);
            return Ok(response);
        }
    }
}
