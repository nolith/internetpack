using System;

namespace RemObjects.InternetPack.Ftp.VirtualFtp
{

  public abstract class FtpItem : IFtpItem
  {
    protected FtpItem(IFtpFolder aParent, string aName)
    {
      fParent = aParent;
      fName = aName;

      UserRead = true;
      UserWrite = true;
      GroupRead = true;
      
      OwningUser = "system";
      OwningGroup = "system";

      Date = DateTime.Now;
    }

    #region General Item attributes
    private IFtpFolder fParent;
    public IFtpFolder Root
        {
          lParent = this as IFtpFolder;
        while (lParent.Parent != null) lParent = lParent.Parent;

    private string fName;
    private DateTime fDate;
    public virtual DateTime Date { get { return fDate; } set { fDate = value; } }
    public abstract int Size { get; set; }
    #region Rights
    private string fOwningUser;

    private string fOwningGroup;

    public virtual bool AllowRead(VirtualFtpSession aSession)
      if (aSession.IsFileAdmin) return true;
      if (aSession.Username == OwningUser && UserRead) return true;
      if (WorldRead) return true;
      return false;
    }
      if (aSession.IsFileAdmin) return true;
      if (aSession.Username == OwningUser && UserWrite) return true;
      if (WorldWrite) return true;
      return false;
    }
    private bool[] fRights = new bool[6];

    #region Invalidation
    private bool fInvalid;
    public virtual void Invalidate()
    public virtual void FillFtpListingItem(FtpListingItem aItem, string aAsName)
    {
      FillFtpListingItem(aItem);
      aItem.FileName = aAsName;
    }
     
    public virtual void FillFtpListingItem(FtpListingItem aItem)
    {
      aItem.Directory = (this is IFtpFolder);
      aItem.FileName = Name;
      aItem.FileDate = Date;
      aItem.Size = Size;
      aItem.User = OwningUser;
      aItem.Group = OwningGroup;
      aItem.UserRead = UserRead;
      aItem.UserWrite = UserWrite;
      aItem.UserExec = aItem.Directory && UserRead;
      aItem.GroupRead = GroupRead;
      aItem.GroupWrite = GroupWrite;
      aItem.GroupExec = aItem.Directory && GroupRead;
      aItem.OtherRead = WorldRead;
      aItem.OtherWrite = WorldWrite;
      aItem.OtherExec = aItem.Directory && WorldRead;
    }

  }

}