﻿using Autofac;
using CoreApi;

namespace WebApi.Autofac.Modules
{
    public class ModulosEncuesta : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TopicManager>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<ProfileManager>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<SurveyManager>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<QuestionManager>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<AnswerManager>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<UserManager>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<CategoryManager>().AsImplementedInterfaces().AsSelf();

            base.Load(builder);
        }
    }
}