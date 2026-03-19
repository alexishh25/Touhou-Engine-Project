# Touhou Engine Project

Prototipo técnico de un engine tipo **bullet-hell (danmaku)** desarrollado en Unity 6 con C#. El proyecto funciona como un experimento de arquitectura y gameplay programming, priorizando la escalabilidad, separación de responsabilidades y reutilización de código.

No es un juego terminado — es una **base técnica extensible** que implementa los sistemas core de un danmaku: control de jugador con múltiples personajes, sistema de disparo con object pooling, enemigos, UI completa con UI Toolkit, y una pipeline de carga de escenas.

---

## Arquitectura

El proyecto separa el código en dos capas principales:

- **`Scripts/`** — Código reutilizable e independiente del engine (managers, controllers, data, utils). Puede extraerse a otros proyectos sin modificaciones.
- **`Characteristics/`** — Lógica específica del danmaku (player state machine, bullets, enemigos). Depende de los sistemas base de `Scripts/`.

### Managers Persistentes (Bootstrap)

El sistema de **Bootstrap** garantiza que los managers globales estén siempre disponibles, sin importar qué escena se abra primero en el editor:

```
PerformBootStrap (static)
│   Se ejecuta antes de cualquier Awake() via [RuntimeInitializeOnLoadMethod]
│   Carga la escena "Bootstrap" de forma aditiva si no existe
│
└── Bootstrap Scene
    ├── BootstrapLoader      → Singleton, DontDestroyOnLoad
    ├── GameManager          → Control de pausa (TimeScale) y flujo general
    ├── InputManager         → Centraliza el InputActionAsset, switch de Action Maps
    ├── SoundManager         → Reproducción de SFX y música
    ├── UIManager            → Sistema de pantallas con UI Toolkit
    └── ButtonManager        → Registro de hover/click SFX en botones UI
```

Todos los managers implementan el patrón **Singleton** con `DontDestroyOnLoad`, asegurando una única instancia persistente durante toda la ejecución.

### State Machine del Jugador

El control del jugador utiliza una **State Machine personalizada** que permite múltiples personajes con lógica compartida y diferenciada:

```
PlayerBaseState (abstract)
│   Define: EnterState(), UpdateState(), OnCollisionEnter()
│   Implementa lógica común: LogicMoverse(), LogicFocus(), FadeBox()
│
├── PlayerReimuState    → Estado concreto de Reimu
├── PlayerMarisaState   → Estado concreto de Marisa
└── PlayerSanaeState    → Estado concreto de Sanae

PlayerStateManager (MonoBehaviour)
│   Mantiene referencias: Rigidbody2D, Animator, InputActions, CharacterData
│   Instancia los tres estados en Start()
│   Delega Update() al estado activo: currentState.UpdateState()
│   Expone SwitchState() para cambiar de personaje
│   LoadCharacterData() aplica datos del ScriptableObject al personaje
```

Cada personaje hereda la lógica base de movimiento y focus, pero puede sobrescribir cualquier método para agregar comportamiento propio. La configuración de cada personaje (velocidad, sprites, animaciones, balas) está desacoplada en `CharacterData` (ScriptableObject).

### Sistema de Disparo y Object Pooling

```
BulletPoolManager (Singleton)
│   Pool por tipo de prefab (Dictionary<BulletController, List<BulletController>>)
│   WarmPool() pre-instancia balas al inicio
│   RequestBullet() retorna una bala inactiva o crea una nueva si el pool se agota
│
BulletManager (por entidad)
│   Cada shooter (jugador/enemigo) tiene su propio BulletManager
│   Controla cooldown, velocidad y punto de disparo
│   Solicita balas al BulletPoolManager
│
BulletController
│   Controla movimiento (velocity * deltaTime) y ciclo de vida (MAX_LIFE_TIME)
│   Se desactiva en lugar de destruirse para volver al pool
│
EnemyShooter
    Dispara hacia el jugador usando el mismo sistema de pooling
    Calcula dirección: (player.position - transform.position).normalized
```

### Sistema de UI (UI Toolkit)

La UI utiliza **UI Toolkit** (UXML/USS) con un sistema de pantallas intercambiables:

```
ScreenLogic (abstract MonoBehaviour)
│   Template base para cada pantalla
│   Define: DefinirElementos(), ButtonActionAlterSusYUnsuscribe(), LoadData()
│   Initialize() / Dispose() para ciclo de vida
│
├── MenuScript           → Menú principal (Game Start, Extra, Practice, Quit...)
├── SelectorScript       → Selector de personaje con navegación y transiciones
├── LoadingScript        → Pantalla de carga con VFX
├── HUDScript            → HUD de gameplay
└── PauseUIScript        → Menú de pausa (Continue, Quit and Restart, Return to Game)

MenuUIController (por escena)
│   Define qué pantallas (ScreenEntry[]) están disponibles en la escena
│   Registra las pantallas en UIManager al iniciar
│
UIManager (Singleton persistente)
    Controla el cambio entre pantallas: ChangeScreen(ScreenType)
    Maneja la carga interpolada de escenas: InterpolateScreenLoad()
    Gestiona el UIDocument activo
```

---

## Flujo de Ejecución

```
1. INICIO
   └── PerformBootStrap.Execute()
       └── Carga escena "Bootstrap" (aditiva)
           └── Se inicializan todos los Managers (DontDestroyOnLoad)

2. MENÚ PRINCIPAL (Escena: UI/Menu)
   └── MenuUIController registra pantallas en UIManager
       └── UIManager muestra MainMenu (MenuScript)
           └── Jugador presiona "Game Start"
               └── UIManager cambia a SelectCharacter (SelectorScript)

3. SELECCIÓN DE PERSONAJE
   └── SelectorScript carga datos de SelectorCharacterData
       └── Jugador navega entre personajes (Navigate input)
           └── Al confirmar (Submit):
               ├── InputManager cambia Action Map: "UI" → "Player"
               └── UIManager.InterpolateScreenLoad("Gameplay")

4. CARGA DE ESCENA
   └── Se carga "LoadingScreen" (aditiva)
       └── Se carga "Gameplay" (aditiva, async)
           └── Al completar:
               ├── Se descarga LoadingScreen
               ├── Se descarga la escena anterior
               ├── InputManager activa Action Map "Player"
               └── UIManager desactiva el UIDocument

5. GAMEPLAY (Escena: Gameplay)
   └── PlayerStateManager inicializa estado del personaje
       ├── Carga CharacterData → Aplica stats, animator, bullet sprite
       ├── Estado activo ejecuta UpdateState() cada frame
       │   ├── LogicMoverse() → Input + Rigidbody2D + Animation Blend
       │   └── LogicFocus() → Reduce velocidad + Fade hitbox
       ├── BulletManager dispara balas desde el pool (Input: Shoot)
       └── EnemyShooter dispara hacia el jugador desde el pool
```

---

## Estructura de Carpetas

```
Assets/TouhouEngine/
│
├── Characteristics/                  # Lógica específica del danmaku
│   ├── Bullets/
│   │   ├── SimpleBullet/             # BulletController, BulletManager, BulletPoolManager
│   │   └── RadialShotSettings.cs     # Configuración para patrones radiales
│   ├── Enemy/
│   │   └── Scripts/                  # EnemyShooter, EnemyBulletManager
│   └── Player/
│       ├── Player Controller.cs      # Controller original (pre-refactorización)
│       └── Scripts/
│           ├── Player Base State.cs      # Clase abstracta base
│           ├── Player State Manager.cs   # Cerebro de la state machine
│           ├── Player Reimu State.cs     # Estado concreto
│           ├── Player Marisa State.cs    # Estado concreto
│           ├── Player Sanae State.cs     # Estado concreto
│           ├── Core/Hitbox.cs
│           └── Data/CharacterData.cs     # ScriptableObject de personaje
│
├── Scripts/                          # Código reutilizable
│   ├── Controllers/                  # AnimationUI, Audio, Editor, Enemy, PlayerAnimation
│   ├── Core/                         # BootstrapLoader
│   ├── Data/                         # AnimationData, BulletPatternData, SelectorCharacterData
│   ├── Managers/                     # GameManager, InputManager, SoundManager, UIManager
│   ├── Services/                     # (Espacio reservado)
│   ├── Systems/                      # (Espacio reservado)
│   └── Utils/                        # BoxColliderFit
│
├── UI/                               # UI Toolkit
│   ├── Components/CustomElements/    # LeafParticleElement, TransitionElement
│   ├── Controllers/                  # MenuUIController
│   ├── HUD/                          # HUDScript, PauseUIScript
│   └── Menu/                         # MenuScript, SelectorScript, LoadingScript, ScreenLogic
│
├── ScriptableObjects/                # Assets de datos configurables
│   ├── Characters/                   # CharacterData por personaje
│   ├── Bullets/                      # Configuración de balas
│   ├── Patterns/                     # Patrones de disparo
│   └── Audio/                        # Configuración de audio
│
├── Shaders/                          # Shader Graph (ScrollingBackground, Leaf)
├── Prefabs/                          # Player, Enemies, Bullets, Effects, VFX
├── Scenes/                           # Bootstrap, Gameplay
├── Art/                              # Assets visuales
├── Sounds/                           # Assets de audio
└── Fonts/                            # Tipografías
```

---

## Tecnologías

- **Unity 6** (URP)
- **C#**
- **Unity Input System** — Action Maps múltiples (Player, UI, Gameplay)
- **UI Toolkit** — UXML/USS para toda la interfaz
- **Shader Graph** — Shaders visuales personalizados
- **ScriptableObjects** — Desacoplamiento de datos de configuración
- **Object Pooling** — Optimización de instanciaciones en el sistema de balas

---

## Estado del Proyecto

Proyecto en evolución activa. Los sistemas core están implementados y funcionales. Áreas preparadas para expansión: `Services/`, `Systems/`, patrones de balas avanzados (radial shots), y lógica diferenciada por personaje.
