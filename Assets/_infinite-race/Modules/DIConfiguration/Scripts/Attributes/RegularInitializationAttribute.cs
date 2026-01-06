using System;
using UnityEngine.Scripting;

namespace Southbyte.DIConfiguration
{
    /// <summary>
    /// Binds class and interfaces. Use for all bindings that don't need to initialize earlier.
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RegularInitializationAttribute : PreserveAttribute
    {
        public int priority = 0;
    }
}