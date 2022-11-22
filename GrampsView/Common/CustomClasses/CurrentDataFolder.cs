﻿using SharedSharp.Errors.Interfaces;

using System.Diagnostics;

namespace GrampsView.Common.CustomClasses
{
    public class CurrentDataFolder
    {
        public CurrentDataFolder()
        {
            try
            {
                string tt = System.IO.Path.Combine(FileSystem.Current.CacheDirectory, Constants.DirectoryCacheBase);

                Value = new DirectoryInfo(tt);

                DirectoryInfo t = new(FileSystem.Current.CacheDirectory);

                if (!Value.Exists)
                {
                    Value = t.CreateSubdirectory(Constants.DirectoryCacheBase);
                }

                Debug.WriteLine("CurrentDataFolder Path:" + Value.FullName);
            }
            catch (System.Exception ex)
            {
                Ioc.Default.GetService<IErrorNotifications>().NotifyException("Exception creating application cache", ex, null);
                throw;
            }
        }

        public string Path => Value.FullName;

        public bool Valid => !(Value == null) && Value.Exists;

        public DirectoryInfo? Value
        {
            get; set;
        } = null;
    }
}