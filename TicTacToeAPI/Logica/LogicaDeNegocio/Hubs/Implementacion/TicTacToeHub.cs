using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using TicTacToeAPI.Logica.Dominio.Dto;
using TicTacToeAPI.Logica.LogicaDeNegocio.Hubs.Interfaz;
using TicTacToeAPI.Logica.LogicaDeNegocio.Interfaz;

namespace TicTacToeAPI.Logica.LogicaDeNegocio.Hubs.Implementacion
{
    public class TicTacToeHub : Hub, ITicTacToeHub
    {
        private IBoardLogic _boardLogic { get;  }

        public TicTacToeHub(IBoardLogic boardLogic)
        {
            _boardLogic = boardLogic;
        }

        public async Task CreateBoard(string firstPlayerId)
        {
            BoardDto board = await _boardLogic.CreateBoard(firstPlayerId);
            string groupName = $"grp_{board.Id}";
            board.Group = groupName;
            _boardLogic.UpdateBoard(board.Id, board);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("CreatedBoard", board);
            Context.Items.Add("groupName", groupName);
            Context.Items.Add("boardId", board.Id);
        }

        public async Task JoinBoard(string groupName, string boardId, string secondPlayerId)
        {
            BoardDto board = _boardLogic.GetBoard(boardId);
            board.SecondPlayerId = secondPlayerId;
            board.GamesPlayed = 0;
            board.CurrentGame = new char[9]{ '1','2','3','4','5','6','7','8','9'};
            board.Available = false;
            board.FirstPlayerWins = 0;
            board.SecondPlayerWins = 0;
            board.Ties = 0;
            board.Group = groupName;
            _boardLogic.UpdateBoard(boardId, board);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("UserJoined", board);
        }

        public async Task DeleteBoard(string groupName, string boardId)
        {
            await _boardLogic.DeleteBoard(boardId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public Task SendMessageNewMovement(string groupName, BoardDto board)
        {
            _boardLogic.UpdateBoard(board.Id, board);
            return Clients.OthersInGroup(groupName).SendAsync("NewMovement", board);
        }

        public Task SendMessageGameOver(string groupName, BoardDto board)
        {
            _boardLogic.UpdateBoard(board.Id, board);
            return Clients.OthersInGroup(groupName).SendAsync("CheckWinner", board);
        }

        public async Task SendMessageLeaveRoomSecondPlayer(string groupName, string boardId)
        {
            BoardDto board = _boardLogic.GetBoard(boardId);
            board.SecondPlayerId = null;
            board.GamesPlayed = 0;
            board.CurrentGame = new char[9] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            board.Available = false;
            board.FirstPlayerWins = 0;
            board.SecondPlayerWins = 0;
            board.Ties = 0;
            board.Group = groupName;
            _boardLogic.UpdateBoard(boardId, board);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("SecondPlayerLeaveGame", board);
        }

        public async Task SendMessageLeaveRoomFirstPlayer(string groupName, string boardId)
        {
            await _boardLogic.DeleteBoard(boardId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("FirstPlayerLeaveGame");
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            object groupName;
            Context.Items.TryGetValue("groupName", out groupName);

            object boardId;
            Context.Items.TryGetValue("boardId", out boardId);

            if(groupName is string group && !String.IsNullOrWhiteSpace(group) && boardId is string board && !String.IsNullOrWhiteSpace(board))
            {
                await _boardLogic.DeleteBoard(board);
                await Clients.OthersInGroup(group).SendAsync("EndGame");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public Task RefreshAllGames()
        {
            return Clients.All.SendAsync("RefreshGames");
        }
    }
}
