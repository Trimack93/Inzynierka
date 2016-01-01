using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Type GetViewClassTypeLocalizer(System.ComponentModel.INotifyPropertyChanged viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException("Modal window ViewModel is null");

            Type viewModelType = viewModel.GetType();

            string typeName = viewModelType.Name;
            string viewTypeName = typeName.Replace("ViewModel", "View");

            // As for now, custom graph mode has the same view as learning mode
            if (viewTypeName == "CustomGraphView")
                viewTypeName = "LearningView";

            string viewModelNamespace = viewModelType.FullName;
            string viewNamespace = viewModelNamespace.Replace("ViewModels." + typeName, "Views." + viewTypeName);

            Type viewType = Type.GetType(viewNamespace);

            if (viewType == null)
                throw new ArgumentNullException("Modal window view couldn't be found");

            return viewType;
        }
    }
}
