public interface Action<T>
{
    public abstract ActionRet Execute(T element);
    public abstract bool isPossible(T element);
    public string Name { get; set; }

}