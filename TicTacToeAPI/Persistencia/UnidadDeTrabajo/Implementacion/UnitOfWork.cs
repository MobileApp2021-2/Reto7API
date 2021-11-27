using TicTacToeAPI.Logica.Dominio.Configurtacion.Implementacion;
using TicTacToeAPI.Persistencia.Repositorio.Implementacion;
using TicTacToeAPI.Persistencia.Repositorio.Interfaz;
using TicTacToeAPI.Persistencia.UnidadDeTrabajo.Interfaz;

namespace TicTacToeAPI.Persistencia.UnidadDeTrabajo.Implementacion
{
    public class UnitOfWork : IUnitOfwork
    {
        public IBoardRepository BoardRepository { get; }

        public UnitOfWork(MongoSettings _mongoSettings)
        {
            BoardRepository = new BoardRepository(_mongoSettings);
        }
    }
}
