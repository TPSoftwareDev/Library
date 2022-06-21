using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Teleperformance
{
	public static class ServiceProviderHelper
	{
		#region Static Method(s) 

		public static T ConstructWithServiceDependancies<T>(this IServiceProvider serviceProvider)
			where T : class
		{
			return (T)serviceProvider.ConstructWithServiceDependancies(typeof(T));
		}

		public static object ConstructWithServiceDependancies(this IServiceProvider serviceProvider, Type objectType)
		{
			ConstructorInfo constructorToUse = null;

			foreach (ConstructorInfo constructorInfo in objectType.GetConstructors())
			{
				if (null == constructorToUse)
				{
					constructorToUse = constructorInfo;
				}
				else if (constructorToUse.GetParameters().Length < constructorInfo.GetParameters().Length)
				{
					constructorToUse = constructorInfo;
				}
			}

			if (null == constructorToUse)
			{
				throw new MissingMethodException();
			}

			return serviceProvider.ConstructWithServiceDependancies(constructorToUse);
		}

		public static object ConstructWithServiceDependancies(this IServiceProvider serviceProvider, ConstructorInfo objectConstructor)
		{
			object @object = null;
			ParameterInfo[] parameterInfos = objectConstructor.GetParameters();

			if (parameterInfos.Length > 0)
			{
				List<object> parameterValues = new List<object>();

				foreach (ParameterInfo parameterInfo in objectConstructor.GetParameters())
				{
					parameterValues.Add(serviceProvider.GetService(parameterInfo.ParameterType, false));
				}

				@object = objectConstructor.Invoke(parameterValues.ToArray());
			}
			else
			{
				@object = objectConstructor.Invoke(null);
			}

			return @object;
		}

		public static TService GetService<TService>(this IServiceProvider serviceProvider, bool throwIfMissing = false)
			where TService : class
		{
			return serviceProvider.GetService(typeof(TService), throwIfMissing) as TService;
		}

		public static object GetService(this IServiceProvider serviceProvider, Type serviceType, bool throwIfMissing = false)
		{
			object serviceInstance = null;

			if (null != serviceProvider)
			{
				serviceInstance = serviceProvider.GetService(serviceType);

				if (throwIfMissing && null == serviceInstance)
				{
					throw new KeyNotFoundException(string.Format("Unable to find the service '{0}'.", serviceType.AssemblyQualifiedName));
				}
			}

			return serviceInstance;
		}

		public static bool TryGetService<TService>(this IServiceProvider serviceProvider, out TService service)
			where TService : class
		{
			service = serviceProvider.GetService<TService>(false);

			return (null != service);
		}

		public static bool TryResolveServiceDependancy(this IServiceProvider serviceProvider, object @objectInstace, string memberName, Type serviceType = null, bool throwIfMissing = false)
		{
			bool sucess = false;
			Type objectType = @objectInstace.GetType();
			PropertyInfo propertyInfo = objectType.GetProperty(memberName);

			if (null == propertyInfo)
			{
				FieldInfo fieldInfo = objectType.GetField(memberName);

				if (null != fieldInfo)
				{
					fieldInfo.SetValue(@objectInstace, serviceProvider.GetService(
						((null != serviceType) ? serviceType : fieldInfo.FieldType), throwIfMissing));

					sucess = true;
				}
			}
			else
			{
				propertyInfo.SetValue(@objectInstace, serviceProvider.GetService(
					((null != serviceType) ? serviceType : propertyInfo.PropertyType), throwIfMissing), null);

				sucess = true;
			}

			return sucess;
		}

		/* NOTE Justin Long: I tried really hard to get this signature to work:
		 *		serviceProvider.TryResolveServiceDependancy(() => myWorker.Service)
		 *	where it would find the instance of myWorker from the expression tree but I couldn't get it so you'll have to live with this:
		 *		serviceProvider.TryResolveServiceDependancy(myWorker, () => myWorker.Service)
		 *	(For now....)
		 */
		public static bool TryResolveServiceDependancy<T>(this IServiceProvider serviceProvider, object @objectInstace, Expression<Func<T>> expression, bool throwIfMissing = false)
			where T : class
		{
			bool sucess = false;

			if (null != expression)
			{
				throw new ArgumentNullException("expression");
			}

			MemberExpression memberExpression = expression.Body as MemberExpression;

			if (null == memberExpression)
			{
				throw new ArgumentException();
			}

			T serviceInstance = serviceProvider.GetService<T>(throwIfMissing);

			PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;

			if (null == propertyInfo)
			{
				FieldInfo fieldInfo = memberExpression.Member as FieldInfo;

				if (null != fieldInfo)
				{
					fieldInfo.SetValue(@objectInstace, serviceInstance);

					sucess = true;
				}
			}
			else
			{
				propertyInfo.SetValue(@objectInstace, serviceInstance, null);

				sucess = true;
			}

			return sucess;
		}

		public static bool TryUseService<TService>(this IServiceProvider serviceProvider, Action<TService> action)
			where TService : class
		{
			bool result = false;
			TService service = serviceProvider.GetService<TService>(false);

			result = (null != service);

			if (result)
			{
				action(service);
			}

			return result;
		}

		public static void UseService<TService>(this IServiceProvider serviceProvider, Action<TService> action)
			where TService : class
		{
			TService service = serviceProvider.GetService<TService>(true);

			action(service);
		}

		public static void UseService(this IServiceProvider serviceProvider, Type serviceType, Action<object> action)
		{
			object service = serviceProvider.GetService(serviceType, true);

			action(service);
		}

		#endregion Static Method(s) 
	}
}
