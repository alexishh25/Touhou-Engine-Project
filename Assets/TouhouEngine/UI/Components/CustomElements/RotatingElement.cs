using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIT_VFX
{
    public enum AnimationType { Clockwise, CounterClockwise }

    [UxmlElement("RotatingElement")]
    public partial class RotatingElement : VisualElement
    {
        public static readonly string UssClassName = "rotating-element";

        private float _speed = 1.0f;
        [UxmlAttribute]
        [Range(0f, 20f)] public float Speed 
        {
            get => _speed;
            set => _speed = Mathf.Clamp(value, 0F, 20F);
        }
        [UxmlAttribute]
        public AnimationType animation { get; set; } = AnimationType.Clockwise;

        private float _currentAngle = 0f;
        private bool _isSpinning = false;
        private IVisualElementScheduledItem _scheduleditem;

        public RotatingElement()
        {
            AddToClassList(UssClassName);
            RegisterCallback<DetachFromPanelEvent>(_ => StopRotation());
        }

        /// <summary>
        /// Suscribe el elemento a un Action externo.
        /// Cada invocación del Action dispara una vuelta completa (360°).
        /// </summary>
        public void Subscribe(ref Action externalAction)
        {
            externalAction += OnActionInvoked;
        }

        /// <summary>
        /// Desuscribe el elemento del Action externo.
        /// Llamar siempre desde Dispose() del ScreenLogic correspondiente.
        /// </summary>
        public void Unsubscribe(ref Action externalAction)
        {
            externalAction -= OnActionInvoked;
            StopRotation();
        }

        private void OnActionInvoked()
        {
            // Si ya está girando, reinicia el conteo para completar
            // una vuelta completa desde el ángulo actual sin corte brusco
            if (_isSpinning)
            {
                _currentAngle = 0f;
                return;
            }

            _currentAngle = 0f;
            StartRotation();
        }
        private void StartRotation() 
        {
            _isSpinning = true;
            _scheduleditem?.Pause();
            _scheduleditem = schedule
                .Execute(Tick)
                .Every(16)
                .StartingIn(0);
        }

        private void StopRotation() 
        {
            _scheduleditem?.Pause();
            _scheduleditem = null;
        }

        private void Tick(TimerState state)
        {
            if (!enabledInHierarchy) return;

            float delta = (float)state.deltaTime / 1000f;
            float direction = animation == AnimationType.Clockwise ? 1f : -1f;

            float step = Speed * direction * delta * 360f;

            _currentAngle += Mathf.Abs(step);

            float visualAngle = direction * _currentAngle;
            style.rotate = new StyleRotate(new Rotate(new Angle(visualAngle, AngleUnit.Degree)));

            if (_currentAngle >= 360f)
                StopRotation();


        }
    }
}