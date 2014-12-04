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
    /// The class Programm represents a console application that calls the ResourceMerger.MergeResources()-Method
    /// </para>
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Call ResourceMerger.MergeResources(..)
        /// </summary>
        /// <param name="args">
        /// 0 = project path
        /// 1 = project name
        /// 2 = relative source path
        /// 3 = relative output path
        /// </param>
        public static void Main(string[] args)
        {
            var count = args.Count();

            // if count is not in valid range throw exception
            if (count < 1 || count > 4)
                Helpers.ThrowException<Exception>(ResourceMerger.COUNT_EXCEPTION);

            switch (count)
            {
                case 1: ResourceMerger.MergeResources(args[0]); break;
                case 2: ResourceMerger.MergeResources(args[0], args[1]); break;
                case 3: ResourceMerger.MergeResources(args[0], args[1], args[2]); break;
                case 4: ResourceMerger.MergeResources(args[0], args[1], args[2], args[3]); break;
            }
        }
    }
}
