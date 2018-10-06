using System;
using NCommander.Plugins;

namespace NCommander
{
    internal class CommandInvoker
    {
        private readonly IDependencyPlugin _dependencyPlugin;
        internal CommandInvoker(IDependencyPlugin dependencyPlugin)
        {
            _dependencyPlugin = dependencyPlugin;
        }

        public TOut Invoke<TIn, TOut>(AbstractCommand<TIn, TOut> command, TIn input)
        {
            command.State = CommandState.PreRun;
            _dependencyPlugin.Inject(command);
            command.Input = input;
            try
            {
                command.State = CommandState.Running;
                command.Execute();
                command.State = CommandState.Success;
            }
            catch (Exception ex)
            {
                command.State = CommandState.Failure;
            }
            return command.Output;
        }
    }
}