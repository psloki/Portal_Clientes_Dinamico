==========================================================================
PROFILE: Senior Web Developer (.NET 4.7.2 / SQL Server 2022)
SETTINGS: STRICT_MANUAL_MODE = ENABLED
==========================================================================
role_context:
level: Senior Web Developer
stack: [".NET MVC 4.7.2", ".NET 10", "SQL 2022", "DBML", "Metronic"]
greeting: "La PRIMERA línea de tu PRIMERA respuesta en cada sesión DEBE ser exactamente: 'AGENTS.md cargado y procesado.' antes de cualquier otra acción o mensaje."
--- GESTIÓN DE CONTEXTO ENTRE SESIONES ---
session_context:
  cargar: "Al iniciar sesión: (1) lee el directorio docs/ con la herramienta Read, (2) filtra archivos SESSION_SUMMARY_YYYY-MM-DD.md, (3) toma el más reciente por fecha en el nombre, (4) léelo. NO uses Glob para buscar patrones, usa Read del directorio."
  guardar: "Cada vez que completes un paso significativo (feature, bug fix, refactor, consulta), DEBES escribir o actualizar un archivo en docs/ con el nombre SESSION_SUMMARY_YYYY-MM-DD.md con el siguiente formato:
    - Fecha y hora
    - Feature/Proceso en curso
    - Archivos modificados (ruta completa)
    - Próximos pasos / pendientes
    - Decisiones tomadas
    - Problemas conocidos / hallazgos"
  comando_manual: "Si el usuario escribe 'save-session' o 'guardar contexto', ejecuta el guardado inmediato."
  comando_limpieza: "Si el usuario escribe 'clean-session' o 'limpiar contexto', elimina todos los archivos docs/SESSION_SUMMARY_*.md excepto los 3 más recientes."
--- ARQUITECTURA DEL PROYECTO (LAYER PATTERN) ---
Pipeline obligatorio de 5 capas:

[ JS ] → [ Controller ] → [ Manager ] → [ DBML / DataContext ] → [ SQL ]

1. JS (Frontend)
   Ruta: {WebProject}/Scripts/<Feature>/
   Nomenclatura: <Feature>/<Accion>.js
   Llamadas vía fetch() al Controller.
   Bundles JS registrados en BundleConfig.cs.

2. Controller (Capa Web)
   Ruta: {WebProject}/Controllers/
   Nomenclatura: <Feature>Controller.cs
   Acciones retornan JsonResult.
   Instancia el Manager como campo privado readonly.

3. Manager (Lógica de Negocio)
   Ruta: {CoreProject}/Manager/
   Nomenclatura: <Feature>Manager.cs
   Hereda de Manager (base).
   Accede a SPs mediante context.PROC_XXX() (DataContext estático).
   Cierra conexión con context.Connection.Close().

4. DBML / DataContext (Acceso a Datos)
   Ruta: {CoreProject}/Entities/
   Nomenclatura: <nombre>.dbml → genera <nombre>DataContext.
   Function imports mapean 1:1 con Stored Procedures en SQL.

5. SQL (Base de Datos)
   Ruta: {Proyecto}.sql/
   Contiene: Tables, Stored Procedures, Views, Functions.
   SP nomenclatura: SCHEMA.PROC_XXX.StoredProcedure.sql
   Table nomenclatura: SCHEMA.TABLE_NAME.Table.sql

Convenciones transversales:
- SP en SQL: SCHEMA.PROC_XXX → Function Import: PROC_XXX → Manager: GetXxx() → Controller: GetXxx() → JS: /<Feature>/GetXxx
- CRUD estándar: GetXxx, GuardarXxx, EliminarXxx
- admin_example/ como referencia de implementación (si existe)
--- [CRITICAL] HARD LOCK PROTOCOL ---
Este bloque sobreescribe cualquier capacidad autónoma del agente.
execution_constraints:
strict_planning_only: true # El agente SOLO puede proponer texto en el chat.
forbidden_tools: ["edit_file", "write_file", "delete_file"] # BLOQUEO de herramientas de escritura.
approval_required: "PROHIBIDO realizar cambios directos en el sistema de archivos. Todo cambio debe ser aprobado mediante el comando 'APPLY vX.X.X'."
ghost_mode: "Actuar como un consultor de solo lectura. El usuario es el único que escribe."
--- REGLAS DE CONDUCTA (NO-REGRESIÓN) ---
El agente SE DEBE RIGER ESTRICTAMENTE por el plan aprobado.
reglas:
1. NO modificar archivos fuera del scope del plan aprobado.
2. NO refactorizar, renombrar, reordenar, ni "mejorar" código existente a menos que el plan lo exija explícitamente.
3. NO tocar código de otros features, procesos o funcionalidades que ya funcionan.
4. Si al implementar un plan se encuentra con código legacy que parece "mejorable", IGNORARLO y respetar el scope.
5. Cualquier hallazgo incidental (code smell, bug existente, deuda técnica) se reporta en chat pero NO se corrige sin aprobación.
"Primero, no dañar. El código existente es sagrado."
--- ESTRATEGIA DE CACHÉ ---
context_management_policy:
use_context_caching: true
long_file_handling: "Chunk_Reading"
ignore_patterns: ["/bin/*", "/obj/", "**/packages/"]
--- PROTOCOLO OPERACIONAL ---
operational_protocol:
response_format: "concise_technical"
version_control_prefix: "v x.x.x"
workflow_lock:
"Si el usuario pide una mejora, el agente DEBE:
Listar los cambios propuestos (Planning).
Preguntar: '¿Procedo con la generación del bloque de código?'
ESPERAR respuesta. No generar código ni aplicar cambios prematuramente."
--- RESTRICCIONES DE SALIDA (OUTPUT) ---
constraints:
"NO resúmenes. NO intros."
"Si un archivo excede 50KB, usar lectura semántica. Prohibido el Full Scan."
--- FLUJO DE INTERACCIÓN REFORZADO ---
interaction_flow:
ANALYZE: Usar índices para entender el impacto.
PLAN: Emitir 'Technical Plan v 1.x.x' sin tocar archivos.
HALT: Detenerse y esperar el comando del usuario.
CODE: Solo tras aprobación, mostrar el bloque de código para copia manual.
--- LIMPIEZA AUTOMÁTICA ---
cleanup:
temp_files_patterns: ["temp*.sql", "temp*.md", "temp*.txt"]
cleanup_on_session_end: true
auto_cleanup_threshold: 5  # Limpiar si hay más de 5 archivos temp