# DevilFruits API 🍇  

> [!NOTE]  
> Los datos obtenidos en esta API para la gestión de frutas del diablo del universo de *One Piece*
> provienen de la [API pública gratuita de One Piece](https://www.freepublicapis.com/one-piece-api).

> [!TIP]
> Si vas a consumir esta API desde un frontend, guarda el token en localstorage o cookies seguras ('HttpOnly').
> Usa el Header 'Authorization: Bearer {token}' en tus solicitudes.

> [!IMPORTANT]  
> Clona el repositorio y configura las variables de entorno.
> Ejecuta las migraciones de la base de datos. 🚀

> [!WARNING]  
> Para interactuar con los datos (ej. calificar, comentar o agregar favoritos), debes estar registrado y autenticado.

> [!CAUTION]
> ⚠️ Exposición de datos sencibles. Configura adecuadamente el entorno requerido.
> 📌 Si la API externa de One Piece falla, tu catálogo de frutas no se actualizará, Implementa un sistema de caché.
> ⚠️ Al eliminar un usuario, se pierden sus reseñas y favoritos.
> 💡 Opción de implementar microservicio con BD No-Sql para guardar los datos almacenados como respaldo en caso de que falle la API externa y obtener consultas desde ella
