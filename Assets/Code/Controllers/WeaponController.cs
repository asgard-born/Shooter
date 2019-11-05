namespace Controllers {
    using System;
    using System.Threading.Tasks;
    using Structures.WeaponTypes;
    using Structures;
    using UnityEngine;

    public class WeaponController : MonoBehaviour {
        public static WeaponController Instance;
        [SerializeField] private ArcRenderer arcRenderer;

        public event Action<string, bool> OnWeaponRearranged;
        public event Action<bool>         SetWeaponEquipped;
        public event Action<float>        OnWeaponChanged;
        public event Action<bool>         OnThrowingWeaponEquip;

        [SerializeField]         private Weapon[] weapons;
        [Space] [SerializeField] private int      currentWeaponNumber = 1;

        private Character character;
        private int       character_id;

        private Weapon     currentWeaponInstance;
        private GameObject currentWeaponObject;

        private bool isChangingWeaponProcess;
        private bool isChangingWeaponOver = true;
        private bool isFiring;

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
            var reloadableInstance = this.currentWeaponInstance as Reloadable;

            if (reloadableInstance != null && !reloadableInstance.IsReloading) {
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

            this.OnWeaponChanged?.Invoke(this.currentWeaponInstance.SerialRate);
        }

        public void SetupTheWeapon(bool isSet) {
            this.OnWeaponRearranged?.Invoke(this.currentWeaponInstance.WeaponName, isSet);
        }

        private void Awake() {
            Instance          = this;
            this.character    = this.GetComponent<Character>();
            this.character_id = this.character.Id;
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

            this.DetectThrowingWeapon();

            this.SetupTheWeapon(true);
            this.SetWeaponEquipped?.Invoke(true);
        }

        private void DetectThrowingWeapon() {
            var throwInstance = this.currentWeaponInstance as ThrowingWeapon;

            if (throwInstance != null) {
                this.OnThrowingWeaponEquip?.Invoke(true);
                this.arcRenderer.SetupRendering(true);
            }
            else {
                this.OnThrowingWeaponEquip?.Invoke(false);
                this.arcRenderer.SetupRendering(false);
            }
        }

        private async void SetWeapon() {
            this.isChangingWeaponProcess = false;
            this.SetWeaponEquipped?.Invoke(false);
            this.currentWeaponObject.SetActive(true);
            await Task.Delay(TimeSpan.FromSeconds(1.2f));
            this.isChangingWeaponOver = true;
            this.OnWeaponChanged?.Invoke(this.currentWeaponInstance.SerialRate);
        }
    }
}