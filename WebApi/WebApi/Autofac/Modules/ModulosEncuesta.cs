using Autofac;
using CoreApi;

namespace WebApi.Autofac.Modules
{
    public class ModulosEncuesta : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TemaManager>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<UsuarioManager>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<PreguntaManager>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<RespuestaManager>().AsImplementedInterfaces().AsSelf();

            base.Load(builder);
        }
    }
}