namespace HongPhysics
{
    public interface IBoundVolume
    {
        bool Overlaps(IBoundVolume other);
        float GetSize();
        float GetGrowth(IBoundVolume other);
    }
}