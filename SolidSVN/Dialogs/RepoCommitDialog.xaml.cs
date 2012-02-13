using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// This window is used to allow the user to select which 
    /// files to commit and manage the process of committing files. 
    /// 
    /// </summary>
    public partial class RepoCommitDialog : Window
    {
        SvnClient client;
        long last_count;
        bool cancel_operation; 

        public RepoCommitDialog(SvnClient c, FileEntityModel files)
        {
            InitializeComponent();

            this.client = c;

            this.last_count = 0;
            this.cancel_operation = false; 

            this.commit_progress_bar.Value = 0;
            this.commit_progress_bar.Minimum = 0; 
            this.commit_progress_bar.Maximum = 100; 

            var model = files;
            this._treeList.Model = model;             
        }

        /// <summary>
        /// Callback that gets invoked when the user pushes the "Commit"
        /// button. Once we have collected all the files that we will be 
        /// committed, we register event handlers that will allow us to 
        /// monitor the progress of the commit. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void on_commit(object sender, RoutedEventArgs e)
        {
            // We will inspect the model to determine which files we 
            // want to commit 
            List<string> files_to_commit = new List<string>();
            foreach (FileEntityModel fe in this._treeList.Model.GetChildren(null))
            {
                if (fe.Root.Commit)
                {
                    files_to_commit.Add(fe.Root.FilePath);
                } 
            } 

            if ( files_to_commit.Count > 0 )
            { 
                // Start a commit operation 
                this.client.Commit((files_to_commit as ICollection<string>));

                this.client.Progress += new EventHandler<SvnProgressEventArgs>(client_Progress);
                this.client.Cancel += new EventHandler<SvnCancelEventArgs>(client_Cancel);
                this.client.SvnError += new EventHandler<SvnErrorEventArgs>(client_SvnError);

            } 
            else 
            { 
                // We don't have any files to actually commit 
                this.Close(); 
            }

        }

        /// <summary>
        /// Remove event handlers
        /// </summary>
        void cleanup_event_handlers()
        {
            this.client.Progress -= new EventHandler<SvnProgressEventArgs>(client_Progress);
            this.client.Cancel -= new EventHandler<SvnCancelEventArgs>(client_Cancel);
            this.client.SvnError -= new EventHandler<SvnErrorEventArgs>(client_SvnError); 
        } 

        /// <summary>
        /// Event Handler for the Subversion Error Event. This is 
        /// called when there is a problem with the transfer of 
        /// data to the remote repository 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_SvnError(object sender, SvnErrorEventArgs e)
        {
            e.Cancel = true;

            // Use the e.Exception parameter to show error message.
            
            this.cleanup_event_handlers();

            // Change color of the progress bar 
            this.commit_progress_bar.Foreground = new LinearGradientBrush(Colors.Red, Colors.DarkRed, 45.0); 

            // Launch Error Dialog 
            SvnErrorDialog error = new SvnErrorDialog();
            error.Show();

        }


        // Delegate function for setting the 
        // progress bar on the UI thread. 
        private delegate void UpdateProgressBarDelegate(
            DependencyProperty dp, Object value);

        /// <summary>
        /// Callback from the SVN Progress Event that allows 
        /// us to track the total progress made for the commit. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_Progress(object sender, SvnProgressEventArgs e)
        {
            if (e.Progress == e.TotalProgress)
            {
                this.cleanup_event_handlers();
                this.Close();
                return;
            }

            double value = (e.Progress / (double)(e.TotalProgress));
            value *= 100;

            this.last_count = e.Progress;
            // Invoke the update to the ProgressBar on 
            // the UI Thread
            UpdateProgressBarDelegate updatedel = new UpdateProgressBarDelegate(this.commit_progress_bar.SetValue);
            Dispatcher.Invoke(
                updatedel,
                System.Windows.Threading.DispatcherPriority.Background,
                new object[] { ProgressBar.ValueProperty, value }
                ); 
            
        } 

        private void on_cancel(object sender, RoutedEventArgs e)
        {
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
