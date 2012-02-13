using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

namespace SolidSVN
{
    /// <summary>
    /// Serializable Object class for holding information about 
    /// a Repository. Note that this is just a prototyping 
    /// implementation and will need to have secure handling of 
    /// passwords added if this implementation enters production. 
    /// </summary>
    [Serializable()]
    public class Repository : ISerializable
    {
        public string Name { get; set; } 
        public Uri Address{ get; set; }
        public string User { get; set; } 
        
        // This function need to be modified to use a
        // secure method of holding the password 
        private string password;
        public string Password
        {
            get
            {
                return (this.password);
            }
            set
            {
                this.password = value;
            }
        }

        public string WorkingDir { get; set; }

        public Repository(string name, Uri address, string user, string password, string working_dir)
        {
            if (name == null || address == null || user == null || password == null || working_dir == null)
            {
                throw new ArgumentNullException();
            } 

            this.Name = name;
            this.Address = address;
            this.User = user;
            this.password = password;
            this.WorkingDir = working_dir;
        }

        public Repository(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            this.Name = info.GetString("Name");
            string uri_temp = info.GetString("Address");
            this.Address = new Uri(uri_temp);
            this.User = info.GetString("User");
            this.password = info.GetString("Password");
            this.WorkingDir = info.GetString("WorkingDir"); 
        }

        protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", this.Name);
            info.AddValue("Address", this.Address.ToString());
            info.AddValue("User", this.User);
            info.AddValue("Password", this.password);
            info.AddValue("WorkingDir", this.WorkingDir); 
        }

        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            this.GetObjectData(info, context);
        }


    }
}
