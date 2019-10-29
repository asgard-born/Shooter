namespace UI {
    using UnityEngine;

    public class Crosshair : MonoBehaviour {
        public static Crosshair Instance;

        [SerializeField] private Texture crosshairTexture;

        private Rect crosshairPlace;

        public float CrosshairRegulator => this.crosshairRegulator;

        private readonly float crosshairSize      = Screen.width * 0.03f;
        private readonly float crosshairRegulator = (Screen.height / 8);

        private void Awake() => Instance = this;

        private void OnGUI() {
            this.crosshairPlace = new Rect(
                Screen.width / 2                           - this.crosshairSize / 2,
                Screen.height / 2 - this.crosshairSize / 2 - this.crosshairRegulator, this.crosshairSize, this.crosshairSize
            );
            GUI.DrawTexture(this.crosshairPlace, this.crosshairTexture);
        }
    }
}