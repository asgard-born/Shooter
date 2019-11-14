namespace Controllers {
    using Abstract;
    using UI.Abstract;
    using System.Threading.Tasks;
    using UnityEngine.UI;
    using System;
    using UnityEngine;

    public class JoystickController : MonoBehaviour, IInputController, Joystick {
        public float ForwardMoving          => this.movingJoystick.GetAxisY();
        public float HorizontalMoving       => this.movingJoystick.GetAxisX();
        public float GetAxisX               => this.movingJoystick.GetAxisX() * 1.5f;
        public float GetAxisY               => this.movingJoystick.GetAxisY();
        public float GetBallisticValue      => this.grenadeBar.GetAxisY();
        public float GetAttackJoystickValue => this.attackJoystick.GetAxisX() / 2;

        public Image  WeaponImage     => this.weaponImg;
        public Button ReloadBtn       => this.reloadBtn;
        public Button ChangeWeaponBtn => this.changeWeaponBtn;

        public bool  IsSneak    { get; }
        public bool  IsJumping  { get; }
        public float SerialRate { get; set; }

        public event Action OnFireOnce;
        public event Action OnReload;
        public event Action OnChangingWeapon;

        public ETCJoystick MovingJoystick => this.movingJoystick;
        public ETCJoystick GrenadeBar     => this.grenadeBar;
        public ETCJoystick AttackJoystick => this.attackJoystick;

        [SerializeField] private ETCJoystick movingJoystick;
        [SerializeField] private ETCJoystick grenadeBar;
        [SerializeField] private ETCJoystick attackJoystick;

        [SerializeField] private Image  weaponImg;
        [SerializeField] private Button reloadBtn;
        [SerializeField] private Button changeWeaponBtn;

        private bool canInputFire = true;
        private bool isAttackHolding;

        public void RearrangeAttackUI(Sprite sprite, bool isThrowable) {
            this.ChangeWeaponSprite(sprite);

            this.grenadeBar.gameObject.SetActive(isThrowable);
            this.reloadBtn.gameObject.SetActive(!isThrowable);
        }

        private void ChangeWeaponSprite(Sprite sprite)
            => this.WeaponImage.GetComponent<Image>().sprite = sprite;

        private async void Awake() {
            this.attackJoystick.activated = false;
            this.changeWeaponBtn.enabled  = false;
            this.changeWeaponBtn.onClick.AddListener(() => this.OnChangingWeapon?.Invoke());
            this.reloadBtn.onClick.AddListener(() => this.OnReload?.Invoke());
            await Task.Delay(TimeSpan.FromSeconds(1));
            this.attackJoystick.activated = true;
            this.changeWeaponBtn.enabled  = true;
        }

        public void OnAttackPressDown() {
            this.isAttackHolding = true;

            if (this.canInputFire) {
                this.PerformFireInputWithFireRate();
            }
        }

        public void OnAttackPressUp() => this.isAttackHolding = false;

        private void Update() {
            if (this.isAttackHolding && this.canInputFire) {
                this.PerformFireInputWithFireRate();
            }
        }

        private async void PerformFireInputWithFireRate() {
            this.canInputFire = false;
            this.OnFireOnce?.Invoke();
            await Task.Delay(TimeSpan.FromSeconds(this.SerialRate));
            this.canInputFire = true;
        }
    }
}