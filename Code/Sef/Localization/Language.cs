using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

using Sef.Xml;

namespace Sef.Localization
{
    public interface ILanguage
    {
        String Name
        { get; }

        String Culture
        { get; }

        ILanguageCommon Common
        { get; }

        ILanguageErrors Errors
        { get; }
    }

    [Serializable, XmlRoot(RootName)]
    public class Language : ILanguage
    {
        #region Constants

        [XmlIgnore]
        public const String RootName = "Language";
        [XmlIgnore]
        public const String AttributeName = "Name";
        [XmlIgnore]
        public const String AttributeCulture = "Culture";
        [XmlIgnore]
        public const String ElementCommon = "Common";
        [XmlIgnore]
        public const String ElementErrors = "Errors";
        [XmlIgnore]
        public const String ElementEditor = "Editor";
        [XmlIgnore]
        public const String DefaultCulture = "ru-RU";
        [XmlIgnore]
        public const String DefaultName = "Русский";
        [XmlIgnore]
        public const String FileFormat = "*.xml";
        [XmlIgnore]
        public const String FolderPath = "Localization";

        #endregion

        #region Properties

        [XmlIgnore]
        public String FileName
        { get; protected set; }

        [XmlAttribute(AttributeName)]
        public String Name
        { get; set; }

        [XmlAttribute(AttributeCulture)]
        public String Culture
        { get; set; }

        [XmlElement(ElementCommon)]
        public LanguageCommon CommonXml
        { get; set; }

        [XmlElement(ElementErrors)]
        public LanguageErrors ErrorsXml
        { get; set; }

        [XmlIgnore]
        public ILanguageCommon Common
        { get { return CommonXml; } }

        [XmlIgnore]
        public ILanguageErrors Errors
        { get { return ErrorsXml; } }

        #endregion

        #region Singleton

        [XmlIgnore]
        public static Language Current
        { get; protected set; }
        
        [XmlIgnore]
        public static Language Default
        { get; protected set; }

        static Language()
        {
            Current = Default = new Language
            {
                FileName = String.Empty,
                Name = DefaultName,
                Culture = DefaultCulture,
                CommonXml = LanguageCommon.CreateDefault(),
                ErrorsXml = LanguageErrors.CreateDefault(),
            };
        }

        public static List<LanguageT> LoadAdditional<LanguageT>(String applicationPath)
            where LanguageT : Language, new()
        {
            var languagesFolder = new DirectoryInfo(Path.Combine(applicationPath, FolderPath));
            if (languagesFolder.Exists)
            {
                var languageFiles = languagesFolder.GetFiles(FileFormat);
                return languageFiles.Length > 0
                    ? languageFiles.Select(f => f.FullName.DeserializeFromFile<LanguageT>()).ToList()
                    : new List<LanguageT>();
            }
            else
            {
                languagesFolder.Create();
                return new List<LanguageT>();
            }
        }

        public static LanguageT FindAppropriate<LanguageT>(IEnumerable<LanguageT> all, LanguageT defaultValue)
            where LanguageT : Language
        {
            return all.FirstOrDefault(l => l.Culture == Thread.CurrentThread.CurrentUICulture.Name) ?? defaultValue;
        }

        #endregion
        
        public override string ToString()
        {
            return Name;
        }
    }

    public class Localizator : ILanguage
    {
        public string Name
        {
            get
            {
                return Language.Current != null
                    ? Language.Current.Name
                    : null;
            }
        }

        public string Culture
        {
            get
            {
                return Language.Current != null
                    ? Language.Current.Culture
                    : null;
            }
        }

        public ILanguageCommon Common
        {
            get
            {
                return Language.Current != null
                    ? Language.Current.Common
                    : null;
            }
        }

        public ILanguageErrors Errors
        {
            get
            {
                return Language.Current != null
                    ? Language.Current.Errors
                    : null;
            }
        }
    }
}
