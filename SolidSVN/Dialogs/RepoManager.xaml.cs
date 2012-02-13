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

namespace SolidSVN.Dialogs
{
    /// <summary>
    /// Repo Manager Window Dialog 
    /// 
    /// The point of this class is to provide a window 
    /// that will pop up and allow the user to add new 
    /// repos that will be visible from the RepositoryView 
    /// Dialog. 
    /// 
    /// This dialog will use a configuration file that contains 
    /// information about the settings for all the repositories 
    /// that have been configured. The following information 
    /// should be included for each of the repos: 
    ///     Repo Name - Identifier for the repo
    ///     Repo URL - Example: "svn+ssh://example.org/repo/trunk"
    ///     User Name -
    ///     Password - Ideally, the user would use Pageant with keys 
    ///         to access the repository, but we can allow the user 
    ///         to also use password. To do this we will need to 
    ///         encrypt the password string. See:
    ///         http://www.codeproject.com/Articles/15750/Simple-Password-Manager-Using-System-Security
    ///         for more information about how we can do this in a secure manner. 
    ///         If the password is blank we will assume that pageant is used. 
    ///     Working Directory - Example: "C:\working"
    /// 
    /// The idea for this window is to provide a listview on the left side that 
    /// shows the repositories by name. On the right side would be a datagrid that 
    /// holds the parameters for a repository. 
    /// Three buttons at the bottom would allow the user to add, edit, or remove listed 
    /// repositories. An added repository would not be updated/checked out until 
    /// the user selects it in the RepositoryView Task Pane. 
    /// The edit button will launch a new dialog that 
    /// 
    /// The configuration file for the repositories should be kept as a 
    /// list of serializable objects in XML format. 
    /// http://msdn.microsoft.com/en-us/library/ms973893.aspx
    /// See the Repository Class. 
    /// </summary>
    public partial class RepoManager : Window
    {
        public RepoManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Callback to add a new repository to the list of repositories. The 
        /// new repository would be populated with dummy information. 
        /// The name of the repository should be "New Repo x" where x 
        /// is replaced with a unique number. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void on_add_repo(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Callback to edit the properties of the selected repository.
        /// This should launch a dialog box that allows the user to enter 
        /// and change information about the repository. When the user clicks 
        /// OK and returns the information, this information should be logged
        /// and shown on the properties data grid. Data checking must be 
        /// done in the dialog window. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void on_edit_repo(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Callback to remove the selected repo from the list of configured 
        /// repositories. Note that when removing a repository from the 
        /// list of repos, this will not delete any of the locally created 
        /// working directories. This is up to the User to remove these directories 
        /// as necessary. This is so that no data is lost. 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void on_remove_repo(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Callback to save all information and close this dialog. 
        /// This will take all of the changed repo info and save it to the 
        /// appropriate configuration file. It will then update the local
        /// information in the Repository View so that newly added repositories 
        /// show. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void on_save_and_close(object sender, RoutedEventArgs e)
        {

        }

    }
}
