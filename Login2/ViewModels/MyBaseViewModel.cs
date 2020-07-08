using GalaSoft.MvvmLight;
using Login2.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Login2.ViewModels
{
    public class MyBaseViewModel: ViewModelBase
	{
		/// <summary>
		/// This gives us the ReSharper option to transform an autoproperty into a property with change notification
		/// Also leverages .net 4.5 callermembername attribute
		/// </summary>
		/// <param name="property">name of the property</param>
		[NotifyPropertyChangedInvocator]
		protected override void RaisePropertyChanged([CallerMemberName]string property = "")
		{
			base.RaisePropertyChanged(property);
		}

		/// <summary>
		/// This gives us the ReSharper option to transform an autoproperty into a property with change notification
		/// Also leverages .net 4.5 callermembername attribute
		/// </summary>
		/// <param name="property">name of the property</param>
		[NotifyPropertyChangedInvocator]
		protected override void RaisePropertyChanging([CallerMemberName]string property = "")
		{
			base.RaisePropertyChanging(property);
		}
	}
}
