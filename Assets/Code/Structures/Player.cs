namespace Structures {
    using System;
    using UnityEngine;

    public class Player : MonoBehaviour {
        public int Id => this.id;

        [SerializeField] private int id;
        [SerializeField] private GameObject VisiblePart;

        private Lifer       lifer;
        public event Action OnDeath;

        public void Respawn() => this.lifer.Respawn();
        

        private void Awake() {
            this.lifer         =  this.GetComponent<Lifer>();
            this.lifer.OnDeath += () => this.OnDeath?.Invoke();
        }
    }
}