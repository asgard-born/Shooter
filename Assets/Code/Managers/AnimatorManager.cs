namespace Managers {
    using System;
    using UnityEngine;

    public class AnimatorManager : MonoBehaviour {
        public static AnimatorManager Instance;

        public float ForwardMoving;
        public float HorizontalMoving;
        public bool  IsSneak;
        public bool  IsJumping;
        public bool  IsFalling;
        public bool  IsGrounded;
        public bool  IsNearToGround;

        public event Action OnWeaponEquip;

        [SerializeField] private Animator animator;

        private readonly int grounded    = Animator.StringToHash("Grounded");
        private readonly int speed       = Animator.StringToHash("Speed");
        private readonly int side        = Animator.StringToHash("Side");
        private readonly int sneak       = Animator.StringToHash("Sneak");
        private readonly int jump        = Animator.StringToHash("Jump");
        private readonly int falling     = Animator.StringToHash("Falling");
        private readonly int nearGround  = Animator.StringToHash("NearGround");
        private readonly int equipWeapon = Animator.StringToHash("EquipWeapon");

        public void SetupWeaponCondition(string WeaponName, bool isSet) =>
            this.animator.SetBool(WeaponName, isSet);

        public void SetWeaponEquipping(bool isEquip) =>
            this.animator.SetBool(this.equipWeapon, isEquip);

        private void Awake() => Instance = this;

        private void Update() {
            if ((this.animator.GetCurrentAnimatorStateInfo(1).IsName("Shotgun Equip")
                 || this.animator.GetCurrentAnimatorStateInfo(1).IsName("Rifle Equip")
                 || this.animator.GetCurrentAnimatorStateInfo(1).IsName("Grenade Equip"))
                && this.animator.GetCurrentAnimatorStateInfo(1).normalizedTime > .3f) {
                this.OnWeaponEquip?.Invoke();
            }
        }

        private void LateUpdate() {
            this.animator.SetFloat(this.speed, this.ForwardMoving);
            this.animator.SetFloat(this.side, this.HorizontalMoving);
            this.animator.SetBool(this.sneak, this.IsSneak);
            this.animator.SetBool(this.jump, this.IsJumping);
            this.animator.SetBool(this.falling, this.IsFalling);
            this.animator.SetBool(this.grounded, this.IsGrounded);
            this.animator.SetBool(this.nearGround, this.IsNearToGround);
        }
    }
}