using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using SharpSvn;

namespace SolidSVN
{
    /// <summary>
    /// This view is intended to give the user an
    /// indication of the status of the update operation. 
    /// When the svn client is used to update/checkout 
    /// files the repository, this will generally take a 
    /// significant amount of time. This window will 
    /// pop-up and tell the user how much progress has 
    /// been made while updating the repository.
    /// </summary>
    public partial class RepoUpdateView : Window    
    {
        private SvnClient client;

        private long last_count; 

        private bool cancel_operation; 

        public RepoUpdateView(SvnClient c)
        {
            this.InitializeComponent();

            this._progress_bar.Value = 0;
            this._progress_bar.Minimum = 0;
            this._progress_bar.Maximum = 100; 
            
            this.last_count = 0; 

            this.cancel_operation = false; 

            this.client = c;
            this.client.Progress += new EventHandler<SvnProgressEventArgs>(client_Progress);
            this.client.Cancel += new EventHandler<SvnCancelEventArgs>(client_Cancel);
        }

        // Delegate function for setting the 
        // progress bar on the UI thread. 
        private delegate void UpdateProgressBarDelegate(
            DependencyProperty dp, Object value);

        void client_Progress(object sender, SvnProgressEventArgs e)
        {
            // Convert the total progress into a value on the progress bar 
            // The event args has two values - Progress and TotalProgres. 
            // Progress is the numerator and TotalProgress is the denominator 

            if (e.Progress == e.TotalProgress)
            {
                // This means that we have completed the operation. 
                this.client.Progress -= new EventHandler<SvnProgressEventArgs>(client_Progress);
                this.Close();
                return; 
            }
            
            double value = (e.Progress / (double)(e.TotalProgress));             
            value *= 100;

            // Count of the latest number of bytes received. 
            this.last_count = e.Progress; 

            UpdateProgressBarDelegate updatedel = new UpdateProgressBarDelegate(this._progress_bar.SetValue);            
            Dispatcher.Invoke(
                updatedel, 
                System.Windows.Threading.DispatcherPriority.Background,
                new object[] { ProgressBar.ValueProperty, value }
                ); 
        }

        private void on_cancel(object sender, RoutedEventArgs e)
        {
            // This is intended to tell the client that we want to 
            // abort the update operation 
            this.cancel_operation = true; 
        }

        void client_Cancel(object sender, SvnCancelEventArgs e)
        {
            if (this.cancel_operation)
            {
                e.Cancel = true;
            }

        }




    }
}
