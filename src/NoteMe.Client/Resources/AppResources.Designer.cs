﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NoteMe.Client.Resources {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class AppResources {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AppResources() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("NoteMe.Client.Resources.AppResources", typeof(AppResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string Email {
            get {
                return ResourceManager.GetString("Email", resourceCulture);
            }
        }
        
        public static string Password {
            get {
                return ResourceManager.GetString("Password", resourceCulture);
            }
        }
        
        public static string ConfirmPassword {
            get {
                return ResourceManager.GetString("ConfirmPassword", resourceCulture);
            }
        }
        
        public static string Login {
            get {
                return ResourceManager.GetString("Login", resourceCulture);
            }
        }
        
        public static string GoToRegister {
            get {
                return ResourceManager.GetString("GoToRegister", resourceCulture);
            }
        }
        
        public static string GoToLogin {
            get {
                return ResourceManager.GetString("GoToLogin", resourceCulture);
            }
        }
        
        public static string IsNotValid {
            get {
                return ResourceManager.GetString("IsNotValid", resourceCulture);
            }
        }
        
        public static string MustBeNotEmpty {
            get {
                return ResourceManager.GetString("MustBeNotEmpty", resourceCulture);
            }
        }
        
        public static string MustBeEqual {
            get {
                return ResourceManager.GetString("MustBeEqual", resourceCulture);
            }
        }
        
        public static string Notes {
            get {
                return ResourceManager.GetString("Notes", resourceCulture);
            }
        }
        
        public static string NoteCreate {
            get {
                return ResourceManager.GetString("NoteCreate", resourceCulture);
            }
        }
        
        public static string NoCameraTitle {
            get {
                return ResourceManager.GetString("NoCameraTitle", resourceCulture);
            }
        }
        
        public static string NoCameraContent {
            get {
                return ResourceManager.GetString("NoCameraContent", resourceCulture);
            }
        }
        
        public static string WaitForDownloadTitle {
            get {
                return ResourceManager.GetString("WaitForDownloadTitle", resourceCulture);
            }
        }
        
        public static string WaitForDownloadContent {
            get {
                return ResourceManager.GetString("WaitForDownloadContent", resourceCulture);
            }
        }
        
        public static string Logout {
            get {
                return ResourceManager.GetString("Logout", resourceCulture);
            }
        }
    }
}
