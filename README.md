# Lab 02 - Sistema de Balanza (WPF)

Aplicación de escritorio WPF (.NET 10) para el control de ingresos y salidas de vehículos.

**Curso:** Desarrollo de Aplicaciones Empresariales Avanzado
**Alumno:** Arévalo Sermeño, Edwin William

## Cómo ejecutar

Abrir `lab2.slnx` en Visual Studio y presionar F5, o desde consola:

```bash
dotnet run --project lab2/lab2.csproj
```

## Credenciales

| Usuario   | Contraseña |
|-----------|------------|
| `admin`   | `123456`   |
| `balanza` | `balanza`  |

## Ventanas

- **Login** — valida usuario y contraseña, y da paso al menú.
- **Menú** — Operaciones (Ingresos, Salida), Mantenimientos (Conductores, Transportistas, Camiones, Productos) y Reportes (Cargas, Ingresos, Salidas).
- **Registro de Ingresos** — tipo y número de documento, placa, turno, conductor, cliente, fecha/hora, peso de ingreso y producto.
- **Registro de Salida** — selecciona un ingreso pendiente y calcula el peso neto.
- **Registro de Conductores** y **Lista de Conductores** — nombre, licencia y transporte.
- **Listado de Ingresos** — fecha, placa, turno, conductor, producto, peso y transporte, con búsqueda por fecha inicio, fecha fin, placa, nombre de conductor y nombre de producto.

## Estructura

```
lab2/
├── Modelos/    Entidades del dominio
├── Datos/      Almacén de datos en memoria
└── Vistas/     Ventanas XAML y su código
```
