# Assesssment-Tecnico

Plataforma de Cursos Online - API REST
📋 Descripción del Proyecto

API REST desarrollada en .NET 8 para la gestión de cursos y lecciones, siguiendo principios básicos de Clean Architecture. Incluye funcionalidades completas de CRUD, autenticación con JWT, y reglas de negocio específicas.
🚀 Características Principales

    Autenticación JWT con Identity

    Soft Delete para cursos y lecciones

    Reglas de negocio validadas en backend

    Paginación y filtrado de cursos

    Reordenamiento de lecciones

    API documentada con Swagger

    Tests unitarios para reglas de negocio

🛠️ Tecnologías Utilizadas
Backend

    .NET 8

    Entity Framework Core 8

    MySQL

    JWT Authentication

    Swagger/OpenAPI


Frontend

    React

📁 Estructura del Proyecto
text

PlataformaCursos/
├── Cursos.API/                 # Proyecto principal de la API
├── Cursos.Application/         # Capa de aplicación (servicios, DTOs)
├── Cursos.Domain/              # Capa de dominio (entidades, interfaces)
├── Cursos.Infrastructure/      # Capa de infraestructura (repositorios, DbContext)

⚙️ Configuración Inicial
Prerrequisitos

    .NET 8 SDK

    MySQL Server

    Git

Configuración de la Base de Datos

    Instalar MySQL si no lo tienes instalado

    Crear la base de datos:

sql

CREATE DATABASE PlataformaCursos;

    Configurar la conexión en appsettings.json:

json

{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=PlataformaCursos;user=root;password=Tu Contraseña"
  }
}


🗄️ Migraciones de Base de Datos
Primera Ejecución
bash

# Navegar al directorio del proyecto de infraestructura
cd Cursos.Infrastructure

# Crear migración inicial
dotnet ef migrations add InitialCreate --startup-project ../Cursos.API

# Aplicar migraciones a la base de datos
dotnet ef database update --startup-project ../Cursos.API

Comandos útiles
bash

# Crear nueva migración
dotnet ef migrations add NombreMigracion

# Actualizar base de datos
dotnet ef database update

# Revertir migración
dotnet ef database update NombreMigracionAnterior

# Eliminar última migración (sin aplicar)
dotnet ef migrations remove

🏃‍♂️ Ejecutar la Aplicación
Backend (API)
bash

# Navegar al directorio del proyecto API
cd Cursos.API

# Restaurar dependencias
dotnet restore

# Ejecutar la aplicación
dotnet run

# O para desarrollo con recarga automática
dotnet watch run

La API estará disponible en:

    API: https://localhost:5146

    Swagger UI: https://localhost:5146/swagger

Frontend
bash

# Navegar al directorio del frontend
cd Frontend

# Instalar dependencias
npm install

# Ejecutar en modo desarrollo
npm start

👤 Usuario de Prueba

Se crea automáticamente un usuario de prueba al iniciar la aplicación:

    Email: admin@cursos.com

    Contraseña: Admin123!

🔐 Endpoints de Autenticación
Registro
http

POST /api/auth/register
Content-Type: application/json

{
  "email": "usuario@ejemplo.com",
  "password": "MiClaveSegura123!"
}

Login
http

POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@cursos.com",
  "password": "Admin123!"
}

Respuesta exitosa
json

{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "expiresIn": 3600
}

📚 Endpoints de la API
Cursos
http

GET    /api/courses/search?q=&status=&page=1&pageSize=10
GET    /api/courses/{id}/summary
POST   /api/courses
PUT    /api/courses/{id}
DELETE /api/courses/{id}
PATCH  /api/courses/{id}/publish
PATCH  /api/courses/{id}/unpublish

Lecciones
http

GET    /api/courses/{courseId}/lessons
POST   /api/courses/{courseId}/lessons
PUT    /api/lessons/{id}
DELETE /api/lessons/{id}
PATCH  /api/lessons/{id}/reorder?direction=up|down

🧪 Ejecutar Tests Unitarios
bash

# Navegar al directorio de tests
cd Cursos.Tests

# Ejecutar todos los tests
dotnet test

# Ejecutar tests con cobertura de código (si se tiene coverlet)
dotnet test --collect:"XPlat Code Coverage"

# Ejecutar tests específicos
dotnet test --filter "PublishCourse_WithLessons_ShouldSucceed"

Tests Implementados

    ✅ PublishCourse_WithLessons_ShouldSucceed

    ✅ PublishCourse_WithoutLessons_ShouldFail

    ✅ CreateLesson_WithUniqueOrder_ShouldSucceed

    ✅ CreateLesson_WithDuplicateOrder_ShouldFail

    ✅ DeleteCourse_ShouldBeSoftDelete

🔧 Variables de Entorno

Crear un archivo .env en la raíz del proyecto (si se necesita):
env

DB_CONNECTION=server=localhost;database=PlataformaCursos;user=root;password=MiguelAngel19!
JWT_KEY=miClaveSuperSecretaDeAlMenos32Caracteres!!!
JWT_ISSUER=PlataformaCursosAPI
JWT_AUDIENCE=PlataformaCursosClient


🚨 Solución de Problemas
Error de conexión a MySQL

    Verificar que MySQL esté ejecutándose:

bash

sudo service mysql status

    Verificar credenciales en appsettings.json

    Probar conexión manual:

bash

mysql -u root -p -h localhost

Error de migraciones
bash

# Eliminar migraciones y comenzar de nuevo
dotnet ef database drop --force
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update

Error de JWT

    Verificar que la clave tenga al menos 32 caracteres

    Verificar issuer y audience en la configuración

    Verificar que el token se envíe en el header:

text

Authorization: Bearer {token}

📝 Notas de Desarrollo
Reglas de Negocio Implementadas

    ✅ Un curso solo puede publicarse si tiene al menos una lección activa

    ✅ El campo Order de las lecciones es único dentro del mismo curso

    ✅ La eliminación es lógica (soft delete)

    ✅ El reordenamiento no genera órdenes duplicados

    ✅ Filtro global por IsDeleted en Entity Framework

Puntos Extra Implementados (si aplica)

    Framework frontend moderno

    Implementación básica de roles

    Hard delete solo para Admin

    Docker y Docker Compose

    Dashboard simple con métricas

    Buen manejo de commits

📄 Licencia

Este proyecto fue desarrollado como parte de un assessment técnico.

🤝 Contribución

    Fork el repositorio

    Crear una rama para tu feature (git checkout -b feature/AmazingFeature)

    Commit tus cambios (git commit -m 'Add some AmazingFeature')

    Push a la rama (git push origin feature/AmazingFeature)

    Abrir un Pull Request

