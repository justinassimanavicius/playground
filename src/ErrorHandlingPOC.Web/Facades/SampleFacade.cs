using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceReference1;

namespace ErrorHandlingPOC.Web.Facades
{
    public class SampleFacade : BaseFacade
	{
	    public virtual async Task<Response<string>> GetSomeString()
	    {
		    var service = new Service1Client();

		    var someString = await service.GetDataAsync(0);

			throw new Exception("test2");

		    return new Response<string>(someString);
	    }

		public virtual Response<string> Fail()
		{
			throw new Exception("test");
		}
	}

	public abstract class BaseFacade
	{
		
	}

	public class Response
	{
		
		private readonly Lazy<Dictionary<string, string>> _errors = new Lazy<Dictionary<string, string>>();
		/// <summary>
		/// The errors for ModelState
		/// </summary>
		public IDictionary<string, string> Errors => _errors.Value;


		/// <summary>
		/// If true, then FactoryResponse.Errors contains values.
		/// </summary>
		/// <value>
		///   <c>true</c> if [is valid]; otherwise, <c>false</c>.
		/// </value>
		public bool IsValid
		{
			get
			{
				if (!_errors.IsValueCreated)
					return true;

				return _errors.Value.Count == 0;
			}
		}


		public Response AddError(string error)
		{
			Errors.Add(Guid.NewGuid().ToString(), error);
			return this;
		}



		public Response AddError(string key, string error)
		{
			Errors.Add(key, error);
			return this;
		}


	}

	/// <summary>
	/// DTO class to move both errors and result in one single object
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class Response<T> : Response
	{

		/// <summary>
		/// Payload
		/// </summary>
		public T Result;

		public Response(T result)
		{
			Result = result;
		}

		public Response() {}
		/// <summary>
		/// Add errors from another Response.
		/// <para>The second Response can be null or valid.</para>
		/// </summary>
		public Response<T> AddErrors<TC>(Response<TC> response)
		{
			if (response == null || response.IsValid)
				return this;

			return AddErrors(response.Errors);
		}



		public Response<T> AddErrors(IDictionary<string, string> errors)
		{
			if (errors == null)
				return this;

			foreach (var error in errors)
			{
				Errors.Add(error.Key, error.Value);
			}
			return this;
		}
	}
}
