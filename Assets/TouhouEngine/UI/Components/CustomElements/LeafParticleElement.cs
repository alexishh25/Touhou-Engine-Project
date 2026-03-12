using UnityEngine;
using UnityEngine.UIElements;

namespace UIT_VFX
{
    [UxmlElement("HojasVFX")]
    public partial class LeafParticleElement : VisualElement
    {
        public LeafParticleElement()
        {
            generateVisualContent += OnGenerateVisualContent;
        }
        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
            var painter = ctx.painter2D;

            painter.fillColor = Color.green;
            painter.BeginPath();
            painter.Arc(new Vector2(100, 100), 30, 0, 360);
            painter.Fill();
        }
    }

}