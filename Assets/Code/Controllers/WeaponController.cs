using System;
using System.Threading.Tasks;

namespace Controllers {
    using Structures;
    using UnityEngine;

    public class WeaponController : MonoBehaviour {
        [SerializeField] private Weapon[]   weapons;
        private                  Animator   animator;
        private                  GameObject currentWeaponObject;

        private int currentWeaponNumber = 0;

        private bool isChangingWeapon;
        private bool canChangeWeapon = true;
        private bool isFiring;
        private bool isReloading;

        [SerializeField] private Transform weaponSlot;

        private Weapon currentWeaponInstance;

        private readonly int equipWeapon  = Animator.StringToHash("EquipWeapon");
        private readonly int firstWeapon  = Animator.StringToHash("EquipWeapon");
        private readonly int secondWeapon = Animator.StringToHash("EquipWeapon");
        private readonly int thirdWeapon  = Animator.StringToHash("EquipWeapon");

        private void Awake() {
            this.animator              = this.GetComponent<Animator>();
            this.currentWeaponInstance = this.weapons[this.currentWeaponNumber];
            this.currentWeaponObject   = this.currentWeaponInstance.WeaponObject;
            this.animator.SetBool(this.currentWeaponInstance.WeaponName, true);
        }

        private void Update() {
            if (this.canChangeWeapon && Input.GetKeyDown(KeyCode.Q)) {
                this.ChangeWeapon();
            }

            if ((this.animator.GetCurrentAnimatorStateInfo(1).IsName("Shotgun Equip")
                 || this.animator.GetCurrentAnimatorStateInfo(1).IsName("Rifle Equip")
                 || this.animator.GetCurrentAnimatorStateInfo(1).IsName("Grenade Equip"))
                && this.animator.GetCurrentAnimatorStateInfo(1).normalizedTime > .3f
                && this.isChangingWeapon) {
                this.SetWeapon();
            }
        }

        private void ChangeWeapon() {
            this.isChangingWeapon = true;
            this.canChangeWeapon  = false;

            if (this.currentWeaponNumber == this.weapons.Length - 1) {
                this.currentWeaponNumber = 0;
            }
            else {
                this.currentWeaponNumber++;
            }

            this.currentWeaponObject.SetActive(false);
            this.animator.SetBool(this.currentWeaponInstance.WeaponName, false);

            this.currentWeaponInstance = this.weapons[this.currentWeaponNumber];
            this.currentWeaponObject   = this.currentWeaponInstance.WeaponObject;

            this.animator.SetBool(this.currentWeaponInstance.WeaponName, true);
            this.animator.SetBool(this.equipWeapon, true);
        }

        private async void SetWeapon() {
            this.isChangingWeapon = false;
            this.animator.SetBool(this.equipWeapon, false);
            this.currentWeaponObject.SetActive(true);
            await Task.Delay(TimeSpan.FromSeconds(1.5f));
            this.canChangeWeapon = true;
        }
    }
}