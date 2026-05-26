USE [master]
GO
/****** Object:  Database [ClinicaDentalAppDB]    Script Date: 15/04/2026 23:13:18 ******/
CREATE DATABASE [ClinicaDentalAppDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ClinicaDentalAppDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER01\MSSQL\DATA\ClinicaDentalAppDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ClinicaDentalAppDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER01\MSSQL\DATA\ClinicaDentalAppDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ClinicaDentalAppDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ClinicaDentalAppDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ClinicaDentalAppDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET RECOVERY FULL 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET  MULTI_USER 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ClinicaDentalAppDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ClinicaDentalAppDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'ClinicaDentalAppDB', N'ON'
GO
ALTER DATABASE [ClinicaDentalAppDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [ClinicaDentalAppDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ClinicaDentalAppDB]
GO
/****** Object:  UserDefinedTableType [dbo].[TVP_DetalleRegistroClinico]    Script Date: 15/04/2026 23:13:18 ******/
CREATE TYPE [dbo].[TVP_DetalleRegistroClinico] AS TABLE(
	[id_categoria_servicio] [int] NOT NULL,
	[pieza_dental] [nvarchar](20) NULL,
	[descripcion_procedimiento] [nvarchar](1000) NOT NULL,
	[precio_aplicado] [decimal](10, 2) NOT NULL,
	[observaciones] [nvarchar](500) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[TVP_DetalleVenta]    Script Date: 15/04/2026 23:13:18 ******/
CREATE TYPE [dbo].[TVP_DetalleVenta] AS TABLE(
	[tipo_detalle] [nvarchar](15) NOT NULL,
	[id_categoria_servicio] [int] NULL,
	[id_producto] [int] NULL,
	[descripcion] [nvarchar](250) NOT NULL,
	[cantidad] [decimal](12, 2) NOT NULL,
	[precio_unitario] [decimal](10, 2) NOT NULL
)
GO
/****** Object:  Table [dbo].[Proveedor]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proveedor](
	[id_proveedor] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](150) NOT NULL,
	[nit] [nvarchar](30) NULL,
	[telefono] [nvarchar](20) NULL,
	[correo] [nvarchar](150) NULL,
	[direccion] [nvarchar](250) NULL,
	[contacto] [nvarchar](100) NULL,
	[activo] [bit] NOT NULL,
	[creado_en] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_proveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Producto]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Producto](
	[id_producto] [int] IDENTITY(1,1) NOT NULL,
	[codigo_producto] [nvarchar](20) NOT NULL,
	[nombre] [nvarchar](150) NOT NULL,
	[descripcion] [nvarchar](300) NULL,
	[id_proveedor] [int] NULL,
	[stock_actual] [decimal](12, 2) NOT NULL,
	[stock_minimo] [decimal](12, 2) NOT NULL,
	[unidad_medida] [nvarchar](30) NOT NULL,
	[fecha_vencimiento] [date] NULL,
	[costo_unitario] [decimal](10, 2) NOT NULL,
	[precio_venta] [decimal](10, 2) NULL,
	[activo] [bit] NOT NULL,
	[creado_en] [datetime2](0) NOT NULL,
	[actualizado_en] [datetime2](0) NULL,
	[row_version] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_producto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_StockBajoMinimo]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* =========================================================
   VISTAS PARA REPORTES
   ========================================================= */

CREATE VIEW [dbo].[vw_StockBajoMinimo]
AS
SELECT
    p.id_producto,
    p.codigo_producto,
    p.nombre,
    p.stock_actual,
    p.stock_minimo,
    p.unidad_medida,
    pr.nombre AS proveedor
FROM dbo.Producto p
LEFT JOIN dbo.Proveedor pr ON pr.id_proveedor = p.id_proveedor
WHERE p.activo = 1
  AND p.stock_actual <= p.stock_minimo;

GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[id_usuario] [int] IDENTITY(1,1) NOT NULL,
	[codigo_usuario] [nvarchar](20) NOT NULL,
	[nombres] [nvarchar](100) NOT NULL,
	[apellidos] [nvarchar](100) NOT NULL,
	[username] [nvarchar](50) NOT NULL,
	[correo] [nvarchar](150) NULL,
	[telefono] [nvarchar](20) NULL,
	[password_hash] [nvarchar](500) NOT NULL,
	[password_salt] [nvarchar](250) NULL,
	[id_rol] [int] NOT NULL,
	[activo] [bit] NOT NULL,
	[creado_en] [datetime2](0) NOT NULL,
	[actualizado_en] [datetime2](0) NULL,
	[row_version] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Paciente]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Paciente](
	[id_paciente] [int] IDENTITY(1,1) NOT NULL,
	[codigo_paciente] [nvarchar](20) NOT NULL,
	[nombres] [nvarchar](100) NOT NULL,
	[apellidos] [nvarchar](100) NOT NULL,
	[telefono] [nvarchar](20) NOT NULL,
	[fecha_nacimiento] [date] NOT NULL,
	[genero] [nvarchar](20) NOT NULL,
	[direccion] [nvarchar](250) NULL,
	[correo] [nvarchar](150) NULL,
	[alergias] [nvarchar](500) NULL,
	[observaciones_generales] [nvarchar](1000) NULL,
	[activo] [bit] NOT NULL,
	[creado_en] [datetime2](0) NOT NULL,
	[actualizado_en] [datetime2](0) NULL,
	[row_version] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_paciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoriaServicio]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoriaServicio](
	[id_categoria_servicio] [int] IDENTITY(1,1) NOT NULL,
	[codigo_categoria] [nvarchar](20) NOT NULL,
	[nombre] [nvarchar](100) NOT NULL,
	[descripcion] [nvarchar](300) NULL,
	[precio_base] [decimal](10, 2) NOT NULL,
	[activo] [bit] NOT NULL,
	[creado_en] [datetime2](0) NOT NULL,
	[actualizado_en] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[id_categoria_servicio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RegistroClinico]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegistroClinico](
	[id_registro_clinico] [int] IDENTITY(1,1) NOT NULL,
	[id_cita] [int] NOT NULL,
	[id_paciente] [int] NOT NULL,
	[id_dentista] [int] NOT NULL,
	[fecha_atencion] [datetime2](0) NOT NULL,
	[diagnostico] [nvarchar](1000) NOT NULL,
	[observaciones] [nvarchar](1000) NULL,
	[indicaciones_generales] [nvarchar](1000) NULL,
	[creado_en] [datetime2](0) NOT NULL,
	[actualizado_en] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[id_registro_clinico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetalleRegistroClinico]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetalleRegistroClinico](
	[id_detalle_registro] [int] IDENTITY(1,1) NOT NULL,
	[id_registro_clinico] [int] NOT NULL,
	[id_categoria_servicio] [int] NOT NULL,
	[pieza_dental] [nvarchar](20) NULL,
	[descripcion_procedimiento] [nvarchar](1000) NOT NULL,
	[precio_aplicado] [decimal](10, 2) NOT NULL,
	[observaciones] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[id_detalle_registro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_HistorialClinicoPaciente]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_HistorialClinicoPaciente]
AS
SELECT
    rc.id_registro_clinico,
    p.id_paciente,
    p.codigo_paciente,
    CONCAT(p.nombres, N' ', p.apellidos) AS paciente,
    rc.fecha_atencion,
    CONCAT(u.nombres, N' ', u.apellidos) AS dentista,
    rc.diagnostico,
    drc.pieza_dental,
    cs.nombre AS servicio,
    drc.descripcion_procedimiento,
    drc.precio_aplicado
FROM dbo.RegistroClinico rc
INNER JOIN dbo.Paciente p ON p.id_paciente = rc.id_paciente
INNER JOIN dbo.Usuario u ON u.id_usuario = rc.id_dentista
LEFT JOIN dbo.DetalleRegistroClinico drc ON drc.id_registro_clinico = rc.id_registro_clinico
LEFT JOIN dbo.CategoriaServicio cs ON cs.id_categoria_servicio = drc.id_categoria_servicio;

GO
/****** Object:  Table [dbo].[EstadoVenta]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoVenta](
	[id_estado_venta] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_estado_venta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MetodoPago]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetodoPago](
	[id_metodo_pago] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_metodo_pago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Venta]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Venta](
	[id_venta] [int] IDENTITY(1,1) NOT NULL,
	[numero_venta] [nvarchar](20) NOT NULL,
	[id_paciente] [int] NULL,
	[id_usuario] [int] NOT NULL,
	[fecha_venta] [datetime2](0) NOT NULL,
	[subtotal] [decimal](12, 2) NOT NULL,
	[descuento] [decimal](12, 2) NOT NULL,
	[total] [decimal](12, 2) NOT NULL,
	[id_estado_venta] [int] NOT NULL,
	[id_metodo_pago] [int] NOT NULL,
	[observaciones] [nvarchar](500) NULL,
	[creado_en] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_venta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetalleVenta]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetalleVenta](
	[id_detalle_venta] [int] IDENTITY(1,1) NOT NULL,
	[id_venta] [int] NOT NULL,
	[tipo_detalle] [nvarchar](15) NOT NULL,
	[id_categoria_servicio] [int] NULL,
	[id_producto] [int] NULL,
	[descripcion] [nvarchar](250) NOT NULL,
	[cantidad] [decimal](12, 2) NOT NULL,
	[precio_unitario] [decimal](10, 2) NOT NULL,
	[subtotal] [decimal](12, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_detalle_venta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_VentasDetalle]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_VentasDetalle]
AS
SELECT
    v.id_venta,
    v.numero_venta,
    v.fecha_venta,
    CONCAT(u.nombres, N' ', u.apellidos) AS usuario_registra,
    CASE WHEN p.id_paciente IS NULL THEN NULL ELSE CONCAT(p.nombres, N' ', p.apellidos) END AS paciente,
    dv.tipo_detalle,
    COALESCE(cs.nombre, pr.nombre, dv.descripcion) AS item,
    dv.cantidad,
    dv.precio_unitario,
    dv.subtotal,
    v.total,
    ev.nombre AS estado_venta,
    mp.nombre AS metodo_pago
FROM dbo.Venta v
INNER JOIN dbo.Usuario u ON u.id_usuario = v.id_usuario
LEFT JOIN dbo.Paciente p ON p.id_paciente = v.id_paciente
INNER JOIN dbo.DetalleVenta dv ON dv.id_venta = v.id_venta
LEFT JOIN dbo.CategoriaServicio cs ON cs.id_categoria_servicio = dv.id_categoria_servicio
LEFT JOIN dbo.Producto pr ON pr.id_producto = dv.id_producto
INNER JOIN dbo.EstadoVenta ev ON ev.id_estado_venta = v.id_estado_venta
INNER JOIN dbo.MetodoPago mp ON mp.id_metodo_pago = v.id_metodo_pago;

GO
/****** Object:  Table [dbo].[Cita]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cita](
	[id_cita] [int] IDENTITY(1,1) NOT NULL,
	[id_paciente] [int] NOT NULL,
	[id_dentista] [int] NOT NULL,
	[fecha] [date] NOT NULL,
	[hora_inicio] [time](0) NOT NULL,
	[hora_fin] [time](0) NOT NULL,
	[motivo] [nvarchar](250) NOT NULL,
	[observaciones] [nvarchar](1000) NULL,
	[id_estado_cita] [int] NOT NULL,
	[creada_por] [int] NOT NULL,
	[creada_en] [datetime2](0) NOT NULL,
	[actualizada_en] [datetime2](0) NULL,
	[row_version] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_cita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CitaHistorialEstado]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CitaHistorialEstado](
	[id_historial] [int] IDENTITY(1,1) NOT NULL,
	[id_cita] [int] NOT NULL,
	[id_estado_cita] [int] NOT NULL,
	[comentario] [nvarchar](500) NULL,
	[cambiado_por] [int] NOT NULL,
	[cambiado_en] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_historial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadoCita]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoCita](
	[id_estado_cita] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_estado_cita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MovimientoInventario]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovimientoInventario](
	[id_movimiento_inventario] [int] IDENTITY(1,1) NOT NULL,
	[id_producto] [int] NOT NULL,
	[id_tipo_movimiento] [int] NOT NULL,
	[cantidad] [decimal](12, 2) NOT NULL,
	[costo_unitario] [decimal](10, 2) NULL,
	[referencia] [nvarchar](100) NULL,
	[observaciones] [nvarchar](500) NULL,
	[realizado_por] [int] NOT NULL,
	[fecha_movimiento] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_movimiento_inventario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OutboxEvent]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OutboxEvent](
	[id_evento] [bigint] IDENTITY(1,1) NOT NULL,
	[tipo_evento] [nvarchar](100) NOT NULL,
	[payload_json] [nvarchar](max) NOT NULL,
	[estado] [nvarchar](20) NOT NULL,
	[creado_en] [datetime2](0) NOT NULL,
	[procesado_en] [datetime2](0) NULL,
	[reintentos] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_evento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permiso]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permiso](
	[id_permiso] [int] IDENTITY(1,1) NOT NULL,
	[clave] [nvarchar](100) NOT NULL,
	[descripcion] [nvarchar](250) NULL,
	[modulo] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_permiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rol]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol](
	[id_rol] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
	[descripcion] [nvarchar](250) NULL,
	[activo] [bit] NOT NULL,
	[creado_en] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_rol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RolPermiso]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolPermiso](
	[id_rol] [int] NOT NULL,
	[id_permiso] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_rol] ASC,
	[id_permiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoMovimientoInventario]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoMovimientoInventario](
	[id_tipo_movimiento] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_tipo_movimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[EstadoCita] ON 

INSERT [dbo].[EstadoCita] ([id_estado_cita], [nombre]) VALUES (5, N'Cancelada')
INSERT [dbo].[EstadoCita] ([id_estado_cita], [nombre]) VALUES (2, N'Confirmada')
INSERT [dbo].[EstadoCita] ([id_estado_cita], [nombre]) VALUES (3, N'EnAtencion')
INSERT [dbo].[EstadoCita] ([id_estado_cita], [nombre]) VALUES (4, N'Finalizada')
INSERT [dbo].[EstadoCita] ([id_estado_cita], [nombre]) VALUES (6, N'NoAsistio')
INSERT [dbo].[EstadoCita] ([id_estado_cita], [nombre]) VALUES (1, N'Programada')
SET IDENTITY_INSERT [dbo].[EstadoCita] OFF
GO
SET IDENTITY_INSERT [dbo].[EstadoVenta] ON 

INSERT [dbo].[EstadoVenta] ([id_estado_venta], [nombre]) VALUES (3, N'Anulada')
INSERT [dbo].[EstadoVenta] ([id_estado_venta], [nombre]) VALUES (2, N'Pagada')
INSERT [dbo].[EstadoVenta] ([id_estado_venta], [nombre]) VALUES (1, N'Registrada')
SET IDENTITY_INSERT [dbo].[EstadoVenta] OFF
GO
SET IDENTITY_INSERT [dbo].[MetodoPago] ON 

INSERT [dbo].[MetodoPago] ([id_metodo_pago], [nombre]) VALUES (1, N'Efectivo')
INSERT [dbo].[MetodoPago] ([id_metodo_pago], [nombre]) VALUES (4, N'Mixto')
INSERT [dbo].[MetodoPago] ([id_metodo_pago], [nombre]) VALUES (2, N'Tarjeta')
INSERT [dbo].[MetodoPago] ([id_metodo_pago], [nombre]) VALUES (3, N'Transferencia')
SET IDENTITY_INSERT [dbo].[MetodoPago] OFF
GO
SET IDENTITY_INSERT [dbo].[Permiso] ON 

INSERT [dbo].[Permiso] ([id_permiso], [clave], [descripcion], [modulo]) VALUES (1, N'USUARIOS_CREAR', N'Crear usuarios', N'Seguridad')
INSERT [dbo].[Permiso] ([id_permiso], [clave], [descripcion], [modulo]) VALUES (2, N'PACIENTES_GESTIONAR', N'Gestionar pacientes', N'Pacientes')
INSERT [dbo].[Permiso] ([id_permiso], [clave], [descripcion], [modulo]) VALUES (3, N'CITAS_GESTIONAR', N'Gestionar citas', N'Citas')
INSERT [dbo].[Permiso] ([id_permiso], [clave], [descripcion], [modulo]) VALUES (4, N'CLINICA_GESTIONAR', N'Gestionar registros clínicos', N'Clínica')
INSERT [dbo].[Permiso] ([id_permiso], [clave], [descripcion], [modulo]) VALUES (5, N'INVENTARIO_GESTIONAR', N'Gestionar inventario', N'Inventario')
INSERT [dbo].[Permiso] ([id_permiso], [clave], [descripcion], [modulo]) VALUES (6, N'VENTAS_GESTIONAR', N'Gestionar ventas', N'Ventas')
INSERT [dbo].[Permiso] ([id_permiso], [clave], [descripcion], [modulo]) VALUES (7, N'REPORTES_VER', N'Consultar reportes', N'Reportes')
SET IDENTITY_INSERT [dbo].[Permiso] OFF
GO
SET IDENTITY_INSERT [dbo].[Rol] ON 

INSERT [dbo].[Rol] ([id_rol], [nombre], [descripcion], [activo], [creado_en]) VALUES (1, N'Administrador', N'Acceso completo al sistema', 1, CAST(N'2026-04-16T04:38:56.0000000' AS DateTime2))
INSERT [dbo].[Rol] ([id_rol], [nombre], [descripcion], [activo], [creado_en]) VALUES (2, N'Dentista', N'Gestión clínica y atención de pacientes', 1, CAST(N'2026-04-16T04:38:56.0000000' AS DateTime2))
INSERT [dbo].[Rol] ([id_rol], [nombre], [descripcion], [activo], [creado_en]) VALUES (3, N'Recepcionista', N'Gestión de agenda y atención en recepción', 1, CAST(N'2026-04-16T04:38:56.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Rol] OFF
GO
INSERT [dbo].[RolPermiso] ([id_rol], [id_permiso]) VALUES (1, 1)
INSERT [dbo].[RolPermiso] ([id_rol], [id_permiso]) VALUES (1, 2)
INSERT [dbo].[RolPermiso] ([id_rol], [id_permiso]) VALUES (2, 2)
INSERT [dbo].[RolPermiso] ([id_rol], [id_permiso]) VALUES (3, 2)
INSERT [dbo].[RolPermiso] ([id_rol], [id_permiso]) VALUES (1, 3)
INSERT [dbo].[RolPermiso] ([id_rol], [id_permiso]) VALUES (2, 3)
INSERT [dbo].[RolPermiso] ([id_rol], [id_permiso]) VALUES (3, 3)
INSERT [dbo].[RolPermiso] ([id_rol], [id_permiso]) VALUES (1, 4)
INSERT [dbo].[RolPermiso] ([id_rol], [id_permiso]) VALUES (2, 4)
INSERT [dbo].[RolPermiso] ([id_rol], [id_permiso]) VALUES (1, 5)
INSERT [dbo].[RolPermiso] ([id_rol], [id_permiso]) VALUES (1, 6)
INSERT [dbo].[RolPermiso] ([id_rol], [id_permiso]) VALUES (3, 6)
INSERT [dbo].[RolPermiso] ([id_rol], [id_permiso]) VALUES (1, 7)
INSERT [dbo].[RolPermiso] ([id_rol], [id_permiso]) VALUES (2, 7)
INSERT [dbo].[RolPermiso] ([id_rol], [id_permiso]) VALUES (3, 7)
GO
SET IDENTITY_INSERT [dbo].[TipoMovimientoInventario] ON 

INSERT [dbo].[TipoMovimientoInventario] ([id_tipo_movimiento], [nombre]) VALUES (4, N'AjusteNegativo')
INSERT [dbo].[TipoMovimientoInventario] ([id_tipo_movimiento], [nombre]) VALUES (3, N'AjustePositivo')
INSERT [dbo].[TipoMovimientoInventario] ([id_tipo_movimiento], [nombre]) VALUES (1, N'Entrada')
INSERT [dbo].[TipoMovimientoInventario] ([id_tipo_movimiento], [nombre]) VALUES (2, N'Salida')
SET IDENTITY_INSERT [dbo].[TipoMovimientoInventario] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_CategoriaServicio_Codigo]    Script Date: 15/04/2026 23:13:18 ******/
ALTER TABLE [dbo].[CategoriaServicio] ADD  CONSTRAINT [UQ_CategoriaServicio_Codigo] UNIQUE NONCLUSTERED 
(
	[codigo_categoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_CategoriaServicio_Nombre]    Script Date: 15/04/2026 23:13:18 ******/
ALTER TABLE [dbo].[CategoriaServicio] ADD  CONSTRAINT [UQ_CategoriaServicio_Nombre] UNIQUE NONCLUSTERED 
(
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Cita_Estado_Fecha]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_Cita_Estado_Fecha] ON [dbo].[Cita]
(
	[id_estado_cita] ASC,
	[fecha] ASC
)
INCLUDE([hora_inicio],[hora_fin],[id_dentista],[id_paciente]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Cita_Fecha_Dentista]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_Cita_Fecha_Dentista] ON [dbo].[Cita]
(
	[fecha] ASC,
	[hora_inicio] ASC,
	[id_dentista] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Cita_Paciente]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_Cita_Paciente] ON [dbo].[Cita]
(
	[id_paciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CitaHistorialEstado_Cita_Fecha]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_CitaHistorialEstado_Cita_Fecha] ON [dbo].[CitaHistorialEstado]
(
	[id_cita] ASC,
	[cambiado_en] DESC
)
INCLUDE([id_estado_cita],[cambiado_por]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DetalleRegistroClinico_Registro]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_DetalleRegistroClinico_Registro] ON [dbo].[DetalleRegistroClinico]
(
	[id_registro_clinico] ASC
)
INCLUDE([id_categoria_servicio],[pieza_dental],[precio_aplicado]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DetalleVenta_CategoriaServicio]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_DetalleVenta_CategoriaServicio] ON [dbo].[DetalleVenta]
(
	[id_categoria_servicio] ASC
)
INCLUDE([id_venta],[cantidad],[precio_unitario],[subtotal]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DetalleVenta_Producto]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_DetalleVenta_Producto] ON [dbo].[DetalleVenta]
(
	[id_producto] ASC
)
INCLUDE([id_venta],[cantidad],[precio_unitario],[subtotal]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DetalleVenta_Venta]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_DetalleVenta_Venta] ON [dbo].[DetalleVenta]
(
	[id_venta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_EstadoCita_Nombre]    Script Date: 15/04/2026 23:13:18 ******/
ALTER TABLE [dbo].[EstadoCita] ADD  CONSTRAINT [UQ_EstadoCita_Nombre] UNIQUE NONCLUSTERED 
(
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_EstadoVenta_Nombre]    Script Date: 15/04/2026 23:13:18 ******/
ALTER TABLE [dbo].[EstadoVenta] ADD  CONSTRAINT [UQ_EstadoVenta_Nombre] UNIQUE NONCLUSTERED 
(
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_MetodoPago_Nombre]    Script Date: 15/04/2026 23:13:18 ******/
ALTER TABLE [dbo].[MetodoPago] ADD  CONSTRAINT [UQ_MetodoPago_Nombre] UNIQUE NONCLUSTERED 
(
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MovimientoInventario_Fecha_Tipo]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_MovimientoInventario_Fecha_Tipo] ON [dbo].[MovimientoInventario]
(
	[fecha_movimiento] DESC,
	[id_tipo_movimiento] ASC
)
INCLUDE([id_producto],[cantidad],[realizado_por]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MovimientoInventario_Producto_Fecha]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_MovimientoInventario_Producto_Fecha] ON [dbo].[MovimientoInventario]
(
	[id_producto] ASC,
	[fecha_movimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_OutboxEvent_Estado_CreadoEn]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_OutboxEvent_Estado_CreadoEn] ON [dbo].[OutboxEvent]
(
	[estado] ASC,
	[creado_en] ASC
)
INCLUDE([tipo_evento],[reintentos]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Paciente_Codigo]    Script Date: 15/04/2026 23:13:18 ******/
ALTER TABLE [dbo].[Paciente] ADD  CONSTRAINT [UQ_Paciente_Codigo] UNIQUE NONCLUSTERED 
(
	[codigo_paciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Permiso_Clave]    Script Date: 15/04/2026 23:13:18 ******/
ALTER TABLE [dbo].[Permiso] ADD  CONSTRAINT [UQ_Permiso_Clave] UNIQUE NONCLUSTERED 
(
	[clave] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Producto_Codigo]    Script Date: 15/04/2026 23:13:18 ******/
ALTER TABLE [dbo].[Producto] ADD  CONSTRAINT [UQ_Producto_Codigo] UNIQUE NONCLUSTERED 
(
	[codigo_producto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Producto_Activo]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_Producto_Activo] ON [dbo].[Producto]
(
	[activo] ASC,
	[nombre] ASC
)
INCLUDE([codigo_producto],[stock_actual],[stock_minimo],[precio_venta]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Producto_Proveedor]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_Producto_Proveedor] ON [dbo].[Producto]
(
	[id_proveedor] ASC
)
INCLUDE([nombre],[codigo_producto],[activo]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Producto_StockActual_StockMinimo]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_Producto_StockActual_StockMinimo] ON [dbo].[Producto]
(
	[stock_actual] ASC,
	[stock_minimo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_RegistroClinico_Cita]    Script Date: 15/04/2026 23:13:18 ******/
ALTER TABLE [dbo].[RegistroClinico] ADD  CONSTRAINT [UQ_RegistroClinico_Cita] UNIQUE NONCLUSTERED 
(
	[id_cita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RegistroClinico_Dentista_Fecha]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_RegistroClinico_Dentista_Fecha] ON [dbo].[RegistroClinico]
(
	[id_dentista] ASC,
	[fecha_atencion] DESC
)
INCLUDE([id_paciente],[id_cita]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RegistroClinico_Paciente_Fecha]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_RegistroClinico_Paciente_Fecha] ON [dbo].[RegistroClinico]
(
	[id_paciente] ASC,
	[fecha_atencion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Rol_Nombre]    Script Date: 15/04/2026 23:13:18 ******/
ALTER TABLE [dbo].[Rol] ADD  CONSTRAINT [UQ_Rol_Nombre] UNIQUE NONCLUSTERED 
(
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RolPermiso_Permiso]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_RolPermiso_Permiso] ON [dbo].[RolPermiso]
(
	[id_permiso] ASC,
	[id_rol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_TipoMovimiento_Nombre]    Script Date: 15/04/2026 23:13:18 ******/
ALTER TABLE [dbo].[TipoMovimientoInventario] ADD  CONSTRAINT [UQ_TipoMovimiento_Nombre] UNIQUE NONCLUSTERED 
(
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Usuario_Codigo]    Script Date: 15/04/2026 23:13:18 ******/
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [UQ_Usuario_Codigo] UNIQUE NONCLUSTERED 
(
	[codigo_usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Usuario_Correo]    Script Date: 15/04/2026 23:13:18 ******/
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [UQ_Usuario_Correo] UNIQUE NONCLUSTERED 
(
	[correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Usuario_Username]    Script Date: 15/04/2026 23:13:18 ******/
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [UQ_Usuario_Username] UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Venta_Numero]    Script Date: 15/04/2026 23:13:18 ******/
ALTER TABLE [dbo].[Venta] ADD  CONSTRAINT [UQ_Venta_Numero] UNIQUE NONCLUSTERED 
(
	[numero_venta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Venta_Estado_Fecha]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_Venta_Estado_Fecha] ON [dbo].[Venta]
(
	[id_estado_venta] ASC,
	[fecha_venta] DESC
)
INCLUDE([id_paciente],[id_usuario],[total],[id_metodo_pago]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Venta_Fecha]    Script Date: 15/04/2026 23:13:18 ******/
CREATE NONCLUSTERED INDEX [IX_Venta_Fecha] ON [dbo].[Venta]
(
	[fecha_venta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CategoriaServicio] ADD  CONSTRAINT [DF_CategoriaServicio_Activo]  DEFAULT ((1)) FOR [activo]
GO
ALTER TABLE [dbo].[CategoriaServicio] ADD  CONSTRAINT [DF_CategoriaServicio_CreadoEn]  DEFAULT (sysutcdatetime()) FOR [creado_en]
GO
ALTER TABLE [dbo].[Cita] ADD  CONSTRAINT [DF_Cita_CreadaEn]  DEFAULT (sysutcdatetime()) FOR [creada_en]
GO
ALTER TABLE [dbo].[CitaHistorialEstado] ADD  CONSTRAINT [DF_CitaHistorial_CambiadoEn]  DEFAULT (sysutcdatetime()) FOR [cambiado_en]
GO
ALTER TABLE [dbo].[MovimientoInventario] ADD  CONSTRAINT [DF_MovimientoInventario_Fecha]  DEFAULT (sysutcdatetime()) FOR [fecha_movimiento]
GO
ALTER TABLE [dbo].[OutboxEvent] ADD  CONSTRAINT [DF_OutboxEvent_Estado]  DEFAULT (N'Pendiente') FOR [estado]
GO
ALTER TABLE [dbo].[OutboxEvent] ADD  CONSTRAINT [DF_OutboxEvent_CreadoEn]  DEFAULT (sysutcdatetime()) FOR [creado_en]
GO
ALTER TABLE [dbo].[OutboxEvent] ADD  CONSTRAINT [DF_OutboxEvent_Reintentos]  DEFAULT ((0)) FOR [reintentos]
GO
ALTER TABLE [dbo].[Paciente] ADD  CONSTRAINT [DF_Paciente_Activo]  DEFAULT ((1)) FOR [activo]
GO
ALTER TABLE [dbo].[Paciente] ADD  CONSTRAINT [DF_Paciente_CreadoEn]  DEFAULT (sysutcdatetime()) FOR [creado_en]
GO
ALTER TABLE [dbo].[Producto] ADD  CONSTRAINT [DF_Producto_StockActual]  DEFAULT ((0)) FOR [stock_actual]
GO
ALTER TABLE [dbo].[Producto] ADD  CONSTRAINT [DF_Producto_StockMinimo]  DEFAULT ((0)) FOR [stock_minimo]
GO
ALTER TABLE [dbo].[Producto] ADD  CONSTRAINT [DF_Producto_Activo]  DEFAULT ((1)) FOR [activo]
GO
ALTER TABLE [dbo].[Producto] ADD  CONSTRAINT [DF_Producto_CreadoEn]  DEFAULT (sysutcdatetime()) FOR [creado_en]
GO
ALTER TABLE [dbo].[Proveedor] ADD  CONSTRAINT [DF_Proveedor_Activo]  DEFAULT ((1)) FOR [activo]
GO
ALTER TABLE [dbo].[Proveedor] ADD  CONSTRAINT [DF_Proveedor_CreadoEn]  DEFAULT (sysutcdatetime()) FOR [creado_en]
GO
ALTER TABLE [dbo].[RegistroClinico] ADD  CONSTRAINT [DF_RegistroClinico_Fecha]  DEFAULT (sysutcdatetime()) FOR [fecha_atencion]
GO
ALTER TABLE [dbo].[RegistroClinico] ADD  CONSTRAINT [DF_RegistroClinico_CreadoEn]  DEFAULT (sysutcdatetime()) FOR [creado_en]
GO
ALTER TABLE [dbo].[Rol] ADD  CONSTRAINT [DF_Rol_Activo]  DEFAULT ((1)) FOR [activo]
GO
ALTER TABLE [dbo].[Rol] ADD  CONSTRAINT [DF_Rol_CreadoEn]  DEFAULT (sysutcdatetime()) FOR [creado_en]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_Activo]  DEFAULT ((1)) FOR [activo]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_CreadoEn]  DEFAULT (sysutcdatetime()) FOR [creado_en]
GO
ALTER TABLE [dbo].[Venta] ADD  CONSTRAINT [DF_Venta_Fecha]  DEFAULT (sysutcdatetime()) FOR [fecha_venta]
GO
ALTER TABLE [dbo].[Venta] ADD  CONSTRAINT [DF_Venta_Descuento]  DEFAULT ((0)) FOR [descuento]
GO
ALTER TABLE [dbo].[Venta] ADD  CONSTRAINT [DF_Venta_CreadoEn]  DEFAULT (sysutcdatetime()) FOR [creado_en]
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [FK_Cita_CreadaPor] FOREIGN KEY([creada_por])
REFERENCES [dbo].[Usuario] ([id_usuario])
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_CreadaPor]
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [FK_Cita_Dentista] FOREIGN KEY([id_dentista])
REFERENCES [dbo].[Usuario] ([id_usuario])
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_Dentista]
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [FK_Cita_Estado] FOREIGN KEY([id_estado_cita])
REFERENCES [dbo].[EstadoCita] ([id_estado_cita])
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_Estado]
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [FK_Cita_Paciente] FOREIGN KEY([id_paciente])
REFERENCES [dbo].[Paciente] ([id_paciente])
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_Paciente]
GO
ALTER TABLE [dbo].[CitaHistorialEstado]  WITH CHECK ADD  CONSTRAINT [FK_CitaHistorial_Cita] FOREIGN KEY([id_cita])
REFERENCES [dbo].[Cita] ([id_cita])
GO
ALTER TABLE [dbo].[CitaHistorialEstado] CHECK CONSTRAINT [FK_CitaHistorial_Cita]
GO
ALTER TABLE [dbo].[CitaHistorialEstado]  WITH CHECK ADD  CONSTRAINT [FK_CitaHistorial_Estado] FOREIGN KEY([id_estado_cita])
REFERENCES [dbo].[EstadoCita] ([id_estado_cita])
GO
ALTER TABLE [dbo].[CitaHistorialEstado] CHECK CONSTRAINT [FK_CitaHistorial_Estado]
GO
ALTER TABLE [dbo].[CitaHistorialEstado]  WITH CHECK ADD  CONSTRAINT [FK_CitaHistorial_Usuario] FOREIGN KEY([cambiado_por])
REFERENCES [dbo].[Usuario] ([id_usuario])
GO
ALTER TABLE [dbo].[CitaHistorialEstado] CHECK CONSTRAINT [FK_CitaHistorial_Usuario]
GO
ALTER TABLE [dbo].[DetalleRegistroClinico]  WITH CHECK ADD  CONSTRAINT [FK_DetalleRegistro_Categoria] FOREIGN KEY([id_categoria_servicio])
REFERENCES [dbo].[CategoriaServicio] ([id_categoria_servicio])
GO
ALTER TABLE [dbo].[DetalleRegistroClinico] CHECK CONSTRAINT [FK_DetalleRegistro_Categoria]
GO
ALTER TABLE [dbo].[DetalleRegistroClinico]  WITH CHECK ADD  CONSTRAINT [FK_DetalleRegistro_Registro] FOREIGN KEY([id_registro_clinico])
REFERENCES [dbo].[RegistroClinico] ([id_registro_clinico])
GO
ALTER TABLE [dbo].[DetalleRegistroClinico] CHECK CONSTRAINT [FK_DetalleRegistro_Registro]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [FK_DetalleVenta_Categoria] FOREIGN KEY([id_categoria_servicio])
REFERENCES [dbo].[CategoriaServicio] ([id_categoria_servicio])
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [FK_DetalleVenta_Categoria]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [FK_DetalleVenta_Producto] FOREIGN KEY([id_producto])
REFERENCES [dbo].[Producto] ([id_producto])
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [FK_DetalleVenta_Producto]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [FK_DetalleVenta_Venta] FOREIGN KEY([id_venta])
REFERENCES [dbo].[Venta] ([id_venta])
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [FK_DetalleVenta_Venta]
GO
ALTER TABLE [dbo].[MovimientoInventario]  WITH CHECK ADD  CONSTRAINT [FK_MovimientoInventario_Producto] FOREIGN KEY([id_producto])
REFERENCES [dbo].[Producto] ([id_producto])
GO
ALTER TABLE [dbo].[MovimientoInventario] CHECK CONSTRAINT [FK_MovimientoInventario_Producto]
GO
ALTER TABLE [dbo].[MovimientoInventario]  WITH CHECK ADD  CONSTRAINT [FK_MovimientoInventario_Tipo] FOREIGN KEY([id_tipo_movimiento])
REFERENCES [dbo].[TipoMovimientoInventario] ([id_tipo_movimiento])
GO
ALTER TABLE [dbo].[MovimientoInventario] CHECK CONSTRAINT [FK_MovimientoInventario_Tipo]
GO
ALTER TABLE [dbo].[MovimientoInventario]  WITH CHECK ADD  CONSTRAINT [FK_MovimientoInventario_Usuario] FOREIGN KEY([realizado_por])
REFERENCES [dbo].[Usuario] ([id_usuario])
GO
ALTER TABLE [dbo].[MovimientoInventario] CHECK CONSTRAINT [FK_MovimientoInventario_Usuario]
GO
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [FK_Producto_Proveedor] FOREIGN KEY([id_proveedor])
REFERENCES [dbo].[Proveedor] ([id_proveedor])
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [FK_Producto_Proveedor]
GO
ALTER TABLE [dbo].[RegistroClinico]  WITH CHECK ADD  CONSTRAINT [FK_RegistroClinico_Cita] FOREIGN KEY([id_cita])
REFERENCES [dbo].[Cita] ([id_cita])
GO
ALTER TABLE [dbo].[RegistroClinico] CHECK CONSTRAINT [FK_RegistroClinico_Cita]
GO
ALTER TABLE [dbo].[RegistroClinico]  WITH CHECK ADD  CONSTRAINT [FK_RegistroClinico_Dentista] FOREIGN KEY([id_dentista])
REFERENCES [dbo].[Usuario] ([id_usuario])
GO
ALTER TABLE [dbo].[RegistroClinico] CHECK CONSTRAINT [FK_RegistroClinico_Dentista]
GO
ALTER TABLE [dbo].[RegistroClinico]  WITH CHECK ADD  CONSTRAINT [FK_RegistroClinico_Paciente] FOREIGN KEY([id_paciente])
REFERENCES [dbo].[Paciente] ([id_paciente])
GO
ALTER TABLE [dbo].[RegistroClinico] CHECK CONSTRAINT [FK_RegistroClinico_Paciente]
GO
ALTER TABLE [dbo].[RolPermiso]  WITH CHECK ADD  CONSTRAINT [FK_RolPermiso_Permiso] FOREIGN KEY([id_permiso])
REFERENCES [dbo].[Permiso] ([id_permiso])
GO
ALTER TABLE [dbo].[RolPermiso] CHECK CONSTRAINT [FK_RolPermiso_Permiso]
GO
ALTER TABLE [dbo].[RolPermiso]  WITH CHECK ADD  CONSTRAINT [FK_RolPermiso_Rol] FOREIGN KEY([id_rol])
REFERENCES [dbo].[Rol] ([id_rol])
GO
ALTER TABLE [dbo].[RolPermiso] CHECK CONSTRAINT [FK_RolPermiso_Rol]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([id_rol])
REFERENCES [dbo].[Rol] ([id_rol])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Estado] FOREIGN KEY([id_estado_venta])
REFERENCES [dbo].[EstadoVenta] ([id_estado_venta])
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Estado]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_MetodoPago] FOREIGN KEY([id_metodo_pago])
REFERENCES [dbo].[MetodoPago] ([id_metodo_pago])
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_MetodoPago]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Paciente] FOREIGN KEY([id_paciente])
REFERENCES [dbo].[Paciente] ([id_paciente])
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Paciente]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Usuario] FOREIGN KEY([id_usuario])
REFERENCES [dbo].[Usuario] ([id_usuario])
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Usuario]
GO
ALTER TABLE [dbo].[CategoriaServicio]  WITH CHECK ADD  CONSTRAINT [CK_CategoriaServicio_Precio] CHECK  (([precio_base]>=(0)))
GO
ALTER TABLE [dbo].[CategoriaServicio] CHECK CONSTRAINT [CK_CategoriaServicio_Precio]
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [CK_Cita_Horas] CHECK  (([hora_fin]>[hora_inicio]))
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [CK_Cita_Horas]
GO
ALTER TABLE [dbo].[DetalleRegistroClinico]  WITH CHECK ADD  CONSTRAINT [CK_DetalleRegistro_Precio] CHECK  (([precio_aplicado]>=(0)))
GO
ALTER TABLE [dbo].[DetalleRegistroClinico] CHECK CONSTRAINT [CK_DetalleRegistro_Precio]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [CK_DetalleVenta_Cantidad] CHECK  (([cantidad]>(0)))
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [CK_DetalleVenta_Cantidad]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [CK_DetalleVenta_Consistencia] CHECK  (([subtotal]=[cantidad]*[precio_unitario]))
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [CK_DetalleVenta_Consistencia]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [CK_DetalleVenta_Precio] CHECK  (([precio_unitario]>=(0)))
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [CK_DetalleVenta_Precio]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [CK_DetalleVenta_Referencia] CHECK  (([tipo_detalle]=N'SERVICIO' AND [id_categoria_servicio] IS NOT NULL AND [id_producto] IS NULL OR [tipo_detalle]=N'PRODUCTO' AND [id_producto] IS NOT NULL AND [id_categoria_servicio] IS NULL))
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [CK_DetalleVenta_Referencia]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [CK_DetalleVenta_Subtotal] CHECK  (([subtotal]>=(0)))
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [CK_DetalleVenta_Subtotal]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [CK_DetalleVenta_Tipo] CHECK  (([tipo_detalle]=N'PRODUCTO' OR [tipo_detalle]=N'SERVICIO'))
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [CK_DetalleVenta_Tipo]
GO
ALTER TABLE [dbo].[MovimientoInventario]  WITH CHECK ADD  CONSTRAINT [CK_MovimientoInventario_Cantidad] CHECK  (([cantidad]>(0)))
GO
ALTER TABLE [dbo].[MovimientoInventario] CHECK CONSTRAINT [CK_MovimientoInventario_Cantidad]
GO
ALTER TABLE [dbo].[MovimientoInventario]  WITH CHECK ADD  CONSTRAINT [CK_MovimientoInventario_Costo] CHECK  (([costo_unitario] IS NULL OR [costo_unitario]>=(0)))
GO
ALTER TABLE [dbo].[MovimientoInventario] CHECK CONSTRAINT [CK_MovimientoInventario_Costo]
GO
ALTER TABLE [dbo].[OutboxEvent]  WITH CHECK ADD  CONSTRAINT [CK_OutboxEvent_Estado] CHECK  (([estado]=N'Error' OR [estado]=N'Procesado' OR [estado]=N'Pendiente'))
GO
ALTER TABLE [dbo].[OutboxEvent] CHECK CONSTRAINT [CK_OutboxEvent_Estado]
GO
ALTER TABLE [dbo].[OutboxEvent]  WITH CHECK ADD  CONSTRAINT [CK_OutboxEvent_Reintentos] CHECK  (([reintentos]>=(0)))
GO
ALTER TABLE [dbo].[OutboxEvent] CHECK CONSTRAINT [CK_OutboxEvent_Reintentos]
GO
ALTER TABLE [dbo].[Paciente]  WITH CHECK ADD  CONSTRAINT [CK_Paciente_FechaNacimiento] CHECK  (([fecha_nacimiento]<=CONVERT([date],getdate())))
GO
ALTER TABLE [dbo].[Paciente] CHECK CONSTRAINT [CK_Paciente_FechaNacimiento]
GO
ALTER TABLE [dbo].[Paciente]  WITH CHECK ADD  CONSTRAINT [CK_Paciente_Genero] CHECK  (([genero]=N'Otro' OR [genero]=N'Femenino' OR [genero]=N'Masculino'))
GO
ALTER TABLE [dbo].[Paciente] CHECK CONSTRAINT [CK_Paciente_Genero]
GO
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [CK_Producto_Costo] CHECK  (([costo_unitario]>=(0)))
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [CK_Producto_Costo]
GO
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [CK_Producto_PrecioVenta] CHECK  (([precio_venta] IS NULL OR [precio_venta]>=(0)))
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [CK_Producto_PrecioVenta]
GO
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [CK_Producto_StockActual] CHECK  (([stock_actual]>=(0)))
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [CK_Producto_StockActual]
GO
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [CK_Producto_StockMinimo] CHECK  (([stock_minimo]>=(0)))
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [CK_Producto_StockMinimo]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [CK_Venta_Descuento] CHECK  (([descuento]>=(0)))
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [CK_Venta_Descuento]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [CK_Venta_Subtotal] CHECK  (([subtotal]>=(0)))
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [CK_Venta_Subtotal]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [CK_Venta_Total] CHECK  (([total]>=(0)))
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [CK_Venta_Total]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [CK_Venta_TotalConsistente] CHECK  (([total]=([subtotal]-[descuento])))
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [CK_Venta_TotalConsistente]
GO
/****** Object:  StoredProcedure [dbo].[sp_CategoriaServicio_Actualizar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_CategoriaServicio_Actualizar]
    @id_categoria_servicio INT,
    @nombre NVARCHAR(100),
    @descripcion NVARCHAR(300) = NULL,
    @precio_base DECIMAL(10,2),
    @activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.CategoriaServicio WHERE id_categoria_servicio = @id_categoria_servicio)
        THROW 50203, 'La categoria de servicio no existe.', 1;

    IF EXISTS (
        SELECT 1
        FROM dbo.CategoriaServicio
        WHERE nombre = @nombre
          AND id_categoria_servicio <> @id_categoria_servicio
    )
        THROW 50204, 'El nombre de categoria ya existe.', 1;

    UPDATE dbo.CategoriaServicio
       SET nombre = @nombre,
           descripcion = @descripcion,
           precio_base = @precio_base,
           activo = @activo,
           actualizado_en = SYSUTCDATETIME()
     WHERE id_categoria_servicio = @id_categoria_servicio;

    EXEC dbo.sp_CategoriaServicio_ObtenerPorId @id_categoria_servicio = @id_categoria_servicio;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_CategoriaServicio_Crear]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* =========================================================
   CATEGORIAS DE SERVICIO
   ========================================================= */

CREATE   PROCEDURE [dbo].[sp_CategoriaServicio_Crear]
    @codigo_categoria NVARCHAR(20),
    @nombre NVARCHAR(100),
    @descripcion NVARCHAR(300) = NULL,
    @precio_base DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.CategoriaServicio WHERE codigo_categoria = @codigo_categoria)
        THROW 50201, 'El codigo de categoria ya existe.', 1;

    IF EXISTS (SELECT 1 FROM dbo.CategoriaServicio WHERE nombre = @nombre)
        THROW 50202, 'El nombre de categoria ya existe.', 1;

    INSERT INTO dbo.CategoriaServicio
    (
        codigo_categoria, nombre, descripcion, precio_base
    )
    VALUES
    (
        @codigo_categoria, @nombre, @descripcion, @precio_base
    );

    SELECT *
    FROM dbo.CategoriaServicio
    WHERE id_categoria_servicio = SCOPE_IDENTITY();
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_CategoriaServicio_Desactivar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_CategoriaServicio_Desactivar]
    @id_categoria_servicio INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.CategoriaServicio WHERE id_categoria_servicio = @id_categoria_servicio)
        THROW 50205, 'La categoria de servicio no existe.', 1;

    UPDATE dbo.CategoriaServicio
       SET activo = 0,
           actualizado_en = SYSUTCDATETIME()
     WHERE id_categoria_servicio = @id_categoria_servicio;

    EXEC dbo.sp_CategoriaServicio_ObtenerPorId @id_categoria_servicio = @id_categoria_servicio;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_CategoriaServicio_Listar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_CategoriaServicio_Listar]
    @texto NVARCHAR(100) = NULL,
    @activo BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.CategoriaServicio
    WHERE (@activo IS NULL OR activo = @activo)
      AND (
            @texto IS NULL OR
            codigo_categoria LIKE '%' + @texto + '%' OR
            nombre LIKE '%' + @texto + '%' OR
            descripcion LIKE '%' + @texto + '%'
          )
    ORDER BY nombre;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_CategoriaServicio_ObtenerPorId]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_CategoriaServicio_ObtenerPorId]
    @id_categoria_servicio INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.CategoriaServicio
    WHERE id_categoria_servicio = @id_categoria_servicio;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Cita_Actualizar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Cita_Actualizar]
    @id_cita INT,
    @id_paciente INT,
    @id_dentista INT,
    @fecha DATE,
    @hora_inicio TIME(0),
    @hora_fin TIME(0),
    @motivo NVARCHAR(250),
    @observaciones NVARCHAR(1000) = NULL,
    @id_estado_cita INT,
    @id_usuario_accion INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Cita WHERE id_cita = @id_cita)
        THROW 50506, 'La cita no existe.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.Cita c
        INNER JOIN dbo.EstadoCita ec ON ec.id_estado_cita = c.id_estado_cita
        WHERE c.id_dentista = @id_dentista
          AND c.fecha = @fecha
          AND c.id_cita <> @id_cita
          AND ec.nombre NOT IN (N'Cancelada', N'NoAsistio')
          AND (@hora_inicio < c.hora_fin AND @hora_fin > c.hora_inicio)
    )
        THROW 50507, 'Ya existe una cita cruzada para el dentista en ese horario.', 1;

    EXEC sys.sp_set_session_context @key = N'id_usuario', @value = @id_usuario_accion;

    UPDATE dbo.Cita
       SET id_paciente = @id_paciente,
           id_dentista = @id_dentista,
           fecha = @fecha,
           hora_inicio = @hora_inicio,
           hora_fin = @hora_fin,
           motivo = @motivo,
           observaciones = @observaciones,
           id_estado_cita = @id_estado_cita,
           actualizada_en = SYSUTCDATETIME()
     WHERE id_cita = @id_cita;

    EXEC dbo.sp_Cita_ObtenerPorId @id_cita = @id_cita;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Cita_Cancelar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Cita_Cancelar]
    @id_cita INT,
    @id_usuario_accion INT,
    @comentario NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @id_estado_cancelada INT;

    SELECT @id_estado_cancelada = id_estado_cita
    FROM dbo.EstadoCita
    WHERE nombre = N'Cancelada';

    IF @id_estado_cancelada IS NULL
        THROW 50508, 'No existe el estado Cancelada.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.Cita WHERE id_cita = @id_cita)
        THROW 50509, 'La cita no existe.', 1;

    EXEC sys.sp_set_session_context @key = N'id_usuario', @value = @id_usuario_accion;

    UPDATE dbo.Cita
       SET id_estado_cita = @id_estado_cancelada,
           observaciones = CONCAT(ISNULL(observaciones + CHAR(10), ''), ISNULL(@comentario, N'Cita cancelada')),
           actualizada_en = SYSUTCDATETIME()
     WHERE id_cita = @id_cita;

    EXEC dbo.sp_Cita_ObtenerPorId @id_cita = @id_cita;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Cita_Crear]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* =========================================================
   CITAS
   ========================================================= */

CREATE   PROCEDURE [dbo].[sp_Cita_Crear]
    @id_paciente INT,
    @id_dentista INT,
    @fecha DATE,
    @hora_inicio TIME(0),
    @hora_fin TIME(0),
    @motivo NVARCHAR(250),
    @observaciones NVARCHAR(1000) = NULL,
    @id_estado_cita INT,
    @creada_por INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Paciente WHERE id_paciente = @id_paciente AND activo = 1)
        THROW 50501, 'El paciente no existe o esta inactivo.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE id_usuario = @id_dentista AND activo = 1)
        THROW 50502, 'El dentista no existe o esta inactivo.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE id_usuario = @creada_por AND activo = 1)
        THROW 50503, 'El usuario que crea la cita no existe o esta inactivo.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.EstadoCita WHERE id_estado_cita = @id_estado_cita)
        THROW 50504, 'El estado de cita no existe.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.Cita c
        INNER JOIN dbo.EstadoCita ec ON ec.id_estado_cita = c.id_estado_cita
        WHERE c.id_dentista = @id_dentista
          AND c.fecha = @fecha
          AND ec.nombre NOT IN (N'Cancelada', N'NoAsistio')
          AND (@hora_inicio < c.hora_fin AND @hora_fin > c.hora_inicio)
    )
        THROW 50505, 'Ya existe una cita cruzada para el dentista en ese horario.', 1;

    BEGIN TRAN;

    INSERT INTO dbo.Cita
    (
        id_paciente, id_dentista, fecha, hora_inicio, hora_fin,
        motivo, observaciones, id_estado_cita, creada_por
    )
    VALUES
    (
        @id_paciente, @id_dentista, @fecha, @hora_inicio, @hora_fin,
        @motivo, @observaciones, @id_estado_cita, @creada_por
    );

    DECLARE @id_cita INT = SCOPE_IDENTITY();

    INSERT INTO dbo.CitaHistorialEstado
    (
        id_cita, id_estado_cita, comentario, cambiado_por
    )
    VALUES
    (
        @id_cita, @id_estado_cita, N'Registro inicial de la cita', @creada_por
    );

    COMMIT TRAN;

    SELECT *
    FROM dbo.Cita
    WHERE id_cita = @id_cita;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Cita_Listar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Cita_Listar]
    @fecha_desde DATE = NULL,
    @fecha_hasta DATE = NULL,
    @id_dentista INT = NULL,
    @id_estado_cita INT = NULL,
    @id_paciente INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.id_cita,
        c.fecha,
        c.hora_inicio,
        c.hora_fin,
        c.motivo,
        c.id_paciente,
        CONCAT(p.nombres, ' ', p.apellidos) AS paciente,
        c.id_dentista,
        CONCAT(u.nombres, ' ', u.apellidos) AS dentista,
        c.id_estado_cita,
        ec.nombre AS estado_cita,
        c.creada_en
    FROM dbo.Cita c
    INNER JOIN dbo.Paciente p ON p.id_paciente = c.id_paciente
    INNER JOIN dbo.Usuario u ON u.id_usuario = c.id_dentista
    INNER JOIN dbo.EstadoCita ec ON ec.id_estado_cita = c.id_estado_cita
    WHERE (@fecha_desde IS NULL OR c.fecha >= @fecha_desde)
      AND (@fecha_hasta IS NULL OR c.fecha <= @fecha_hasta)
      AND (@id_dentista IS NULL OR c.id_dentista = @id_dentista)
      AND (@id_estado_cita IS NULL OR c.id_estado_cita = @id_estado_cita)
      AND (@id_paciente IS NULL OR c.id_paciente = @id_paciente)
    ORDER BY c.fecha DESC, c.hora_inicio ASC;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Cita_ObtenerPorId]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Cita_ObtenerPorId]
    @id_cita INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.id_cita,
        c.id_paciente,
        CONCAT(p.nombres, ' ', p.apellidos) AS paciente,
        c.id_dentista,
        CONCAT(u.nombres, ' ', u.apellidos) AS dentista,
        c.fecha,
        c.hora_inicio,
        c.hora_fin,
        c.motivo,
        c.observaciones,
        c.id_estado_cita,
        ec.nombre AS estado_cita,
        c.creada_por,
        c.creada_en,
        c.actualizada_en
    FROM dbo.Cita c
    INNER JOIN dbo.Paciente p ON p.id_paciente = c.id_paciente
    INNER JOIN dbo.Usuario u ON u.id_usuario = c.id_dentista
    INNER JOIN dbo.EstadoCita ec ON ec.id_estado_cita = c.id_estado_cita
    WHERE c.id_cita = @id_cita;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_MovimientoInventario_Listar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_MovimientoInventario_Listar]
    @id_producto INT = NULL,
    @id_tipo_movimiento INT = NULL,
    @fecha_desde DATETIME2(0) = NULL,
    @fecha_hasta DATETIME2(0) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        mi.id_movimiento_inventario,
        mi.id_producto,
        p.nombre AS producto,
        mi.id_tipo_movimiento,
        tm.nombre AS tipo_movimiento,
        mi.cantidad,
        mi.costo_unitario,
        mi.referencia,
        mi.observaciones,
        mi.realizado_por,
        CONCAT(u.nombres, ' ', u.apellidos) AS usuario,
        mi.fecha_movimiento
    FROM dbo.MovimientoInventario mi
    INNER JOIN dbo.Producto p ON p.id_producto = mi.id_producto
    INNER JOIN dbo.TipoMovimientoInventario tm ON tm.id_tipo_movimiento = mi.id_tipo_movimiento
    INNER JOIN dbo.Usuario u ON u.id_usuario = mi.realizado_por
    WHERE (@id_producto IS NULL OR mi.id_producto = @id_producto)
      AND (@id_tipo_movimiento IS NULL OR mi.id_tipo_movimiento = @id_tipo_movimiento)
      AND (@fecha_desde IS NULL OR mi.fecha_movimiento >= @fecha_desde)
      AND (@fecha_hasta IS NULL OR mi.fecha_movimiento <= @fecha_hasta)
    ORDER BY mi.fecha_movimiento DESC;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_MovimientoInventario_Registrar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* =========================================================
   MOVIMIENTOS DE INVENTARIO
   ========================================================= */

CREATE   PROCEDURE [dbo].[sp_MovimientoInventario_Registrar]
    @id_producto INT,
    @id_tipo_movimiento INT,
    @cantidad DECIMAL(12,2),
    @costo_unitario DECIMAL(10,2) = NULL,
    @referencia NVARCHAR(100) = NULL,
    @observaciones NVARCHAR(500) = NULL,
    @realizado_por INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Producto WHERE id_producto = @id_producto AND activo = 1)
        THROW 50601, 'El producto no existe o esta inactivo.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.TipoMovimientoInventario WHERE id_tipo_movimiento = @id_tipo_movimiento)
        THROW 50602, 'El tipo de movimiento no existe.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE id_usuario = @realizado_por AND activo = 1)
        THROW 50603, 'El usuario que registra el movimiento no existe o esta inactivo.', 1;

    INSERT INTO dbo.MovimientoInventario
    (
        id_producto, id_tipo_movimiento, cantidad, costo_unitario,
        referencia, observaciones, realizado_por
    )
    VALUES
    (
        @id_producto, @id_tipo_movimiento, @cantidad, @costo_unitario,
        @referencia, @observaciones, @realizado_por
    );

    SELECT *
    FROM dbo.MovimientoInventario
    WHERE id_movimiento_inventario = SCOPE_IDENTITY();
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Paciente_Actualizar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Paciente_Actualizar]
    @id_paciente INT,
    @nombres NVARCHAR(100),
    @apellidos NVARCHAR(100),
    @telefono NVARCHAR(20),
    @fecha_nacimiento DATE,
    @genero NVARCHAR(20),
    @direccion NVARCHAR(250) = NULL,
    @correo NVARCHAR(150) = NULL,
    @alergias NVARCHAR(500) = NULL,
    @observaciones_generales NVARCHAR(1000) = NULL,
    @activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Paciente WHERE id_paciente = @id_paciente)
        THROW 50102, 'El paciente no existe.', 1;

    UPDATE dbo.Paciente
       SET nombres = @nombres,
           apellidos = @apellidos,
           telefono = @telefono,
           fecha_nacimiento = @fecha_nacimiento,
           genero = @genero,
           direccion = @direccion,
           correo = @correo,
           alergias = @alergias,
           observaciones_generales = @observaciones_generales,
           activo = @activo,
           actualizado_en = SYSUTCDATETIME()
     WHERE id_paciente = @id_paciente;

    EXEC dbo.sp_Paciente_ObtenerPorId @id_paciente = @id_paciente;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Paciente_Crear]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* =========================================================
   PACIENTES
   ========================================================= */

CREATE   PROCEDURE [dbo].[sp_Paciente_Crear]
    @codigo_paciente NVARCHAR(20),
    @nombres NVARCHAR(100),
    @apellidos NVARCHAR(100),
    @telefono NVARCHAR(20),
    @fecha_nacimiento DATE,
    @genero NVARCHAR(20),
    @direccion NVARCHAR(250) = NULL,
    @correo NVARCHAR(150) = NULL,
    @alergias NVARCHAR(500) = NULL,
    @observaciones_generales NVARCHAR(1000) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Paciente WHERE codigo_paciente = @codigo_paciente)
        THROW 50101, 'El codigo del paciente ya existe.', 1;

    INSERT INTO dbo.Paciente
    (
        codigo_paciente, nombres, apellidos, telefono, fecha_nacimiento,
        genero, direccion, correo, alergias, observaciones_generales
    )
    VALUES
    (
        @codigo_paciente, @nombres, @apellidos, @telefono, @fecha_nacimiento,
        @genero, @direccion, @correo, @alergias, @observaciones_generales
    );

    SELECT *
    FROM dbo.Paciente
    WHERE id_paciente = SCOPE_IDENTITY();
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Paciente_Desactivar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Paciente_Desactivar]
    @id_paciente INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Paciente WHERE id_paciente = @id_paciente)
        THROW 50103, 'El paciente no existe.', 1;

    UPDATE dbo.Paciente
       SET activo = 0,
           actualizado_en = SYSUTCDATETIME()
     WHERE id_paciente = @id_paciente;

    EXEC dbo.sp_Paciente_ObtenerPorId @id_paciente = @id_paciente;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Paciente_Listar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Paciente_Listar]
    @texto NVARCHAR(100) = NULL,
    @activo BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_paciente,
        codigo_paciente,
        nombres,
        apellidos,
        telefono,
        fecha_nacimiento,
        genero,
        direccion,
        correo,
        alergias,
        observaciones_generales,
        activo,
        creado_en
    FROM dbo.Paciente
    WHERE (@activo IS NULL OR activo = @activo)
      AND (
            @texto IS NULL OR
            codigo_paciente LIKE '%' + @texto + '%' OR
            nombres LIKE '%' + @texto + '%' OR
            apellidos LIKE '%' + @texto + '%' OR
            telefono LIKE '%' + @texto + '%' OR
            correo LIKE '%' + @texto + '%'
          )
    ORDER BY nombres, apellidos;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Paciente_ObtenerPorId]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Paciente_ObtenerPorId]
    @id_paciente INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Paciente
    WHERE id_paciente = @id_paciente;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Producto_Actualizar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Producto_Actualizar]
    @id_producto INT,
    @nombre NVARCHAR(150),
    @descripcion NVARCHAR(300) = NULL,
    @id_proveedor INT = NULL,
    @stock_minimo DECIMAL(12,2),
    @unidad_medida NVARCHAR(30),
    @fecha_vencimiento DATE = NULL,
    @costo_unitario DECIMAL(10,2),
    @precio_venta DECIMAL(10,2) = NULL,
    @activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Producto WHERE id_producto = @id_producto)
        THROW 50403, 'El producto no existe.', 1;

    IF @id_proveedor IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Proveedor WHERE id_proveedor = @id_proveedor)
        THROW 50404, 'El proveedor no existe.', 1;

    UPDATE dbo.Producto
       SET nombre = @nombre,
           descripcion = @descripcion,
           id_proveedor = @id_proveedor,
           stock_minimo = @stock_minimo,
           unidad_medida = @unidad_medida,
           fecha_vencimiento = @fecha_vencimiento,
           costo_unitario = @costo_unitario,
           precio_venta = @precio_venta,
           activo = @activo,
           actualizado_en = SYSUTCDATETIME()
     WHERE id_producto = @id_producto;

    EXEC dbo.sp_Producto_ObtenerPorId @id_producto = @id_producto;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Producto_Crear]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* =========================================================
   PRODUCTOS
   ========================================================= */

CREATE   PROCEDURE [dbo].[sp_Producto_Crear]
    @codigo_producto NVARCHAR(20),
    @nombre NVARCHAR(150),
    @descripcion NVARCHAR(300) = NULL,
    @id_proveedor INT = NULL,
    @stock_minimo DECIMAL(12,2),
    @unidad_medida NVARCHAR(30),
    @fecha_vencimiento DATE = NULL,
    @costo_unitario DECIMAL(10,2),
    @precio_venta DECIMAL(10,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Producto WHERE codigo_producto = @codigo_producto)
        THROW 50401, 'El codigo del producto ya existe.', 1;

    IF @id_proveedor IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Proveedor WHERE id_proveedor = @id_proveedor)
        THROW 50402, 'El proveedor no existe.', 1;

    INSERT INTO dbo.Producto
    (
        codigo_producto, nombre, descripcion, id_proveedor, stock_minimo,
        unidad_medida, fecha_vencimiento, costo_unitario, precio_venta
    )
    VALUES
    (
        @codigo_producto, @nombre, @descripcion, @id_proveedor, @stock_minimo,
        @unidad_medida, @fecha_vencimiento, @costo_unitario, @precio_venta
    );

    SELECT *
    FROM dbo.Producto
    WHERE id_producto = SCOPE_IDENTITY();
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Producto_Desactivar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Producto_Desactivar]
    @id_producto INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Producto WHERE id_producto = @id_producto)
        THROW 50405, 'El producto no existe.', 1;

    UPDATE dbo.Producto
       SET activo = 0,
           actualizado_en = SYSUTCDATETIME()
     WHERE id_producto = @id_producto;

    EXEC dbo.sp_Producto_ObtenerPorId @id_producto = @id_producto;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Producto_Listar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Producto_Listar]
    @texto NVARCHAR(100) = NULL,
    @solo_activos BIT = NULL,
    @stock_bajo BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.id_producto,
        p.codigo_producto,
        p.nombre,
        p.descripcion,
        p.id_proveedor,
        pr.nombre AS proveedor,
        p.stock_actual,
        p.stock_minimo,
        p.unidad_medida,
        p.fecha_vencimiento,
        p.costo_unitario,
        p.precio_venta,
        p.activo,
        p.creado_en,
        p.actualizado_en
    FROM dbo.Producto p
    LEFT JOIN dbo.Proveedor pr ON pr.id_proveedor = p.id_proveedor
    WHERE (@solo_activos IS NULL OR p.activo = @solo_activos)
      AND (@stock_bajo IS NULL OR (@stock_bajo = 1 AND p.stock_actual <= p.stock_minimo) OR (@stock_bajo = 0 AND p.stock_actual > p.stock_minimo))
      AND (
            @texto IS NULL OR
            p.codigo_producto LIKE '%' + @texto + '%' OR
            p.nombre LIKE '%' + @texto + '%' OR
            p.descripcion LIKE '%' + @texto + '%'
          )
    ORDER BY p.nombre;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Producto_ObtenerPorId]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Producto_ObtenerPorId]
    @id_producto INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.*, pr.nombre AS proveedor
    FROM dbo.Producto p
    LEFT JOIN dbo.Proveedor pr ON pr.id_proveedor = p.id_proveedor
    WHERE p.id_producto = @id_producto;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Proveedor_Actualizar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Proveedor_Actualizar]
    @id_proveedor INT,
    @nombre NVARCHAR(150),
    @nit NVARCHAR(30) = NULL,
    @telefono NVARCHAR(20) = NULL,
    @correo NVARCHAR(150) = NULL,
    @direccion NVARCHAR(250) = NULL,
    @contacto NVARCHAR(100) = NULL,
    @activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Proveedor WHERE id_proveedor = @id_proveedor)
        THROW 50301, 'El proveedor no existe.', 1;

    UPDATE dbo.Proveedor
       SET nombre = @nombre,
           nit = @nit,
           telefono = @telefono,
           correo = @correo,
           direccion = @direccion,
           contacto = @contacto,
           activo = @activo
     WHERE id_proveedor = @id_proveedor;

    EXEC dbo.sp_Proveedor_ObtenerPorId @id_proveedor = @id_proveedor;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Proveedor_Crear]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* =========================================================
   PROVEEDORES
   ========================================================= */

CREATE   PROCEDURE [dbo].[sp_Proveedor_Crear]
    @nombre NVARCHAR(150),
    @nit NVARCHAR(30) = NULL,
    @telefono NVARCHAR(20) = NULL,
    @correo NVARCHAR(150) = NULL,
    @direccion NVARCHAR(250) = NULL,
    @contacto NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Proveedor
    (
        nombre, nit, telefono, correo, direccion, contacto
    )
    VALUES
    (
        @nombre, @nit, @telefono, @correo, @direccion, @contacto
    );

    SELECT *
    FROM dbo.Proveedor
    WHERE id_proveedor = SCOPE_IDENTITY();
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Proveedor_Desactivar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Proveedor_Desactivar]
    @id_proveedor INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Proveedor WHERE id_proveedor = @id_proveedor)
        THROW 50302, 'El proveedor no existe.', 1;

    UPDATE dbo.Proveedor
       SET activo = 0
     WHERE id_proveedor = @id_proveedor;

    EXEC dbo.sp_Proveedor_ObtenerPorId @id_proveedor = @id_proveedor;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Proveedor_Listar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Proveedor_Listar]
    @texto NVARCHAR(100) = NULL,
    @activo BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Proveedor
    WHERE (@activo IS NULL OR activo = @activo)
      AND (
            @texto IS NULL OR
            nombre LIKE '%' + @texto + '%' OR
            nit LIKE '%' + @texto + '%' OR
            telefono LIKE '%' + @texto + '%' OR
            correo LIKE '%' + @texto + '%' OR
            contacto LIKE '%' + @texto + '%'
          )
    ORDER BY nombre;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Proveedor_ObtenerPorId]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Proveedor_ObtenerPorId]
    @id_proveedor INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Proveedor
    WHERE id_proveedor = @id_proveedor;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_RegistroClinico_ListarPorPaciente]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_RegistroClinico_ListarPorPaciente]
    @id_paciente INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        rc.id_registro_clinico,
        rc.id_cita,
        rc.fecha_atencion,
        rc.diagnostico,
        rc.observaciones,
        rc.indicaciones_generales,
        CONCAT(u.nombres, ' ', u.apellidos) AS dentista
    FROM dbo.RegistroClinico rc
    INNER JOIN dbo.Usuario u ON u.id_usuario = rc.id_dentista
    WHERE rc.id_paciente = @id_paciente
    ORDER BY rc.fecha_atencion DESC;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_RegistroClinico_ObtenerPorId]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_RegistroClinico_ObtenerPorId]
    @id_registro_clinico INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        rc.*,
        CONCAT(p.nombres, ' ', p.apellidos) AS paciente,
        CONCAT(u.nombres, ' ', u.apellidos) AS dentista
    FROM dbo.RegistroClinico rc
    INNER JOIN dbo.Paciente p ON p.id_paciente = rc.id_paciente
    INNER JOIN dbo.Usuario u ON u.id_usuario = rc.id_dentista
    WHERE rc.id_registro_clinico = @id_registro_clinico;

    SELECT
        drc.id_detalle_registro,
        drc.id_registro_clinico,
        drc.id_categoria_servicio,
        cs.nombre AS categoria_servicio,
        drc.pieza_dental,
        drc.descripcion_procedimiento,
        drc.precio_aplicado,
        drc.observaciones
    FROM dbo.DetalleRegistroClinico drc
    INNER JOIN dbo.CategoriaServicio cs ON cs.id_categoria_servicio = drc.id_categoria_servicio
    WHERE drc.id_registro_clinico = @id_registro_clinico
    ORDER BY drc.id_detalle_registro;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_RegistroClinico_Registrar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* =========================================================
   REGISTRO CLINICO
   ========================================================= */

CREATE   PROCEDURE [dbo].[sp_RegistroClinico_Registrar]
    @id_cita INT,
    @id_paciente INT,
    @id_dentista INT,
    @diagnostico NVARCHAR(1000),
    @observaciones NVARCHAR(1000) = NULL,
    @indicaciones_generales NVARCHAR(1000) = NULL,
    @detalles dbo.TVP_DetalleRegistroClinico READONLY,
    @id_usuario_accion INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Cita WHERE id_cita = @id_cita)
        THROW 50701, 'La cita no existe.', 1;

    IF EXISTS (SELECT 1 FROM dbo.RegistroClinico WHERE id_cita = @id_cita)
        THROW 50702, 'La cita ya tiene un registro clinico asociado.', 1;

    IF NOT EXISTS (SELECT 1 FROM @detalles)
        THROW 50703, 'Debes enviar al menos un detalle del registro clinico.', 1;

    BEGIN TRY
        BEGIN TRAN;

        INSERT INTO dbo.RegistroClinico
        (
            id_cita, id_paciente, id_dentista, diagnostico,
            observaciones, indicaciones_generales
        )
        VALUES
        (
            @id_cita, @id_paciente, @id_dentista, @diagnostico,
            @observaciones, @indicaciones_generales
        );

        DECLARE @id_registro_clinico INT = SCOPE_IDENTITY();

        INSERT INTO dbo.DetalleRegistroClinico
        (
            id_registro_clinico, id_categoria_servicio, pieza_dental,
            descripcion_procedimiento, precio_aplicado, observaciones
        )
        SELECT
            @id_registro_clinico,
            d.id_categoria_servicio,
            d.pieza_dental,
            d.descripcion_procedimiento,
            d.precio_aplicado,
            d.observaciones
        FROM @detalles d;

        DECLARE @id_estado_finalizada INT;

        SELECT @id_estado_finalizada = id_estado_cita
        FROM dbo.EstadoCita
        WHERE nombre = N'Finalizada';

        IF @id_estado_finalizada IS NOT NULL
        BEGIN
            EXEC sys.sp_set_session_context @key = N'id_usuario', @value = @id_usuario_accion;

            UPDATE dbo.Cita
               SET id_estado_cita = @id_estado_finalizada,
                   actualizada_en = SYSUTCDATETIME()
             WHERE id_cita = @id_cita;
        END;

        COMMIT TRAN;

        SELECT *
        FROM dbo.RegistroClinico
        WHERE id_registro_clinico = @id_registro_clinico;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;
        THROW;
    END CATCH
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_Actualizar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Usuario_Actualizar]
    @id_usuario INT,
    @nombres NVARCHAR(100),
    @apellidos NVARCHAR(100),
    @username NVARCHAR(50),
    @correo NVARCHAR(150) = NULL,
    @telefono NVARCHAR(20) = NULL,
    @id_rol INT,
    @activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE id_usuario = @id_usuario)
        THROW 50005, 'El usuario no existe.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE id_rol = @id_rol)
        THROW 50006, 'El rol indicado no existe.', 1;

    IF EXISTS (SELECT 1 FROM dbo.Usuario WHERE username = @username AND id_usuario <> @id_usuario)
        THROW 50007, 'El username ya esta siendo utilizado por otro usuario.', 1;

    IF @correo IS NOT NULL AND EXISTS (SELECT 1 FROM dbo.Usuario WHERE correo = @correo AND id_usuario <> @id_usuario)
        THROW 50008, 'El correo ya esta siendo utilizado por otro usuario.', 1;

    UPDATE dbo.Usuario
       SET nombres = @nombres,
           apellidos = @apellidos,
           username = @username,
           correo = @correo,
           telefono = @telefono,
           id_rol = @id_rol,
           activo = @activo,
           actualizado_en = SYSUTCDATETIME()
     WHERE id_usuario = @id_usuario;

    EXEC dbo.sp_Usuario_ObtenerPorId @id_usuario = @id_usuario;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_Crear]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* =========================================================
   USUARIOS
   ========================================================= */

CREATE   PROCEDURE [dbo].[sp_Usuario_Crear]
    @codigo_usuario NVARCHAR(20),
    @nombres NVARCHAR(100),
    @apellidos NVARCHAR(100),
    @username NVARCHAR(50),
    @correo NVARCHAR(150) = NULL,
    @telefono NVARCHAR(20) = NULL,
    @password_hash NVARCHAR(500),
    @password_salt NVARCHAR(250) = NULL,
    @id_rol INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE id_rol = @id_rol AND activo = 1)
        THROW 50001, 'El rol indicado no existe o esta inactivo.', 1;

    IF EXISTS (SELECT 1 FROM dbo.Usuario WHERE codigo_usuario = @codigo_usuario)
        THROW 50002, 'El codigo de usuario ya existe.', 1;

    IF EXISTS (SELECT 1 FROM dbo.Usuario WHERE username = @username)
        THROW 50003, 'El username ya existe.', 1;

    IF @correo IS NOT NULL AND EXISTS (SELECT 1 FROM dbo.Usuario WHERE correo = @correo)
        THROW 50004, 'El correo ya existe.', 1;

    INSERT INTO dbo.Usuario
    (
        codigo_usuario, nombres, apellidos, username, correo,
        telefono, password_hash, password_salt, id_rol
    )
    VALUES
    (
        @codigo_usuario, @nombres, @apellidos, @username, @correo,
        @telefono, @password_hash, @password_salt, @id_rol
    );

    SELECT *
    FROM dbo.Usuario
    WHERE id_usuario = SCOPE_IDENTITY();
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_Desactivar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Usuario_Desactivar]
    @id_usuario INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE id_usuario = @id_usuario)
        THROW 50009, 'El usuario no existe.', 1;

    UPDATE dbo.Usuario
       SET activo = 0,
           actualizado_en = SYSUTCDATETIME()
     WHERE id_usuario = @id_usuario;

    EXEC dbo.sp_Usuario_ObtenerPorId @id_usuario = @id_usuario;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_Listar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Usuario_Listar]
    @texto NVARCHAR(100) = NULL,
    @activo BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.id_usuario,
        u.codigo_usuario,
        u.nombres,
        u.apellidos,
        u.username,
        u.correo,
        u.telefono,
        u.id_rol,
        r.nombre AS rol,
        u.activo,
        u.creado_en
    FROM dbo.Usuario u
    INNER JOIN dbo.Rol r ON r.id_rol = u.id_rol
    WHERE (@activo IS NULL OR u.activo = @activo)
      AND (
            @texto IS NULL OR
            u.codigo_usuario LIKE '%' + @texto + '%' OR
            u.nombres LIKE '%' + @texto + '%' OR
            u.apellidos LIKE '%' + @texto + '%' OR
            u.username LIKE '%' + @texto + '%' OR
            u.correo LIKE '%' + @texto + '%'
          )
    ORDER BY u.nombres, u.apellidos;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_ObtenerPorId]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Usuario_ObtenerPorId]
    @id_usuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.id_usuario,
        u.codigo_usuario,
        u.nombres,
        u.apellidos,
        u.username,
        u.correo,
        u.telefono,
        u.id_rol,
        r.nombre AS rol,
        u.activo,
        u.creado_en,
        u.actualizado_en
    FROM dbo.Usuario u
    INNER JOIN dbo.Rol r ON r.id_rol = u.id_rol
    WHERE u.id_usuario = @id_usuario;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Venta_Anular]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Venta_Anular]
    @id_venta INT,
    @id_usuario_accion INT,
    @observaciones NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @id_estado_anulada INT;
    DECLARE @id_tipo_entrada INT;
    DECLARE @numero_venta NVARCHAR(20);

    SELECT @id_estado_anulada = id_estado_venta
    FROM dbo.EstadoVenta
    WHERE nombre = N'Anulada';

    SELECT @id_tipo_entrada = id_tipo_movimiento
    FROM dbo.TipoMovimientoInventario
    WHERE nombre = N'Entrada';

    IF @id_estado_anulada IS NULL
        THROW 50814, 'No existe el estado Anulada.', 1;

    IF @id_tipo_entrada IS NULL
        THROW 50815, 'No existe el tipo de movimiento Entrada.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.Venta WHERE id_venta = @id_venta)
        THROW 50816, 'La venta no existe.', 1;

    IF EXISTS (SELECT 1 FROM dbo.Venta WHERE id_venta = @id_venta AND id_estado_venta = @id_estado_anulada)
        THROW 50817, 'La venta ya esta anulada.', 1;

    SELECT @numero_venta = numero_venta
    FROM dbo.Venta
    WHERE id_venta = @id_venta;

    BEGIN TRY
        BEGIN TRAN;

        UPDATE dbo.Venta
           SET id_estado_venta = @id_estado_anulada,
               observaciones = CONCAT(ISNULL(observaciones + CHAR(10), ''), ISNULL(@observaciones, N'Venta anulada'))
         WHERE id_venta = @id_venta;

        INSERT INTO dbo.MovimientoInventario
        (
            id_producto, id_tipo_movimiento, cantidad, costo_unitario,
            referencia, observaciones, realizado_por
        )
        SELECT
            dv.id_producto,
            @id_tipo_entrada,
            dv.cantidad,
            p.costo_unitario,
            CONCAT(N'ANULACION VENTA:', @numero_venta),
            N'Reingreso automatico por anulacion de venta',
            @id_usuario_accion
        FROM dbo.DetalleVenta dv
        INNER JOIN dbo.Producto p ON p.id_producto = dv.id_producto
        WHERE dv.id_venta = @id_venta
          AND dv.tipo_detalle = N'PRODUCTO';

        COMMIT TRAN;

        EXEC dbo.sp_Venta_ObtenerPorId @id_venta = @id_venta;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;
        THROW;
    END CATCH
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Venta_Listar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Venta_Listar]
    @fecha_desde DATETIME2(0) = NULL,
    @fecha_hasta DATETIME2(0) = NULL,
    @id_paciente INT = NULL,
    @id_estado_venta INT = NULL,
    @id_metodo_pago INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        v.id_venta,
        v.numero_venta,
        v.fecha_venta,
        CASE WHEN p.id_paciente IS NULL THEN NULL ELSE CONCAT(p.nombres, ' ', p.apellidos) END AS paciente,
        CONCAT(u.nombres, ' ', u.apellidos) AS usuario_registra,
        v.subtotal,
        v.descuento,
        v.total,
        ev.nombre AS estado_venta,
        mp.nombre AS metodo_pago
    FROM dbo.Venta v
    LEFT JOIN dbo.Paciente p ON p.id_paciente = v.id_paciente
    INNER JOIN dbo.Usuario u ON u.id_usuario = v.id_usuario
    INNER JOIN dbo.EstadoVenta ev ON ev.id_estado_venta = v.id_estado_venta
    INNER JOIN dbo.MetodoPago mp ON mp.id_metodo_pago = v.id_metodo_pago
    WHERE (@fecha_desde IS NULL OR v.fecha_venta >= @fecha_desde)
      AND (@fecha_hasta IS NULL OR v.fecha_venta <= @fecha_hasta)
      AND (@id_paciente IS NULL OR v.id_paciente = @id_paciente)
      AND (@id_estado_venta IS NULL OR v.id_estado_venta = @id_estado_venta)
      AND (@id_metodo_pago IS NULL OR v.id_metodo_pago = @id_metodo_pago)
    ORDER BY v.fecha_venta DESC;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Venta_ObtenerPorId]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Venta_ObtenerPorId]
    @id_venta INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        v.id_venta,
        v.numero_venta,
        v.id_paciente,
        CASE WHEN p.id_paciente IS NULL THEN NULL ELSE CONCAT(p.nombres, ' ', p.apellidos) END AS paciente,
        v.id_usuario,
        CONCAT(u.nombres, ' ', u.apellidos) AS usuario_registra,
        v.fecha_venta,
        v.subtotal,
        v.descuento,
        v.total,
        v.id_estado_venta,
        ev.nombre AS estado_venta,
        v.id_metodo_pago,
        mp.nombre AS metodo_pago,
        v.observaciones,
        v.creado_en
    FROM dbo.Venta v
    LEFT JOIN dbo.Paciente p ON p.id_paciente = v.id_paciente
    INNER JOIN dbo.Usuario u ON u.id_usuario = v.id_usuario
    INNER JOIN dbo.EstadoVenta ev ON ev.id_estado_venta = v.id_estado_venta
    INNER JOIN dbo.MetodoPago mp ON mp.id_metodo_pago = v.id_metodo_pago
    WHERE v.id_venta = @id_venta;

    SELECT
        dv.id_detalle_venta,
        dv.id_venta,
        dv.tipo_detalle,
        dv.id_categoria_servicio,
        cs.nombre AS categoria_servicio,
        dv.id_producto,
        pr.nombre AS producto,
        dv.descripcion,
        dv.cantidad,
        dv.precio_unitario,
        dv.subtotal
    FROM dbo.DetalleVenta dv
    LEFT JOIN dbo.CategoriaServicio cs ON cs.id_categoria_servicio = dv.id_categoria_servicio
    LEFT JOIN dbo.Producto pr ON pr.id_producto = dv.id_producto
    WHERE dv.id_venta = @id_venta
    ORDER BY dv.id_detalle_venta;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Venta_Registrar]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* =========================================================
   VENTAS
   ========================================================= */

CREATE   PROCEDURE [dbo].[sp_Venta_Registrar]
    @numero_venta NVARCHAR(20),
    @id_paciente INT = NULL,
    @id_usuario INT,
    @id_metodo_pago INT,
    @descuento DECIMAL(12,2) = 0,
    @observaciones NVARCHAR(500) = NULL,
    @detalles dbo.TVP_DetalleVenta READONLY
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @id_estado_pagada INT;
    DECLARE @id_tipo_salida INT;
    DECLARE @subtotal DECIMAL(12,2);
    DECLARE @total DECIMAL(12,2);
    DECLARE @id_venta INT;

    SELECT @id_estado_pagada = id_estado_venta
    FROM dbo.EstadoVenta
    WHERE nombre = N'Pagada';

    SELECT @id_tipo_salida = id_tipo_movimiento
    FROM dbo.TipoMovimientoInventario
    WHERE nombre = N'Salida';

    IF @id_estado_pagada IS NULL
        THROW 50801, 'No existe el estado Pagada.', 1;

    IF @id_tipo_salida IS NULL
        THROW 50802, 'No existe el tipo de movimiento Salida.', 1;

    IF EXISTS (SELECT 1 FROM dbo.Venta WHERE numero_venta = @numero_venta)
        THROW 50803, 'El numero de venta ya existe.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE id_usuario = @id_usuario AND activo = 1)
        THROW 50804, 'El usuario que registra la venta no existe o esta inactivo.', 1;

    IF @id_paciente IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Paciente WHERE id_paciente = @id_paciente AND activo = 1)
        THROW 50805, 'El paciente no existe o esta inactivo.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.MetodoPago WHERE id_metodo_pago = @id_metodo_pago)
        THROW 50806, 'El metodo de pago no existe.', 1;

    IF NOT EXISTS (SELECT 1 FROM @detalles)
        THROW 50807, 'Debes enviar al menos un detalle de venta.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM @detalles d
        WHERE d.cantidad <= 0 OR d.precio_unitario < 0
    )
        THROW 50808, 'Hay detalles de venta con cantidad o precio invalido.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM @detalles d
        WHERE (d.tipo_detalle = N'SERVICIO' AND (d.id_categoria_servicio IS NULL OR d.id_producto IS NOT NULL))
           OR (d.tipo_detalle = N'PRODUCTO' AND (d.id_producto IS NULL OR d.id_categoria_servicio IS NOT NULL))
           OR (d.tipo_detalle NOT IN (N'SERVICIO', N'PRODUCTO'))
    )
        THROW 50809, 'Hay detalles con referencias invalidas.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM @detalles d
        WHERE d.tipo_detalle = N'PRODUCTO'
          AND NOT EXISTS (SELECT 1 FROM dbo.Producto p WHERE p.id_producto = d.id_producto AND p.activo = 1)
    )
        THROW 50810, 'Uno o mas productos no existen o estan inactivos.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM @detalles d
        WHERE d.tipo_detalle = N'SERVICIO'
          AND NOT EXISTS (SELECT 1 FROM dbo.CategoriaServicio cs WHERE cs.id_categoria_servicio = d.id_categoria_servicio AND cs.activo = 1)
    )
        THROW 50811, 'Una o mas categorias de servicio no existen o estan inactivas.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM
        (
            SELECT d.id_producto, SUM(d.cantidad) AS cantidad_requerida
            FROM @detalles d
            WHERE d.tipo_detalle = N'PRODUCTO'
            GROUP BY d.id_producto
        ) x
        INNER JOIN dbo.Producto p ON p.id_producto = x.id_producto
        WHERE p.stock_actual < x.cantidad_requerida
    )
        THROW 50812, 'Stock insuficiente para uno o mas productos.', 1;

    SELECT @subtotal = ISNULL(SUM(cantidad * precio_unitario), 0)
    FROM @detalles;

    SET @total = @subtotal - ISNULL(@descuento, 0);

    IF @total < 0
        THROW 50813, 'El total no puede ser negativo.', 1;

    BEGIN TRY
        BEGIN TRAN;

        INSERT INTO dbo.Venta
        (
            numero_venta, id_paciente, id_usuario, subtotal,
            descuento, total, id_estado_venta, id_metodo_pago, observaciones
        )
        VALUES
        (
            @numero_venta, @id_paciente, @id_usuario, @subtotal,
            ISNULL(@descuento, 0), @total, @id_estado_pagada, @id_metodo_pago, @observaciones
        );

        SET @id_venta = SCOPE_IDENTITY();

        INSERT INTO dbo.DetalleVenta
        (
            id_venta, tipo_detalle, id_categoria_servicio, id_producto,
            descripcion, cantidad, precio_unitario, subtotal
        )
        SELECT
            @id_venta,
            d.tipo_detalle,
            d.id_categoria_servicio,
            d.id_producto,
            d.descripcion,
            d.cantidad,
            d.precio_unitario,
            d.cantidad * d.precio_unitario
        FROM @detalles d;

        INSERT INTO dbo.MovimientoInventario
        (
            id_producto, id_tipo_movimiento, cantidad, costo_unitario,
            referencia, observaciones, realizado_por
        )
        SELECT
            d.id_producto,
            @id_tipo_salida,
            d.cantidad,
            p.costo_unitario,
            CONCAT(N'VENTA:', @numero_venta),
            N'Salida automatica por registro de venta',
            @id_usuario
        FROM @detalles d
        INNER JOIN dbo.Producto p ON p.id_producto = d.id_producto
        WHERE d.tipo_detalle = N'PRODUCTO';

        COMMIT TRAN;

        EXEC dbo.sp_Venta_ObtenerPorId @id_venta = @id_venta;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;
        THROW;
    END CATCH
END;

GO
/****** Object:  Trigger [dbo].[TR_Cita_AuditarCambioEstado]    Script Date: 15/04/2026 23:13:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_Cita_AuditarCambioEstado]
ON [dbo].[Cita]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT UPDATE(id_estado_cita)
        RETURN;

    INSERT INTO dbo.CitaHistorialEstado
    (
        id_cita,
        id_estado_cita,
        comentario,
        cambiado_por,
        cambiado_en
    )
    SELECT
        i.id_cita,
        i.id_estado_cita,
        CONCAT(N'Cambio de estado de cita: ', ec_ant.nombre, N' -> ', ec_nuevo.nombre),
        COALESCE(
            TRY_CAST(SESSION_CONTEXT(N'id_usuario') AS INT),
            i.creada_por
        ) AS cambiado_por,
        SYSUTCDATETIME()
    FROM inserted i
    INNER JOIN deleted d
        ON d.id_cita = i.id_cita
    INNER JOIN dbo.EstadoCita ec_ant
        ON ec_ant.id_estado_cita = d.id_estado_cita
    INNER JOIN dbo.EstadoCita ec_nuevo
        ON ec_nuevo.id_estado_cita = i.id_estado_cita
    WHERE ISNULL(i.id_estado_cita, -1) <> ISNULL(d.id_estado_cita, -1);
END;

GO
ALTER TABLE [dbo].[Cita] ENABLE TRIGGER [TR_Cita_AuditarCambioEstado]
GO
/****** Object:  Trigger [dbo].[TR_MovimientoInventario_AfterInsert]    Script Date: 15/04/2026 23:13:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* =========================================================
   TRIGGER PARA ACTUALIZAR STOCK A PARTIR DE MOVIMIENTOS
   ========================================================= */

CREATE TRIGGER [dbo].[TR_MovimientoInventario_AfterInsert]
ON [dbo].[MovimientoInventario]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Delta AS
    (
        SELECT
            i.id_producto,
            SUM(
                CASE
                    WHEN t.nombre IN (N'Entrada', N'AjustePositivo') THEN i.cantidad
                    WHEN t.nombre IN (N'Salida', N'AjusteNegativo') THEN -i.cantidad
                    ELSE 0
                END
            ) AS delta_stock
        FROM inserted i
        INNER JOIN dbo.TipoMovimientoInventario t
            ON t.id_tipo_movimiento = i.id_tipo_movimiento
        GROUP BY i.id_producto
    )
    UPDATE p
       SET p.stock_actual = p.stock_actual + d.delta_stock,
           p.actualizado_en = SYSUTCDATETIME()
    FROM dbo.Producto p
    INNER JOIN Delta d ON d.id_producto = p.id_producto;

    IF EXISTS (SELECT 1 FROM dbo.Producto WHERE stock_actual < 0)
    BEGIN
        RAISERROR(N'El movimiento deja stock negativo. Operación cancelada.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;

GO
ALTER TABLE [dbo].[MovimientoInventario] ENABLE TRIGGER [TR_MovimientoInventario_AfterInsert]
GO
/****** Object:  Trigger [dbo].[TR_MovimientoInventario_NoDelete]    Script Date: 15/04/2026 23:13:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_MovimientoInventario_NoDelete]
ON [dbo].[MovimientoInventario]
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    RAISERROR(N'No se permite DELETE en MovimientoInventario. Debes registrar un movimiento inverso o de ajuste para mantener la trazabilidad.', 16, 1);
END;

GO
ALTER TABLE [dbo].[MovimientoInventario] ENABLE TRIGGER [TR_MovimientoInventario_NoDelete]
GO
/****** Object:  Trigger [dbo].[TR_MovimientoInventario_NoUpdate]    Script Date: 15/04/2026 23:13:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_MovimientoInventario_NoUpdate]
ON [dbo].[MovimientoInventario]
INSTEAD OF UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    RAISERROR(N'No se permite UPDATE en MovimientoInventario. Debes registrar un nuevo movimiento de ajuste para mantener la trazabilidad.', 16, 1);
END;

GO
ALTER TABLE [dbo].[MovimientoInventario] ENABLE TRIGGER [TR_MovimientoInventario_NoUpdate]
GO
USE [master]
GO
ALTER DATABASE [ClinicaDentalAppDB] SET  READ_WRITE 
GO
