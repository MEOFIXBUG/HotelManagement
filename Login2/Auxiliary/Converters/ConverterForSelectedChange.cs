using Login2.Auxiliary.DomainObjects;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Login2.Auxiliary.Converters
{
    class ConverterForSelectedChange : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var parametersForSwitchView = new ParametersForSwitchView();
            parametersForSwitchView.transitioningContentSlide = (TransitioningContent)values[0];
            parametersForSwitchView.gridCursor = (Grid)values[1];
            parametersForSwitchView.selectedIndex = (int)values[2];
            return parametersForSwitchView;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
