using ResMerger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;

/*
The MIT License (MIT)

Copyright (c) 2013 - ERGOSIGN http://www.ergosign.de

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
namespace ResMerger
{
    /// <summary>
    /// <para>
    /// The class Data represents a container for Documents and their DependencyCount
    /// </para>
    /// </summary>
    public class Data
    {
        public XDocument Document { get; set; }
        public int DependencyCount { get; set; }

        public Data(XDocument document, int dependencyCount)
        {
            Document = document;
            DependencyCount = dependencyCount;
        }
    }

    /// <summary>
    /// <para>
    /// The class ResMerge copies all resource dictionary entries into one large file respecting the dependencies to increase performance.
    /// </para>
    /// </summary>
    public static class ResourceMerger
    {
        public const string COUNT_EXCEPTION = "Param count must be >= 1 and < 4.";
        public const int COUNT_EXCEPTION_CODE = 0;

        public const string PROJECT_PATH_EXCEPTION = "Project path does not exist.";
        public const int PROJECT_PATH_EXCEPTION_CODE = 1;

        public const string FILE_EXCEPTION = "File does not exist: ";
        public const int FILE_EXCEPTION_CODE = 2;

        public const string SOURCE_EXCEPTION = "Source file does not exist: ";
        public const int SOURCE_EXCEPTION_CODE = 3;

        public const string SOURCE_TYPE_EXCEPTION = "Wrong file type for source - must be .xaml";
        public const int SOURCE_TYPE_EXCEPTION_CODE = 4;

        public const string OUTPUT_TYPE_EXCEPTION = "Wrong file type for output - must be .xaml";
        public const int OUTPUT_TYPE_EXCEPTION_CODE = 5;

        private const string LOG_FILE_NAME = "ResMergerLog.txt";

        /// <summary>
        /// save all resources into one big resource dictionary respecting the dependencies to increase performance
        /// </summary>
        /// <param name="projectPath">project path (C:/..)</param>
        /// <param name="relativeSourceFilePath">relative source file path (/LookAndFeel.xaml)</param>
        /// <param name="relativeOutputFilePath">relative output file path (/xGeneric/Generic.xaml)</param>
        /// <param name="projectName">project name</param>
        /// <param name="resDictString">resource dictionary string (node name)</param>
        public static void MergeResources(string projectPath, string projectName = null, string relativeSourceFilePath = "/LookAndFeel.xaml", string relativeOutputFilePath = "/FullLookAndFeel.xaml")
        {
            // if project path does not exist throw exception
            if (!Directory.Exists(projectPath))
                Helpers.ThrowException<Exception>(PROJECT_PATH_EXCEPTION);

            // Get default values for optional parameters
            projectName = string.IsNullOrEmpty(projectName) ? Path.GetFileName(Path.GetDirectoryName(projectPath)) : projectName;

            // if relativeSourceFilePath is not of type .xaml throw exception
            if (!relativeSourceFilePath.EndsWith(".xaml", StringComparison.InvariantCultureIgnoreCase))
                Helpers.ThrowException<Exception>(SOURCE_TYPE_EXCEPTION);

            // if relativeOutputFilePath is not of type .xaml throw exception
            if (!relativeOutputFilePath.EndsWith(".xaml", StringComparison.InvariantCultureIgnoreCase))
                Helpers.ThrowException<Exception>(OUTPUT_TYPE_EXCEPTION);

            // create sourceFilePath
            var sourceFilePath = projectPath + relativeSourceFilePath;

            // if source file does not exist throw exception
            if (!File.Exists(sourceFilePath))
                Helpers.ThrowException<Exception>(SOURCE_EXCEPTION + sourceFilePath);

            // load source doc
            var sourceDoc = XDocument.Load(sourceFilePath);

            // get default namespace for doc creation and filtering
            var defaultNameSpace = sourceDoc.Root.GetDefaultNamespace();

            // get res dict string
            var resDictString = sourceDoc.Root.Name.LocalName;

            // create output doc
            var outputDoc = XDocument.Parse("<" + resDictString + " xmlns=\"" + defaultNameSpace + "\"/>");

            // create documents
            var documents = new Dictionary<string, Data>();
          
            // add elements
            ResourceMerger.PrepareDocuments(ref documents, projectPath, projectName, relativeSourceFilePath);
       
            // add elements (ordered by dependency count)
            foreach (var item in documents.OrderByDescending(item => item.Value.DependencyCount))
            {
                // add attributes
                foreach (var attribute in item.Value.Document.Root.Attributes())
                    outputDoc.Root.SetAttributeValue(attribute.Name, attribute.Value);

                // add elements
                outputDoc.Root.Add(item.Value.Document.Root.Elements().Where(e => !e.Name.LocalName.StartsWith(resDictString))); 
            }

            // save file
            outputDoc.Save(projectPath + relativeOutputFilePath);
        }


        /// <summary>
        /// Get a collection of resource dictionary source paths respecting the dependencies 
        /// </summary>
        /// <param name="documents">output document collection</param>
        /// <param name="projectPath">project path</param>
        /// <param name="projectName">project name</param>
        /// <param name="relativeSourceFilePath">relative source file path</param>
        /// <param name="resDictString">resource dictionary string (node name)</param>
        /// <param name="firstTime">first time, is LookAndFeel?</param>
        /// <param name="parentDependencyCount">dependency count</param>
        private static void PrepareDocuments(ref Dictionary<string, Data> documents, string projectPath, string projectName, string relativeSourceFilePath, bool firstTime = true, int parentDependencyCount = 0)
        {
            // load current doc
            var absoluteSourceFilePath = projectPath + relativeSourceFilePath;

            // if file does not exist throw exception
            if (!File.Exists(absoluteSourceFilePath))
                Helpers.ThrowException<Exception>(FILE_EXCEPTION + absoluteSourceFilePath);

            // load the doc
            var doc = XDocument.Load(absoluteSourceFilePath);

            // get the corresponding res dict name
            var resDictString = doc.Root.Name.LocalName;

            // get default namespace
            var defaultNameSpace = doc.Root.GetDefaultNamespace();

            // if key already added increase dependency count else add item with dependency count set to 0
            if (documents.ContainsKey(absoluteSourceFilePath))
                documents[absoluteSourceFilePath].DependencyCount = Math.Max(documents[absoluteSourceFilePath].DependencyCount + 1, parentDependencyCount + 1);
            else
                documents.Add(absoluteSourceFilePath, new Data(doc, firstTime ? -1 : parentDependencyCount + 1));

            // call PrepareDocuments() for each merged dictionary
            foreach (var dict in doc.Root.Descendants(defaultNameSpace + resDictString))
                PrepareDocuments(ref documents, projectPath, projectName, dict.Attribute("Source").Value.Replace("/" + projectName + ";component/", string.Empty), false, documents[absoluteSourceFilePath].DependencyCount);
        }
    }
}
