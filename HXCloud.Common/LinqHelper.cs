using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace HXCloud.Common
{
    public class LinqHelper<T>
    {
        public static Expression<Func<T, bool>> True()
        {
            return f => true;
        }

        public static Expression<Func<T, bool>> False()
        {
            return f => false;
        }
        /// <summary>
        /// lambda表达式:t=>t.propName.Contains(propValue)
        /// 多用于where条件
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="propName">属性名称</param>
        /// <param name="propValue">属性值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Contains(string propName, string propValue)
        {
            // 创建节点参数t
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
            // 创建一个成员(字段/属性)
            MemberExpression member = Expression.PropertyOrField(parameter, propName);
            // 创建一个常数
            ConstantExpression constant = Expression.Constant(propValue, typeof(string));
            // 创建一个方法
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            // 创建一个带参数方法Expression
            MethodCallExpression methodCall = Expression.Call(member, method, constant);
            // 生成lambda表达式
            return Expression.Lambda<Func<T, bool>>(methodCall, parameter);
        }
        /// <summary>
        /// lambda表达式:t=>t.propName
        /// 多用于order排序
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <typeparam name="TKey">返回类型</typeparam>
        /// <param name="propName">属性名</param>
        /// <returns></returns>
        public static Expression<Func<T, TKey>> Order<TKey>(string propName)
        {
            // 创建节点参数t
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
            // 创建一个属性
            MemberExpression property = Expression.Property(parameter, propName);
            // 生成lambda表达式
            return Expression.Lambda<Func<T, TKey>>(property, parameter);
        }

        /// <summary>
        /// lambda表达式:t=>t.propName==propValue
        /// 多用于where条件
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="propName">属性名称</param>
        /// <param name="propValue">属性值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Equal(string propName, object propValue)
        {
            // 创建节点参数t
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
            // 创建一个成员(字段/属性)
            MemberExpression member = Expression.PropertyOrField(parameter, propName);
            // 创建一个常数
            ConstantExpression constant = Expression.Constant(propValue);
            // 创建一个相等比较Expression
            BinaryExpression binary = Expression.Equal(member, constant);
            // 生成lambda表达式
            return Expression.Lambda<Func<T, bool>>(binary, parameter);
        }

        /// <summary>
        /// lambda表达式:t=>t.propName!=propValue
        /// 多用于where条件
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="propName">属性名称</param>
        /// <param name="propValue">属性值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> NotEqual(string propName, object propValue)
        {
            // 创建节点参数t
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
            // 创建一个成员(字段/属性)
            MemberExpression member = Expression.PropertyOrField(parameter, propName);
            // 创建一个常数
            ConstantExpression constant = Expression.Constant(propValue);
            // 创建一个不相等比较Expression
            BinaryExpression binary = Expression.NotEqual(member, constant);
            // 生成lambda表达式
            return Expression.Lambda<Func<T, bool>>(binary, parameter);
        }

        /// <summary>
        /// lambda表达式:t=>t.propName&lt;propValue
        /// 多用于where条件
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="propName">属性名称</param>
        /// <param name="propValue">属性值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> LessThan(string propName, object propValue)
        {
            // 创建节点参数t
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
            // 创建一个成员(字段/属性)
            MemberExpression member = Expression.PropertyOrField(parameter, propName);
            // 创建一个常数
            ConstantExpression constant = Expression.Constant(propValue);
            // 创建一个不相等比较Expression
            BinaryExpression binary = Expression.LessThan(member, constant);
            // 生成lambda表达式
            return Expression.Lambda<Func<T, bool>>(binary, parameter);
        }

        /// <summary>
        /// lambda表达式:t=>t.propName&lt;=propValue
        /// 多用于where条件
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="propName">属性名称</param>
        /// <param name="propValue">属性值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> LessThanOrEqual(string propName, object propValue)
        {
            // 创建节点参数t
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
            // 创建一个成员(字段/属性)
            MemberExpression member = Expression.PropertyOrField(parameter, propName);
            // 创建一个常数
            ConstantExpression constant = Expression.Constant(propValue);
            // 创建一个不相等比较Expression
            BinaryExpression binary = Expression.LessThanOrEqual(member, constant);
            // 生成lambda表达式
            return Expression.Lambda<Func<T, bool>>(binary, parameter);
        }

        /// <summary>
        /// lambda表达式:t=>t.propName>propValue
        /// 多用于where条件
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="propName">属性名称</param>
        /// <param name="propValue">属性值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GreaterThan(string propName, object propValue)
        {
            // 创建节点参数t
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
            // 创建一个成员(字段/属性)
            MemberExpression member = Expression.PropertyOrField(parameter, propName);
            // 创建一个常数
            ConstantExpression constant = Expression.Constant(propValue);
            // 创建一个不相等比较Expression
            BinaryExpression binary = Expression.GreaterThan(member, constant);
            // 生成lambda表达式
            return Expression.Lambda<Func<T, bool>>(binary, parameter);
        }

        /// <summary>
        /// lambda表达式:t=>t.propName>=propValue
        /// 多用于where条件
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="propName">属性名称</param>
        /// <param name="propValue">属性值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GreaterThanOrEqual(string propName, object propValue)
        {
            // 创建节点参数t
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
            // 创建一个成员(字段/属性)
            MemberExpression member = Expression.PropertyOrField(parameter, propName);
            // 创建一个常数
            ConstantExpression constant = Expression.Constant(propValue);
            // 创建一个不相等比较Expression
            BinaryExpression binary = Expression.GreaterThanOrEqual(member, constant);
            // 生成lambda表达式
            return Expression.Lambda<Func<T, bool>>(binary, parameter);
        }

        /// <summary>
        /// lambda表达式:t=>{t.contains(propvalue1) ||...||t.contains(propvalueN)}
        /// 多用于where条件
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="propName">属性名称</param>
        /// <param name="propValues">属性值数组</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> In(string propName, string[] propValues)
        {
            // 创建节点参数t
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t"); // left
            // 创建一个成员(字段/属性)
            MemberExpression member = Expression.PropertyOrField(parameter, propName);
            // 创建一个常数
            Expression constant = Expression.Constant(false);
            // 创建一个方法
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            foreach (string item in propValues)
            {
                // 创建一个带参数方法Expression
                MethodCallExpression methodCall = Expression.Call(member, method, Expression.Constant(item)); // right
                // 连接参数方法
                constant = Expression.Or(methodCall, constant);
            }

            // 生成lambda表达式
            return Expression.Lambda<Func<T, bool>>(constant, new ParameterExpression[] { parameter });
        }

        /// <summary>
        /// lambda表达式:t=>{!(t.contains(propvalue1) ||...||t.contains(propvalueN))}
        /// 多用于where条件
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="propName">属性名称</param>
        /// <param name="propValues">属性值数组</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> NotIn(string propName, string[] propValues)
        {
            // 创建节点参数t
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
            // 创建一个成员(字段/属性)
            MemberExpression member = Expression.PropertyOrField(parameter, propName);
            // 创建一个常数
            Expression constant = Expression.Constant(false);
            // 创建一个方法
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            foreach (string item in propValues)
            {
                // 创建一个带参数方法Expression
                MethodCallExpression methodCall = Expression.Call(member, method, Expression.Constant(item)); // right
                // 连接参数方法
                constant = Expression.Or(methodCall, constant);
            }

            // 生成lambda表达式
            return Expression.Lambda<Func<T, bool>>(Expression.Not(constant), new ParameterExpression[] { parameter });
        }
        /// <summary>
        /// lambda表达式:t=>t.propName.Contains(propValue)
        /// 多用于where条件
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="propName">属性名称</param>
        /// <param name="propValue">属性值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> StartWith(string propName, string propValue)
        {
            // 创建节点参数t
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
            // 创建一个成员(字段/属性)
            MemberExpression member = Expression.PropertyOrField(parameter, propName);
            // 创建一个常数
            ConstantExpression constant = Expression.Constant(propValue, typeof(string));
            // 创建一个方法
            MethodInfo method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            // 创建一个带参数方法Expression
            MethodCallExpression methodCall = Expression.Call(member, method, constant);
            // 生成lambda表达式
            return Expression.Lambda<Func<T, bool>>(methodCall, parameter);
        }

        /// <summary>
        /// lambda表达式:t=>t.propName.Contains(propValue)
        /// 多用于where条件
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="propName">属性名称</param>
        /// <param name="propValue">属性值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> EndsWith(string propName, string propValue)
        {
            // 创建节点参数t
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
            // 创建一个成员(字段/属性)
            MemberExpression member = Expression.PropertyOrField(parameter, propName);
            // 创建一个常数
            ConstantExpression constant = Expression.Constant(propValue, typeof(string));
            // 创建一个方法
            MethodInfo method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            // 创建一个带参数方法Expression
            MethodCallExpression methodCall = Expression.Call(member, method, constant);
            // 生成lambda表达式
            return Expression.Lambda<Func<T, bool>>(methodCall, parameter);
        }

        /// <summary>
        /// lambda表达式:!(t=>t.propName.Contains(propValue))
        /// 多用于where条件
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="propName">属性名称</param>
        /// <param name="propValue">属性值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> NotContains(string propName, string propValue)
        {
            // 创建节点参数t
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
            // 创建一个成员(字段/属性)
            MemberExpression member = Expression.PropertyOrField(parameter, propName);
            // 创建一个常数
            ConstantExpression constant = Expression.Constant(propValue, typeof(string));
            // 创建一个方法
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            // 创建一个带参数方法Expression
            MethodCallExpression methodCall = Expression.Call(member, method, constant);
            // 生成lambda表达式
            return Expression.Lambda<Func<T, bool>>(Expression.Not(methodCall), parameter);
        }

        /// <summary>
        /// lambda表达式:t=>{left and right}
        /// 多用于where条件
        /// </summary>
        /// <param name="left">左侧条件</param>
        /// <param name="right">右侧条件</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            // 创建参数表达式
            InvocationExpression invocation = Expression.Invoke(right, left.Parameters.Cast<Expression>());
            // 创建and运算
            BinaryExpression binary = Expression.And(left.Body, invocation);
            // 生成lambda表达式
            return Expression.Lambda<Func<T, bool>>(binary, left.Parameters);
        }

        /// <summary>
        /// lambda表达式:t=>{left or right}
        /// 多用于where条件
        /// </summary>
        /// <param name="left">左侧条件</param>
        /// <param name="right">右侧条件</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            // 创建参数表达式
            InvocationExpression invocation = Expression.Invoke(right, left.Parameters.Cast<Expression>());
            // 创建or运算
            BinaryExpression binary = Expression.Or(left.Body, invocation);
            // 生成lambda表达式
            return Expression.Lambda<Func<T, bool>>(binary, left.Parameters);
        }
    }
}
