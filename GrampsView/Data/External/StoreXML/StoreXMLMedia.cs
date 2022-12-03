﻿using GrampsView.Common;
using GrampsView.Common.Interfaces;
using GrampsView.Data.External.StoreFile;
using GrampsView.Data.Model;
using GrampsView.Data.Repository;
using GrampsView.Models.DataModels;
using GrampsView.Models.DataModels.Interfaces;

using SharedSharp.Errors;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;



namespace GrampsView.Data.ExternalStorage
{
    /// <summary>
    /// Private Storage Routines.
    /// </summary>
    public partial class StoreXML : IStoreXML
    {
        /// <summary>
        /// Load media objects from external storage.
        /// </summary>
        /// <returns>
        /// Flag showing of loaded successfully.
        /// </returns>
        public Task<bool> LoadMediaObjectsAsync()
        {
            myCommonLogging.RoutineEntry("loadMediaObjects");

            myCommonLogging.DataLogEntryAdd("Loading Media Objects");
            {
                //// start file load
                //await _iocCommonNotifications.DataLogEntryAdd("Loading Media File").ConfigureAwait(false);

                IImageSize PlatformImageHandler = new ImageSize();

                // Load notes Run query
                System.Collections.Generic.IEnumerable<XElement> de =
                    from el in localGrampsXMLdoc.Descendants(ns + "object")
                    select el;

                try
                {
                    foreach (XElement pname in de)
                    {
                        // <code> < define name = "object-content" > <ref name=
                        // "SecondaryColor-object" /> < element name = "file" > < attribute name =
                        // "src" > < text /> </ attribute > < attribute name = "mime" >

                        // < text />

                        // </ attribute >

                        // < optional >

                        // < attribute name = "checksum" >

                        // < text />

                        // </ attribute >

                        // </ optional > </code>

                        // < optional >

                        // < attribute name = "description" >

                        // < text />

                        // </ attribute >

                        // </ optional >

                        // </ element >

                        // < zeroOrMore >

                        // < element name = "attribute" >

                        // <ref name="attribute-content"/>

                        IMediaModel loadObject = new MediaModel();
                        loadObject.LoadBasics(GetBasics(pname));

                        if (loadObject.Id == "O0204")
                        {
                        }

                        // file details
                        XElement filedetails = pname.Element(ns + "file");
                        if (filedetails != null)
                        {
                            // load filename
                            string mediaFileName = (string)filedetails.Attribute("src");

                            if (mediaFileName.Length == 0)
                            {
                                myCommonNotifications.NotifyError(new ErrorInfo("Error trying to load a media file for object listed in the GRAMPS file.  FileName is null") { { "Id", loadObject.Id }, });

                                loadObject.MediaStorageFile = null;
                            }
                            else
                            {
                                try
                                {
                                    string temp = StoreFileUtility.CleanFilePath(mediaFileName);

                                    loadObject.OriginalFilePath = temp;

                                    // Load FileInfoEx and metadata
                                    loadObject.MediaStorageFile = FileInfoEx.GetStorageFile(loadObject.OriginalFilePath);

                                    if (loadObject.MediaStorageFile.Valid)
                                    {
                                        // TODO add this back in
                                        Size imageSize = PlatformImageHandler.GetSize(loadObject.MediaStorageFilePath);

                                        loadObject.MetaDataHeight = imageSize.Height;
                                        loadObject.MetaDataWidth = imageSize.Width;

                                        // TODO check File Content Type if ( loadObject.MediaStorageFile.FInfo.)
                                    }
                                    else
                                    {
                                        myCommonNotifications.NotifyError(new ErrorInfo("Bad media file path") { { "Path", loadObject.OriginalFilePath } });
                                    }
                                }
                                catch (Exception ex)
                                {
                                    myCommonNotifications.NotifyException("Error trying to load a media file (" + loadObject.OriginalFilePath + ") listed in the GRAMPS file",ex,null);
                                    throw;
                                }
                            }

                            // Load mime types
                            loadObject.FileContentType = (string)filedetails.Attribute("mime");

                            if (loadObject.FileMimeType == "unknown")
                            {
                                loadObject.FileContentType = CommonRoutines.MimeFileContentTypeGet(Path.GetExtension(loadObject.OriginalFilePath));
                            }
                        }

                        // Get attributes

                        // Get description
                        loadObject.GDescription = (string)filedetails.Attribute("description");

                        // date details
                        XElement dateval = pname.Element(ns + "dateval");
                        if (dateval != null)
                        {
                            loadObject.GDateValue = SetDate(pname);
                        }

                        // Load NoteRefs
                        loadObject.GNoteRefCollection.Clear();
                        loadObject.GNoteRefCollection.AddRange(GetNoteCollection(pname));

                        // citationref details TODO Event References
                        loadObject.GCitationRefCollection.Clear();
                        loadObject.GCitationRefCollection.AddRange(GetCitationCollection(pname));

                        foreach (HLinkTagModel item in GetTagCollection(pname))
                        {
                            loadObject.GTagRefCollection.Add(item);
                        }

                        // save the object
                        DataStore.Instance.DS.MediaData.Add((MediaModel)loadObject);

                        myCommonLogging.Variable("LoadMedia", loadObject.GDescription);
                    }
                }
                catch (Exception e)
                {
                    // TODO handle this
                    myCommonNotifications.NotifyException("Loading Media Objects",e,null);

                    throw;
                }
            }

            myCommonLogging.DataLogEntryReplace("Media load complete");

            myCommonLogging.RoutineExit(nameof(LoadMediaObjectsAsync));

            return Task.FromResult(true);
        }
    }
}