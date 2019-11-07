namespace Controllers {
    using Structures.Weapons;
    using Structures.Weapons.Interfaces;
    using Structures.Weapons.WeaponTypes;
    using System;
    using System.Threading.Tasks;
    using Structures;
    using UnityEngine;

    public class WeaponController : MonoBehaviour {
        public static WeaponController Instance;
        public        Weapon[]         weapons;

        [SerializeField] private ArcRenderer arcRenderer;

        public event Action<string, bool>        OnWeaponRearranged;
        public event Action<float, Sprite, bool> OnWeaponChanged;
        public event Action<bool>                SetWeaponEquipped;

        [Space] [SerializeField] private int currentWeaponNumber = 1;

        private Player player;
        private int    character_id;

        private Weapon     currentWeaponInstance;
        private GameObject currentWeaponObject;

        private bool isChangingWeaponProcess;
        private bool isChangingWeaponOver = true;
        private bool isFiring;
        private bool isThrowable;

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
            if (this.currentWeaponInstance is Reloadable reloadableInstance && !reloadableInstance.IsReloading) {
                reloadableInstance.Reload();
            }
        }

        public void Initialize() {
            foreach (var weapon in this.weapons) {
                weapon.WeaponObject.SetActive(false);
            }

            this.currentWeaponInstance = this.weapons[this.currentWeaponNumber];
            this.currentWeaponObject   = this.currentWeaponInstance.WeaponObject;
            this.currentWeaponObject.SetActive(true);

            this.SetupTheWeapon(true);
            this.DetectThrowingWeapon();

            this.OnWeaponChanged?.Invoke(this.currentWeaponInstance.SerialRate, this.currentWeaponInstance.Sprite, this.isThrowable);
        }

        public void SetupTheWeapon(bool isSet) {
            this.OnWeaponRearranged?.Invoke(this.currentWeaponInstance.WeaponName, isSet);
        }

        private void Awake() {
            Instance          = this;
            this.player       = this.GetComponent<Player>();
            this.character_id = this.player.Id;
        }

        public void ChangeWeapon() {
            if (this.isChangingWeaponOver) {
                this.isChangingWeaponProcess = true;
                this.isChangingWeaponOver    = false;

                if (this.currentWeaponNumber == this.weapons.Length - 1) {
                    this.currentWeaponNumber = 0;
                }
                else {
                    this.currentWeaponNumber++;
                }

                this.currentWeaponObject.SetActive(false);
                this.SetupTheWeapon(false);

                this.currentWeaponInstance = this.weapons[this.currentWeaponNumber];
                this.currentWeaponObject   = this.currentWeaponInstance.WeaponObject;

                this.DetectThrowingWeapon();

                this.SetupTheWeapon(true);
                this.SetWeaponEquipped?.Invoke(true);
            }
        }

        private void DetectThrowingWeapon() {
            var throwInstance = this.currentWeaponInstance as ThrowingWeapon;
            this.isThrowable = throwInstance != null;
            this.arcRenderer.SetupRendering(this.isThrowable);
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