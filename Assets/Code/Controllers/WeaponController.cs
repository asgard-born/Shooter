using Structures.WeaponTypes;

namespace Controllers {
    using System;
    using System.Threading.Tasks;
    using Structures;
    using UnityEngine;

    public class WeaponController : MonoBehaviour {
        public static WeaponController Instance;

        public event Action<string, bool> OnWeaponChanged;
        public event Action<bool>         SetWeaponEquipped;

        [SerializeField] private Weapon[] weapons;

        private Weapon     currentWeaponInstance;
        private GameObject currentWeaponObject;

        private int currentWeaponNumber = 0;

        private bool isChangingWeaponProcess;
        private bool isChangingWeaponOver = true;
        private bool isFiring;
        private bool isReloading;

        [SerializeField] private Transform weaponSlot;

        public void OnWeaponEquip() {
            if (this.isChangingWeaponProcess) {
                this.SetWeapon();
            }
        }

        public void OnFire() {
            this.currentWeaponInstance.Fire();
        }

        public async void Reload() {
            var reloadableInstance = currentWeaponInstance as FiringWeapon;
            
            if (!this.isReloading && reloadableInstance != null) {
                reloadableInstance.Reload();
                this.isReloading = true;

                await Task.Delay(TimeSpan.FromSeconds(reloadableInstance.GetReloadRate()));

                Debug.Log("finish reload");
                this.isReloading = false;
            }
        }

        public void SetupTheWeapon(bool isSet) {
            this.OnWeaponChanged?.Invoke(this.currentWeaponInstance.WeaponName, isSet);
        }

        private void Awake() {
            Instance = this;

            this.currentWeaponInstance = this.weapons[this.currentWeaponNumber];
            this.currentWeaponObject   = this.currentWeaponInstance.WeaponObject;
        }

        private void Update() {
            if (this.isChangingWeaponOver && Input.GetKeyDown(KeyCode.Q)) {
                this.ChangeWeapon();
            }
        }

        public bool CanShoot => this.isChangingWeaponOver && !this.isReloading;

        private void ChangeWeapon() {
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

            this.SetupTheWeapon(true);
            this.SetWeaponEquipped?.Invoke(true);
        }

        private async void SetWeapon() {
            this.isChangingWeaponProcess = false;
            this.SetWeaponEquipped?.Invoke(false);
            this.currentWeaponObject.SetActive(true);
            await Task.Delay(TimeSpan.FromSeconds(1.2f));
            this.isChangingWeaponOver = true;
        }
    }
}