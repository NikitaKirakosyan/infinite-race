using System;
using UnityEngine.Scripting;

namespace Southbyte.DIConfiguration
{
    /// <summary>
    /// Binds class and interfaces. Use for services with asynchronous initialization and their dependencies.
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EarlyInitializationAttribute : PreserveAttribute
    {
    }
}