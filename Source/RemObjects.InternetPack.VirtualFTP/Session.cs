using System;
using RemObjects.InternetPack.Ftp;

namespace RemObjects.InternetPack.Ftp.VirtualFtp
{
	public class VirtualFtpSession : FtpSession
	{
		public VirtualFtpSession()
		{
		}

    public override String Directory 
    { 
      get 
      { 
        return CurrentFolder.FullPath; 
      } 
      set 
      { 
        /* ToDo: browse to folder */
        base.Directory = value; 
      }
    }


    private bool fIsSuperUser;

    private bool fIsFileAdmin;
    public bool IsFileAdmin
    {
      get { return fIsFileAdmin; }
      set	{ fIsFileAdmin = value; }
    }

    private IFtpFolder fCurrentFolder;

	}
}