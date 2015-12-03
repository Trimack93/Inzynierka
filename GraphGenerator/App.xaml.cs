// Dialog Service implementation based on:
// http://www.codeproject.com/Articles/36745/Showing-Dialogs-When-Using-the-MVVM-Pattern?fid=1541292

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using GraphGenerator.Views;
using GraphGenerator.ViewModels;
using System.Reflection;
using MvvmDialogs;
using Autofac;

namespace GraphGenerator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IContainer container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            container = CreateContainer();

            var window = container.Resolve<MainWindow>();
            var viewModel = container.Resolve<MainViewModel>();

            window.DataContext = viewModel;
            window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            container.Dispose();
        }

        private static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            Func<System.ComponentModel.INotifyPropertyChanged, Type> typeLocator = (t) => GetViewClassTypeLocalizer(t);

            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsSelf();

            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AssignableTo<AddNodeViewModel>()
                .As<AddNodeViewModel>();

            builder
                .RegisterType<DialogService>()
                .AsImplementedInterfaces()
                .WithParameter("dialogTypeLocator", typeLocator)
                .SingleInstance();

            return builder.Build();
        }

        private static Type GetViewClassTypeLocalizer(System.ComponentModel.INotifyPropertyChanged viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException("Modal window ViewModel is null");

            Type viewModelType = viewModel.GetType();
            
            string typeName = viewModelType.Name;
            string viewTypeName = typeName.Replace("ViewModel", "View");

            string viewModelNamespace = viewModelType.FullName;
            string viewNamespace = viewModelNamespace.Replace("ViewModels." + typeName, "Views." + viewTypeName);

            Type viewType = Type.GetType(viewNamespace);

            if (viewType == null)
                throw new ArgumentNullException("Modal window view couldn't be found");

            return viewType;
        }
    }
}