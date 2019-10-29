namespace Controllers {
    using RootMotion.FinalIK;
    using System;
    using System.Threading.Tasks;
    using Structures;
    using UnityEngine;

    public class WeaponController : MonoBehaviour {
        public static WeaponController Instance;

        public bool IsAiming => this.isChangingWeaponOver && !this.isReloading;

        public event Action<string, bool> OnWeaponChanged;
        public event Action<bool>         SetWeaponEquipped;

        [SerializeField] private Weapon[] weapons;
        [SerializeField] private AimIK    aimIk;

        private GameObject currentWeaponObject;

        private int currentWeaponNumber = 0;

        private bool isChangingWeaponProcess;
        private bool isChangingWeaponOver = true;
        private bool isFiring;
        private bool isReloading;

        [SerializeField] private Transform weaponSlot;

        private Weapon currentWeaponInstance;

        public void OnWeaponEquip() {
            if (this.isChangingWeaponProcess) {
                this.SetWeapon();
            }
        }

        public void Fire() {
            this.currentWeaponInstance.Fire();
        }

        public void SetupTheWeapon(bool isSet) {
            this.OnWeaponChanged?.Invoke(this.currentWeaponInstance.WeaponName, isSet);
        }

        public void ChangeAimPositionWeight(int value) =>
            this.aimIk.solver.IKPositionWeight = value;

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