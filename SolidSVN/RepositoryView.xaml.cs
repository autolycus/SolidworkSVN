using System;
using System.IO; 
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using SharpSvn; 


namespace SolidSVN
{
    /// <summary>
    /// Main View for Viewing Repositories
    /// 
    /// This user control would be embedded into the 
    /// task pane of the Solidworks application. It would 
    /// provide a means that the user can interact with repositories
    /// and inspect their files and properties. 
    /// 
    /// 
    /// </summary>
    public partial class RepositoryView : UserControl
    {        
        private SvnClient client; 
        public SvnClient Client        
        { 
            get { return this.client; } 
        } 

        /// <summary>
        /// Create a new repository view 
        /// </summary>
        /// <param name="repo"> URI of the repo we want to work with. </param>
        /// <param name="working_dir"></param>
        public RepositoryView(Uri repo, string working_dir)
        {

            InitializeComponent();

            // Setup the repository.             
            this.client = new SvnClient(); 
            SvnUpdateResult res; 
            SvnUriTarget repo_target = new SvnUriTarget(repo.ToString());
            this.client.CheckOut(repo_target, working_dir, out res); 

        }
        
        private void on_update_repository(object sender, RoutedEventArgs e)
        {
            // Launch the RepuUpdateView 
            RepoUpdateView updater = new RepoUpdateView(this.client);
            updater.Show(); 
        }

        private void on_find_in_assy(object sender, RoutedEventArgs e)
        {
            

        }

        private void on_commit_all(object sender, RoutedEventArgs e)
        {



        }
    }
}
