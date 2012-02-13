using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SolidSVN
{
    /// <summary>
    /// File Entity description used to access and record 
    /// information about the files that we intend to commit
    /// </summary>
    public class FileEntity
    {
        // Indicates that the file should be commited 
        public bool Commit { get; set; }
        // Indicates that the lock on the file should 
        // be released. 
        public bool ReleaseLock { get; set; }

        // Path to the file in question 
        public string FilePath { get; set; }
        // Part number of the file in question 
        public string PartNumber { get; set; }
        // Revision string for this file. 
        public string Revision { get; set; }
        // Description of the file. 
        public string Description { get; set; }

        private readonly ObservableCollection<FileEntity> _children = new ObservableCollection<FileEntity>();
        public ObservableCollection<FileEntity> Children
        {
            get { return _children; }
        }

        public int Id { get; set; }
        static int i;
        public FileEntity()
        {
            this.Id = ++FileEntity.i;
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }

    /// <summary>
    /// Model for the FileEntity class that implements 
    /// the ITreeModel for adding FileEntities to a 
    /// TreeList View.
    /// </summary>
    public class FileEntityModel : ITreeModel
    {
        public FileEntity Root { get; private set; }

        public FileEntityModel()
        {
            this.Root = new FileEntity();
        }

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            if (parent == null)
                parent = this.Root;
            return (parent as FileEntity).Children;
        }

        public bool HasChildren(object parent)
        {
            return (parent as FileEntity).Children.Count > 0;
        }

    } 
}
