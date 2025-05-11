using System;

namespace Thaumaturgia.Core.Client.Rendering
{
    /// <summary>
    /// Interface for disposable rendering resources
    /// </summary>
    public interface IDisposableResource : IDisposable
    {
        /// <summary>
        /// Clean up any resources used by this object
        /// </summary>
        void Cleanup();
    }
}
