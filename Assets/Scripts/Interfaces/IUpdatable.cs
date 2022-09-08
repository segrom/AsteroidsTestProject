namespace Interfaces
{
    public interface IUpdatable
    {
        delegate void DestroyEvent(IUpdatable obj);
        event DestroyEvent OnDestroy;
        void Update(float deltaTime);
    }
}