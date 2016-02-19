using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace ErrorHandlingPOC.Web.Facades
{
    public class FacadeInterceptor : IInterceptor
	{

	    private static readonly Type ResponseType = typeof (Response);
		


		public void Intercept(IInvocation invocation)
	    {
		    try
		    {
			    invocation.Proceed();
		    }
		    catch (Exception ex)
		    {
			    HandleException(invocation, ex);
			    return;
		    }
			var taskType = invocation.Method.ReturnType;
			
			

			var task = (invocation.ReturnValue as Task);
			
			if (task != null && task.GetType().IsGenericType)
		    {
			    
				invocation.ReturnValue = invocation.ReturnValue.GetType()
				    .GetMethod("ContinueWith")
				    .Invoke(invocation.ReturnValue, new object[] { new Func<Task, Object>(ttt) });


			    Task.FromResult(1).ContinueWith(x => x.Result);

			    // invocation.ReturnValue = task1.ContinueWith(t => ttt(t));
		    }
	    }

	    private static object ttt(Task t)
	    {
			if (t.IsFaulted)
			{

				var resultField = t.GetType().GetProperty("Result");
				var returnType = resultField.PropertyType;
				var returnValue = Activator.CreateInstance(returnType) as Response;
				returnValue.AddError(t.Exception.Message);
				return returnValue;
			}
		    return null;
	    }

	    private static void HandleException(IInvocation invocation, Exception ex)
	    {
		    var returnType = invocation.Method.ReturnType;
		    if (ResponseType.IsAssignableFrom(returnType))
		    {
			    var returnValue = Activator.CreateInstance(returnType) as Response;
			    returnValue.AddError(ex.Message);
			    invocation.ReturnValue = returnValue;
		    }
	    }
    }
}
