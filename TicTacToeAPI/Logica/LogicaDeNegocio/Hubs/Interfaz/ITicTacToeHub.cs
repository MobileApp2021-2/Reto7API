using System.Threading.Tasks;
using TicTacToeAPI.Logica.Dominio.Dto;

namespace TicTacToeAPI.Logica.LogicaDeNegocio.Hubs.Interfaz
{
    public interface ITicTacToeHub
    {
        /// <summary>
        /// Cuando un jugador se une al tablero, actualiza el tablero con el id del segundo jugador y envia un mensaje de inicio del juego, con contadores en 0, habilita el tablero cn mensaje recibido. cada vez que se une un usuario, los contadores vuelven a cero
        /// </summary>
        /// <param name="groupName"> nombre del grupo en el hub</param>
        /// <param name="secondPlayerId"> id jugador que se quier eunir a la partida</param>
        /// <returns></returns>
        Task JoinBoard(string groupName, string boardId,  string secondPlayerId);

        /// <summary>
        /// Cuando la persona que creo el tablero termina la partida, se elimina el tablero y el grupo, no se envía nada al cliente
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task DeleteBoard(string groupName, string boardId);

        /// <summary>
        /// Se envia cada vez que un usuario hace un movimiento para actualizar los dos tableros y deshabilitar quien acaba de jugar
        /// </summary>
        /// <param name="groupName"> nombre del grupo</param>
        /// <param name="board"> tablero nuevo </param>
        /// <returns></returns>
        Task SendMessageNewMovement(string groupName, int movementPosition);

        /// <summary>
        /// se envia cuando una partida acabo para informar el ganador o empate y actualizar los contadores en base de datos
        /// </summary>
        /// <param name="groupName">nombre del grupo</param>
        /// <param name="board">tablero nuevo</param>
        /// <returns></returns>
        Task SendMessageGameOver(string groupName, string boardId, int ties, int firstWins, 
            int secondWins, int gamesPlayed, int winner);

        /// <summary>
        /// se envia cuando el usuario que no creo la partida abandona, el usuario que lo recibe puede temrinar la partida o esperar por otro jugador, se reinicia los contadores
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        Task SendMessageLeaveRoomSecondPlayer(string groupName, string boardId);


        /// <summary>
        /// se envia cuando el usuario que creo la partida abandona, el usuario que lo recibe se sale de la partida. Se borra el tablero
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        Task SendMessageLeaveRoomFirstPlayer(string groupName, string boardId);

        /// <summary>
        /// se envia para avisar quien va de primero (este evento se ejecuta del lado del primer jugar, quien creo la sala
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        Task SendMessageFirstPlayer(string groupName, char player);

    }
}
