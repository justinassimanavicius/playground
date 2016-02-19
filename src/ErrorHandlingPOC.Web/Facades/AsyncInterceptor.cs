using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace ErrorHandlingPOC.Web.Facades
{
	public abstract class AsyncExceptionHandlingInterceptor : IInterceptor
	{
		public void Intercept(IInvocation invocation)
		{
			throw new NotImplementedException();
		}
	}
}
