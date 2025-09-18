# 🏡 RealEstate Backend

API construida en **.NET 9** con **arquitectura limpia** y **MongoDB** como base de datos.

Repositorio: [https://github.com/sebasam/backend-real-state](https://github.com/sebasam/backend-real-state)

---

## ⚙️ Tecnologías usadas

- .NET 9 (C#)
- MongoDB
- Heroku
- Swagger (documentación de API)
- Arquitectura limpia
- Inyección de dependencias

---

## ⚡ Endpoints principales

- `/api/property`
- `/api/owner`
- `/api/image`

---

## ⚙️ Configuración de la base de datos

En el archivo `appsettings.json`, asegúrate de tener:

```json
"MongoSettings": {
  "ConnectionString": "mongodb+srv://sebasxd:Clave1234@cluster0.vkox5.mongodb.net/RealEstateDb",
  "DatabaseName": "RealEstateDb"
}
```

---

## 💻 Instalación y ejecución local

### 1. Clonar el repositorio

```bash
git clone https://github.com/sebasam/backend-real-state.git
cd backend-real-state
```

### 2. Ejecutar la API

```bash
dotnet restore
dotnet build
dotnet run --project RealEstate.Api
```

La API estará disponible en:  
🔗 [https://localhost:7174](https://localhost:7174)

---

## 🚀 Despliegue en Heroku

```bash
heroku login
heroku container:login
heroku create <nombre-de-tu-app>
heroku config:set ASPNETCORE_URLS=http://+:5000
heroku container:push web -a <nombre-de-tu-app>
heroku container:release web -a <nombre-de-tu-app>
```

---

## 📑 Documentación

Una vez levantado, entra a:

```
https://localhost:7174/swagger
```

para ver la documentación generada automáticamente con Swagger.

---

## 📝 Notas

- Al iniciar, la API ejecuta un **seed de datos** para cargar propiedades iniciales.
- Usa inyección de dependencias y principios de arquitectura limpia para mantener el código modular y escalable.
