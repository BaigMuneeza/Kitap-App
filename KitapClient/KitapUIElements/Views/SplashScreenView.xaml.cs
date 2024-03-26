using KitapUIElements.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace KitapUIElements.Views
{
	public partial class SplashScreenView : Page
	{
		private BackgroundWorker backgroundWorker;
		public SplashScreenView()
		{
			InitializeComponent();
			backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += SplashScreen_DoWork;
			backgroundWorker.ProgressChanged += ProgressBar_ProgressChanged;
			backgroundWorker.RunWorkerCompleted += ProgressBar_RunWorkerCompleted;
			backgroundWorker.WorkerReportsProgress = true;


			backgroundWorker.RunWorkerAsync();
		}

		private void SplashScreen_DoWork(object sender, DoWorkEventArgs e)
		{

			for (int i = 0; i <= 100; i++)
			{
				Thread.Sleep(50);
				backgroundWorker.ReportProgress(i);
			}
		}

		private void ProgressBar_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			Dispatcher.Invoke(() =>
			{
				progressBar.Value = e.ProgressPercentage;
			});
		}

		private void ProgressBar_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Dispatcher.Invoke(() =>
			{
				GUIHandler.Instance.SetView<LoginView>(Application.Current.MainWindow);
			});
		}

	}
}
