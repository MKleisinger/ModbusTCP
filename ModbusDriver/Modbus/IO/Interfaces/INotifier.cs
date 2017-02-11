using System;

namespace Modbus.Interfaces
{
    public interface INotifier<T>
    {
        /// <summary>
        /// Gets a stream for publishing notifications 
        /// that objects can subscribe to.
        /// </summary>
        IObservable<T> NotificationStream { get; }
    }
}