using System;
using System.Text;
using System.Collections;
using System.IO;

namespace RemObjects.InternetPack.Ftp.VirtualFtp
{
  public abstract class FtpFolder : FtpItem, IFtpFolder
  {
    protected FtpFolder(IFtpFolder aParent, string aName) : base(aParent, aName)
    {
      fSubFolderList = new Hashtable();
    }

    public override int Size { get { return 0; } set { /* no-op */ } }


    #region IFtpFolder implementation

    public abstract IEnumerable SubFolders { get ; }
        return this;
      
      if (p >= 0)
      {
        string lSubFolder = aFullPath.Substring(0,p);

        IFtpFolder lFolder = GetSubFolder(lSubFolder, aSession);

        if (lFolder != null)
        {
          aFullPath = aFullPath.Substring(p+1);
          return lFolder.DigForSubFolder(aFullPath, aSession);
        }  

        else
        {
          return null;
        }
      }
      else 
      {
        if (aPath.StartsWith("/"))
        {
          aFolder = Root;
          aPath = aPath.Substring(1); /* remove / */
        }
        else
        {
          aFolder = this;
        }

        {
          string lFolderName = aPath.Substring(0,p);
          aFolder = aFolder.GetSubFolder(lFolderName, aSession);

          if (aFolder == null || !aFolder.AllowBrowse(aSession))
            throw new FtpException(550, String.Format("Folder \"{0}\" does not exists, or access denied.", aPath));
        
          aPath = aPath.Substring(p+1);
          p = aPath.IndexOf('/');
      }
      else
      {
        aFolder = this;
      }

      aFilename = aPath;
    }
        if (Parent != null)
        {
          return Parent.FullPath+Name+"/";
        }
        else
    }
    }
    }
    }
    public virtual bool AllowDeleteItems(VirtualFtpSession aSession)
    }
    public virtual bool AllowRenameItems(VirtualFtpSession aSession)
    }
    public virtual bool AllowDeleteThis(VirtualFtpSession aSession)
    }

    #region FolderListing
    public void ListFolderItems(FtpListing aListing)
      FtpListingItem lListingItem;
      lListingItem = aListing.Add();
      FillFtpListingItem(lListingItem, ".");
      
      /* Add parent folder (..) */
      if (Parent != null)
      {
        lListingItem = aListing.Add();
        Parent.FillFtpListingItem(lListingItem, "..");
      }

      DoListFolderItems(aListing);
    }
      AddListingItems(aListing, Files);
    }
    {
      if (aFtpItems != null)
      {
        foreach (IFtpItem lFtpItem in aFtpItems)
        {
          FtpListingItem lListingItem = aListing.Add();
          lFtpItem.FillFtpListingItem(lListingItem);
        }
      }
    }
    #endregion

  }

}