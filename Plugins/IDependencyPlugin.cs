namespace NCommander.Plugins
{
    public interface IDependencyPlugin
    {
        void Inject<T>(T obj) where T : ICommand;
    }
}