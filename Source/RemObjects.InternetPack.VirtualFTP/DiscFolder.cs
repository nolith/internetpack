/*---------------------------------------------------------------------------
  RemObjects Internet Pack for .NET - Virtual FTP Library
  (c)opyright RemObjects Software, LLC. 2003-2012. All rights reserved.

  Using this code requires a valid license of the RemObjects Internet Pack
  which can be obtained at http://www.remobjects.com?ip.
---------------------------------------------------------------------------*/

using System;
using System.Collections;
using System.IO;

namespace RemObjects.InternetPack.Ftp.VirtualFtp
{
    public class DiscFolder : FtpFolder
    {
        public DiscFolder(IFtpFolder parent, String name, String localPath)
            : base(parent, name)
        {
            this.fLocalPath = localPath;
            this.WorldRead = true;
            this.WorldWrite = true;
        }

        public String LocalPath
        {
            get
            {
                return fLocalPath;
            }
        }
        private String fLocalPath;

        public override IEnumerable SubFolders
        {
            get
            {
                return null;
            }
        }

        public override IEnumerable Files
        {
            get
            {
                return null;
            }
        }

        public override Boolean Invalid
        {
            get
            {
                return !Directory.Exists(LocalPath);
            }
        }

        public override void DoListFolderItems(FtpListing listing)
        {
            String[] lNames = Directory.GetDirectories(LocalPath);

            foreach (String name in lNames)
            {
                FtpListingItem lListingItem = listing.Add();
                lListingItem.Directory = true;
                lListingItem.FileName = Path.GetFileName(name);
                lListingItem.FileDate = Directory.GetLastWriteTime(Path.Combine(LocalPath, name));
                lListingItem.Size = 0;
                lListingItem.User = "system";
                lListingItem.Group = "system";
                lListingItem.UserRead = UserRead;
                lListingItem.UserWrite = UserWrite;
                lListingItem.UserExec = UserRead;
                lListingItem.GroupRead = GroupRead;
                lListingItem.GroupWrite = GroupWrite;
                lListingItem.GroupExec = GroupRead;
                lListingItem.OtherRead = WorldRead;
                lListingItem.OtherWrite = WorldWrite;
                lListingItem.OtherExec = WorldRead;
            }

            DirectoryInfo lDirectory = new DirectoryInfo(LocalPath);
            FileInfo[] lFiles = lDirectory.GetFiles();
            foreach (FileInfo file in lFiles)
            {
                FtpListingItem lListingItem = listing.Add();
                lListingItem.Directory = false;
                lListingItem.FileName = file.Name;
                lListingItem.FileDate = file.LastWriteTime;
                lListingItem.Size = file.Length;
                lListingItem.User = "system";
                lListingItem.Group = "system";
                lListingItem.UserRead = UserRead;
                lListingItem.UserWrite = UserWrite;
                lListingItem.UserExec = false;
                lListingItem.GroupRead = GroupRead;
                lListingItem.GroupWrite = GroupWrite;
                lListingItem.GroupExec = false;
                lListingItem.OtherRead = WorldRead;
                lListingItem.OtherWrite = WorldWrite;
                lListingItem.OtherExec = false;
            }

            AddListingItems(listing, SubFolders);
            AddListingItems(listing, Files);
        }

        public override Boolean HasSubfolder(String filename)
        {
            return Directory.Exists(Path.Combine(this.LocalPath, filename));
        }

        public override IFtpFolder GetSubFolder(String folder, VirtualFtpSession session)
        {
            if (!AllowBrowse(session))
                throw new FtpException(550, String.Format("Cannot access folder \"{0}\", permission to access items in this folder denied.", folder));

            if (!HasSubfolder(folder))
                throw new FtpException(550, String.Format("A folder named \"{0}\" does not exist.", folder));

            return new DiscFolder(this, folder, Path.Combine(LocalPath, folder));
        }

        public override IFtpFolder CreateFolder(String folder, VirtualFtpSession session)
        {
            if (!AllowMkDir(session))
                throw new FtpException(550, String.Format("Cannot create folder \"{0}\", permission to mkdir in this folder denied.", folder));

            if (HasSubfolder(folder))
                throw new FtpException(550, String.Format("A folder named \"{0}\" already exist.", folder));

            Directory.CreateDirectory(Path.Combine(LocalPath, folder));
            return new DiscFolder(this, folder, Path.Combine(LocalPath, folder));
        }

        public override void DeleteFolder(String folder, Boolean recursive, VirtualFtpSession session)
        {
            if (!AllowDeleteItems(session))
                throw new FtpException(550, String.Format("Cannot delete folder \"{0}\", permission to delete from this folder denied.", folder));

            if (!HasSubfolder(folder))
                throw new FtpException(550, String.Format("A folder named \"{0}\" does not exist.", folder));

            Directory.Delete(Path.Combine(LocalPath, folder), recursive);

        }

        public override Boolean HasFile(String filename)
        {
            return File.Exists(Path.Combine(this.LocalPath, filename));
        }

        public override IFtpFile GetFile(String filename, VirtualFtpSession session)
        {
            if (!AllowGet(session))
                throw new FtpException(550, String.Format("Cannot retrieve file \"{0}\", permission to get files from this folder denied.", filename));

            if (!HasFile(filename))
                throw new FtpException(550, String.Format("A file named \"{0}\" does not exist.", filename));

            return new DiscFile(this, filename, Path.Combine(LocalPath, filename));
        }

        public override IFtpFile CreateFile(String filename, VirtualFtpSession session)
        {
            if (!AllowPut(session))
                throw new FtpException(550, String.Format("Cannot upload file \"{0}\", permission to upload files to this folder denied.", filename));

            if (HasFile(filename))
                throw new FtpException(550, String.Format("A file named \"{0}\" already exist.", filename));

            return new DiscFile(this, filename, Path.Combine(LocalPath, filename));
        }

        public override void DeleteFile(String filename, VirtualFtpSession session)
        {
            if (!AllowDeleteItems(session))
                throw new FtpException(550, String.Format("Cannot delete file \"{0}\", permission to delete from this folder denied.", filename));

            if (!HasFile(filename))
                throw new FtpException(550, String.Format("A file named \"{0}\" does not exist.", filename));

            File.Delete(Path.Combine(LocalPath, filename));
        }

        public override void RenameFileOrFolder(String oldFilename, String newFilename, VirtualFtpSession session)
        {
        }

        public override void RemoveItem(IFtpItem item)
        {
        }
    }
}
