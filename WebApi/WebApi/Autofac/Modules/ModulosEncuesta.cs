using Autofac;
using CoreApi;

namespace WebApi.Autofac.Modules
{
    public class ModulosEncuesta : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TopicManager>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<ProfileManager>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<QuestionManager>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<AnswerManager>().AsImplementedInterfaces().AsSelf();

            base.Load(builder);
        }
    }
}