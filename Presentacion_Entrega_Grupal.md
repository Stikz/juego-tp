# PROGRAMACIÓN DE SISTEMAS DE JUEGO - UADE
## Primera Entrega Grupal: Arquitectura de Software, Patrones de Diseño y Principios SOLID

---

### FICHA TÉCNICA DEL PROYECTO
- **Género**: Top-Down Stealth / Shooter de Sigilo 2D.
- **Motor / Versión**: Unity 6 (6000.0.x) - Universal Render Pipeline (URP).
- **Mecánicas Principales**:
  - Movimiento del jugador con rotación de torso y piernas independiente.
  - Taser con munición limitada para neutralizar guardias.
  - Conos de visión con oscilación para cámaras de seguridad y guardias de patrulla.
  - Detección inmediata que desencadena Game Over al ser descubierto.
  - Recolección de dinero en mesas de la instalación para alcanzar la condición de Victoria.
  - Tarjetas de acceso (Keycards) para apertura de puertas restringidas.

---

# GUION DE LA PRESENTACIÓN (DIAPOSITIVA POR DIAPOSITIVA)

---

## DIAPOSITIVA 1: Portada
- **Título**: Primera Entrega Grupal: Arquitectura, Patrones de Diseño y Principios SOLID
- **Materia**: Programación de Sistemas de Juego
- **Institución**: Universidad Argentina de la Empresa (UADE)
- **Integrantes**: [Nombres y Apellidos del Grupo]
- **Juego**: *Stealth Heist 2D* (Top-Down de Sigilo)

---

## DIAPOSITIVA 2: Descripción y Alcance del Juego
- **Concepto**: El jugador debe infiltrarse en un complejo de alta seguridad, evitar la detección de guardias y cámaras de seguridad, neutralizar amenazas con su taser y robar todo el dinero de las mesas para ganar el nivel.
- **Objetivo de la Entrega**: Aplicar una arquitectura limpia y desacoplada mediante principios **SOLID** y la implementación justificada de patrones de diseño vistos durante el cuatrimestre, evitando la sobreingeniería.

---

## DIAPOSITIVA 3: Resumen de Patrones Aplicados
*Se implementaron 7 patrones de diseño justificados de forma práctica y teórica:*
1. **State (State Machine)** - IA del Enemigo Guardián (`EnemyPatrol`).
2. **Strategy** - Sistema de Armamento e Intercambio de Herramientas (`PlayerShooting`).
3. **Factory (con Dictionary)** - Generación de Enemigos (`EnemyFactory`).
4. **Singleton** - Administrador Global del Juego (`GameManager`).
5. **Observer** - Notificación de Eventos de Detección y HUD (`GameEvents`).
6. **Object Pool** - Reutilización de Balas del Taser (`BulletPool`).
7. **Template Method** - Esqueleto de Detección Sensorial (`StealthSensorBase`).

---

## DIAPOSITIVA 4: Patrón 1 - STATE (State Machine)
- **Problema previo**: `EnemyPatrol` acumulaba múltiples booleanos (`isWaiting`, `isDead`, `isInvestigating`) y condicionales anidados en `Update()`. La clase tenía demasiadas responsabilidades y era propensa a bugs de transición.
- **Solución Teórica (PPT UADE)**:
  - Se define la interfaz `IEnemyState` con los métodos `Enter()`, `UpdateState(float deltaTime, EnemyPatrol enemy)` y `Exit()`.
  - Se crean clases concretas para cada estado:
    - `EnemyPatrolState`: Recorre los waypoints asignados.
    - `EnemyWaitState`: Pausa al guardia en cada nodo el tiempo configurado (`waitTimeAtPoint`).
    - `EnemyAlertState`: Detiene la patrulla y activa la alarma/Game Over al detectar al jugador.
    - `EnemyDeadState`: Apaga luces, desactiva físicas y dispara la animación de neutralización.
- **Beneficio**: Elimina los `if-else` encadenados y cumple con el principio de **Responsabilidad Única (SRP)**.

---

## DIAPOSITIVA 5: Patrón 2 - STRATEGY
- **Problema previo**: `PlayerShooting` instanciaba directamente un proyectil fijo de taser con comportamiento cableado en el código, imposibilitando agregar gadgets o armas sin alterar la clase.
- **Solución Teórica (PPT UADE)**:
  - Se define la interfaz común `IWeaponStrategy` con el método `Attack()`, `Reload()`, `WeaponName` y propiedades de munición.
  - Se encapsulan familias de algoritmos intercambiables:
    - `TaserWeaponStrategy`: Disparo electrificado de media distancia con munición contada que neutraliza guardias.
    - `EMPWeaponStrategy`: Disparo silencioso de pulso electromagnético que desactiva cámaras en radio y guardias sin ruido.
  - `PlayerShooting` actúa como **Contexto**, permitiendo cambiar de estrategia en tiempo de ejecución (teclas 1 y 2 o colisión con items) mediante `SetWeapon(IWeaponStrategy)`.
- **Beneficio**: Cumple **Open-Closed Principle (OCP)**; se pueden crear nuevas herramientas sin tocar el script del jugador.

---

## DIAPOSITIVA 6: Patrón 3 - FACTORY (con Dictionary)
- **Problema previo**: Instanciar enemigos con `Instantiate()` disperso en escena o con un `switch(id)` viola el principio OCP, ya que cada nuevo tipo de enemigo obligaría a modificar la clase del Factory.
- **Solución Teórica (PPT UADE - Diapositiva 8)**:
  - Se implementó `EnemyFactory` utilizando un catálogo serializable y un `Dictionary<string, GameObject>`.
  - En `Awake()`, la fábrica mapea los IDs (`"patrol_guard"`, etc.) con sus prefabs correspondientes.
  - El método público `Create(string id, Vector3 pos, Quaternion rot)` busca en el diccionario en tiempo $O(1)$ e instancia el objeto.
- **Beneficio**: Para sumar un nuevo tipo de guardia o dron, basta con asignarlo en el Inspector de Unity sin modificar una sola línea de código C#.

---

## DIAPOSITIVA 7: Patrón 4 - SINGLETON
- **Problema previo**: Se requería una entidad única y de acceso global que supervise el conteo de dinero en mesas de tilemap, el estado de pausa y la victoria/derrota.
- **Solución Teórica (PPT UADE)**:
  - Implementación en `GameManager.cs` verificando en `Awake()`:
    ```csharp
    if (Instance == null) { Instance = this; }
    else if (Instance != this) { Destroy(gameObject); return; }
    ```
- **Análisis Crítico Teórico (PPT UADE - Diapositiva 9)**:
  - Reconocemos las advertencias de la cátedra: el Singleton introduce dependencias globales y puede violar SRP. Por eso, limitamos su uso a managers globales (`GameManager`, `BulletPool`) y evitamos que los componentes de bajo nivel dependan fuertemente de él.

---

## DIAPOSITIVA 8: Patrones Complementarios (OBSERVER, OBJECT POOL y TEMPLATE METHOD)
- **Patrón OBSERVER (`GameEvents.cs`)**:
  - Desacopla las cámaras y enemigos del `GameOverManager`.
  - Cuando un sensor detecta al jugador, dispara `GameEvents.TriggerPlayerDetected()`.
  - `GameOverManager` se suscribe en `OnEnable` y cancela en `OnDisable`. Los sensores no conocen a los menús.
- **Patrón OBJECT POOL (`BulletPool.cs`)**:
  - Evita la sobrecarga del Garbage Collector (GC) generada por `Instantiate` y `Destroy` continuo de dardos del taser. Las balas se desactivan y encolan en una `Queue<GameObject>`.
- **Patrón TEMPLATE METHOD (`StealthSensorBase.cs`)**:
  - Define la plantilla invariable del cono de visión: 1) Chequeo de rango, 2) Chequeo de ángulo, 3) Raycast de máscara de colisión, 4) Filtro de indetectabilidad (`CheatManager`), 5) Disparo del evento.
  - `CameraConeOscillatingWithPause` hereda de esta base y solo implementa su origen y oscilación específica.

---

## DIAPOSITIVA 9: Justificación de Patrones NO Utilizados (15 Puntos)
*Explicación de por qué se descartaron los patrones restantes para evitar sobreingeniería:*

1. **Abstract Factory**: No existen múltiples familias cruzadas de productos incompatibles (ej. UI medieval vs UI espacial al mismo tiempo). El *Factory con Diccionario* cubre perfectamente la necesidad.
2. **Builder**: Las entidades del juego se configuran directamente como Prefabs integrales en Unity. No existen objetos complejos que requieran múltiples pasos de ensamble dinámico.
3. **Prototype**: Unity ya provee la clonación de objetos mediante la serialización de Prefabs y la función nativa `Instantiate()`. Crear una interfaz `IPrototype` manual sería reinventar la rueda.
4. **Decorator**: No existen buffs temporales ni habilidades acumulativas dinámicas sobre el jugador; los componentes modulares de Unity (`MonoBehaviour`) ya aportan composición limpia.
5. **Composite**: El juego no requiere estructuras jerárquicas recursivas (como árboles de nodos o inventarios anidados).
6. **Flyweight**: Al ser un nivel 2D cerrado, el motor de Unity gestiona de manera transparente los sprites compartidos y materiales en GPU sin riesgo de falta de memoria.
7. **Memento**: El juego reinicia el nivel completo o avanza de escena; no posee mecánicas de "deshacer" (Undo) ni de "rebobinar el tiempo".

---

## DIAPOSITIVA 10: Aplicación de Principios SOLID (15 Puntos)

### S - Single Responsibility Principle (Responsabilidad Única)
- **Antes**: `EnemyPatrol` calculaba visión, reproducía audio, gestionaba animación, controlaba la patrulla y llamaba a la UI de Game Over.
- **Después**: 
  - `StealthSensorBase` maneja la visión.
  - `EnemyStateMachine` / `IEnemyState` maneja el comportamiento.
  - `EnemySoundController` maneja el audio.
  - `GameOverManager` maneja la interfaz.

### O - Open/Closed Principle (Abierto / Cerrado)
- **Antes**: Para agregar un nuevo tipo de enemigo o arma se requería modificar `switch` e `if` en las clases cliente.
- **Después**: 
  - `EnemyFactory` permite añadir nuevos enemigos mediante configuración en Inspector (Diccionario).
  - `IWeaponStrategy` permite crear cualquier arma nueva creando una clase que implemente la interfaz sin tocar `PlayerShooting`.

---

## DIAPOSITIVA 11: Aplicación de Principios SOLID (Continuación)

### L - Liskov Substitution Principle (Sustitución de Liskov)
- Cualquier estado derivado (`EnemyPatrolState`, `EnemyWaitState`, `EnemyDeadState`) puede sustituir a `IEnemyState` sin alterar la coherencia de la máquina de estados.
- Cualquier arma derivada (`TaserWeaponStrategy`, `EMPWeaponStrategy`) puede sustituir a `IWeaponStrategy` sin romper a `PlayerShooting`.

### I - Interface Segregation Principle (Segregación de Interfaces)
- En lugar de crear interfaces monolíticas y sobrecargadas como `IGameEntity`, creamos contratos pequeños, cohesivos y precisos:
  - `IEnemyState`: solo métodos de ciclo de vida del estado.
  - `IWeaponStrategy`: solo métodos de armamento.

### D - Dependency Inversion Principle (Inversión de Dependencias)
- **Antes**: Los sensores de las cámaras y los enemigos tenían referencias directas y concretas a `GameOverManager`.
- **Después**: Los sensores notifican mediante la abstracción del patrón Observer (`GameEvents.OnPlayerDetected`). Las clases de alto nivel no dependen de las de bajo nivel.

---

## DIAPOSITIVA 12: Funcionamiento y Demostración Jugable (10 Puntos)
- **Compilación Limpia**: 0 errores en `Assembly-CSharp.csproj`.
- **Flujo Jugable Completo**:
  1. El jugador camina y apunta con el mouse o joystick.
  2. Dispara el taser (tecla izquierda del mouse / gatillo) para incapacitar guardias que patrullan.
  3. Puede cambiar al Pulso EMP con la tecla `2` para desactivar cámaras en radio.
  4. Si entra en el cono de visión de un guardia o cámara, se activa Game Over al instante.
  5. Al interactuar (`E` / botón de interacción) con las mesas que contienen fajos de dinero, el contador avanza.
  6. Al recolectar todo el dinero del nivel, se despliega la pantalla de Victoria y se habilita el siguiente nivel.

---

## DIAPOSITIVA 13: Conclusión
- Se logró un juego de sigilo 100% funcional y pulido.
- La arquitectura pasó de ser monolítica a estar modularizada en componentes desacoplados.
- Cada patrón utilizado responde a una necesidad real y concreta del juego, respaldada por la teoría de la cátedra de Programación de Sistemas de Juego.
