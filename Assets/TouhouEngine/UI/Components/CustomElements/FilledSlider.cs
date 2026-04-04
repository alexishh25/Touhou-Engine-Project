using UnityEngine;
using UnityEngine.UIElements;

namespace TouhouEngine.UI.Components
{
    [UxmlElement("FilledSlider")]
    public partial class FilledSlider : Slider
    {
        private VisualElement _fillPart;

        public FilledSlider() : base()
        {
            _fillPart = new VisualElement { name = "unity-fill" };

            // Inyectamos el fill background detrás del tracker/dragger
            var dragContainer = this.Q<VisualElement>("unity-drag-container");
            if (dragContainer != null)
            {
                dragContainer.Insert(0, _fillPart);
            }

            this.RegisterValueChangedCallback(OnValueChanged);
            
            // Register geometry changed to apply fill the very first frame it has layout
            this.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            UpdateFill(this.value);
            this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        private void OnValueChanged(ChangeEvent<float> evt)
        {
            UpdateFill(evt.newValue);
        }

        public override void SetValueWithoutNotify(float newValue)
        {
            base.SetValueWithoutNotify(newValue);
            UpdateFill(newValue);
        }

        private void UpdateFill(float val)
        {
            if (Mathf.Approximately(highValue, lowValue)) return;

            float percent = (val - lowValue) / (highValue - lowValue);
            percent = Mathf.Clamp01(percent);

            _fillPart.style.width = Length.Percent(percent * 100f);
        }
    }
}
