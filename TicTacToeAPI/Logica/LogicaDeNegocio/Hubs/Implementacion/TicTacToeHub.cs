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

        public async Task JoinBoard(string groupName, string boardId, string secondPlayerId)
        {
            if(!String.IsNullOrWhiteSpace(secondPlayerId))
            {
                BoardDto board = _boardLogic.GetBoard(boardId);
                board.SecondPlayerId = secondPlayerId;
                board.Available = false;
                board.Group = groupName;
                _boardLogic.UpdateBoard(boardId, board);
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.OthersInGroup(groupName).SendAsync("UserJoined");
            await Clients.All.SendAsync("RefreshBoard");
        }

        public async Task DeleteBoard(string groupName, string boardId)
        {
            await _boardLogic.DeleteBoard(boardId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessageLeaveRoomSecondPlayer(string groupName, string boardId)
        {
            await _boardLogic.DeleteBoard(boardId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("SecondPlayerLeaveGame");
            await Clients.All.SendAsync("RefreshBoard");
        }

        public async Task SendMessageLeaveRoomFirstPlayer(string groupName, string boardId)
        {
            await _boardLogic.DeleteBoard(boardId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("FirstPlayerLeaveGame");
            await Clients.All.SendAsync("RefreshBoard");
        }

        public Task SendMessageNewMovement(string groupName, int movementPosition)
        {
            return Clients.OthersInGroup(groupName).SendAsync("NewMovementReceive", movementPosition);
        }

        public Task SendMessageGameOver(string groupName, string boardId, int ties, 
            int firstWins, int secondWins, int gamesPlayed, int winner)
        {
            BoardDto board = _boardLogic.GetBoard(boardId);
            board.GamesPlayed = gamesPlayed;
            board.FirstPlayerWins = firstWins;
            board.SecondPlayerWins = secondWins;
            board.Ties = ties;
            _boardLogic.UpdateBoard(boardId, board);
            return Clients.OthersInGroup(groupName).SendAsync("CheckWinner", winner);
        }

        public Task SendMessageFirstPlayer(string groupName, char player)
        {
            return Clients.OthersInGroup(groupName).SendAsync("FirstPlayer", player);
        }
    }
}
