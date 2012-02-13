using System;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using SolidWorks.Interop.swconst;
using System.Windows.Forms;

using System.Windows.Forms.Integration;

namespace SolidSVN
{
    public class UserPMPage
    {
        //Local Objects
        IPropertyManagerPage2 swPropertyPage;
        PMPHandler handler;
        ISldWorks iSwApp;
        SwAddin userAddin;

        WPFControl MyWPFControl;
        ElementHost elhost;
        UserControl1 MyUserControl;
        Form1 MyWinFormControl;

        #region Property Manager Page Controls
        //Groups
        IPropertyManagerPageGroup group1;
        IPropertyManagerPageGroup group2;
        IPropertyManagerPageGroup group3;

        //Controls
        IPropertyManagerPageWindowFromHandle dotnet1;
        IPropertyManagerPageWindowFromHandle dotnet2;
        IPropertyManagerPageWindowFromHandle dotnet3;

        //Control IDs
        public const int group1ID = 0;
        public const int group2ID = 1;
        public const int group3ID = 2;

        public const int DotNet1ID = 3;
        public const int DotNet2ID = 4;
        public const int DotNet3ID = 5;

        #endregion

        public UserPMPage(SwAddin addin)
        {
            userAddin = addin;
            iSwApp = (ISldWorks)userAddin.SwApp;
            CreatePropertyManagerPage();
        }


        protected void CreatePropertyManagerPage()
        {
            int errors = -1;
            int options = (int)swPropertyManagerPageOptions_e.swPropertyManagerOptions_OkayButton |
                (int)swPropertyManagerPageOptions_e.swPropertyManagerOptions_CancelButton;

            handler = new PMPHandler(userAddin);
            swPropertyPage = (IPropertyManagerPage2)iSwApp.CreatePropertyManagerPage(".Net Control Disply PMP", options, handler, ref errors);
            if (swPropertyPage != null && errors == (int)swPropertyManagerPageStatus_e.swPropertyManagerPage_Okay)
            {
                try
                {
                    AddControls();
                }
                catch (Exception e)
                {
                    iSwApp.SendMsgToUser2(e.Message, 0, 0);
                }
            }
        }


        //Controls are displayed on the page top to bottom in the order 
        //in which they are added to the object.
        protected void AddControls()
        {
            short controlType = -1;
            short align = -1;
            int options = -1;


            //Add the groups
            options = (int)swAddGroupBoxOptions_e.swGroupBoxOptions_Expanded |
                      (int)swAddGroupBoxOptions_e.swGroupBoxOptions_Visible;

            group1 = (IPropertyManagerPageGroup)swPropertyPage.AddGroupBox(group1ID, "Window Form", options);

            options = (int)swAddGroupBoxOptions_e.swGroupBoxOptions_Checkbox |
                      (int)swAddGroupBoxOptions_e.swGroupBoxOptions_Visible;

            group2 = (IPropertyManagerPageGroup)swPropertyPage.AddGroupBox(group2ID, "User Control", options);

            options = (int)swAddGroupBoxOptions_e.swGroupBoxOptions_Checkbox |
                        (int)swAddGroupBoxOptions_e.swGroupBoxOptions_Visible;

            group3 = (IPropertyManagerPageGroup)swPropertyPage.AddGroupBox(group3ID, "WPF Control", options);

            //Add the controls to group1`
            controlType = (int)swPropertyManagerPageControlType_e.swControlType_WindowFromHandle;
            align = (int)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;
            options = (int)swAddControlOptions_e.swControlOptions_Enabled |
                      (int)swAddControlOptions_e.swControlOptions_Visible;

            dotnet1 = (IPropertyManagerPageWindowFromHandle)group1.AddControl(DotNet1ID, controlType, ".Net Windows Form Control", align, options, "This Control is added without COM");
            dotnet1.Height = 150;

            controlType = (int)swPropertyManagerPageControlType_e.swControlType_WindowFromHandle;
            align = (int)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;
            options = (int)swAddControlOptions_e.swControlOptions_Enabled |
                      (int)swAddControlOptions_e.swControlOptions_Visible;

            dotnet2 = (IPropertyManagerPageWindowFromHandle)group2.AddControl(DotNet2ID, controlType, ".Net User Form Control", align, options, "This Control is added without COM");
            dotnet2.Height = 150;

            dotnet3 = (IPropertyManagerPageWindowFromHandle)group3.AddControl(DotNet3ID, controlType, ".Net WPF Control", align, options, "This Control is added without COM");
            dotnet3.Height = 50;


        }

        public void Show()
        {

            // For Dot net control user need to create object at every Display

            //WinForm Control
            MyWinFormControl = new Form1();
            //If you are adding Winform in Property Page need to set TopLevel Property to false
            MyWinFormControl.TopLevel = false;
            MyWinFormControl.Show();
            dotnet1.SetWindowHandle(MyWinFormControl.Handle.ToInt64());


            //User Control
            MyUserControl = new UserControl1();
            dotnet2.SetWindowHandle(MyUserControl.Handle.ToInt64());


            //WPF control
            elhost = new ElementHost();
            MyWPFControl = new WPFControl();
            elhost.Child = MyWPFControl;
            dotnet3.SetWindowHandle(elhost.Handle.ToInt64());

            //Show Proppety page
            swPropertyPage.Show();

        }
    }
}