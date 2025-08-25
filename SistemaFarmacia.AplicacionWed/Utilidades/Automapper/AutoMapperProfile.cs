using SistemaFarmacia.AplicacionWed.Models.ViewModels;
using SistemaFarmacia.Entity;
using System.Globalization;
using AutoMapper;

namespace SistemaFarmacia.AplicacionWed.Utilidades.Automapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            #region Cargo
            CreateMap<Cargo, VMcargo>().ReverseMap();
            #endregion

            #region Usuario
            CreateMap<Usuario, VMUsuario>()
                .ForMember(destino => destino.NombreCargo,
                           opt => opt.MapFrom(origen => origen.IdCargoNavigation != null ? origen.IdCargoNavigation.cargo : "Sin Cargo"))
                .ReverseMap();

            CreateMap<VMUsuario, Usuario>()
                .ForMember(destino => destino.IdCargoNavigation,
                            opt => opt.Ignore()); // Evitamos que AutoMapper intente mapear esta navegación
            #endregion

            #region Presentaciones
            CreateMap<Presentacione, VMPresentacione>().ReverseMap();
            #endregion

            #region Productos
            CreateMap<Producto, VMProducto>()
               .ForMember(destino => destino.NombreMarca,
               opt => opt.MapFrom(origen => origen.IdMarcaNavigation != null ? origen.IdMarcaNavigation.NombreMarca : "Sin marca"))
               .ForMember(destino => destino.NombrePresentacion,
               opt => opt.MapFrom(origen => origen.IdPresentacionNavigation != null ? origen.IdPresentacionNavigation.TipoPresentacion : "Sin presentación"));

            CreateMap<VMProducto, Producto>()
                .ForMember(destino => destino.IdMarcaNavigation, opt => opt.Ignore())
                .ForMember(destino => destino.IdPresentacionNavigation, opt => opt.Ignore());

            #endregion

            #region Marcas
            CreateMap<Marca, VMMarca>().ReverseMap();
            #endregion

            #region PrecioProducto
            CreateMap<PrecioProducto, VMPrecioProducto>()
                .ForMember(destino => destino.IdPrecioProducto,
                opt => opt.MapFrom(src => src.IdPrecioProducto))
                .ForMember(destino => destino.Precio,
                opt => opt.MapFrom(src => src.Precio))
                .ForMember(destino => destino.IdProducto,
                opt => opt.MapFrom(src => src.IdProducto))
                .ForMember(destino => destino.IdProveedor,
                opt => opt.MapFrom(src => src.IdProveedor));

            CreateMap<VMPrecioProducto, PrecioProducto>()
                .ForMember(destino => destino.IdPrecioProducto, 
                opt => opt.MapFrom(src => src.IdPrecioProducto))
                .ForMember(dest => dest.Precio, 
                opt => opt.MapFrom(src => src.Precio))
                .ForMember(dest => dest.IdProducto, 
                opt => opt.MapFrom(src => src.IdProducto))
                .ForMember(dest => dest.IdProveedor, 
                opt => opt.MapFrom(src => src.IdProveedor));
            #endregion

            #region Proveedores 
            CreateMap<Proveedore, VMProveedores>().ReverseMap();
            #endregion

            #region  Compras 
            CreateMap<Compra, VMCompra>().ReverseMap();
            #endregion

            #region FormasPago 
            CreateMap<Formaspago, VMFormaspago>().ReverseMap();
            #endregion

            #region tipodevoluciones 
            CreateMap<TipoDevolucione, VMTipoDevolucione>().ReverseMap();
            #endregion

            #region  Ventas 
            CreateMap<Venta, VMVenta>().ReverseMap();

            #endregion

            #region Transaciones 
            CreateMap<Transaccione, VMTransacciones>().ReverseMap();
            #endregion

            #region DEvoluciones 
            CreateMap<Devolucione, VMDevolucione>().ReverseMap();
            #endregion
        }
    }
}
