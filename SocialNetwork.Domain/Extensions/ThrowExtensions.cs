﻿using SocialNetwork.Domain.Exceptions;
using System;

namespace SocialNetwork.Domain.Extensions
{
    public interface IThrow
    {
    }

    public class Throw : IThrow
    {
        public static IThrow Exception { get; } = new Throw();
        private Throw() { }
    }

    public static class ThrowExtensions
    {
        public static void IfNull<T>(this IThrow validatR, T value, string propertyName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(propertyName);
            }
        }
        public static void IfNull<T>(this IThrow validatR, T value, string propertyName, string message)
        {
            if (value == null)
            {
                throw new ArgumentException($"{propertyName} is NULL. {message}");
            }
        }
        public static void IfNotNull<T>(this IThrow validatR, T value, string message)
        {
            if (value != null)
            {
                throw new ArgumentException(message);
            }
        }
        public static void IfNullOrWhiteSpace(this IThrow validatR, string value, string propertyName)
        {
            Throw.Exception.IfNull(value, propertyName);
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"Paramater {propertyName} cannot be empty.");
            }
        }
        public static void IfEntityNotFound<T>(this IThrow validatR, int entityId, T value, string entityName)
        {
            if (value == null)
            {
                throw new EntityNotFoundException($"{entityName} Entity with Id {entityId} not found.");
            }
        }
        public static void IfNotEqual<T>(this IThrow validatR, int valueOne, int valueTwo, string property)
        {
            if (valueOne != valueTwo)
            {
                throw new ArgumentException($"Supplied {property} Values are not equal.");
            }
        }
        public static void IfFalse(this IThrow validatR, bool value, string message)
        {
            if (value == false)
            {
                throw new ArgumentException(message);
            }
        }
        public static void IfTrue(this IThrow validatR, bool value, string message)
        {
            if (value == true)
            {
                throw new ArgumentException(message);
            }
        }
    }
}