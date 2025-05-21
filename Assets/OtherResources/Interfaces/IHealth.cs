namespace OtherResources.Interfaces
{
    public abstract class IHealth : DestroyNetwork
    {
        public float health { get; }
        public float maxHealth { get; }
        
        public abstract void TakeDamage(int damage);
    }
}