/******************************************************************************
 * MediaInfo.NET - A fast, easy-to-use .NET wrapper for MediaInfo.
 * Use at your own risk, under the same license as MediaInfo itself.
 * Copyright (C) 2012 Charles N. Burns
 * Copyright (C) 2013 Carsten Schlote
 ******************************************************************************
 * MenuStream.cs
 * 
 * Presents information and functionality specific to a menu stream.
 ******************************************************************************
 */

using MediaInfoDotNet.Models;
using MediaInfoLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MediaInfoDotNet.Models
{
    ///<summary>Represents a single menu stream.</summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class MenuStream : BaseStreamCommons
    {
        ///<summary>MenuStream constructor.</summary>
        ///<param name="mediaInfo">A MediaInfo object which has already opened a media file.</param>
        ///<param name="id">The MediaInfo ID for this menu stream.</param>
        public MenuStream(MediaInfo mediaInfo, int id)
            : base(mediaInfo, StreamKind.Menu, id) {
        }
        /// <summary>Overides base method to provide short summary of stream kind.</summary>
        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(this.Format)) sb.AppendFormat("{0}", this.Format);
            if (!String.IsNullOrEmpty(this.FormatProfile)) sb.AppendFormat(" {0}", this.FormatProfile);
            if (!String.IsNullOrEmpty(this.title)) sb.AppendFormat(", '{0}'", this.Title);
            if (Chapters.Count > 0) sb.AppendFormat(", '{0}'", $"{Chapters.Count} Chapters");

                return sb.ToString().TrimStart(',');
        }

        #region AllStreamsCommon

        ///<summary>The ID of this stream in the file.</summary>
        [Description("The ID of this stream in the file."), Category("AllStreamsCommon")]
        public int ID { get { return this.streamid; } }

        ///<summary>The format or container of this file or stream.</summary>
        [Description("The format or container of this file or stream."), Category("AllStreamsCommon")]
        public string Format { get { return this.format; } }

        ///<summary>Format information of this stream.</summary>
        [Description("Format information for this container or stream."), Category("AllStreamsCommon")]
        public string FormatInfo { get { return this.format_info; } }

        ///<summary>Format profile of this stream.</summary>
        [Description("Format profile for this container or stream."), Category("AllStreamsCommon")]
        public string FormatProfile { get { return this.format_profile; } }

        ///<summary>Format version of this stream.</summary>
        [Description("Format version for this container or stream."), Category("AllStreamsCommon")]
        public string FormatVersion { get { return this.format_version; } }

        ///<summary>The title of this stream.</summary>
        [Description("The title of this container or stream."), Category("AllStreamsCommon")]
        public string Title { get { return this.title; } }

        ///<summary>This stream's globally unique ID (GUID).</summary>
        [Description("This stream's or container's globally unique ID (GUID)."), Category("AllStreamsCommon")]
        public string UniqueId { get { return this.uniqueId; } }

        #endregion

        #region GeneralVideoAudioTextImageMenuCommon

        ///<summary>Codec ID available from some codecs.</summary>
        ///<example>AAC audio:A_AAC, h.264 video:V_MPEG4/ISO/AVC</example>
        [Description("Codec ID available from some codecs."), Category("GeneralVideoAudioTextImageMenuCommon")]
        public string CodecId { get { return this.codecId; } }

        ///<summary>Common name of the codec.</summary>
        [Description("Common name of the codec."), Category("GeneralVideoAudioTextImageMenuCommon")]
        public string CodecCommonName { get { return this.codecCommonName; } }

        #endregion

        #region GeneralVideoAudioTextMenu

        ///<summary>Stream delay (e.g. to sync audio/video) in ms.</summary>
        [Description("Stream delay (e.g. to sync audio/video) in ms."), Category("GeneralVideoAudioTextMenu")]
        public int Delay { get { return this.delay; } }

        ///<summary>Duration of the stream in milliseconds.</summary>
        [Description("Duration of the stream in milliseconds."), Category("GeneralVideoAudioTextMenu")]
        public int Duration { get { return this.duration; } }

        #endregion

        #region VideoAudioTextImageMenuCommon

        ///<summary>2-letter (if available) or 3-letter ISO code.</summary>
        [Description("2-letter (if available) or 3-letter ISO code."), Category("VideoAudioTextImageMenuCommon")]
        public string Language { get { return this.language; } }

        #endregion

        #region Menu
        private IDictionary<string, string> _chapters;
        ///<summary>Encoder settings used for encoding this stream (as dictionary).</summary>
        [Description("Movie chapters. (Dictionary)"), Category("Menu")]
        public IDictionary<string, string> Chapters
        {
            get
            {
                if (_chapters == null)
                {
                    _chapters = new Dictionary<string, string>();
                    int chapterBegin = miGetInt("Chapters_Pos_Begin");
                    int chapterEnd = miGetInt("Chapters_Pos_End");
                    for (int i = chapterBegin; i < chapterEnd; i++)
                    {
                        string name = mediaInfo.Get(kind, 0, i, InfoKind.Name);
                        string text = mediaInfo.Get(kind, 0, i, InfoKind.Text);
                        _chapters.Add(name, text);
                    }
                }
                return _chapters;
            }
        }
        #endregion
    }
}
