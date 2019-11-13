namespace Controllers {
    using Weapons.Abstract;
    using Managers;
    using Weapons.WeaponTypes;
    using System;
    using System.Threading.Tasks;
    using UnityEngine;

    public class WeaponController : MonoBehaviour {
        public Weapon[] Weapons;

        [SerializeField] private ArcRenderer       arcRenderer;
        [SerializeField] private GrenadeController grenadeController;

        public event Action<float, Sprite, bool> OnWeaponChanged;
        public event Action<string, bool>        OnWeaponRearranged;
        public event Action<bool>                SetWeaponEquipped;

        [Space] [SerializeField] private int currentWeaponNumber = 1;

        private CharacterManager player;

        private Weapon     currentWeaponInstance;
        private GameObject currentWeaponObject;

        private int  character_id;
        private bool isChangingWeaponProcess;
        private bool isChangingWeaponOver = true;
        private bool isFiring;
        private bool isThrowable;

        public float BallisticValue {
            set {
                this.ballisticValue += value;
                this.ballisticValue =  Mathf.Clamp(this.ballisticValue, 15, 90);
            }
        }

        private float ballisticValue = 45f;

        public void OnWeaponEquip() {
            if (this.isChangingWeaponProcess) {
                this.SetWeapon();
            }
        }

        public void OnFire() {
            if (this.isChangingWeaponOver) {
                this.currentWeaponInstance.Attack(this.character_id);
            }
        }

        public void Reload() {
            if (this.currentWeaponInstance is IReloadable reloadableInstance && !reloadableInstance.IsReloading) {
                reloadableInstance.Reload();
            }
        }

        public void Initialize(StatManager statManager) {
            foreach (var weapon in this.Weapons) {
                weapon.Initialize(statManager);
                weapon.WeaponObject.SetActive(false);
            }

            this.currentWeaponInstance = this.Weapons[this.currentWeaponNumber];
            this.currentWeaponObject   = this.currentWeaponInstance.WeaponObject;
            this.currentWeaponObject.SetActive(true);

            this.SetupTheWeapon(true);
            this.DetectThrowingWeapon();

            this.OnWeaponChanged?.Invoke(this.currentWeaponInstance.SerialRate, this.currentWeaponInstance.Sprite, this.isThrowable);
        }

        private void SetupTheWeapon(bool isSet) {
            this.OnWeaponRearranged?.Invoke(this.currentWeaponInstance.WeaponName, isSet);
        }

        private void Awake() {
            this.player       = this.GetComponent<CharacterManager>();
            this.character_id = this.player.Id;
        }

        private void Update() {
            if (this.grenadeController != null) {
                this.arcRenderer.Angle       = this.ballisticValue;
                this.grenadeController.Angle = this.ballisticValue;
            }
        }

        public void ChangeWeapon() {
            if (this.isChangingWeaponOver) {
                this.isChangingWeaponProcess = true;
                this.isChangingWeaponOver    = false;

                if (this.currentWeaponNumber == this.Weapons.Length - 1) {
                    this.currentWeaponNumber = 0;
                }
                else {
                    this.currentWeaponNumber++;
                }

                this.currentWeaponObject.SetActive(false);
                this.SetupTheWeapon(false);

                this.currentWeaponInstance = this.Weapons[this.currentWeaponNumber];
                this.currentWeaponObject   = this.currentWeaponInstance.WeaponObject;

                this.DetectThrowingWeapon();

                this.SetupTheWeapon(true);
                this.SetWeaponEquipped?.Invoke(true);
            }
        }

        private void DetectThrowingWeapon() {
            var throwInstance = this.currentWeaponInstance as ThrowingWeapon;
            this.isThrowable = throwInstance != null;

            if (this.arcRenderer != null) {
                this.arcRenderer.SetupRendering(this.isThrowable);
            }
        }

        private async void SetWeapon() {
            this.isChangingWeaponProcess = false;
            this.SetWeaponEquipped?.Invoke(false);
            this.currentWeaponObject.SetActive(true);
            await Task.Delay(TimeSpan.FromSeconds(1.2f));
            this.isChangingWeaponOver = true;
            this.OnWeaponChanged?.Invoke(this.currentWeaponInstance.SerialRate, this.currentWeaponInstance.Sprite, this.isThrowable);
        }
    }
}