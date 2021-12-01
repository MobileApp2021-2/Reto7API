using AutoMapper;
using TicTacToeAPI.Logica.Dominio.Dto;

namespace TicTacToeAPI.Logica.Dominio.Perfiles
{
    public class BoardProfile : Profile
    {
        public BoardProfile()
        {
            CreateMap<Board, BoardDto>().ReverseMap();
        }
    }
}
