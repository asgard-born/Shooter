using System;
using System.Threading.Tasks;

namespace Controllers {
    using UnityEngine;

    public class MovementController : MonoBehaviour {
        private Animator animator;

        // basic motion variables
        private                  float forwardMoving;
        private                  float horizontalMoving;
        private                  float run;
        private                  bool  sneak;
        [SerializeField] private float movingSpeed;

        private float normalizedSpeed;

        // jumping variables
        private       bool  isJumping;
        private const float JUMP_VELOCITY       = 7f;
        private       float JUMP_ROTATION_SPEED = 2f;

        private float NEAR_GROUND_MARKER = 1.2f;

        private float     mouseX;
        private Rigidbody rigidbody;
        private Weapon    weapon;
        private float     turnSmoothVelocity;

        private static readonly int Grounded        = Animator.StringToHash("Grounded");
        private static readonly int Speed           = Animator.StringToHash("Speed");
        private static readonly int NormalizedSpeed = Animator.StringToHash("NormalizedSpeed");
        private static readonly int Side            = Animator.StringToHash("Side");
        private static readonly int Sneak           = Animator.StringToHash("Sneak");
        private static readonly int Jump            = Animator.StringToHash("Jump");
        private static readonly int Falling         = Animator.StringToHash("Falling");
        private static readonly int NearGround      = Animator.StringToHash("NearGround");

        public bool getSneak => this.sneak;

        public float getRun => this.run;

        public bool getGroundedMarker => this.IsGrounded();

        private void Start() {
            this.animator  = this.GetComponent<Animator>();
            this.rigidbody = this.GetComponent<Rigidbody>();
            this.weapon    = this.GetComponent<Weapon>();
        }

        private void Update() {
            this.forwardMoving    =  Input.GetAxis("Vertical");
            this.horizontalMoving =  Input.GetAxis("Horizontal");
            this.sneak            =  this.IsGrounded() && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
            this.mouseX           += Input.GetAxis("Mouse X");
            this.run              =  Input.GetButton("Run") && this.IsGrounded() ? 1 : 0;

            this.forwardMoving    += this.forwardMoving    > 0 ? this.run : this.forwardMoving < 0 ? -this.run : 0;
            this.horizontalMoving += this.horizontalMoving > 0 ? this.run : this.horizontalMoving < 0 ? -this.run : 0;

            this.transform.rotation = Quaternion.Euler(0, this.mouseX, 0);
        }

        private void FixedUpdate() {
            this.forwardMoving    *= Time.fixedDeltaTime * this.movingSpeed;
            this.horizontalMoving *= Time.fixedDeltaTime * this.movingSpeed;

            this.rigidbody.MovePosition(this.transform.position + (this.transform.right * this.horizontalMoving + this.transform.forward * this.forwardMoving));
        }

        private async void LateUpdate() {
            this.animator.SetFloat(Speed, this.forwardMoving);
            this.animator.SetFloat(NormalizedSpeed, this.normalizedSpeed);
            this.animator.SetFloat(Side, this.horizontalMoving);
            this.animator.SetBool(Sneak, this.sneak);
            this.animator.SetBool(Jump, this.isJumping);
            this.animator.SetBool(Falling, this.isFalling());
            this.animator.SetBool(Grounded, this.IsGrounded());
            this.animator.SetBool(NearGround, this.IsNearToGround());

            if (this.IsGrounded() && Input.GetKeyDown(KeyCode.Space)) {
                this.StartJumping();
                this.isJumping = true;
            }

            Debug.Log("isFalling " + this.isFalling());

            if (this.isFalling() && this.IsNearToGround()) {
                await this.UpdateJumpChecker();
            }
        }

        private async Task UpdateJumpChecker() {
            await Task.Delay(TimeSpan.FromSeconds(0.7f));
            this.isJumping = false;
        }

        private bool IsGrounded() {
            RaycastHit hit;
            return Physics.Raycast(this.transform.position + new Vector3(0, 0.2f, 0), Vector3.down, out hit, 0.2f);
        }

        private bool IsNearToGround() {
            RaycastHit hit;
            return (!this.IsGrounded()) && Physics.Raycast(this.transform.position, Vector3.down, out hit, this.NEAR_GROUND_MARKER);
        }

        private bool isFalling() {
            return (!this.IsGrounded() && this.rigidbody.velocity.y < 0);
        }

        private void StartJumping() {
            this.rigidbody.velocity = new Vector3(this.horizontalMoving, JUMP_VELOCITY, this.forwardMoving);
        }
    }
}