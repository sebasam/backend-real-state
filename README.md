# 🏡 RealEstate Backend

API construida en **.NET 9** con **arquitectura limpia** y **MongoDB** como base de datos.

---

## ⚙️ Tecnologías usadas

- .NET 9 (C#)
- MongoDB
- Docker
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

## ⚙️ Variables de entorno

Configúralas en un archivo `.env` en local o como **Config Vars** en Heroku:

```env
ASPNETCORE_URLS=http://+:5000
MongoSettings__ConnectionString=mongodb+srv://sebasxd:Clave1234@cluster0.vkox5.mongodb.net/RealEstateDb
MongoSettings__DatabaseName=RealEstateDb
```

---

## 💻 Instalación y ejecución

### 1. Clonar el repositorio

```bash
git clone <URL_DEL_REPOSITORIO_BACKEND>
cd realestate-backend
```

### 2. Ejecutar con Docker (local)

```bash
docker build -t realestate-backend .
docker run -p 7174:5000 --env-file .env realestate-backend
```

La API estará disponible en:  
🔗 [https://localhost:7174](https://localhost:7174)

### 3. Despliegue en Heroku con Docker

```bash
heroku login
heroku container:login
heroku create <nombre-de-tu-app>
heroku config:set ASPNETCORE_URLS=http://+:5000
heroku config:set MongoSettings__ConnectionString=<cadena-conexion>
heroku config:set MongoSettings__DatabaseName=RealEstateDb
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
