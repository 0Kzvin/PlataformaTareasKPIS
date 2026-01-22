# SistemaProductividad - Plan Maestro (clon evolutivo de SistemaExplosivos)

Este documento completa la aplicación **SistemaProductividad** siguiendo la **misma arquitectura, estructura de carpetas, patrones, estilos de endpoints, autenticación y UX** de **SistemaExplosivos**.

> Objetivo: que **SistemaProductividad** sea un clon evolutivo y escalable de SistemaExplosivos, respetando el stack **Quasar + .NET** y los patrones de DTOs/servicios/repositorios/auth/logging existentes.

---

## 1) Arquitectura basada en Inventario de Silos

La arquitectura se organiza en **silos funcionales** alineados a dominios de negocio, cada uno con:

- **API** (.NET) con controladores y servicios
- **DTOs** y validaciones
- **Entidades** y migraciones
- **Frontend** (Quasar) con módulos, rutas, componentes y stores

### Silos principales

1. **Identidad & Seguridad**
   - Auth JWT, refresh token, roles/claims por departamento.
   - Policies por recursos (Departamento/Tarea/Reporte)
   - Auditoría centralizada (acciones + entidad + usuario + timestamp)

2. **Departamentos**
   - CRUD dinámico
   - Configuración de asignación (Modo A/B/C)
   - KPIs por departamento

3. **Tareas**
   - CRUD y workflow de estados
   - Campos públicos y privados según rol
   - Historial de cambios y comentarios
   - Evidencias

4. **Productividad & KPIs**
   - Dashboards por departamento
   - Dashboards globales
   - KPIs comparativos

5. **Reportes**
   - Exportación PDF/Excel/CSV
   - Reportes por usuario/departamento/global

6. **Notificaciones**
   - Email y notificaciones internas
   - Plantillas configurables
   - Integración con SignalR

---

## 2) Modelo de datos completo (base conceptual)

### Entidades clave

- **Departamentos**
  - Id, Nombre, Descripcion, LiderId, ConfiguracionAsignacion
  - KPIs configurables (JSON)

- **Usuarios** (heredado de identidad)
  - Id, Nombre, Correo, Rol, DepartamentoId
  - Claims por departamento

- **Tareas**
  - **Campos públicos**: DepartamentoId, Titulo, Descripcion, AsignadoId, CreadorId, FechaCreacion, Deadline, Prioridad, Estado
  - **Campos privados**: DificultadEstimada, TiempoEstimadoHoras, TiempoRealHoras, EvaluacionDesempeno, NotasPrivadas, ImpactoInterno, ClasificacionInterna
  - Relaciones: Comentarios, HistorialCambios, Evidencias

- **Comentarios**
  - TareaId, UsuarioId, Mensaje, Fecha

- **HistorialCambios**
  - Entidad, EntidadId, UsuarioId, Cambio, Fecha

- **KPIs**
  - DepartamentoId, Fecha, Tipo, Valor

- **Notificaciones**
  - UsuarioId, Tipo, Mensaje, Leida, Fecha

- **Auditoria**
  - UsuarioId, Accion, Entidad, EntidadId, Timestamp, Ip

---

## 3) Endpoints REST (alineados a SistemaExplosivos)

### Departamentos
- `GET /api/core/Departamentos/Listar`
- `POST /api/core/Departamentos/Registrar`
- `PUT /api/core/Departamentos/Actualizar/{id}`
- `DELETE /api/core/Departamentos/Eliminar/{id}`

### Tareas
- `GET /api/core/Tareas/ListarPorDepartamento/{dptoId}`
- `GET /api/core/Tareas/Detalle/{id}`
- `POST /api/core/Tareas/Registrar`
- `PUT /api/core/Tareas/Actualizar/{id}`
- `PUT /api/core/Tareas/CambiarEstado/{id}`
- `POST /api/core/Tareas/AgregarComentario/{id}`

### Dashboards
- `GET /api/core/Dashboards/Departamento/{dptoId}`
- `GET /api/core/Dashboards/Global`

### Reportes
- `GET /api/core/Reportes/Departamento/{dptoId}`
- `GET /api/core/Reportes/Global`
- `GET /api/core/Reportes/Usuario/{usuarioId}`

### Notificaciones
- `GET /api/core/Notificaciones/Listar`
- `POST /api/core/Notificaciones/MarcarLeida/{id}`

---

## 4) UI/UX Quasar por vistas

### Vista SuperAdmin
- Dashboard global con KPIs comparativos
- Gestión de departamentos
- Gestión de usuarios y roles
- Auditoría y logs

### Vista Líder de Departamento
- Dashboard del departamento
- Gestión de tareas del departamento
- Configuración de asignación
- Reportes

### Vista Colaborador
- Lista de tareas asignadas
- Crear tareas públicas
- Comentarios y seguimiento

---

## 5) Estrategia On-Premise

- **Instalación local** en servidores Windows/Linux
- **Base de datos SQL Server** con backups automáticos
- **Configuración por cliente** (multi-departamento)
- **Actualizaciones controladas** (release notes + migraciones)
- **Auditoría y logging** centralizado

---

## 6) Roadmap por fases

### Fase 1 - Base funcional
- Autenticación JWT + roles
- CRUD departamentos
- CRUD tareas (campos públicos)
- UI básica

### Fase 2 - Privacidad y flujos
- Campos privados
- Modo A/B/C en asignación
- Auditoría

### Fase 3 - KPIs y dashboards
- Dashboards por departamento
- KPIs globales
- Reportes básicos

### Fase 4 - Notificaciones
- SignalR + email
- Plantillas configurables

### Fase 5 - Optimización
- Escalabilidad
- Multi-tenant avanzado
- Integración BI

---

✅ Este documento deja la ruta clara para terminar la aplicación con la misma filosofía y arquitectura que **SistemaExplosivos**.
