# Sistema Productividad (clon evolutivo de SistemaExplosivos)

Este documento define el **diseño completo** del Sistema Productividad como **clon evolutivo** del proyecto existente **SistemaExplosivos**, respetando su arquitectura, estructura de carpetas, patrones, estilo de endpoints y filosofía de UI/UX. Se parte explícitamente de la estructura técnica observada en `SOFTWARE/Proyectos/API` (backend C# con módulos, controladores, servicios, repositorios, utilidades y autorización por permisos) y `SOFTWARE/Proyectos/WEB/AplicacionWeb` (frontend Quasar con módulos, stores, services, composables e i18n).【F:SistemaExplosivos-odev/SOFTWARE/Proyectos/API/Program.cs†L1-L70】【F:SistemaExplosivos-odev/SOFTWARE/Proyectos/API/Controladores/Administracion/IdentidadControlador.cs†L1-L120】【F:SistemaExplosivos-odev/SOFTWARE/Proyectos/WEB/AplicacionWeb/src/App.vue†L1-L1】

---

## 1) Arquitectura basada en Inventario de Silos

### 1.1 Silos (módulos funcionales) alineados a SistemaExplosivos
Siguiendo el enfoque modular por **módulos** (con agrupación de Swagger por módulo y rutas base en el controlador), el Sistema Productividad tendrá los siguientes **silos**:

1. **Administración**
   - Identidad, roles, permisos, claims por departamento
   - Usuarios, departamentos, configuración global
   - Auditoría y logs
2. **Departamentos**
   - Creación/edición dinámica de departamentos
   - Líder, usuarios, políticas de asignación
   - Configuración propia y KPIs propios
3. **Tareas**
   - CRUD de tareas
   - Comentarios, evidencias, historial
   - Estados, prioridades y fechas
4. **KPIs & Analítica**
   - Dashboards por departamento y global
   - Cálculos de productividad, tiempos y cumplimiento
5. **Reportes**
   - Generación PDF/Excel/CSV
   - Reportes por usuario, departamento y global
6. **Notificaciones**
   - Email + notificaciones internas
   - Plantillas configurables
7. **Auditoría & Trazabilidad**
   - Registro estructurado de acciones
   - Consulta histórica y exportación

> **Justificación técnica**: SistemaExplosivos ya organiza el backend en módulos (ej. Administracion, Recepcion, Gerencia) y agrupa Swagger por `ConstantesModulos`; replicamos el patrón para mantener consistencia y permitir escalabilidad por silos.【F:SistemaExplosivos-odev/SOFTWARE/Proyectos/API/Program.cs†L20-L35】

### 1.2 Capas y patrones (idénticos a SistemaExplosivos)
- **Controladores**: rutas por módulo, convenciones `Modulo/Recurso` y endpoints `AccionNombre`.
- **Servicios**: lógica de negocio (preterminados/propios).
- **Repositorios/Database**: EF Core con contextos por módulo.
- **DTOs**: contratos por módulo, sin exponer entidades.
- **Utilidades/Atributos**: permisos, validaciones y helpers.

> **Justificación**: La estructura de carpetas en `API` (Controladores, Servicios, Database, Modelos, Utilidades, Atributos) y la configuración de autenticación/autorización por permisos ya existe; se mantiene para minimizar desviaciones del clon evolutivo.【F:SistemaExplosivos-odev/SOFTWARE/Proyectos/API/Program.cs†L1-L70】【F:SistemaExplosivos-odev/SOFTWARE/Proyectos/API/Controladores/Administracion/IdentidadControlador.cs†L1-L120】

### 1.3 On-Premise y despliegue
- **Hosting**: Kestrel + IIS/Reverse Proxy según estándar de SistemaExplosivos.
- **Configuración**: `appsettings.json` y `appsettings.Development.json`.
- **Storage**: Ruta de almacenador configurable (mismo patrón de `OpcionesAlmacenadorDTO`).
- **SignalR**: Canal para eventos críticos (asignaciones, cambios de estado, alertas).

> **Justificación**: SistemaExplosivos ya usa `UseUrls`, archivos estáticos vía `OpcionesAlmacenadorDTO`, y SignalR en el pipeline; replicar asegura operación on-premise con el mismo esquema de despliegue y notificaciones en tiempo real.【F:SistemaExplosivos-odev/SOFTWARE/Proyectos/API/Program.cs†L14-L66】

---

## 2) Modelo de datos completo (multi-departamento y multi-tenant)

### 2.1 Entidades principales
- **Departamento**
  - `Id`, `Nombre`, `Descripcion`, `IdLider`, `ConfiguracionId`, `Activo`
- **DepartamentoUsuario** (relación N:N)
  - `DepartamentoId`, `UsuarioId`, `RolDepartamento`
- **ConfiguracionDepartamento**
  - `ModoAsignacion` (A/B/C)
  - `PermiteAsignarOtros`
  - `PermiteCamposPrivados`
  - `KpisActivos`
- **Tarea**
  - `Id`, `DepartamentoId`, `Titulo`, `Descripcion`, `Prioridad`, `Estado`
  - `FechaCreacion`, `Deadline`, `FechaCierre`
  - `CreadorId`, `ResponsablePrincipalId`
- **TareaAsignado**
  - `TareaId`, `UsuarioId`, `RolAsignacion` (principal/colaborador)
- **TareaComentario**
  - `Id`, `TareaId`, `UsuarioId`, `Comentario`, `Fecha`
- **TareaHistorial**
  - `Id`, `TareaId`, `Cambio`, `UsuarioId`, `Fecha`
- **TareaEvidencia**
  - `Id`, `TareaId`, `RutaArchivo`, `Tipo`
- **CamposPrivadosTarea**
  - `TareaId` (FK)
  - `DificultadEstimada`, `TiempoEstimado`, `TiempoReal`
  - `EvaluacionDesempeno`, `NotasPrivadas`
  - `ImpactoInterno`, `ClasificacionInterna`
- **KpiDepartamento**
  - `Id`, `DepartamentoId`, `Nombre`, `Formula`, `Meta`, `Activo`
- **RegistroAuditoria**
  - `Id`, `UsuarioId`, `Entidad`, `Accion`, `Fecha`, `Payload`
- **Notificacion**
  - `Id`, `UsuarioId`, `Tipo`, `Titulo`, `Mensaje`, `Leido`, `Fecha`

### 2.2 Consideraciones clave
- **Separación pública/privada**: `Tarea` + `CamposPrivadosTarea` evita exposición accidental.
- **Multi-departamento**: tabla pivote `DepartamentoUsuario` con rol contextual.
- **KPIs configurables**: KPIs por departamento con fórmulas predefinidas.

> **Justificación**: el patrón de DTOs/entidades separadas y el uso de contextos EF por módulo está alineado con la base existente de SistemaExplosivos, evitando cambios estructurales en el backend y manteniendo coherencia con los módulos actuales.【F:SistemaExplosivos-odev/SOFTWARE/Proyectos/API/Controladores/Administracion/IdentidadControlador.cs†L1-L120】

---

## 3) Endpoints REST (estilo SistemaExplosivos)

### 3.1 Convención de rutas
- Base: `/<modulo>/<recurso>/<Accion>`
- Swagger agrupado por módulo (como `Administracion`, `Recepcion`, etc.)
- Autorización basada en JWT + permisos por policy

> **Justificación**: SistemaExplosivos utiliza rutas estilo `administracion/Identidad/ListarUsuarios` y permisos declarativos con atributos; se replica para mantener uniformidad de acceso y trazabilidad.【F:SistemaExplosivos-odev/SOFTWARE/Proyectos/API/Controladores/Administracion/IdentidadControlador.cs†L24-L120】

### 3.2 Endpoints sugeridos

#### Administracion
- `GET administracion/Usuarios/Listar`
- `POST administracion/Usuarios/Crear`
- `POST administracion/Usuarios/Editar`
- `POST administracion/Usuarios/CambiarEstado`
- `GET administracion/Departamentos/Listar`
- `POST administracion/Departamentos/Crear`
- `POST administracion/Departamentos/Editar`

#### Departamentos
- `GET departamentos/Detalle/Ver`
- `POST departamentos/Configuracion/Actualizar`
- `POST departamentos/Usuarios/Invitar`

#### Tareas
- `GET tareas/Listar`
- `POST tareas/Crear`
- `POST tareas/Editar`
- `POST tareas/CambiarEstado`
- `POST tareas/Asignar`
- `POST tareas/AgregarComentario`
- `POST tareas/SubirEvidencia`
- `POST tareas/CompletarCamposPrivados`

#### KPIs & Analítica
- `GET kpis/Departamento/Resumen`
- `GET kpis/Global/Resumen`

#### Reportes
- `POST reportes/ExportarPDF`
- `POST reportes/ExportarExcel`
- `POST reportes/ExportarCSV`

#### Notificaciones
- `GET notificaciones/Listar`
- `POST notificaciones/MarcarLeida`

---

## 4) UI/UX Quasar por vistas (clon evolutivo)

### 4.1 Estructura de carpetas (misma filosofía)
- `src/pages`, `src/layouts`, `src/modules`, `src/services`, `src/stores`, `src/composables`, `src/i18n`.

> **Justificación**: la estructura actual del front en `AplicacionWeb/src` tiene módulos, servicios y stores claramente separados, y se replica para garantizar consistencia en arquitectura y mantenimiento.【F:SistemaExplosivos-odev/SOFTWARE/Proyectos/WEB/AplicacionWeb/src/App.vue†L1-L1】

### 4.2 Vistas principales
- **Login / Autenticación**
- **Dashboard Global (SuperAdmin)**
- **Dashboard Departamento (Líder/Admin)**
- **Mis Tareas (Colaborador)**
- **Gestión de Departamentos**
- **Gestión de Usuarios**
- **Gestión de KPIs**
- **Reportes**
- **Auditoría**

### 4.3 Componentes reutilizables
- Cards KPI, estados con badges de colores, tabla avanzada con filtros
- Timeline de historial de cambios
- Panel de notificaciones internas

---

## 5) Estrategia on-premise (enterprise ready)

- **Infra**: servidores on-premise con backup y replicación.
- **DB**: SQL Server con partición por módulo si aplica.
- **Logs**: log estructurado + exportación + retención configurable.
- **Auditoría**: todas las acciones críticas auditadas.

> **Justificación**: SistemaExplosivos ya integra logs, manejo de errores y auditoría; se mantiene como estándar corporativo de cumplimiento y soporte.【F:SistemaExplosivos-odev/SOFTWARE/Proyectos/API/Program.cs†L42-L68】

---

## 6) Roadmap por fases

### Fase 1 – Fundación
- Clonado de arquitectura base
- Identidad, JWT, roles, claims
- Departamentos y configuración

### Fase 2 – Tareas y flujo operativo
- CRUD tareas
- Estados, comentarios, evidencias
- Historial y auditoría

### Fase 3 – KPIs y dashboards
- KPIs por departamento
- Dashboard global

### Fase 4 – Reportes & Notificaciones
- Reportes exportables
- Email + notificaciones internas

### Fase 5 – Optimización y escalabilidad
- Performance, índices, cache
- Alertas inteligentes

---

## Matriz de roles (resumen)
- **SuperAdmin**: control global, KPIs globales, auditoría total.
- **Líder/Admin Departamento**: administra depto, KPIs locales, reportes.
- **Colaborador**: crea tareas, comenta, cambia estado (sin campos privados).

---

## Reglas críticas de creación y asignación
- **Campos públicos**: visibles a todos.
- **Campos privados**: solo líder/superadmin.
- **Asignación configurable por departamento** (A/B/C).

---

## Conclusión
Este diseño sigue estrictamente el enfoque, estilo y organización de SistemaExplosivos, garantizando un clon evolutivo sin alterar patrones ni stack. Se mantiene el mismo esquema de módulos, servicios, DTOs, autorización por permisos y estructura Quasar para asegurar continuidad técnica, operativa y visual.
