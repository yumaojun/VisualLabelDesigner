﻿using ICSharpCode.SharpDevelop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YProgramStudio.LabelsDesigner.Views
{
	/// <summary>
	/// LabelReference.xaml 的交互逻辑
	/// </summary>
	public partial class LabelReference : Window
	{
		public LabelReference()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			SD.PropertyService.Set("LabelProperties.Units", "MM");
			SD.PropertyService.Save();
			Close();
		}
	}
}
