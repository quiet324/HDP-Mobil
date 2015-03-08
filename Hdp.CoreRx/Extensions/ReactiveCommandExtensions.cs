﻿using System;
using ReactiveUI;

namespace Hdp.CoreRx.Extensions
{
    public static class ReactiveCommandExtensions
    {
        public static void ExecuteIfCan(this IReactiveCommand @this, object o)
        {
            if (@this == null)
                return;

            if (@this.CanExecute(o))
                @this.Execute(o);
        }

        public static void ExecuteIfCan(this IReactiveCommand @this)
        {
            ExecuteIfCan(@this, null);
        }

        public static IReactiveCommand<object> WithSubscription(this IReactiveCommand<object> @this, Action<object> action)
        {
            @this.Subscribe(action);
            return @this;
        }
    }
}

