//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com
//All rights reserved.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Kooboo.IndexedDB.Helper
{
    public static class ObjectHelper
    {
        public static int GetHashCode(string fieldName)
        {
            if (fieldName == null)
            {
                fieldName = string.Empty;
            }
            return fieldName.ToLower().GetHashCode32();
        }

        /// <summary>
        /// Test if this is a dictionary...
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDictionary(Type type)
        {
            if (!type.IsGenericType)
            {
                return false;
            }

            return type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        public static bool IsList(Type type)
        {
            if (!type.IsGenericType)
            { return false; }

            var enumeGenericDefi = typeof(IList<>);

            foreach (var item in type.GetInterfaces())
            {
                if (item.IsGenericType && item.GetGenericTypeDefinition() == enumeGenericDefi)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsCollection(Type type)
        {
            if (!type.IsGenericType)
            { return false; }

            var enumeGenericDefi = typeof(ICollection<>);

            foreach (var item in type.GetInterfaces())
            {
                if (item.IsGenericType && item.GetGenericTypeDefinition() == enumeGenericDefi)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get the datatype within the eg LIST<T>
        /// </summary>
        /// <param name="enumberableType"></param>
        /// <returns></returns>
        public static Type GetEnumberableType(Type enumberableType)
        {
            return enumberableType.GetGenericArguments().Single();
        }

        public static Type GetDictionaryKeyType(Type dictionaryType)
        {
            return dictionaryType.GetGenericArguments()[0];
        }

        public static Type GetDictionaryValueType(Type dictionaryType)
        {
            return dictionaryType.GetGenericArguments()[1];
        }

        public static Type GetFieldType(Type objectType, string fieldName)
        {
            var property = objectType.GetProperty(fieldName);
            if (property != null)
            {
                return property.PropertyType;
            }

            var field = objectType.GetField(fieldName);
            if (field != null)
            {
                return field.FieldType;
            }
            return null;
        }

        public static Func<TValue, TFieldType> GetGetValue<TValue, TFieldType>(string fieldName)
        {
            ParameterExpression arg = Expression.Parameter(typeof(TValue));
            Expression expr = Expression.PropertyOrField(arg, fieldName);
            return Expression.Lambda<Func<TValue, TFieldType>>(expr, arg).Compile();
        }

        public static Action<TValue, TFieldType> GetSetValue<TValue, TFieldType>(string fieldName)
        {
            ParameterExpression arg = Expression.Parameter(typeof(TValue));
            Expression expr = Expression.PropertyOrField(arg, fieldName);
            var valueExp = Expression.Parameter(typeof(TFieldType));
            return Expression.Lambda<Action<TValue, TFieldType>>(Expression.Assign(expr, valueExp), arg, valueExp).Compile();
        }

        public static Action<TValue, object> GetSetObjectValue<TValue>(string fieldName, Type fieldtype)
        {
            ParameterExpression arg = Expression.Parameter(typeof(TValue));

            Expression expr = Expression.PropertyOrField(arg, fieldName);

            var objectpara = Expression.Parameter(typeof(object));

            var righttype = Expression.Convert(objectpara, fieldtype);

            var valueExp = Expression.Parameter(fieldtype);

            return Expression.Lambda<Action<TValue, object>>(Expression.Assign(expr, righttype), arg, objectpara).Compile();
        }

        public static Func<TValue, object> GetGetObjectValue<TValue>(string fieldName)
        {
            ParameterExpression arg = Expression.Parameter(typeof(TValue));
            Expression expr = Expression.PropertyOrField(arg, fieldName);
            return Expression.Lambda<Func<TValue, object>>(expr, arg).Compile();
        }

        public static Func<TValue, object> GetGetEnumObjectValue<TValue>(string fieldName)
        {
            ParameterExpression arg = Expression.Parameter(typeof(TValue));
            Expression expr = Expression.PropertyOrField(arg, fieldName);
            Expression convertobject = Expression.Convert(expr, typeof(object));
            return Expression.Lambda<Func<TValue, object>>(convertobject, arg).Compile();
        }

        public static Action<object, object> GetSetObjectValue(string fieldName, Type objectType, Type fieldtype)
        {
            var objectpara = Expression.Parameter(typeof(object));
            var fieldpara = Expression.Parameter(typeof(object));
            var rightObjectType = Expression.Convert(objectpara, objectType);
            Expression expr = Expression.PropertyOrField(rightObjectType, fieldName);
            var righttype = Expression.Convert(fieldpara, fieldtype);
            return Expression.Lambda<Action<object, object>>(Expression.Assign(expr, righttype), objectpara, fieldpara).Compile();
        }

        public static Func<object, object> GetGetObjectValue(string FieldName, Type objecttype)
        {
            ParameterExpression objectPara = Expression.Parameter(typeof(object));

            var expression = Expression.Convert(objectPara, objecttype);

            //  Expression expr = Expression.PropertyOrField(expression, FieldName);

            Expression expr;
            if (objecttype.GetField(FieldName) != null)
            {
                expr = Expression.Field(expression, FieldName);
            }
            else
            {
                if (objecttype.GetProperty(FieldName) != null)
                {
                    expr = Expression.Property(expression, FieldName);
                }
                else
                {
                    return null;
                }
            }

            Expression convertobject = Expression.Convert(expr, typeof(object));

            return Expression.Lambda<Func<object, object>>(convertobject, objectPara).Compile();
        }

        public static Action<object, TFieldType> GetSetFieldValue<TFieldType>(string fieldName, Type objectType)
        {
            var objectpara = Expression.Parameter(typeof(object));
            var fieldpara = Expression.Parameter(typeof(TFieldType));
            var rightObjectType = Expression.Convert(objectpara, objectType);
            Expression expr = Expression.PropertyOrField(rightObjectType, fieldName);
            var righttype = Expression.Convert(fieldpara, typeof(TFieldType));
            return Expression.Lambda<Action<object, TFieldType>>(Expression.Assign(expr, righttype), objectpara, fieldpara).Compile();
        }

        public static Func<object, TFieldType> GetGetFieldValue<TFieldType>(string fieldName, Type objecttype)
        {
            ParameterExpression objectPara = Expression.Parameter(typeof(object));
            var expression = Expression.Convert(objectPara, objecttype);
            Expression expr = Expression.PropertyOrField(expression, fieldName);

            return Expression.Lambda<Func<object, TFieldType>>(expr, objectPara).Compile();
        }
    }
}