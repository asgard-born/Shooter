namespace Controllers {
    using Structures;
    using System.Collections;
    using UnityEngine;

    public class WeaponController : MonoBehaviour {
        [SerializeField] private Weapon[]   weapons;
        private Animator   animator;
        private GameObject currentWeaponObject;

        private int currentWeaponNumber = 0;

        private bool isChangingWeapon;
        private bool isFiring;
        private bool isReloading;

        [SerializeField] private Transform weaponSlot;

        private Weapon currentWeaponInstance;

        private readonly int equipWeapon = Animator.StringToHash("EquipWeapon");

        void Awake() {
            this.animator = this.GetComponent<Animator>();
            this.currentWeaponInstance = this.weapons[this.currentWeaponNumber];
            this.currentWeaponObject = this.currentWeaponInstance.WeaponObject;
        }

        void Update() {
            if (!this.isChangingWeapon && !this.isFiring && !this.isReloading && Input.GetKeyDown(KeyCode.Q)) {
                this.ChangeWeapon();
            }
            
            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("EquipWeapon")
                && this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) {
                this.SetWeapon();
            }
        }

        private void ChangeWeapon() {
            this.isChangingWeapon = true;

            if (this.currentWeaponNumber == this.weapons.Length) {
                this.currentWeaponNumber = 0;
            }
            else {
                this.currentWeaponNumber++;
            }

            this.animator.SetBool(this.equipWeapon, true);
            
            this.currentWeaponObject.SetActive(false);
            this.currentWeaponObject = this.currentWeaponInstance.WeaponObject;
            this.currentWeaponInstance = this.weapons[this.currentWeaponNumber];
        }

        private void SetWeapon() {
            this.currentWeaponObject.SetActive(true);
            this.isChangingWeapon = false;
        }
    }
}