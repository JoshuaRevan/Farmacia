using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SistemaFarmacia.Entity;

namespace SistemaFarmacia.DAL.DBContext;

public partial class FarmaciaTroyaContext : DbContext
{
    public FarmaciaTroyaContext()
    {
    }

    public FarmaciaTroyaContext(DbContextOptions<FarmaciaTroyaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cargo> Cargos { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<Devolucione> Devoluciones { get; set; }

    public virtual DbSet<Formaspago> Formaspagos { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<PrecioProducto> PrecioProductos { get; set; }

    public virtual DbSet<Presentacione> Presentaciones { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<TipoDevolucione> TipoDevoluciones { get; set; }

    public virtual DbSet<Transaccione> Transacciones { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-SG8MU6B;Database=Farmacia_Troya;Integrated Security=True;Encrypt=False");
    */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.IdCargo).HasName("PK__cargos__3D0E29B8CFFAFE3D");

            entity.ToTable("cargos");

            entity.Property(e => e.IdCargo).HasColumnName("idCargo");
            entity.Property(e => e.cargo)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("cargo");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra).HasName("PK__compras__48B99DB74F7B79E4");

            entity.ToTable("compras");

            entity.Property(e => e.IdCompra).HasColumnName("idCompra");
            entity.Property(e => e.CantidadProductosCompra).HasColumnName("cantidadProductosCompra");
            entity.Property(e => e.FechaHoraCompra)
                .HasColumnType("datetime")
                .HasColumnName("fechaHoraCompra");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fechaVencimiento");
            entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");
            entity.Property(e => e.LoteInterno).HasColumnName("loteInterno");
            entity.Property(e => e.LoteProveedor).HasColumnName("loteProveedor");
            entity.Property(e => e.TotalCompra)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalCompra");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK__compras__idProve__534D60F1");
        });

        modelBuilder.Entity<Devolucione>(entity =>
        {
            entity.HasKey(e => e.IdDevolucion).HasName("PK__devoluci__BFAF069ADA9A0789");

            entity.ToTable("devoluciones");

            entity.Property(e => e.IdDevolucion).HasColumnName("idDevolucion");
            entity.Property(e => e.IdCompra).HasColumnName("idCompra");
            entity.Property(e => e.IdTipoDevolucion).HasColumnName("idTipoDevolucion");
            entity.Property(e => e.IdVenta).HasColumnName("idVenta");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.Devoluciones)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("FK__devolucio__idCom__5629CD9C");

            entity.HasOne(d => d.IdTipoDevolucionNavigation).WithMany(p => p.Devoluciones)
                .HasForeignKey(d => d.IdTipoDevolucion)
                .HasConstraintName("FK__devolucio__idTip__5812160E");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.Devoluciones)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK__devolucio__idVen__571DF1D5");
        });

        modelBuilder.Entity<Formaspago>(entity =>
        {
            entity.HasKey(e => e.IdFormPago).HasName("PK__formaspa__AAFBA02F23A692C6");

            entity.ToTable("formaspago");

            entity.Property(e => e.IdFormPago).HasColumnName("idFormPago");
            entity.Property(e => e.FormPago)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("formPago");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.IdMarca).HasName("PK__marcas__7033181277625EC1");

            entity.ToTable("marcas");

            entity.Property(e => e.IdMarca).HasColumnName("idMarca");
            entity.Property(e => e.NombreMarca)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nombreMarca");
        });

        modelBuilder.Entity<PrecioProducto>(entity =>
        {
            entity.HasKey(e => e.IdPrecioProducto).HasName("PK__PrecioPr__2559D7C25DA3BF7C");

            entity.ToTable("PrecioProducto");

            entity.Property(e => e.IdPrecioProducto).HasColumnName("idPrecioProducto");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.PrecioProductos)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__PrecioPro__idPro__48CFD27E");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.PrecioProductos)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK__PrecioPro__idPro__49C3F6B7");
        });

        modelBuilder.Entity<Presentacione>(entity =>
        {
            entity.HasKey(e => e.IdPresentacion).HasName("PK__presenta__2F35305541AEB985");

            entity.ToTable("presentaciones");

            entity.Property(e => e.IdPresentacion).HasColumnName("idPresentacion");
            entity.Property(e => e.TipoPresentacion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("tipoPresentacion");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__producto__07F4A132D2F9D751");

            entity.ToTable("productos");

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdMarca).HasColumnName("idMarca");
            entity.Property(e => e.IdPresentacion).HasColumnName("idPresentacion");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nombreProducto");
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("ubicacion");

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdMarca)
                .HasConstraintName("FK__productos__idMar__44FF419A");

            entity.HasOne(d => d.IdPresentacionNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdPresentacion)
                .HasConstraintName("FK__productos__idPre__45F365D3");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__proveedo__A3FA8E6BD8187A4B");

            entity.ToTable("proveedores");

            entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");
            entity.Property(e => e.NombreProveedor)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nombreProveedor");
            entity.Property(e => e.TelefonoProveedor)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("telefonoProveedor");
        });

        modelBuilder.Entity<TipoDevolucione>(entity =>
        {
            entity.HasKey(e => e.IdTipoDevolucion).HasName("PK__tipoDevo__0A4907F1D8D7CAAB");

            entity.ToTable("tipoDevoluciones");

            entity.Property(e => e.IdTipoDevolucion).HasColumnName("idTipoDevolucion");
            entity.Property(e => e.TipoDevolucion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipoDevolucion");
        });

        modelBuilder.Entity<Transaccione>(entity =>
        {
            entity.HasKey(e => e.IdTransaccion).HasName("PK__transacc__5B8761F0D5F658F0");

            entity.ToTable("transacciones");

            entity.Property(e => e.IdTransaccion).HasColumnName("idTransaccion");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.FechaHoraTransaccion)
                .HasColumnType("datetime")
                .HasColumnName("fechaHoraTransaccion");
            entity.Property(e => e.IdCompra).HasColumnName("idCompra");
            entity.Property(e => e.IdDevolucion).HasColumnName("idDevolucion");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdVenta).HasColumnName("idVenta");
            entity.Property(e => e.TipoTransaccion)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("tipoTransaccion");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.Transacciones)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("FK__transacci__idCom__5CD6CB2B");

            entity.HasOne(d => d.IdDevolucionNavigation).WithMany(p => p.Transacciones)
                .HasForeignKey(d => d.IdDevolucion)
                .HasConstraintName("FK__transacci__idDev__5EBF139D");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Transacciones)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__transacci__idPro__5BE2A6F2");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.Transacciones)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK__transacci__idVen__5DCAEF64");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            
            entity.HasKey(e => e.IdUsuario).HasName("PK__usuarios__645723A6D0FAC1CE");

            entity.ToTable("usuarios");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.ApellidoUsuario)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("apellidoUsuario");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.IdCargo).HasColumnName("idCargo");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nombreUsuario");
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdCargoNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdCargo)
                .HasConstraintName("FK__usuarios__idCarg__398D8EEE");


        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PK__ventas__077D561489117173");

            entity.ToTable("ventas");

            entity.Property(e => e.IdVenta).HasColumnName("idVenta");
            entity.Property(e => e.CantidadProductosVenta).HasColumnName("cantidadProductosVenta");
            entity.Property(e => e.Cliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("cliente");
            entity.Property(e => e.FechaHoraVenta)
                .HasColumnType("datetime")
                .HasColumnName("fechaHoraVenta");
            entity.Property(e => e.IdFormPago).HasColumnName("idFormPago");
            entity.Property(e => e.IdPrecioProducto).HasColumnName("idPrecioProducto");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.TotalVenta)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalVenta");

            entity.HasOne(d => d.IdFormPagoNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdFormPago)
                .HasConstraintName("FK__ventas__idFormPa__4F7CD00D");

            entity.HasOne(d => d.IdPrecioProductoNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdPrecioProducto)
                .HasConstraintName("FK__ventas__idPrecio__5070F446");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__ventas__idUsuari__4E88ABD4");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
