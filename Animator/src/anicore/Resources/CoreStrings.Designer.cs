﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Animator.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class CoreStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CoreStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Animator.Resources.CoreStrings", typeof(CoreStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Read scope not allowed for non-recursive lock that holds lock.
        /// </summary>
        internal static string LockReadScope_NonRecursiveWithLockNotAllowed {
            get {
                return ResourceManager.GetString("LockReadScope_NonRecursiveWithLockNotAllowed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Upgradeable read scope not allowed for non-recursive lock that holds lock.
        /// </summary>
        internal static string LockUpgradeableReadScope_NonRecursiveWithLockNotAllowed {
            get {
                return ResourceManager.GetString("LockUpgradeableReadScope_NonRecursiveWithLockNotAllowed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Write scope not allowed for non-recursive lock that holds writable lock.
        /// </summary>
        internal static string LockWriteScope_NonRecursiveWithWriteLockNotAllowed {
            get {
                return ResourceManager.GetString("LockWriteScope_NonRecursiveWithWriteLockNotAllowed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument &apos;{0}&apos; must be greater than {1} (actual value: {2}).
        /// </summary>
        internal static string Require_ArgGreaterThan {
            get {
                return ResourceManager.GetString("Require_ArgGreaterThan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument &apos;{0}&apos; must be greater than or equal to {1} (actual value: {2}).
        /// </summary>
        internal static string Require_ArgGreaterThanOrEqualTo {
            get {
                return ResourceManager.GetString("Require_ArgGreaterThanOrEqualTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument &apos;{0}&apos; must be less than {1} (actual value: {2}).
        /// </summary>
        internal static string Require_ArgLessThan {
            get {
                return ResourceManager.GetString("Require_ArgLessThan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument &apos;{0}&apos; must be less than or equal to {1} (actual value: {2}).
        /// </summary>
        internal static string Require_ArgLessThanOrEqualTo {
            get {
                return ResourceManager.GetString("Require_ArgLessThanOrEqualTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Arg must be of type {0}.
        /// </summary>
        internal static string Require_ArgMustBeOfType {
            get {
                return ResourceManager.GetString("Require_ArgMustBeOfType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument cannot be empty.
        /// </summary>
        internal static string Require_ArgNotNullOrEmpty {
            get {
                return ResourceManager.GetString("Require_ArgNotNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument &apos;{0}&apos; cannot be zero.
        /// </summary>
        internal static string Require_ArgNotZero {
            get {
                return ResourceManager.GetString("Require_ArgNotZero", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot parse string as Time: &apos;{0}&apos;.
        /// </summary>
        internal static string Time_CannotParseString {
            get {
                return ResourceManager.GetString("Time_CannotParseString", resourceCulture);
            }
        }
    }
}
