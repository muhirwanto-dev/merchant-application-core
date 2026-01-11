using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.Messaging;
using SingleScope.Mvvm.Maui;

namespace JualIn.App.Mobile.Presentation.Core.Messaging
{
    public static class MessengerExtensions
    {
        extension (IMessenger messenger)
        {
            public void Register<TReceiver, TMessage>()
                where TMessage : class, IUIMessage
                where TReceiver : class, IRecipient<TMessage>
            {
                var receiver = MauiServiceProvider.Current.GetService<TReceiver>();
                if (receiver == null)
                {
                    return;
                }

                messenger.Register(receiver);
            }

            public void Unregister<TReceiver, TMessage>()
                where TMessage : class, IUIMessage
                where TReceiver : class, IRecipient<TMessage>
            {
                var receiver = MauiServiceProvider.Current.GetService<TReceiver>();
                if (receiver == null)
                {
                    return;
                }

                messenger.Unregister<TMessage>(receiver);
            }

            public void RegisterAll<TReceiver>()
            {
                var receiver = MauiServiceProvider.Current.GetService<TReceiver>();
                if (receiver == null)
                {
                    return;
                }

                messenger.RegisterAll(receiver);
            }

            public void UnregisterAll<TReceiver>()
            {
                var receiver = MauiServiceProvider.Current.GetService<TReceiver>();
                if (receiver == null)
                {
                    return;
                }

                messenger.UnregisterAll(receiver);
            }
        }
    }
}
