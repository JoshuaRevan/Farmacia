using SistemaFarmacia.AplicacionWed.Models.ViewModels;
using SistemaFarmacia.Entity;
using System.Globalization;
using AutoMapper;
using SistemaFarmacia.BLL.Models;

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
                opt => opt.MapFrom(origen => origen.IdPresentacionNavigation != null ? origen.IdPresentacionNavigation.TipoPresentacion : "Sin presentación"))
                .ReverseMap() // ← ESTO NO ES SUFICIENTE para propiedades personalizadas
                .ForMember(destino => destino.IdMarcaNavigation, opt => opt.Ignore())
                .ForMember(destino => destino.IdPresentacionNavigation, opt => opt.Ignore());

            // CONFIGURACIÓN EXPLÍCITA para el mapeo VMProducto → Producto
            CreateMap<VMProducto, Producto>()
                .ForMember(destino => destino.NombreProducto, opt => opt.MapFrom(src => src.NombreProducto))
                .ForMember(destino => destino.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(destino => destino.Ubicacion, opt => opt.MapFrom(src => src.Ubicacion))
                .ForMember(destino => destino.IdMarca, opt => opt.MapFrom(src => src.IdMarca))
                .ForMember(destino => destino.IdPresentacion, opt => opt.MapFrom(src => src.IdPresentacion))
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

            #region Compras
            // ✅ MAPEO PARA CREAR - De RequestDto a Entidad (esto SÍ usas)
            CreateMap<CompraRequestDto, Compra>()       
                .ForMember(dest => dest.IdCompra, opt => opt.MapFrom(src => src.IdCompra))
                .ForMember(dest => dest.IdProducto, opt => opt.MapFrom(src => src.IdProducto))
                .ForMember(dest => dest.IdProveedor, opt => opt.MapFrom(src => src.IdProveedor))
                .ForMember(dest => dest.TotalCompra, opt => opt.MapFrom(src => src.TotalCompra))
                .ForMember(dest => dest.CantidadProductosCompra, opt => opt.MapFrom(src => src.CantidadProductosCompra))
                .ForMember(dest => dest.LoteInterno, opt => opt.MapFrom(src => src.LoteInterno))
                .ForMember(dest => dest.LoteProveedor, opt => opt.MapFrom(src => src.LoteProveedor))
                .ForMember(dest => dest.FechaHoraCompra, opt => opt.MapFrom(src => src.FechaHoraCompra))
                .ForMember(dest => dest.FechaVencimiento, opt => opt.MapFrom(src => src.fechaVencimiento))

                // IGNORAR propiedades que no existen en Compra
                .ForMember(dest => dest.IdProductoNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdProveedorNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Devoluciones, opt => opt.Ignore())
                .ForMember(dest => dest.Transacciones, opt => opt.Ignore())

                .ReverseMap()
                // Para el ReverseMap también ignorar lo que no existe en CompraRequestDto
                .ForMember(dest => dest.IdPresentacion, opt => opt.Ignore())
                .ForMember(dest => dest.IdMarca, opt => opt.Ignore())
                .ForMember(dest => dest.IdPrecioProducto, opt => opt.Ignore());

            // ✅ MAPEO BÁSICO para casos simples (opcional - si lo necesitas)
            CreateMap<Compra, CompraListaDto>()
                // Solo mapear propiedades básicas que existen en ambas clases
                .ForMember(dest => dest.IdCompra, opt => opt.MapFrom(src => src.IdCompra))
                .ForMember(dest => dest.TotalCompra, opt => opt.MapFrom(src => src.TotalCompra))
                .ForMember(dest => dest.CantidadProductosCompra, opt => opt.MapFrom(src => src.CantidadProductosCompra))
                .ForMember(dest => dest.LoteInterno, opt => opt.MapFrom(src => src.LoteInterno))
                .ForMember(dest => dest.LoteProveedor, opt => opt.MapFrom(src => src.LoteProveedor))
                .ForMember(dest => dest.FechaHoraCompra, opt => opt.MapFrom(src => src.FechaHoraCompra))
                .ForMember(dest => dest.fechaVencimiento, opt => opt.MapFrom(src => src.FechaVencimiento))
                .ForMember(dest => dest.IdProveedor, opt => opt.MapFrom(src => src.IdProveedor))
                .ForMember(dest => dest.IdProducto, opt => opt.MapFrom(src => src.IdProducto))

                // ❌ IGNORAR todo lo complejo - tu método Lista lo maneja manualmente
                .ForMember(dest => dest.NombreProveedor, opt => opt.Ignore())
                .ForMember(dest => dest.NombreProducto, opt => opt.Ignore())
                .ForMember(dest => dest.IdMarca, opt => opt.Ignore())
                .ForMember(dest => dest.NombreMarca, opt => opt.Ignore())
                .ForMember(dest => dest.IdPresentacion, opt => opt.Ignore())
                .ForMember(dest => dest.TipoPresentacion, opt => opt.Ignore())
                .ForMember(dest => dest.IdPrecioProducto, opt => opt.Ignore())
                .ForMember(dest => dest.Precio, opt => opt.Ignore());
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
