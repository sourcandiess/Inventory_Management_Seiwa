-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Waktu pembuatan: 24 Agu 2023 pada 10.14
-- Versi server: 10.4.28-MariaDB
-- Versi PHP: 8.0.28

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `inventory`
--

-- --------------------------------------------------------

--
-- Struktur dari tabel `barang`
--

CREATE TABLE `barang` (
  `kode_barang` varchar(50) NOT NULL,
  `nama_barang` varchar(50) NOT NULL,
  `quantity` int(50) NOT NULL,
  `min_stock` int(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data untuk tabel `barang`
--

INSERT INTO `barang` (`kode_barang`, `nama_barang`, `quantity`, `min_stock`) VALUES
('BK T01D1', 'TINTA', 4, 4),
('C T05B2 ', 'TINTA', 4, 4),
('C T01D2', 'TINTA', 3, 4),
('M T05B3', 'TINTA', 4, 4),
('M T01D3', 'TINTA', 3, 4),
('Y T05B4', 'TINTA', 4, 4),
('Y T01D4', 'TINTA', 4, 4),
('CE285A', 'TONER', 6, 4),
('Q7516A', 'TONER', 2, 4),
('K 1000 PUTIH MINI', 'KEYBOARD', 2, 3),
('HP 225 BLACK WIRED', 'SET KB MS', 2, 3),
('HP WHT WRLS', 'SET KB MS', 6, 3);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
