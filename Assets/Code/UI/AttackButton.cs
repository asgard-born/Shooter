namespace UI {
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class AttackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler {
        public event Action OnPointerDownEvent;
        public event Action OnPointerUpEvent;
        public event Action PointerEnterEvent;

        public void OnPointerDown(PointerEventData eventData)  => this.OnPointerDownEvent?.Invoke();
        public void OnPointerUp(PointerEventData eventData)    => this.OnPointerUpEvent?.Invoke();
        public void OnPointerEnter(PointerEventData eventData) => this.PointerEnterEvent?.Invoke();
    }
}