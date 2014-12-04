using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResMerger;

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
namespace ResMergerTest
{
    /// <summary>
    /// <para>
    /// The class ResMergerTest represents test class to ensure the res merger is working as expected
    /// </para>
    /// </summary>
    [TestClass]
    public class ResMergerTest
    {
        private string PROJECT_PATH = Environment.CurrentDirectory + @"/../../../../../ProjectTemplates/A.Showcase/A.Styling/";
        private string PROJECT_NAME = "A.Styling";
        private string RELATIVE_SOURCE_PATH = "/LookAndFeel.xaml";
        private string RELATIVE_OUTPUT_PATH = "/FullLookAndFeel.xaml";
        private string WRONG = "wrong";

        [TestMethod]
        public void ParamCountGreater4()
        {
            ExceptionAssert.Throws<Exception>(
                () => Program.Main(new string[]
                {
                    PROJECT_PATH,
                    PROJECT_NAME,
                    RELATIVE_SOURCE_PATH,
                    RELATIVE_OUTPUT_PATH,
                    RELATIVE_OUTPUT_PATH,
                }), ResourceMerger.COUNT_EXCEPTION);
        }

        [TestMethod]
        public void ParamCountSmaller1()
        {
            ExceptionAssert.Throws<Exception>(
                () => Program.Main(new string[0]), ResourceMerger.COUNT_EXCEPTION);
        }

        [TestMethod]
        public void AllParamsSet()
        {
            Program.Main(new string[]
            {
                PROJECT_PATH,
                PROJECT_NAME,
                RELATIVE_SOURCE_PATH,
                RELATIVE_OUTPUT_PATH
            });
        }

        [TestMethod]
        public void WrongProjectPathSet()
        {
            ExceptionAssert.Throws<Exception>(
                () => Program.Main(new string[]
                {
                    PROJECT_PATH + WRONG,
                }), ResourceMerger.PROJECT_PATH_EXCEPTION);
        }

        [TestMethod]
        public void WrongProjectNameSet()
        {
            ExceptionAssert.Throws<Exception>(
                () => Program.Main(new string[]
                {
                    PROJECT_PATH,
                    PROJECT_NAME + WRONG,
                }), ResourceMerger.FILE_EXCEPTION, true);
        }

        [TestMethod]
        public void WrongSourceFilePathSet()
        {
            ExceptionAssert.Throws<Exception>(
                () => Program.Main(new string[]
                {
                    PROJECT_PATH,
                    PROJECT_NAME,
                    WRONG + RELATIVE_SOURCE_PATH
                }), ResourceMerger.SOURCE_EXCEPTION, true);
        }

        [TestMethod]
        public void WrongSourceFileType()
        {
            ExceptionAssert.Throws<Exception>(
                () => Program.Main(new string[]
                {
                    PROJECT_PATH,
                    PROJECT_NAME,
                    RELATIVE_SOURCE_PATH + WRONG
                }), ResourceMerger.SOURCE_TYPE_EXCEPTION);
        }

        [TestMethod]
        public void WrongOutputFileType()
        {
            ExceptionAssert.Throws<Exception>(
                () => Program.Main(new string[]
                {
                    PROJECT_PATH,
                    PROJECT_NAME,
                    RELATIVE_SOURCE_PATH,
                    RELATIVE_OUTPUT_PATH + WRONG,
                }), ResourceMerger.OUTPUT_TYPE_EXCEPTION);
        }
    }
}
