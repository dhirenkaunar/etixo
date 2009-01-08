﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

#pragma warning disable 1591

namespace Oxite.Search.Live.Search.Soap
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Web.Services.WebServiceBinding(Name = "MSNSearchPortBinding",
        Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public partial class MSNSearchService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {
        private System.Threading.SendOrPostCallback SearchOperationCompleted;

        private bool useDefaultCredentialsSetExplicitly;

        /// <remarks/>
        public MSNSearchService()
        {
            this.Url =
                global::Oxite.Search.Properties.Settings.Default.
                    Oxite_LiveSearchProvider_Live_Search_Soap_MSNSearchService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true))
            {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else
            {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        public new string Url
        {
            get
            {
                return base.Url;
            }
            set
            {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true)
                      && (this.useDefaultCredentialsSetExplicitly == false))
                     && (this.IsLocalFileSystemWebService(value) == false)))
                {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }

        public new bool UseDefaultCredentials
        {
            get
            {
                return base.UseDefaultCredentials;
            }
            set
            {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        /// <remarks/>
        public event SearchCompletedEventHandler SearchCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethod("http://schemas.microsoft.com/MSNSearch/2005/09/fex/Search",
            RequestNamespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex",
            ResponseNamespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex",
            Use = System.Web.Services.Description.SoapBindingUse.Literal,
            ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElement("Response")]
        public SearchResponse Search(SearchRequest Request)
        {
            object[] results = this.Invoke("Search", new object[]
                                                     {
                                                         Request
                                                     });
            return ((SearchResponse)(results[0]));
        }

        /// <remarks/>
        public void SearchAsync(SearchRequest Request)
        {
            this.SearchAsync(Request, null);
        }

        /// <remarks/>
        public void SearchAsync(SearchRequest Request, object userState)
        {
            if ((this.SearchOperationCompleted == null))
            {
                this.SearchOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSearchOperationCompleted);
            }
            this.InvokeAsync("Search", new object[]
                                       {
                                           Request
                                       }, this.SearchOperationCompleted, userState);
        }

        private void OnSearchOperationCompleted(object arg)
        {
            if ((this.SearchCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs =
                    ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SearchCompleted(this,
                                     new SearchCompletedEventArgs(invokeArgs.Results, invokeArgs.Error,
                                                                  invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        private bool IsLocalFileSystemWebService(string url)
        {
            if (((url == null)
                 || (url == string.Empty)))
            {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024)
                 && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0)))
            {
                return true;
            }
            return false;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public partial class SearchRequest
    {
        private string appIDField;

        private string cultureInfoField;

        private SearchFlags flagsField;

        private Location locationField;
        private string queryField;

        private SourceRequest[] requestsField;
        private SafeSearchOptions safeSearchField;

        public SearchRequest()
        {
            this.safeSearchField = SafeSearchOptions.Moderate;
            this.flagsField = SearchFlags.None;
        }

        /// <remarks/>
        public string AppID
        {
            get
            {
                return this.appIDField;
            }
            set
            {
                this.appIDField = value;
            }
        }

        /// <remarks/>
        public string Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }

        /// <remarks/>
        public string CultureInfo
        {
            get
            {
                return this.cultureInfoField;
            }
            set
            {
                this.cultureInfoField = value;
            }
        }

        /// <remarks/>
        public SafeSearchOptions SafeSearch
        {
            get
            {
                return this.safeSearchField;
            }
            set
            {
                this.safeSearchField = value;
            }
        }

        /// <remarks/>
        public SearchFlags Flags
        {
            get
            {
                return this.flagsField;
            }
            set
            {
                this.flagsField = value;
            }
        }

        /// <remarks/>
        public Location Location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItem(IsNullable = false)]
        public SourceRequest[] Requests
        {
            get
            {
                return this.requestsField;
            }
            set
            {
                this.requestsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public enum SafeSearchOptions
    {
        /// <remarks/>
        Moderate,

        /// <remarks/>
        Strict,

        /// <remarks/>
        Off,
    }

    /// <remarks/>
    [System.Flags()]
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public enum SearchFlags
    {
        /// <remarks/>
        None = 1,

        /// <remarks/>
        MarkQueryWords = 2,

        /// <remarks/>
        DisableSpellCorrectForSpecialWords = 4,

        /// <remarks/>
        DisableHostCollapsing = 8,

        /// <remarks/>
        DisableLocationDetection = 16,

        /// <remarks/>
        DisableWebQueryAlteration = 32,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public partial class Location
    {
        private double latitudeField;

        private double longitudeField;

        private double radiusField;

        public Location()
        {
            this.radiusField = 5;
        }

        /// <remarks/>
        public double Latitude
        {
            get
            {
                return this.latitudeField;
            }
            set
            {
                this.latitudeField = value;
            }
        }

        /// <remarks/>
        public double Longitude
        {
            get
            {
                return this.longitudeField;
            }
            set
            {
                this.longitudeField = value;
            }
        }

        /// <remarks/>
        [System.ComponentModel.DefaultValue(5)]
        public double Radius
        {
            get
            {
                return this.radiusField;
            }
            set
            {
                this.radiusField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public partial class MotionThumbnail
    {
        private int fileSizeField;

        private bool fileSizeFieldSpecified;
        private string formatField;
        private int heightField;

        private bool heightFieldSpecified;

        private int runTimeField;

        private bool runTimeFieldSpecified;
        private string uRLField;

        private int widthField;

        private bool widthFieldSpecified;

        /// <remarks/>
        public string URL
        {
            get
            {
                return this.uRLField;
            }
            set
            {
                this.uRLField = value;
            }
        }

        /// <remarks/>
        public string Format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
            }
        }

        /// <remarks/>
        public int RunTime
        {
            get
            {
                return this.runTimeField;
            }
            set
            {
                this.runTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool RunTimeSpecified
        {
            get
            {
                return this.runTimeFieldSpecified;
            }
            set
            {
                this.runTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int Width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool WidthSpecified
        {
            get
            {
                return this.widthFieldSpecified;
            }
            set
            {
                this.widthFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int Height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool HeightSpecified
        {
            get
            {
                return this.heightFieldSpecified;
            }
            set
            {
                this.heightFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int FileSize
        {
            get
            {
                return this.fileSizeField;
            }
            set
            {
                this.fileSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool FileSizeSpecified
        {
            get
            {
                return this.fileSizeFieldSpecified;
            }
            set
            {
                this.fileSizeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public partial class StaticThumbnail
    {
        private int fileSizeField;

        private bool fileSizeFieldSpecified;
        private string formatField;

        private int heightField;

        private bool heightFieldSpecified;
        private string uRLField;
        private int widthField;

        private bool widthFieldSpecified;

        /// <remarks/>
        public string URL
        {
            get
            {
                return this.uRLField;
            }
            set
            {
                this.uRLField = value;
            }
        }

        /// <remarks/>
        public string Format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
            }
        }

        /// <remarks/>
        public int Width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool WidthSpecified
        {
            get
            {
                return this.widthFieldSpecified;
            }
            set
            {
                this.widthFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int Height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool HeightSpecified
        {
            get
            {
                return this.heightFieldSpecified;
            }
            set
            {
                this.heightFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int FileSize
        {
            get
            {
                return this.fileSizeField;
            }
            set
            {
                this.fileSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool FileSizeSpecified
        {
            get
            {
                return this.fileSizeFieldSpecified;
            }
            set
            {
                this.fileSizeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public partial class Video
    {
        private int fileSizeField;

        private bool fileSizeFieldSpecified;
        private string formatField;
        private int heightField;

        private bool heightFieldSpecified;
        private MotionThumbnail motionThumbnailField;
        private string playUrlField;

        private int runTimeField;

        private bool runTimeFieldSpecified;
        private string sourceTitleField;
        private StaticThumbnail staticThumbnailField;

        private int widthField;

        private bool widthFieldSpecified;

        /// <remarks/>
        public string PlayUrl
        {
            get
            {
                return this.playUrlField;
            }
            set
            {
                this.playUrlField = value;
            }
        }

        /// <remarks/>
        public string SourceTitle
        {
            get
            {
                return this.sourceTitleField;
            }
            set
            {
                this.sourceTitleField = value;
            }
        }

        /// <remarks/>
        public string Format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
            }
        }

        /// <remarks/>
        public int RunTime
        {
            get
            {
                return this.runTimeField;
            }
            set
            {
                this.runTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool RunTimeSpecified
        {
            get
            {
                return this.runTimeFieldSpecified;
            }
            set
            {
                this.runTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int Width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool WidthSpecified
        {
            get
            {
                return this.widthFieldSpecified;
            }
            set
            {
                this.widthFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int Height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool HeightSpecified
        {
            get
            {
                return this.heightFieldSpecified;
            }
            set
            {
                this.heightFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int FileSize
        {
            get
            {
                return this.fileSizeField;
            }
            set
            {
                this.fileSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool FileSizeSpecified
        {
            get
            {
                return this.fileSizeFieldSpecified;
            }
            set
            {
                this.fileSizeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public StaticThumbnail StaticThumbnail
        {
            get
            {
                return this.staticThumbnailField;
            }
            set
            {
                this.staticThumbnailField = value;
            }
        }

        /// <remarks/>
        public MotionThumbnail MotionThumbnail
        {
            get
            {
                return this.motionThumbnailField;
            }
            set
            {
                this.motionThumbnailField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public partial class Image
    {
        private int imageFileSizeField;

        private bool imageFileSizeFieldSpecified;
        private int imageHeightField;

        private bool imageHeightFieldSpecified;
        private string imageURLField;

        private int imageWidthField;

        private bool imageWidthFieldSpecified;
        private int thumbnailFileSizeField;

        private bool thumbnailFileSizeFieldSpecified;
        private int thumbnailHeightField;

        private bool thumbnailHeightFieldSpecified;

        private string thumbnailURLField;

        private int thumbnailWidthField;

        private bool thumbnailWidthFieldSpecified;

        /// <remarks/>
        public string ImageURL
        {
            get
            {
                return this.imageURLField;
            }
            set
            {
                this.imageURLField = value;
            }
        }

        /// <remarks/>
        public int ImageWidth
        {
            get
            {
                return this.imageWidthField;
            }
            set
            {
                this.imageWidthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool ImageWidthSpecified
        {
            get
            {
                return this.imageWidthFieldSpecified;
            }
            set
            {
                this.imageWidthFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int ImageHeight
        {
            get
            {
                return this.imageHeightField;
            }
            set
            {
                this.imageHeightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool ImageHeightSpecified
        {
            get
            {
                return this.imageHeightFieldSpecified;
            }
            set
            {
                this.imageHeightFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int ImageFileSize
        {
            get
            {
                return this.imageFileSizeField;
            }
            set
            {
                this.imageFileSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool ImageFileSizeSpecified
        {
            get
            {
                return this.imageFileSizeFieldSpecified;
            }
            set
            {
                this.imageFileSizeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string ThumbnailURL
        {
            get
            {
                return this.thumbnailURLField;
            }
            set
            {
                this.thumbnailURLField = value;
            }
        }

        /// <remarks/>
        public int ThumbnailWidth
        {
            get
            {
                return this.thumbnailWidthField;
            }
            set
            {
                this.thumbnailWidthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool ThumbnailWidthSpecified
        {
            get
            {
                return this.thumbnailWidthFieldSpecified;
            }
            set
            {
                this.thumbnailWidthFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int ThumbnailHeight
        {
            get
            {
                return this.thumbnailHeightField;
            }
            set
            {
                this.thumbnailHeightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool ThumbnailHeightSpecified
        {
            get
            {
                return this.thumbnailHeightFieldSpecified;
            }
            set
            {
                this.thumbnailHeightFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int ThumbnailFileSize
        {
            get
            {
                return this.thumbnailFileSizeField;
            }
            set
            {
                this.thumbnailFileSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool ThumbnailFileSizeSpecified
        {
            get
            {
                return this.thumbnailFileSizeFieldSpecified;
            }
            set
            {
                this.thumbnailFileSizeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public partial class SearchTag
    {
        private string nameField;

        private string valueField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public partial class Address
    {
        private string addressLineField;
        private string countryRegionField;

        private string formattedAddressField;
        private string postalCodeField;

        private string primaryCityField;

        private string secondaryCityField;

        private string subdivisionField;

        /// <remarks/>
        public string AddressLine
        {
            get
            {
                return this.addressLineField;
            }
            set
            {
                this.addressLineField = value;
            }
        }

        /// <remarks/>
        public string PrimaryCity
        {
            get
            {
                return this.primaryCityField;
            }
            set
            {
                this.primaryCityField = value;
            }
        }

        /// <remarks/>
        public string SecondaryCity
        {
            get
            {
                return this.secondaryCityField;
            }
            set
            {
                this.secondaryCityField = value;
            }
        }

        /// <remarks/>
        public string Subdivision
        {
            get
            {
                return this.subdivisionField;
            }
            set
            {
                this.subdivisionField = value;
            }
        }

        /// <remarks/>
        public string PostalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }

        /// <remarks/>
        public string CountryRegion
        {
            get
            {
                return this.countryRegionField;
            }
            set
            {
                this.countryRegionField = value;
            }
        }

        /// <remarks/>
        public string FormattedAddress
        {
            get
            {
                return this.formattedAddressField;
            }
            set
            {
                this.formattedAddressField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public partial class DateTime
    {
        private int dayField;

        private int hourField;

        private int minuteField;
        private int monthField;

        private int secondField;
        private int yearField;

        /// <remarks/>
        public int Year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
            }
        }

        /// <remarks/>
        public int Month
        {
            get
            {
                return this.monthField;
            }
            set
            {
                this.monthField = value;
            }
        }

        /// <remarks/>
        public int Day
        {
            get
            {
                return this.dayField;
            }
            set
            {
                this.dayField = value;
            }
        }

        /// <remarks/>
        public int Hour
        {
            get
            {
                return this.hourField;
            }
            set
            {
                this.hourField = value;
            }
        }

        /// <remarks/>
        public int Minute
        {
            get
            {
                return this.minuteField;
            }
            set
            {
                this.minuteField = value;
            }
        }

        /// <remarks/>
        public int Second
        {
            get
            {
                return this.secondField;
            }
            set
            {
                this.secondField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public partial class Result
    {
        private Address addressField;
        private string cacheUrlField;

        private DateTime dateTimeField;
        private string descriptionField;
        private string displayUrlField;
        private Image imageField;

        private Location locationField;
        private string phoneField;
        private string resultTypeField;

        private SearchTag[] searchTagsArrayField;
        private string searchTagsField;
        private string sourceField;

        private string summaryField;
        private string titleField;
        private string urlField;

        private Video videoField;

        /// <remarks/>
        public string Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string Url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }

        /// <remarks/>
        public string DisplayUrl
        {
            get
            {
                return this.displayUrlField;
            }
            set
            {
                this.displayUrlField = value;
            }
        }

        /// <remarks/>
        public string CacheUrl
        {
            get
            {
                return this.cacheUrlField;
            }
            set
            {
                this.cacheUrlField = value;
            }
        }

        /// <remarks/>
        public string Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        /// <remarks/>
        public string SearchTags
        {
            get
            {
                return this.searchTagsField;
            }
            set
            {
                this.searchTagsField = value;
            }
        }

        /// <remarks/>
        public string Phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }

        /// <remarks/>
        public DateTime DateTime
        {
            get
            {
                return this.dateTimeField;
            }
            set
            {
                this.dateTimeField = value;
            }
        }

        /// <remarks/>
        public Address Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        public Location Location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItem(IsNullable = false)]
        public SearchTag[] SearchTagsArray
        {
            get
            {
                return this.searchTagsArrayField;
            }
            set
            {
                this.searchTagsArrayField = value;
            }
        }

        /// <remarks/>
        public string Summary
        {
            get
            {
                return this.summaryField;
            }
            set
            {
                this.summaryField = value;
            }
        }

        /// <remarks/>
        public string ResultType
        {
            get
            {
                return this.resultTypeField;
            }
            set
            {
                this.resultTypeField = value;
            }
        }

        /// <remarks/>
        public Image Image
        {
            get
            {
                return this.imageField;
            }
            set
            {
                this.imageField = value;
            }
        }

        /// <remarks/>
        public Video Video
        {
            get
            {
                return this.videoField;
            }
            set
            {
                this.videoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public partial class SourceResponse
    {
        private int offsetField;

        private string recourseQueryField;

        private Result[] resultsField;
        private SourceType sourceField;
        private int totalField;

        /// <remarks/>
        public SourceType Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        /// <remarks/>
        public int Offset
        {
            get
            {
                return this.offsetField;
            }
            set
            {
                this.offsetField = value;
            }
        }

        /// <remarks/>
        public int Total
        {
            get
            {
                return this.totalField;
            }
            set
            {
                this.totalField = value;
            }
        }

        /// <remarks/>
        public string RecourseQuery
        {
            get
            {
                return this.recourseQueryField;
            }
            set
            {
                this.recourseQueryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItem(IsNullable = false)]
        public Result[] Results
        {
            get
            {
                return this.resultsField;
            }
            set
            {
                this.resultsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public enum SourceType
    {
        /// <remarks/>
        Web,

        /// <remarks/>
        News,

        /// <remarks/>
        Ads,

        /// <remarks/>
        InlineAnswers,

        /// <remarks/>
        PhoneBook,

        /// <remarks/>
        WordBreaker,

        /// <remarks/>
        Spelling,

        /// <remarks/>
        QueryLocation,

        /// <remarks/>
        Image,

        /// <remarks/>
        Video,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public partial class SearchResponse
    {
        private SourceResponse[] responsesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItem(IsNullable = false)]
        public SourceResponse[] Responses
        {
            get
            {
                return this.responsesField;
            }
            set
            {
                this.responsesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public partial class SourceRequest
    {
        private int countField;

        private string fileTypeField;
        private int offsetField;

        private ResultFieldMask resultFieldsField;

        private string[] searchTagFiltersField;
        private SortByType sortByField;
        private SourceType sourceField;

        public SourceRequest()
        {
            this.sourceField = SourceType.Web;
            this.offsetField = 0;
            this.countField = 10;
            this.sortByField = SortByType.Default;
            this.resultFieldsField = ((ResultFieldMask.Title | ResultFieldMask.Description)
                                      | ResultFieldMask.Url);
        }

        /// <remarks/>
        public SourceType Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        /// <remarks/>
        public int Offset
        {
            get
            {
                return this.offsetField;
            }
            set
            {
                this.offsetField = value;
            }
        }

        /// <remarks/>
        public int Count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }

        /// <remarks/>
        public string FileType
        {
            get
            {
                return this.fileTypeField;
            }
            set
            {
                this.fileTypeField = value;
            }
        }

        /// <remarks/>
        [System.ComponentModel.DefaultValue(SortByType.Default)]
        public SortByType SortBy
        {
            get
            {
                return this.sortByField;
            }
            set
            {
                this.sortByField = value;
            }
        }

        /// <remarks/>
        public ResultFieldMask ResultFields
        {
            get
            {
                return this.resultFieldsField;
            }
            set
            {
                this.resultFieldsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItem(IsNullable = false)]
        public string[] SearchTagFilters
        {
            get
            {
                return this.searchTagFiltersField;
            }
            set
            {
                this.searchTagFiltersField = value;
            }
        }
    }

    /// <remarks/>
    [System.Flags()]
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public enum SortByType
    {
        /// <remarks/>
        Default = 1,

        /// <remarks/>
        Relevance = 2,

        /// <remarks/>
        Distance = 4,
    }

    /// <remarks/>
    [System.Flags()]
    [System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.3053")]
    [System.Serializable()]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/MSNSearch/2005/09/fex")]
    public enum ResultFieldMask
    {
        /// <remarks/>
        All = 1,

        /// <remarks/>
        Title = 2,

        /// <remarks/>
        Description = 4,

        /// <remarks/>
        Url = 8,

        /// <remarks/>
        DisplayUrl = 16,

        /// <remarks/>
        CacheUrl = 32,

        /// <remarks/>
        Source = 64,

        /// <remarks/>
        SearchTags = 128,

        /// <remarks/>
        Phone = 256,

        /// <remarks/>
        DateTime = 512,

        /// <remarks/>
        Address = 1024,

        /// <remarks/>
        Location = 2048,

        /// <remarks/>
        SearchTagsArray = 4096,

        /// <remarks/>
        Summary = 8192,

        /// <remarks/>
        ResultType = 16384,

        /// <remarks/>
        Image = 32768,

        /// <remarks/>
        Video = 65536,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Web.Services", "2.0.50727.3053")]
    public delegate void SearchCompletedEventHandler(object sender, SearchCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class SearchCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;

        internal SearchCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
            :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SearchResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SearchResponse)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591