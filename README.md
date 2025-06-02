# Controlador de Equipos

**Controlador de Equipos** es una aplicación web desarrollada en ASP.NET MVC 
que permite escanear, monitorear y gestionar los dispositivos conectados a una red corporativa. 
Su objetivo principal es ofrecer a los administradores de sistemas una visión actualizada del estado de la red, 
detectando puertos abiertos y posibles vulnerabilidades de seguridad.

---

## 🧩 Características principales

- Escaneo automático de dispositivos en red mediante Nmap.
- Almacenamiento estructurado de datos en SQL Server.
- Interfaz web intuitiva con funciones de búsqueda por hostname, IP y puerto.
- Gestión de descripciones y niveles de riesgo de los puertos abiertos.
- Visualización de historial de escaneos para análisis comparativos.
- Acceso seguro mediante HTTPS.
- Integración con IIS para despliegue local o en servidor empresarial.

---

## 🏗️ Arquitectura del sistema

### 🔍 1. Script de escaneo (Python + Nmap)
- Detecta los hosts activos en la red.
- Recoge información como hostname, IP, MAC, sistema operativo y puertos abiertos.
- Inserta los datos directamente en la base de datos SQL Server.
- Ejecutable en segundo plano con soporte para múltiples hilos.

### 🗃️ 2. Base de datos SQL Server
Estructura relacional optimizada para rendimiento y análisis:

- `Equipos`: Información general de los dispositivos.
- `Puertos`: Catálogo de puertos con descripción y nivel de riesgo.
- `Equipos_tienen_puertos`: Relación muchos-a-muchos entre equipos y puertos.
- `HistorialEscaneos`: Registro histórico de cada escaneo.
- `NivelRiesgo`: Tabla auxiliar con los niveles: Bajo, Medio, Alto.

### 🌐 3. Aplicación web ASP.NET MVC
- Listado y detalle de equipos conectados y sus puertos abiertos.
- Función para iniciar escaneos manuales desde la interfaz.
- Edición de descripciones de puertos y sus riesgos.
- Panel de control actualizado en tiempo real.
- Soporte para búsquedas por IP, hostname o número de puerto.

---

## 🛠️ Configuración del entorno

### 1. Clonar el repositorio

```bash
git clone https://github.com/tuusuario/controlador-equipos.git
```

### 2. Abrir el proyecto

1. Abre la solución `ControladorEquipos.sln` con Visual Studio 2022 o superior.
2. Restaura los paquetes NuGet si es necesario.
3. Compila el proyecto.

---

## 🔧 Configurar la cadena de conexión

Antes de ejecutar la aplicación, **debes modificar la cadena de conexión a tu servidor SQL Server**.

### En `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=ControladorEquipos;User Id=TU_USUARIO;Password=TU_CONTRASEÑA;"
}
```

> Reemplaza `TU_SERVIDOR`, `TU_USUARIO` y `TU_CONTRASEÑA` por los valores correspondientes de tu entorno.

**Importante:**  
- El usuario de SQL Server debe tener permisos de lectura y escritura.  
- La base de datos debe existir previamente o ser creada mediante migraciones de Entity Framework (si están configuradas).

---

## 🧪 Requisitos técnicos

- Visual Studio 2022 o superior  
- SQL Server (Express, Developer o Enterprise)  
- IIS o IIS Express para pruebas o despliegue  
- Python 3.x instalado en el sistema  
- Nmap instalado y en el `PATH`  
- Git (opcional)  

---

## 🔐 Seguridad y despliegue

- HTTPS habilitado con certificado SSL.  
- Accesible solo desde red corporativa o VPN.  
- Alojado en IIS local o servidor Windows.  
- Datos cifrados en tránsito (TLS).  

---

## 🧾 Documentación del sistema

La documentación completa del proyecto se encuentra en el archivo:

📄 `Controlador de Equipos - Documento para usuarios.pdf`

Incluye:
- Introducción general  
- Diagrama de arquitectura  
- Uso de la interfaz  
- Explicación técnica y funcional de cada componente  
- Detalles de seguridad y despliegue  

---

## ✅ Estado del proyecto

🟢 Producción interna (empresa GEKA)  
🔧 En mantenimiento activo  

---

## 📄 Licencia

Este proyecto ha sido desarrollado exclusivamente para uso interno en la empresa GEKA.  
No se distribuye públicamente bajo licencia open source por el momento.  

---

## ✉️ Contacto

**Desarrollador:** Eneko Alabort  
**Empresa:** GEKA  
**Email:** enekoalabort@gmail.com  

---

© 2025 GEKA · Todos los derechos reservados
