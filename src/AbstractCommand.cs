namespace NCommander
{
    public abstract class AbstractCommand<TIn, TOut> : ICommand
    {

        private CommandObserver _observer;
        
        public TIn Input { get; set; }     
        public TOut Output { get; set; }

        private CommandState _state;
        public CommandState State
        {
            get => _state;
            set
            {
                _observer.Notify(value);
                _state = value;
            }
        }

        public virtual void AttatchCommandObserver(CommandObserver observer)
        {
            _observer = observer;
        }
         
        public abstract void Execute();
    }
}