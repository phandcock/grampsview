﻿namespace GrampsView.Data.ExternalStorageNS
{
    using GrampsView.Common;
    using GrampsView.Data.Model;
    using GrampsView.Data.Repository;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    public partial class StorePostLoad : CommonBindableBase, IStorePostLoad
    {
        private List<MediaModel> addLater = new List<MediaModel>();

        public async static Task SetAddressImages()
        {
        }

        public async static Task SetCitationImages()
        {
            foreach (CitationModel argModel in DataStore.Instance.DS.CitationData.Values)
            {
                if (argModel is null)
                {
                    throw new ArgumentNullException(nameof(argModel));
                }

                if (argModel.ModelItemGlyph.ImageType == CommonEnums.HLinkGlyphType.Image)
                {
                }

                ItemGlyph hlink = argModel.ModelItemGlyph;

                if (argModel.Id == "C0682")
                {
                }

                // Try media reference collection first
                ItemGlyph t = argModel.GMediaRefCollection.FirstHLinkHomeImage;
                if ((!hlink.ValidImage) && (t.ValidImage))
                {
                    hlink = t;
                }

                // Check Source for Image
                t = argModel.GSourceRef.DeRef.ModelItemGlyph;
                if ((!hlink.ValidImage) && (t.ValidImage))
                {
                    hlink = t;
                }

                // Handle the link if we can
                if (hlink.Valid)
                {
                    argModel.ModelItemGlyph.ImageType = hlink.ImageType;
                    argModel.ModelItemGlyph.HLinkMediHLink = hlink.HLinkMediHLink;
                    argModel.ModelItemGlyph.ImageSymbol = hlink.ImageSymbol;
                    argModel.ModelItemGlyph.ImageSymbolColour = hlink.ImageSymbolColour;
                }
            }

            return;
        }

        public async static Task SetEventImages()
        {
            foreach (EventModel argModel in DataStore.Instance.DS.EventData.Values)
            {
                if (argModel is null)
                {
                    throw new ArgumentNullException(nameof(argModel));
                }

                // Setup home images
                ItemGlyph hlink = argModel.ModelItemGlyph;

                // check media
                ItemGlyph t = argModel.GMediaRefCollection.FirstHLinkHomeImage;
                if (!hlink.ValidImage & t.ValidImage)
                {
                    hlink = t;
                }

                // Check Citation for Images
                t = argModel.GCitationRefCollection.FirstHLinkHomeImage;
                if (!hlink.ValidImage & t.ValidImage)
                {
                    hlink = t;
                }

                // Handle the link if we can
                if (hlink.Valid)
                {
                    argModel.ModelItemGlyph.ImageType = hlink.ImageType;
                    argModel.ModelItemGlyph.HLinkMediHLink = hlink.HLinkMediHLink;
                    argModel.ModelItemGlyph.ImageSymbol = hlink.ImageSymbol;
                    argModel.ModelItemGlyph.ImageSymbolColour = hlink.ImageSymbolColour;
                }
            }
        }

        public async static Task SetFamilyImages()
        {
            foreach (FamilyModel argModel in DataStore.Instance.DS.FamilyData.Values)
            {
                ItemGlyph hlink = argModel.ModelItemGlyph;

                // check media
                ItemGlyph t = argModel.GMediaRefCollection.FirstHLinkHomeImage;
                if (!hlink.ValidImage & t.ValidImage)
                {
                    hlink = t;
                }

                t = argModel.GCitationRefCollection.FirstHLinkHomeImage;
                if (!hlink.ValidImage & t.ValidImage)
                {
                    hlink = t;
                }

                t = argModel.GEventRefCollection.FirstHLinkHomeImage;
                if (!hlink.ValidImage & t.ValidImage)
                {
                    hlink = t;
                }

                t = argModel.GNoteRefCollection.FirstHLinkHomeImage;
                if (!hlink.ValidImage & t.ValidImage)
                {
                    hlink = t;
                }

                // Set the image if available
                if (hlink.Valid)
                {
                    argModel.ModelItemGlyph.ImageType = hlink.ImageType;
                    argModel.ModelItemGlyph.HLinkMediHLink = hlink.HLinkMediHLink;
                    argModel.ModelItemGlyph.ImageSymbol = hlink.ImageSymbol;
                    argModel.ModelItemGlyph.ImageSymbolColour = hlink.ImageSymbolColour;
                }
            }
        }

        public async static Task SetHeaderImages()
        {
        }

        public async static Task SetNameMapImages()
        {
        }

        //            //// People last as they pretty much depend on everything else
        //            //await SetPersonImages().ConfigureAwait(false);
        //        }
        //    }
        //}
        public async static Task SetNotesImages()
        {
        }

        // //await SetPersonNameImages().ConfigureAwait(false); People last as they pretty much
        // depend on everything else
        public async static Task SetPersonImages()
        {
            foreach (PersonModel argModel in DataStore.Instance.DS.PersonData.Values)
            {
                if (argModel.Id == "I0704")
                {
                }

                if (argModel is null)
                {
                    throw new ArgumentNullException(nameof(argModel));
                }

                ItemGlyph hlink = argModel.ModelItemGlyph;

                // check media
                ItemGlyph t = argModel.GMediaRefCollection.FirstHLinkHomeImage;
                if (!hlink.ValidImage & t.ValidImage)
                {
                    hlink = t;
                }

                // Check Citation for Images
                t = argModel.GCitationRefCollection.FirstHLinkHomeImage;
                if (!hlink.ValidImage & t.ValidImage)
                {
                    hlink = t;
                }

                // Check Events for Images
                t = argModel.GEventRefCollection.FirstHLinkHomeImage;
                if (!hlink.ValidImage & t.ValidImage)
                {
                    hlink = t;
                }

                // Action any Link
                if (hlink.Valid)
                {
                    argModel.ModelItemGlyph.ImageType = hlink.ImageType;
                    argModel.ModelItemGlyph.HLinkMediHLink = hlink.HLinkMediHLink;
                    argModel.ModelItemGlyph.ImageSymbol = hlink.ImageSymbol;
                    argModel.ModelItemGlyph.ImageSymbolColour = hlink.ImageSymbolColour;
                }
            }
        }

        // //await SetAddressImages().ConfigureAwait(false);
        public async static Task SetPersonNameImages()
        {
        }

        // //await SetTagImages().ConfigureAwait(false);
        public async static Task SetPlaceImages()
        {
        }

        // //await SetRepositoryImages().ConfigureAwait(false);
        public async static Task SetRepositoryImages()
        {
        }

        public static async Task SetSourceImages()
        {
            foreach (SourceModel argModel in DataStore.Instance.DS.SourceData.Values)
            {
                if (argModel.Id == "S0299")
                {
                }

                if (argModel is null)
                {
                    throw new ArgumentNullException(nameof(argModel));
                }

                ItemGlyph hlink = argModel.ModelItemGlyph;

                // Get default image if available
                ItemGlyph t = argModel.GMediaRefCollection.FirstHLinkHomeImage;
                if (!hlink.ValidImage && t.ValidImage)
                {
                    hlink = t;
                }

                // Set default
                if (hlink.Valid)
                {
                    argModel.ModelItemGlyph.ImageType = hlink.ImageType;
                    argModel.ModelItemGlyph.HLinkMediHLink = hlink.HLinkMediHLink;
                    argModel.ModelItemGlyph.ImageSymbol = hlink.ImageSymbol;
                    argModel.ModelItemGlyph.ImageSymbolColour = hlink.ImageSymbolColour;
                }
            }
        }

        public async static Task SetTagImages()
        {
            //foreach (TagModel argModel in DataStore.Instance.DS.TagData.Values)
            //{
            //}
        }

        public async Task<ItemGlyph> GetThumbImageFromPDF(MediaModel argMediaModel)
        {
            ItemGlyph returnItemGlyph = argMediaModel.ModelItemGlyph;

            returnItemGlyph.ImageSymbol = CommonFontNamesFAS.FilePdf;

            // check if we can get an image for the first page of the PDF
            MediaModel pdfimage = await _iocPlatformSpecific.GenerateThumbImageFromPDF(DataStore.Instance.AD.CurrentDataFolder, argMediaModel);

            if (pdfimage.Valid)
            {
                addLater.Add(pdfimage);

                returnItemGlyph.ImageType = CommonEnums.HLinkGlyphType.Image;
                returnItemGlyph.HLinkMediHLink = pdfimage.HLinkKey;
            }
            else
            {
                returnItemGlyph.ImageType = CommonEnums.HLinkGlyphType.Symbol;
                returnItemGlyph.ImageSymbol = CommonFontNamesFAS.FilePdf;
            }

            return returnItemGlyph;
        }

        public async Task<bool> SetMediaImages()
        {
            // Save new mediaModels for later as we can not modify a list in the middle of a foreach loop
            addLater = new List<MediaModel>();

            foreach (MediaModel argModel in DataStore.Instance.DS.MediaData.Values)
            {
                Contract.Requires(argModel != null);

                if (argModel.Id == "O0592")
                {
                }

                // Setup HomeImage
                argModel.ModelItemGlyph.HLinkMediHLink = argModel.HLink.HLinkKey;

                switch (argModel.FileMimeType)
                {
                    case "application":
                        {
                            argModel.ModelItemGlyph.ImageType = CommonEnums.HLinkGlyphType.Symbol;

                            switch (argModel.FileMimeSubType)
                            {
                                case "pdf":
                                    {
                                        argModel.ModelItemGlyph = await GetThumbImageFromPDF(argModel);
                                        break;
                                    }

                                case "x-zip-compressed":
                                    {
                                        argModel.ModelItemGlyph.ImageSymbol = CommonFontNamesFAS.FileArchive;
                                        break;
                                    }

                                case "zip":
                                    {
                                        argModel.ModelItemGlyph.ImageSymbol = CommonFontNamesFAS.FileArchive;
                                        break;
                                    }
                            }

                            break;
                        }

                    case "audio":
                        {
                            argModel.ModelItemGlyph.ImageType = CommonEnums.HLinkGlyphType.Symbol;
                            argModel.ModelItemGlyph.ImageSymbol = CommonFontNamesFAS.FileAudio;
                            break;
                        }

                    case "image":
                        {
                            argModel.ModelItemGlyph.ImageType = CommonEnums.HLinkGlyphType.Image;
                            argModel.ModelItemGlyph.HLinkMediHLink = argModel.HLinkKey;
                            argModel.ModelItemGlyph.ImageSymbol = CommonFontNamesFAS.FileImage;
                            break;
                        }

                    case "video":
                        {
                            argModel.ModelItemGlyph.ImageType = CommonEnums.HLinkGlyphType.Symbol;
                            argModel.ModelItemGlyph.ImageSymbol = CommonFontNamesFAS.FileVideo;
                            break;
                        }
                }
            }

            foreach (MediaModel item in addLater)
            {
                DataStore.Instance.DS.MediaData.Add(item);
            }

            return true;
        }
    }
}