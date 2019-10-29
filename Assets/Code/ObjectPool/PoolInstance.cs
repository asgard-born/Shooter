namespace ObjectPool {
    [System.Serializable]
    public class PoolInstance {
        public PoolObject Prefab;
        public string     Name;
        public int        Count;
        public Pool       Pool;
    }
}