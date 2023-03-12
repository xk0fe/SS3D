﻿using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using SS3D.Logging;
using UnityEngine;
using Object = System.Object;

namespace SS3D.Core
{
    /// <summary>
    /// Class used to get game systems, using generics and then making cache of said systems.
    /// </summary>
    public static class Subsystems
    {
        /// <summary>
        /// A dictionary containing all the objects that registered themselves.
        /// </summary>
        private static readonly Dictionary<Type, object> RegisteredSubsystems = new();

        /// <summary>
        /// Registers a system in the dictionary so we don't have to use find object of type.
        /// </summary>
        /// <param name="system">The object to be stored.</param>
        public static void Register([NotNull] MonoBehaviour system)
        {
            Type type = system.GetType();

            if (!RegisteredSubsystems.TryGetValue(type, out object _))
            {
                RegisteredSubsystems.Add(type, system);
            }
        }

        /// <summary>
        /// Gets any system at runtime, make sure there's no duplicates of said system before using.
        /// </summary>
        /// <typeparam name="T">The Type of object you want to get.</typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : MonoBehaviour
        {
            if (RegisteredSubsystems.TryGetValue(typeof(T), out object match))
            {
                return (T)match;
            }

            Object subsystem = UnityEngine.Object.FindObjectOfType(typeof(T), true);

            if (subsystem != null)
            {
                Register((MonoBehaviour)subsystem);
                return (T)subsystem;
            }

            string message = $"Couldn't find subsystem of {typeof(T).Name} in the scene";

            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            Punpun.Error(typeof(Subsystems), message, Logs.Important);

            return null;
        }

        /// <summary>
        /// Unregister the system from the dictionary. 
        /// </summary>
        /// <param name="system">The system to unregister.</param>
        public static void Unregister([NotNull] object system)
        {
            RegisteredSubsystems.Remove(system.GetType());
        }
    }
}