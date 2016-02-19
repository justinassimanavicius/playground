using System.Reflection;
using System.ServiceModel;
using Autofac;
using ErrorHandlingPOC.Web.Facades;
using Module = Autofac.Module;
using Autofac.Extras.DynamicProxy2;

namespace ErrorHandlingPOC.Web
{
	public class DefaultAutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			var dataAccess = Assembly.GetExecutingAssembly();

			var baseFacade = typeof (BaseFacade);

			builder.Register(c => new FacadeInterceptor());

			builder.RegisterAssemblyTypes(dataAccess)
				.Where(t => baseFacade.IsAssignableFrom(t) && !t.IsAbstract)
				.EnableClassInterceptors()
				//.InterceptTransparentProxy()
				//.InterceptedBy(typeof(FacadeInterceptor))
				//.EnableInterfaceInterceptors()
				.InterceptedBy(typeof(FacadeInterceptor));
				//.AsSelf();
		}
	}
}