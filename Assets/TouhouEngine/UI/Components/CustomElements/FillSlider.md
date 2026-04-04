# 🎚️ FilledSlider - Custom UI Toolkit Component (Unity)

Este componente extiende el `Slider` nativo de Unity para agregar una barra de progreso visual ("fill") sincronizada con el valor actual. Está diseñado para integrarse perfectamente con UI Toolkit y aprovechar al máximo USS + C#.

---

## 🧩 1. Integración con UI Builder

```csharp
[UxmlElement("FilledSlider")]
public partial class FilledSlider : Slider
```

### 🔍 ¿Qué hace esto?

* **`[UxmlElement]`**
  Permite que el componente aparezca directamente en el UI Builder como un elemento reutilizable.
  Internamente, Unity genera código automáticamente (Source Generators), evitando el uso manual de `UxmlFactory`.

* **`partial class`**
  Permite que Unity extienda esta clase con código generado sin ensuciar tu implementación.

* **Herencia de `Slider`**
  El componente reutiliza toda la funcionalidad base:

  * Input (mouse, teclado)
  * Límites (`lowValue`, `highValue`)
  * Comportamiento nativo

---

## ⚙️ 2. Constructor (Inicialización)

```csharp
public FilledSlider() : base()
{
    _fillPart = new VisualElement { name = "unity-fill" };

    var dragContainer = this.Q<VisualElement>("unity-drag-container");
    if (dragContainer != null) {
        dragContainer.Insert(0, _fillPart);
    }

    this.RegisterValueChangedCallback(OnValueChanged);
    this.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
}
```

### 🔍 ¿Qué ocurre aquí?

1. **Se crea el elemento visual**

   * `_fillPart` representa la barra de relleno.

2. **Se inyecta dentro del Slider**

   * Se busca el contenedor interno (`unity-drag-container`)
   * Se inserta en el índice `0` (al fondo visual)

3. **Se registran eventos**

   * `OnValueChanged`: cuando el usuario cambia el valor
   * `OnGeometryChanged`: cuando el layout se construye

---

## 📡 3. Eventos ("Mensajeros")

```csharp
private void OnGeometryChanged(GeometryChangedEvent evt) ...
private void OnValueChanged(ChangeEvent<float> evt) ...
public override void SetValueWithoutNotify(float newValue) ...
```

### 🧠 Responsabilidades

* **OnGeometryChanged**

  * Sincroniza el fill al iniciar la UI

* **OnValueChanged**

  * Actualiza la barra en tiempo real cuando el usuario interactúa

* **SetValueWithoutNotify**

  * Garantiza sincronización visual incluso si el valor cambia desde código sin eventos

---

## 🧮 4. Lógica de cálculo (UpdateFill)

```csharp
private void UpdateFill(float val)
{
    if (Mathf.Approximately(highValue, lowValue)) return;

    float percent = (val - lowValue) / (highValue - lowValue);
    percent = Mathf.Clamp01(percent);

    _fillPart.style.width = Length.Percent(percent * 100f);
}
```

### 🔍 Paso a paso

1. **Validación**

   * Evita divisiones inválidas

2. **Cálculo de porcentaje**

   ```
   (valor actual - mínimo) / (máximo - mínimo)
   ```

3. **Clamp**

   * Limita el valor entre 0 y 1

4. **Aplicación visual**

   * Convierte a porcentaje (`0.42 → 42%`)
   * Se asigna directamente al ancho del elemento

---

## 🎨 5. Integración con USS

```css
#unity-fill {
    background-color: rgb(255, 68, 0);
}
```

### 🧠 Separación de responsabilidades

* **C#**

  * Controla tamaño (lógica)

* **USS**

  * Controla apariencia (color, estilo)

👉 Esto permite cambiar completamente el diseño sin tocar código.

---

## 💡 Conclusión

Este componente demuestra una implementación limpia y escalable de UI Toolkit basada en:

* Extensión de componentes nativos
* Inyección en jerarquía interna
* Sincronización robusta de estado
* Separación clara entre lógica y estilo

---

## 🚀 Posibles mejoras

* Soporte para animaciones de transición en el fill
* Integración con Data Binding
* Personalización de dirección (horizontal/vertical)
* Soporte para temas dinámicos

---

## 🧪 Uso

1. Añadir el script al proyecto
2. Usar `FilledSlider` directamente desde UI Builder
3. Aplicar estilos USS personalizados

---

✨ Este enfoque permite construir componentes reutilizables, mantenibles y altamente integrados con el flujo moderno de UI Toolkit.
