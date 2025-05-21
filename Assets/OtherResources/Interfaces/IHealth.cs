namespace OtherResources.Interfaces
{
    public interface IHealth
    {
        public float health { get; }
        public float maxHealth { get; }
        
        public void TakeDamage(int damage);
    }
}