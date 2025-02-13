-- Eliminar tablas si ya existen (para evitar errores en reinicios)
SET FOREIGN_KEY_CHECKS = 0;
DROP TABLE IF EXISTS ListasContenidos;
DROP TABLE IF EXISTS Listas;
DROP TABLE IF EXISTS PeliculasSeries;
DROP TABLE IF EXISTS Usuarios;
SET FOREIGN_KEY_CHECKS = 1;

-- Crear la tabla de Usuarios
CREATE TABLE Usuarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL
);

-- Crear la tabla de Películas y Series
CREATE TABLE PeliculasSeries (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Titulo VARCHAR(255) NOT NULL,
    Categoria VARCHAR(100) NOT NULL,
    Tipo ENUM('pelicula', 'serie') NOT NULL,
    Temporadas INT NULL,
    CapitulosPorTemporada INT NULL
);

-- Crear la tabla de Listas
CREATE TABLE Listas (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(255) NOT NULL,
    UsuarioId INT NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) ON DELETE CASCADE
);

-- Crear la tabla intermedia ListaContenido
CREATE TABLE ListasContenidos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ListaId INT NOT NULL,
    PeliculaSerieId INT NOT NULL,
    FOREIGN KEY (ListaId) REFERENCES Listas(Id) ON DELETE CASCADE,
    FOREIGN KEY (PeliculaSerieId) REFERENCES PeliculasSeries(Id) ON DELETE CASCADE
);

/* Se guarda el hash de la password no la propia password, y se comparan los hash

using BCrypt.Net;

Console.WriteLine(BCrypt.HashPassword("testpassword1")); > $2a$11$uJf5dFX4Jdf9sQqLAlnZY.BE7kDxtms9M3UQZBjtFDeBcBGR.aXWa
Console.WriteLine(BCrypt.HashPassword("testpassword2")); > $2a$11$K5XdhAVFJBLrSPyBo/uvL.0HKjQUqZPbb0XyZckxvcBZvFwmeU52G

*/
-- Insertar Usuarios de Prueba
INSERT INTO Usuarios (Username, PasswordHash) VALUES 
('usuario1', '$2a$11$uJf5dFX4Jdf9sQqLAlnZY.BE7kDxtms9M3UQZBjtFDeBcBGR.aXWa'),
('usuario2', '$2a$11$K5XdhAVFJBLrSPyBo/uvL.0HKjQUqZPbb0XyZckxvcBZvFwmeU52G');

-- Insertar Películas y Series de Prueba
INSERT INTO PeliculasSeries (Titulo, Categoria, Tipo, Temporadas, CapitulosPorTemporada) VALUES 
('Inception', 'Ciencia Ficción', 'pelicula', NULL, NULL),
('Breaking Bad', 'Drama', 'serie', 5, 13),
('The Dark Knight', 'Acción', 'pelicula', NULL, NULL),
('Stranger Things', 'Ciencia Ficción', 'serie', 4, 10);

-- Insertar Listas de Prueba
INSERT INTO Listas (Nombre, UsuarioId) VALUES 
('Favoritas', 1),
('Para Ver', 2);

-- Insertar Contenido en Listas
INSERT INTO ListasContenidos (ListaId, PeliculaSerieId) VALUES 
(1, 1), -- Inception en "Favoritas"
(1, 3), -- The Dark Knight en "Favoritas"
(2, 2), -- Breaking Bad en "Para Ver"
(2, 4); -- Stranger Things en "Para Ver"
