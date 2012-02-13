using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Reflection;
using System.Diagnostics;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using SolidWorks.Interop.swconst;
using SolidWorksTools;
using SolidWorksTools.File;
using System.Windows.Forms;
using System.Windows.Forms.Integration;


namespace SolidSVN
{
    /// <summary>
    /// Summary description for DotNetControlsDemo.
    /// </summary>
    [Guid("4ca45de8-6831-4a4c-83ac-d9d968803794"), ComVisible(true)]
    [SwAddin(
        Description = "DotNetControlsDemo description",
        Title = "DotNetControlsDemo",
        LoadAtStartup = true
        )]
    public class SwAddin : ISwAddin
    {
        #region Local Variables
        ISldWorks iSwApp;
        ICommandManager iCmdMgr;
        ICommandGroup cmdGroup;
        int addinID;
        Form1 TaskPanWinFormControl;
        UserControl1 TaskPanUserControl;
        Form1 FeatureMgrControl;
        UserControl1 ModelView1Control;
        WPFControl WpfModelView1Control;
        ElementHost ModelViewelhost;

        #region Event Handler Variables
        Hashtable openDocs;
        SolidWorks.Interop.sldworks.SldWorks SwEventPtr;
        #endregion

        #region Property Manager Variables
        UserPMPage ppage;
        #endregion


        // Public Properties
        public ISldWorks SwApp
        {
            get { return iSwApp; }
        }
        public ICommandManager CmdMgr
        {
            get { return iCmdMgr; }
        }

        public Hashtable OpenDocs
        {
            get { return openDocs; }
        }

        #endregion

        #region SolidWorks Registration
        [ComRegisterFunctionAttribute]
        public static void RegisterFunction(Type t)
        {

            #region Get Custom Attribute: SwAddinAttribute
            SwAddinAttribute SWattr = null;
            Type type = typeof(SwAddin);
            foreach (System.Attribute attr in type.GetCustomAttributes(false))
            {
                if (attr is SwAddinAttribute)
                {
                    SWattr = attr as SwAddinAttribute;
                    break;
                }
            }
            #endregion

            Microsoft.Win32.RegistryKey hklm = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey hkcu = Microsoft.Win32.Registry.CurrentUser;

            string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + t.GUID.ToString() + "}";
            Microsoft.Win32.RegistryKey addinkey = hklm.CreateSubKey(keyname);
            addinkey.SetValue(null, 0);

            addinkey.SetValue("Description", SWattr.Description);
            addinkey.SetValue("Title", SWattr.Title);

            keyname = "Software\\SolidWorks\\AddInsStartup\\{" + t.GUID.ToString() + "}";
            addinkey = hkcu.CreateSubKey(keyname);
            addinkey.SetValue(null, Convert.ToInt32(SWattr.LoadAtStartup), Microsoft.Win32.RegistryValueKind.DWord);
        }

        [ComUnregisterFunctionAttribute]
        public static void UnregisterFunction(Type t)
        {
            Microsoft.Win32.RegistryKey hklm = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey hkcu = Microsoft.Win32.Registry.CurrentUser;

            string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + t.GUID.ToString() + "}";
            hklm.DeleteSubKey(keyname);

            keyname = "Software\\SolidWorks\\AddInsStartup\\{" + t.GUID.ToString() + "}";
            hkcu.DeleteSubKey(keyname);
        }

        #endregion

        #region ISwAddin Implementation
        public SwAddin()
        {
        }

        public bool ConnectToSW(object ThisSW, int cookie)
        {
            iSwApp = (ISldWorks)ThisSW;
            addinID = cookie;

            //Setup callbacks
            iSwApp.SetAddinCallbackInfo(0, this, addinID);

            #region Setup the Command Manager
            iCmdMgr = iSwApp.GetCommandManager(cookie);
            AddCommandMgr();
            #endregion

            #region Setup the Event Handlers
            SwEventPtr = (SolidWorks.Interop.sldworks.SldWorks)iSwApp;
            openDocs = new Hashtable();
            AttachEventHandlers();
            #endregion

            #region Setup Sample Property Manager
            AddPMP();
            #endregion

            return true;
        }

        public bool DisconnectFromSW()
        {
            RemoveCommandMgr();
            RemovePMP();
            DetachEventHandlers();

            iSwApp = null;
            //The addin _must_ call GC.Collect() here in order to retrieve all managed code pointers 
            GC.Collect();
            return true;
        }
        #endregion

        #region UI Methods
        public void AddCommandMgr()
        {

            BitmapHandler iBmp = new BitmapHandler();

            Assembly thisAssembly;
            int cmdIndex0, cmdIndex1, cmdIndex2, cmdIndex3, cmdIndex4;
            string Title = "DotNetControlsDemo", ToolTip = "DotNetControlsDemo";


            int[] docTypes = new int[]{(int)swDocumentTypes_e.swDocASSEMBLY,
                                       (int)swDocumentTypes_e.swDocDRAWING,
                                       (int)swDocumentTypes_e.swDocPART};

            thisAssembly = System.Reflection.Assembly.GetAssembly(this.GetType());

            cmdGroup = iCmdMgr.CreateCommandGroup(1, Title, ToolTip, "", -1);
            cmdGroup.LargeIconList = iBmp.CreateFileFromResourceBitmap("DotNetControlsDemo.ToolbarLarge.bmp", thisAssembly);
            cmdGroup.SmallIconList = iBmp.CreateFileFromResourceBitmap("DotNetControlsDemo.ToolbarSmall.bmp", thisAssembly);
            cmdGroup.LargeMainIcon = iBmp.CreateFileFromResourceBitmap("DotNetControlsDemo.MainIconLarge.bmp", thisAssembly);
            cmdGroup.SmallMainIcon = iBmp.CreateFileFromResourceBitmap("DotNetControlsDemo.MainIconSmall.bmp", thisAssembly);

            cmdIndex0 = cmdGroup.AddCommandItem("WinFromInTaskPane", -1, "Add Windows Form In Task Pane", "WinFormInTaskPane", 0, "WinFormInTaskPane", "EnableWinFormInTaskPane", 0);
            cmdIndex1 = cmdGroup.AddCommandItem("UserControlInModelView", -1, "Add User Control In Model View", "UserControlInModelView", 1, "UserControlInModelView", "EnableUserControlInModelView", 1);
            cmdIndex2 = cmdGroup.AddCommandItem("WPFInModelView", -1, "Add WPF In ModelView", "WPFInModelView", 2, "WPFInModelView", "EnableWPFInModelView", 2);
            cmdIndex3 = cmdGroup.AddCommandItem("WinFormInFeatureMgr", -1, "Add Windows Form In FeatureManager", "WinFormInFeatureMgr", 3, "WinFormInFeatureMgr", "EnableWinFormInFeatureMgr", 3);
            cmdIndex4 = cmdGroup.AddCommandItem("Show PMP", -1, "Display PropertyManager with .NET Controls", "Show PMP", 4, "ShowPMP", "EnablePMP", 4);

            cmdGroup.HasToolbar = true;
            cmdGroup.HasMenu = true;
            cmdGroup.Activate();

            bool bResult;

            foreach (int type in docTypes)
            {
                ICommandTab cmdTab;

                cmdTab = iCmdMgr.GetCommandTab(type, Title);

                if (cmdTab == null)
                {
                    cmdTab = (ICommandTab)iCmdMgr.AddCommandTab(type, Title);

                    CommandTabBox cmdBox = cmdTab.AddCommandTabBox();

                    int[] cmdIDs = new int[6];
                    int[] TextType = new int[6];


                    cmdIDs[0] = cmdGroup.get_CommandID(cmdIndex0);
                    System.Diagnostics.Debug.Print(cmdGroup.get_CommandID(cmdIndex0).ToString());
                    TextType[0] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextHorizontal;

                    cmdIDs[1] = cmdGroup.get_CommandID(cmdIndex1);
                    System.Diagnostics.Debug.Print(cmdGroup.get_CommandID(cmdIndex1).ToString());
                    TextType[1] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextHorizontal;

                    cmdIDs[2] = cmdGroup.get_CommandID(cmdIndex2);
                    System.Diagnostics.Debug.Print(cmdGroup.get_CommandID(cmdIndex2).ToString());
                    TextType[2] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextHorizontal;

                    cmdIDs[3] = cmdGroup.get_CommandID(cmdIndex3);
                    System.Diagnostics.Debug.Print(cmdGroup.get_CommandID(cmdIndex3).ToString());
                    TextType[3] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextHorizontal;


                    cmdIDs[4] = cmdGroup.get_CommandID(cmdIndex4);
                    System.Diagnostics.Debug.Print(cmdGroup.get_CommandID(cmdIndex4).ToString());
                    TextType[4] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextHorizontal;

                    cmdIDs[5] = cmdGroup.ToolbarId;
                    System.Diagnostics.Debug.Print(cmdIDs[5].ToString());
                    TextType[5] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextHorizontal | (int)swCommandTabButtonFlyoutStyle_e.swCommandTabButton_ActionFlyout;

                    bResult = cmdBox.AddCommands(cmdIDs, TextType);



                    CommandTabBox cmdBox1 = cmdTab.AddCommandTabBox();
                    cmdIDs = new int[1];
                    TextType = new int[1];

                    cmdIDs[0] = cmdGroup.ToolbarId;
                    TextType[0] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow | (int)swCommandTabButtonFlyoutStyle_e.swCommandTabButton_ActionFlyout;

                    bResult = cmdBox1.AddCommands(cmdIDs, TextType);

                    cmdTab.AddSeparator(cmdBox1, cmdGroup.ToolbarId);

                }

            }
            thisAssembly = null;
            iBmp.Dispose();
        }


        public void RemoveCommandMgr()
        {
            iCmdMgr.RemoveCommandGroup(1);
        }

        public Boolean AddPMP()
        {
            ppage = new UserPMPage(this);
            return true;
        }

        public Boolean RemovePMP()
        {
            ppage = null;
            return true;
        }

        #endregion

        #region UI Callbacks
        public void WinFormInTaskPane()
        {
            ITaskpaneView pTaskPanView;
            pTaskPanView = iSwApp.CreateTaskpaneView2("", "C# .NET Control");
            TaskPanWinFormControl = new Form1();
            pTaskPanView.DisplayWindowFromHandle(TaskPanWinFormControl.Handle.ToInt64());
            TaskPanUserControl = (UserControl1)pTaskPanView.GetControl();
            Debug.Print(TaskPanUserControl.Name);

        }


        public int EnableWinFromInTaskPane()
        {
            return 1;

        }

        public void UserControlInModelView()
        {
            IModelDoc2 pDoc;
            pDoc = (IModelDoc2)iSwApp.ActiveDoc;
            IModelViewManager swModelViewMgr;
            swModelViewMgr = pDoc.ModelViewManager;
            ModelView1Control = new UserControl1();
            swModelViewMgr.DisplayWindowFromHandle(".NET User Control1", ModelView1Control.Handle.ToInt64(), false);
        }

        public int EnableUserControlInModelView()
        {
            if (iSwApp.ActiveDoc != null)
                return 1;
            else
                return 0;
        }

        public void WPFInModelView()
        {
            IModelDoc2 pDoc;
            pDoc = (IModelDoc2)iSwApp.ActiveDoc;
            IModelViewManager swModelViewMgr;
            swModelViewMgr = pDoc.ModelViewManager;
            WpfModelView1Control = new WPFControl();

            ModelViewelhost = new ElementHost();
            ModelViewelhost.Child = WpfModelView1Control;

            swModelViewMgr.DisplayWindowFromHandle(".NET WPF Control", ModelViewelhost.Handle.ToInt64(), true);

        }

        public int EnableWPFInModelView()
        {
            if (iSwApp.ActiveDoc != null)
                return 1;
            else
                return 0;
        }

        public void WinFormInFeatureMgr()
        {
            IModelDoc2 pDoc;
            pDoc = (IModelDoc2)iSwApp.ActiveDoc;
            IModelViewManager swModelViewMgr;
            swModelViewMgr = pDoc.ModelViewManager;
            IFeatMgrView swFeatMgrTabTop;
            FeatureMgrControl = new Form1();
            swFeatMgrTabTop = swModelViewMgr.CreateFeatureMgrWindowFromHandle(cmdGroup.SmallIconList, (int)FeatureMgrControl.Handle.ToInt64(), "MyDotNetControl", (int)swFeatMgrPane_e.swFeatMgrPaneTop);
            pDoc.FeatureManagerSplitterPosition = 0.5;
            swFeatMgrTabTop.ActivateView();
        }

        public int EnableWinFormInFeatureMgr()
        {
            if (iSwApp.ActiveDoc != null)
                return 1;
            else
                return 0;
        }

        public void ShowPMP()
        {
            if (ppage != null)
                ppage.Show();
        }

        public int EnablePMP()
        {
            if (iSwApp.ActiveDoc != null)
                return 1;
            else
                return 0;
        }
        #endregion

        #region Event Methods
        public bool AttachEventHandlers()
        {
            AttachSwEvents();
            //Listen for events on all currently open docs
            AttachEventsToAllDocuments();
            return true;
        }

        private bool AttachSwEvents()
        {
            try
            {
                SwEventPtr.ActiveDocChangeNotify += new DSldWorksEvents_ActiveDocChangeNotifyEventHandler(OnDocChange);
                SwEventPtr.DocumentLoadNotify2 += new DSldWorksEvents_DocumentLoadNotify2EventHandler(OnDocLoad);
                SwEventPtr.FileNewNotify2 += new DSldWorksEvents_FileNewNotify2EventHandler(OnFileNew);
                SwEventPtr.ActiveModelDocChangeNotify += new DSldWorksEvents_ActiveModelDocChangeNotifyEventHandler(OnModelChange);
                SwEventPtr.FileOpenPostNotify += new DSldWorksEvents_FileOpenPostNotifyEventHandler(FileOpenPostNotify);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }



        private bool DetachSwEvents()
        {
            try
            {
                SwEventPtr.ActiveDocChangeNotify -= new DSldWorksEvents_ActiveDocChangeNotifyEventHandler(OnDocChange);
                SwEventPtr.DocumentLoadNotify2 -= new DSldWorksEvents_DocumentLoadNotify2EventHandler(OnDocLoad);
                SwEventPtr.FileNewNotify2 -= new DSldWorksEvents_FileNewNotify2EventHandler(OnFileNew);
                SwEventPtr.ActiveModelDocChangeNotify -= new DSldWorksEvents_ActiveModelDocChangeNotifyEventHandler(OnModelChange);
                SwEventPtr.FileOpenPostNotify -= new DSldWorksEvents_FileOpenPostNotifyEventHandler(FileOpenPostNotify);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public void AttachEventsToAllDocuments()
        {
            ModelDoc2 modDoc = (ModelDoc2)iSwApp.GetFirstDocument();
            while (modDoc != null)
            {
                if (!openDocs.Contains(modDoc))
                {
                    AttachModelDocEventHandler(modDoc);
                }
                modDoc = (ModelDoc2)modDoc.GetNext();
            }
        }

        public bool AttachModelDocEventHandler(ModelDoc2 modDoc)
        {
            if (modDoc == null)
                return false;

            DocumentEventHandler docHandler = null;

            if (!openDocs.Contains(modDoc))
            {
                switch (modDoc.GetType())
                {
                    case (int)swDocumentTypes_e.swDocPART:
                        {
                            docHandler = new PartEventHandler(modDoc, this);
                            break;
                        }
                    case (int)swDocumentTypes_e.swDocASSEMBLY:
                        {
                            docHandler = new AssemblyEventHandler(modDoc, this);
                            break;
                        }
                    case (int)swDocumentTypes_e.swDocDRAWING:
                        {
                            docHandler = new DrawingEventHandler(modDoc, this);
                            break;
                        }
                    default:
                        {
                            return false; //Unsupported document type
                        }
                }
                docHandler.AttachEventHandlers();
                openDocs.Add(modDoc, docHandler);
            }
            return true;
        }

        public bool DetachModelEventHandler(ModelDoc2 modDoc)
        {
            DocumentEventHandler docHandler;
            docHandler = (DocumentEventHandler)openDocs[modDoc];
            openDocs.Remove(modDoc);
            modDoc = null;
            docHandler = null;
            return true;
        }

        public bool DetachEventHandlers()
        {
            DetachSwEvents();

            //Close events on all currently open docs
            DocumentEventHandler docHandler;
            int numKeys = openDocs.Count;
            object[] keys = new Object[numKeys];

            //Remove all document event handlers
            openDocs.Keys.CopyTo(keys, 0);
            foreach (ModelDoc2 key in keys)
            {
                docHandler = (DocumentEventHandler)openDocs[key];
                docHandler.DetachEventHandlers(); //This also removes the pair from the hash
                docHandler = null;
            }
            return true;
        }
        #endregion

        #region Event Handlers
        //Events
        public int OnDocChange()
        {
            return 0;
        }

        public int OnDocLoad(string docTitle, string docPath)
        {
            return 0;
        }

        int FileOpenPostNotify(string FileName)
        {
            AttachEventsToAllDocuments();
            return 0;
        }

        public int OnFileNew(object newDoc, int docType, string templateName)
        {
            AttachEventsToAllDocuments();
            return 0;
        }

        public int OnModelChange()
        {
            return 0;
        }

        #endregion
    }

}