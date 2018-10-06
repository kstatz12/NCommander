using System;
using System.Collections.Generic;
using System.Runtime;
using System.Transactions;
using Autofac.Core;
using Microsoft.Win32.SafeHandles;
using NCommander.Plugins;

namespace NCommander
{
    public class CommandInvokerBuilder
    {
        private List<dynamic> _postRunActions;
        private ICommand _command;
        private dynamic _input;
        private IDependencyPlugin _dependencyPlugin;
        public CommandInvokerBuilder()
        {
            _postRunActions = new List<dynamic>();
        }
        public CommandInvokerBuilder WithCommand<T>(T command) where T : ICommand
        {
            _command = command;
            return this;
        }

        public CommandInvokerBuilder WithDependencyPlugin(IDependencyPlugin dependencyPlugin)
        {
            _dependencyPlugin = dependencyPlugin;
            return this;
        }

        public CommandInvokerBuilder WithInput<T>(T input)
        {
            _input = input;
            return this;
        }
        
        public CommandInvokerBuilder WithPostRunAction<T>(Action<T> postRun) where T : ICommand
        {
            _postRunActions.Add(postRun);
            return this;
        }

        public TOut Invoke<TIn, TOut>()
        {
            var commandInvoker = new CommandInvoker(_dependencyPlugin);
            if (!(_command is AbstractCommand<TIn, TOut> cmd))
            {
                return default(TOut);
            }

            if (!(_input is TIn input))
            {
                return default(TOut);
            }
            
            var outVal = commandInvoker.Invoke(cmd, input);
            
            if (cmd.State != CommandState.Success)
            {
                return outVal;
            }
            
            foreach (var p in _postRunActions)
            {
                if (p is Action<AbstractCommand<TIn, TOut>> action)
                {
                    action(cmd);
                }
            }
            return outVal;
        }

    }
}