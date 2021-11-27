using TicTacToeAPI.Persistencia.Repositorio.Interfaz;

namespace TicTacToeAPI.Persistencia.UnidadDeTrabajo.Interfaz
{
    public interface IUnitOfwork
    {
        IBoardRepository BoardRepository { get; }
    }
}
