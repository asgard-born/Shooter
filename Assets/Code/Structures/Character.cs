namespace Structures {
    using UnityEngine;

    public class Character : MonoBehaviour {
        public int Id => this.id; 
        [SerializeField] private int id;
    }
}