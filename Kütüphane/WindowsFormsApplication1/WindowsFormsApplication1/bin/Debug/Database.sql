USE [master]
GO
/****** Object:  Database [kutuphane]    Script Date: 21.12.2016 21:27:57 ******/
CREATE DATABASE [kutuphane]
GO
ALTER DATABASE [kutuphane] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [kutuphane].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [kutuphane] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [kutuphane] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [kutuphane] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [kutuphane] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [kutuphane] SET ARITHABORT OFF 
GO
ALTER DATABASE [kutuphane] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [kutuphane] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [kutuphane] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [kutuphane] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [kutuphane] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [kutuphane] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [kutuphane] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [kutuphane] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [kutuphane] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [kutuphane] SET  DISABLE_BROKER 
GO
ALTER DATABASE [kutuphane] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [kutuphane] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [kutuphane] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [kutuphane] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [kutuphane] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [kutuphane] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [kutuphane] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [kutuphane] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [kutuphane] SET  MULTI_USER 
GO
ALTER DATABASE [kutuphane] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [kutuphane] SET DB_CHAINING OFF 
GO
ALTER DATABASE [kutuphane] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [kutuphane] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [kutuphane]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [kutuphane]
GO
/****** Object:  Table [dbo].[hareketler]    Script Date: 21.12.2016 21:27:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hareketler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ogr_id] [int] NOT NULL,
	[ktp_id] [int] NOT NULL,
	[alis_tarihi] [date] NOT NULL,
	[veris_tarihi] [date] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[kitaplar]    Script Date: 21.12.2016 21:27:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[kitaplar](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[kitap_adi] [nvarchar](50) NOT NULL,
	[yazar] [nvarchar](50) NOT NULL,
	[tur] [nvarchar](50) NULL,
	[sayfa] [int] NULL,
	[basim_tarihi] [int] NULL,
	[kullanan] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ogrenci]    Script Date: 21.12.2016 21:27:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ogrenci](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[adi] [nvarchar](50) NOT NULL,
	[telefon] [int] NULL,
	[ogrenci_no] [int] NOT NULL,
	[borcu] [int] NOT NULL,
	[bolumu] [nvarchar](50) NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ogrenci] ADD  CONSTRAINT [DF_ogrenci_borcu]  DEFAULT ((0)) FOR [borcu]
GO
USE [master]
GO
ALTER DATABASE [kutuphane] SET  READ_WRITE 
GO
