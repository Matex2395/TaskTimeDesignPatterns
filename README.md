  # TaskTimePredicter

Este repositorio contiene un proyecto que abarca principalmente un CORE de análisis de Productividad de 
Proyectos de Software y también calcula el tiempo promedio de estimaciones de tiempo para completar tareas
y el tiempo actual que tomó finalizarla. Este CORE es alimentado por varios CRUDs, de entre los cuales se destacan:
- Usuarios: Gestión de Usuarios que tendrán o el rol de ```"Administrador"``` o el de ```"Desarrollador"```.
- Proyectos: Gestión de Proyectos de Software.
- Categorías: Gestión de Categorías de Tareas para clasificarlas de mejor manera.
- Subcategorías: Gestión de Subcategorías de Tareas para mejorar su clasificación aún más.
- Tareas: Gestión de Tareas, en las que se puede asignar un tiempo estimado para completar la tarea y, cuando se finalice, se puede colocar el tiempo real que tomó completarla.

De igual forma, este proyecto cuenta con la funcionalidad de realizar comparaciones entre tiempos reales que les tomó a ciertos proyectos para finalizar, mostrando una 
estadística de cuál de los Proyectos de Software duró menos, cual duró más y con qué diferencia se encuentran estos dos.

Finalmente, este proyecto cuenta con las funciones de Registro, Inicio de Sesión y Autenticación de Usuarios, para poner
en práctica la restricción de ciertas funcionalidades a los usuarios ```"Desarrolladores"```.

## Tecnologías Utilizadas
Para este proyecto se utilizaron las tecnologías de ASP.NET & EntityFramework, siendo esta última una gran 
ayuda para poder gestionar bases de datos desde la consola del Administrador de Paquetes NuGet. 

## Desafíos Encontrados
Algunos desafíos encontrados fueron:
1. Validar Campos Input: Hay muchas formas de validar estos campos para el usuario, pero a mi parecer es mejor validar con etiquetas de Modelo.
2. Implementación del Core: Fue difícil definir el Core a desarrollar desde las tablas de información que se tenía, pero con el debido análisis se planteó una idea interesante.

## Requisitos Previos

- Tener instalado Visual Studio 2022
- Tener instalado Microsoft SQL Server Management Studio 20.

## Instrucciones de Uso

1. **Clona el Repositorio:** Clona este repositorio utilizando Git:

    ```bash
    git clone https://github.com/Matex2395/TaskTimePredicter.git
    ```

2. **Abre el Proyecto en Visual Studio 2022:** Abre Visual Studio 2022 y ejecuta el archivo que contiene la solución del proyecto, con esto podrás acceder a los archivos del proyecto.

3. **Crea la Base de Datos**

   1. Modifica la cadena de conexión en ```appsettings.json``` para que esta responda a tu base de datos en SQL Server.
   2. Si no quieres utilizar tu propia base de datos, procura dejar la cadena de conexión desplegada:
     ```// [Connection String del Proyecto en Local]```
     ```[Connection String del Proyecto Deployado]```
   3. Añade las migraciones y actualiza la Base de Datos por medio de la consola del Administrador de Paquetes NuGet:
     ```add-migration "Initial Migration"```
     ```update-database```

4. **Ejecuta el proyecto**

   - Pon a correr al proyecto y prueba las funcionalidades de este, en caso de que trabajes en ```localhost```.
   - Caso contrario, accede a la URL del proyecto desplegado.
   - Puedes comprobar la seguridad de las URLs al intentar iniciar sesión.

## Enlace al Proyecto Desplegado
Puedes acceder a este Core por medio del siguiente enlace:
- https://tasktimepredicter20241127171934.azurewebsites.net

No obstante, necesitarás unas credenciales de inicio de sesión, así que aquí te las proporcionaré:
- **Correo:** admin@gmail.com
- **Contraseña:** admin123
Si bien puedes registrarte en el sistema, tendrás acceso limitado a las funcionalidades porque estarás con un rol de "Desarrollador". Por lo tanto,
con este usuario podrás visualizar todas las funciones que ofrece el sistema, pues tendrás un rol "Administrador". 

## Créditos
La realización del Core fue posible gracias a los siguientes videos de YouTube:

### CRUD MVC
Este video permimtió entender la lógica de un CRUD basado en MVC en ASP.NET 8.0, combinado con la tecnología de EntityFramework:
- https://www.youtube.com/watch?v=dhguXv3vRIk

### Registro, Login & Autenticación
Por otro lado, el siguiente video permitió incluir la funcionalidad de registro, inicio de sesión y autorización de usuarios por medio de Cookies:
- https://www.youtube.com/watch?v=pJb9O7foT-8

## Contribuciones

¡Siéntete libre de contribuir al proyecto abriendo issues o enviando pull requests!
