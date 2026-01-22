
import csv
import shutil
import os

csv_file = r"c:\Users\oscar\OneDrive\Escritorio\DEV\SistemaExplosivos\SOFTWARE\Proyectos\WEB\AplicacionWeb\translations.csv"
backup_file = r"c:\Users\oscar\OneDrive\Escritorio\DEV\SistemaExplosivos\SOFTWARE\Proyectos\WEB\AplicacionWeb\translations.csv.bak"

# Dictionary of Spanish -> English translations for the recently added keys
translations_map = {
    "No se encontró el módulo": "Module not found",
    "Sin texto definido": "Undefined text",
    "Presione para seleccionar una hora de corte": "Click to select a cutoff time",
    "Hora Corte": "Cutoff Time",
    "Total (Semana Anterior)": "Total (Last Week)",
    "Análisis": "Analysis",
    "Almacén": "Warehouse",
    "Categorías": "Categories",
    "Límites SEDENA": "SEDENA Limits",
    "Administración": "Administration",
    "Correos Automáticos": "Automatic Emails",
    "Auditoría": "Audits",
    "Módulos": "Modules",
    "Supersacos": "Super Sacks",
    "Recepción": "Reception",
    "Gestión de Altas": "Registration Management",
    "ADM": "ADM",
    "ALM": "STR", # Storage
    "REC": "REC",
    "ACC": "ACC",
    "GER": "MGT", # Management
    "Configuración de Aplicación": "Application Settings",
    "Recuperación de Credenciales": "Credentials Recovery",
    "Ingresa tu usuario o correo": "Enter your username or email",
    "Usuario o Correo": "Username or Email",
    "Contraseña": "Password",
    "Mantener sesión iniciada": "Keep session signed in",
    "Iniciar sesión": "Login",
    "¿Olvidaste tu contraseña?": "Forgot password?",
    "Ingresa tu usuario o correo electrónico asociado para buscar tu cuenta y enviarte las instrucciones.": "Enter your username or associated email to search for your account and send you instructions.",
    "No se encontraron modulos para el usuario": "No modules found for user",
    "Por favor ingresa tu usuario o correo": "Please enter your username or email",
    "Acceso Denegado": "Access Denied",
    "Sección en Mantenimiento": "Section Under Maintenance",
    "Disponible Próximamente": "Available Soon",
    "Contenido Restringido": "Restricted Content",
    "Sin Permisos": "No Permissions",
    "Buenos Días": "Good Morning",
    "Buenas Tardes": "Good Afternoon",
    "Buenas Noches": "Good Night",
    "Ver perfil y ajustes": "View profile and settings",
    "Ver más información": "View more information",
    "Expandir menú": "Expand menu",
    "Contraer menú": "Collapse menu",
    "Recuperación": "Recovery",
    "Cambiar Contraseña": "Change Password",
    "Ingresa el código enviado a tu correo": "Enter the code sent to your email",
    "Define tus nuevas credenciales": "Define your new credentials",
    "Revisa tu bandeja de entrada o spam.": "Check your inbox or spam folder.",
    "Listo para reenviar": "Ready to resend",
    "¿No llegó?": "Didn't arrive?",
    "Reenviar código": "Resend code",
    "Espere": "Wait",
    "Las contraseñas deben tener al menos 5 caracteres": "Passwords must be at least 5 characters long",
    "Las contraseñas no coinciden": "Passwords do not match",
    "Cambiar Foto de Perfil": "Change Profile Photo",
    "Nueva": "New",
    "Vista previa de la nueva imagen": "New image preview",
    "Imagen actual": "Current image",
    "Seleccionar imagen": "Select image",
    "Por favor seleccione una imagen válida": "Please select a valid image",
    "Guardar Foto": "Save Photo",
    "¿Está seguro de asignar esta imagen?": "Are you sure you want to assign this image?",
    "Error al subir imagen": "Error uploading image",
    "Rol no asignado": "Role not assigned",
    "Correo Electrónico": "Email",
    "Usuario": "Username",
    "Teléfono": "Phone",
    "Fecha de Registro": "Registration Date",
    "Nombre *": "Name *",
    "Apellido *": "Last Name *",
    "Usuario *": "Username *",
    "Vacío para mantener actual": "Empty to keep current",
    "Contraseña (Opcional)": "Password (Optional)",
    "Enviar Cambios": "Send Changes",
    "Formulario Inválido": "Invalid Form",
    "¿Está seguro de editar este usuario?": "Are you sure you want to edit this user?",
    "Limpiar Filtros Descripción": "Clear Filters Description",
    "Opciones Menú Filtros": "Filter Menu Options",
    "Reestablecer Búsquedas": "Reset Searches",
    "Reestablecer Búsquedas Descripción": "Reset Searches Description",
    "Reestablecer Opciones Filtrado": "Reset Filtering Options",
    "Reestablecer Opciones Filtrado Descripción": "Reset Filtering Options Description",
    "Reestablecer Opciones Totales": "Reset Total Options",
    "Reestablecer Opciones Totales Descripción": "Reset Total Options Description",
    "Exportar Tabla": "Export Table",
    "Exportar Tabla Descripción": "Export Table Description",
    "Opciones Menú Exportación": "Export Menu Options",
    "CSV": "CSV",
    "EXCEL": "EXCEL",
    "PDF": "PDF",
    "Opciones Tabla Descripción": "Table Options Description",
    "Opciones Menú De Tabla": "Table Menu Options",
    "Guardar Configuraciones Tabla": "Save Table Configurations",
    "Guardar Filtros De Tabla": "Save Table Filters",
    "Guardar Orden De Tabla": "Save Table Order",
    "Seleccionadas": "Selected",
    "Columnas Ocultas": "Hidden Columns",
    "Separador Celdas": "Cell Separator",
    "Quitar Pantalla Completa": "Exit Full Screen",
    "Colocar Pantalla Completa": "Enter Full Screen",
    "Columna Bloqueada": "Locked Column",
    "Opciones": "Options",
    "Tooltip Mover Columna": "Drag to Move Column",
    "Deseleccionar Toda Tabla": "Deselect All",
    "Seleccionar Toda Tabla": "Select All",
    "Contraer Toda Tabla": "Collapse All",
    "Expandir Toda Tabla": "Expand All",
    "Opción Filtrado Descripción": "Filter Option Description",
    "Opción Filtrado": "Filter Option",
    "Filtro Selector Descripción": "Selector Filter Description",
    "Filtro": "Filter",
    "Filtro Descripción": "Filter Description",
    "Filtro Time Descripción": "Time Filter Description",
    "Filtro Date Descripción": "Date Filter Description",
    "Hoy": "Today",
    "Cerrar": "Close",
    "Deseleccionar Registro Tabla": "Deselect Row",
    "Seleccionar Registro Tabla": "Select Row",
    "Contraer Registro Tabla": "Collapse Row",
    "Expandir Registro Tabla": "Expand Row",
    "Completar Campo": "Complete Field",
    "Sin Opciones": "No Options",
    "Título Select": "Select Title",
    "Seleccionar Fechas": "Select Dates",
    "Tabla Sin Datos": "No Data",
    "Tabla Sin Resultados Filtro": "No Results Found",
    "Actualizar Tablero": "Update Dashboard",
    "Crear Usuario": "Create User",
    "Crear Rol": "Create Role",
    "Crear Producto": "Create Product",
    "Information": "Information",
    "Warning": "Warning",
    "Cancelar": "Cancel",
    "Nombre del rol": "Role Name",
    "Descripción": "Description",
    "Módulos Habilitados": "Enabled Modules",
    "Seleccione un módulo": "Select a module",
    "Permisos otorgados": "Permissions granted",
    "Seleccionar todos": "Select all",
    "Módulos operativos": "Operational modules",
    "Crear Estación": "Create Station",
    "Editar Estación": "Edit Station",
    "Llenar datos requeridos": "Fill required data",
    "Estaciones": "Stations",
    "Ip": "IP",
    "Fechas": "Dates",
    "Crear Operador": "Create Operator",
    "Editar Operador": "Edit Operator",
    "Editar Productos": "Edit Products",
    "Crear Productos": "Create Products",
    "Editar Producto": "Edit Product",
    "Crear Producto": "Create Product",
    "Apodo": "Nickname",
    "Color": "Color",
    "Color": "Color",
    "Código Aplicación": "Application Code",
    "Crear Equipo": "Create Equipment",
    "Editar Equipo": "Edit Equipment",
    "Crear Tanque": "Create Tank",
    "Editar Tanque": "Edit Tank",
    "Crear correo": "Create Email",
    "Editar Correo": "Edit Email",
    "Número Económico": "Economic Number",
    "No. Eco": "Eco No.",
    "Cantidad Actual": "Current Quantity",
    "Capacidad": "Capacity",
    "Es Externo": "Is External",
    "Externo": "External",
    "Local": "Local",
    "Fecha de Creación": "Creation Date",
    "Capacidades": "Capacities",
    "Alturas": "Heights",
    "Limites": "Limits",
    "Máximo": "Maximum",
    "Alto": "High",
    "Bajo": "Low",
    "Mínimo": "Minimum",
    "Operativa": "Operating",
    "Máxima": "Maximum",
    "Ubicación": "Location",
    "Tipo": "Type",
    "Llenar datos requeridos": "Fill required data",
    "La capacidad del equipo debe de ser mayor a 0": "Equipment capacity must be greater than 0",
    "La cantidad actual del equipo debe de ser mayor a 0": "Current equipment quantity must be greater than 0",
    "Capacidad Operativa": "Operating Capacity",
    "Altura": "Height",
    "Altura Operativa": "Operating Height",
    "Límite Máximo": "Maximum Limit",
    "Límite Alto": "High Limit",
    "Límite Bajo": "Low Limit",
    "Límite Mínimo": "Minimum Limit",
    "No puede ser mayor a la capacidad máximo": "Cannot be greater than maximum capacity",
    "No puede ser mayor a la altura máxima": "Cannot be greater than maximum height",
    "No puede ser mayor a límite máximo": "Cannot be greater than maximum limit",
    "No puede ser mayor a límite alto": "Cannot be greater than high limit",
    "Todos los límites deben tener un valor numérico": "All limits must have a numeric value",
    "Configure límites válidos para este depósito": "Configure valid limits for this deposit",
    "El límite alto no puede ser mayor que el límite máximo": "High limit cannot be greater than maximum limit",
    "El límite bajo no puede ser mayor que el límite alto": "Low limit cannot be greater than high limit",
    "El límite mínimo no puede ser mayor que el límite bajo": "Minimum limit cannot be greater than low limit",
    "El límite máximo debe ser mayor que el límite mínimo": "Maximum limit must be greater than minimum limit",
    "LLene los datos requeridos": "Fill required data",
    "A las": "At",
    "Cada Horas": "Every Hours",
    "Todos los Dias": "Every Day",
    "Todos los": "Every",
    "Domingo": "Sunday",
    "Lunes": "Monday",
    "Martes": "Tuesday",
    "Miercoles": "Wednesday",
    "Jueves": "Thursday",
    "Viernes": "Friday",
    "Sabado": "Saturday",
    "Accesorios": "Accessories",
    "Almacenamiento": "Storage",
    "Recepcion": "Reception",
    "y": "and",
    "Cada": "Every",
    "Horas": "Hours",
    "AM": "AM",
    "PM": "PM",
    "Depósito": "Deposit",
    "Niveles": "Levels",
    "Volumenes": "Volumes",
    "Carga": "Load",
    "Inicial": "Initial",
    "Final": "Final",
    "Ver detalles": "View details",
    "Vea a mayor detalle": "View in greater detail",
    "Asignar equipo": "Assign equipment",
    "Registrar movimiento": "Register movement",
    "Inventario Inicial": "Initial Inventory",
    "Inventario Final": "Final Inventory",
    "Cantidades": "Quantities",
    "Movimiento": "Movement",
    "Registrado": "Registered",
    "Modificado": "Modified",
    "Editar Movimiento": "Edit Movement",
    "Crear Movimiento": "Create Movement",
    "Hora:": "Time:",
    "Seleccione una hora": "Select a time",
    "Seleccionar fecha": "Select date",
    "Información del Silo": "Silo Information",
    "Resumen de Cambios": "Summary of Changes",
    "Diferencia de Nivel": "Level Difference",
    "Diferencia de Peso": "Weight Difference",
    "Diferencia de Porcentaje": "Percentage Difference",
    "Estado Final": "Final State",
    "Estado Inicial": "Initial State",
    "Activo": "Active",
    "Sin título": "Untitled",
    "Sin fecha": "No date",
    "Buscar estación...": "Search station...",
    "Buscar equipo...": "Search equipment...",
    "Monitoreo en tiempo real activo": "Real-time monitoring active",
    "Último guardado en la base de datos": "Last saved to database",
    "Máxima / Operativa": "Maximum / Operating",
    "No hay datos recientes": "No recent data",
    "Mensaje": "Message",
    "Excepcion": "Exception",
    "Accion": "Action",
    "FechaHora": "Date/Time",
    "Debe ser mayor a 0": "Must be greater than 0",
    "Las capacidades tienen que ser mayores que 0": "Capacities must be greater than 0",
    "La capacidad operativa no puede ser mayor que la capacidad máxima": "Operating capacity cannot be greater than maximum capacity",
    "Las alturas tienen que ser mayores que 0": "Heights must be greater than 0",
    "La altura operativa no puede ser mayor que la altura máxima": "Operating height cannot be greater than maximum height",
    "No puede ser mayor a límite bajo": "Cannot be greater than lower limit",
    "Cantidad Inicial": "Initial Quantity",
    "Cantidad Final": "Final Quantity",
    "Debe ser distinto de 0": "Must be different from 0",
    "No puede retirar más de la cantidad inicial": "Cannot withdraw more than initial quantity",
}

def fill_english():
    if not os.path.exists(backup_file):
        shutil.copyfile(csv_file, backup_file)

    updated_rows = []
    
    # Read with latin-1 fallback if utf-8 fails (common in this project context)
    try:
        f = open(csv_file, 'r', encoding='utf-8', newline='')
        header_line = f.readline() # Read header manually to detect format
    except UnicodeDecodeError:
        f = open(csv_file, 'r', encoding='latin-1', newline='')
        header_line = f.readline()

    f.seek(0)
    reader = csv.reader(f)
    header = next(reader)
    updated_rows.append(header)

    # Determine index of English column (usually index 2)
    # Header check: "Key","Spanish (Mexico), es_MX","English (US), en-US"
    # So English is index 2.
    english_idx = 2
    spanish_idx = 1
    
    count = 0
    for row in reader:
        if len(row) > spanish_idx:
            spanish_text = row[spanish_idx]
            current_english = row[english_idx] if len(row) > english_idx else ""
            
            # If English is empty or just whitespace, try to fill it
            if not current_english or not current_english.strip():
                if spanish_text in translations_map:
                    # Fill it
                    if len(row) <= english_idx:
                        row.append(translations_map[spanish_text])
                    else:
                        row[english_idx] = translations_map[spanish_text]
                    count += 1
                elif spanish_text.strip() == "":
                    # Empty spanish? 
                    pass
                else:
                    # Logic to just copy Spanish if we don't know it? 
                    # For now, let's leave it unless we are sure.
                    # Or maybe add a prefix [EN]? No, user wants real translations.
                    pass
        
        updated_rows.append(row)
        
    f.close()
    
    print(f"Filled {count} missing English translations.")
    
    with open(csv_file, 'w', encoding='utf-8', newline='') as f:
        writer = csv.writer(f)
        writer.writerows(updated_rows)

if __name__ == "__main__":
    fill_english()
