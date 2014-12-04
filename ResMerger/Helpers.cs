using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
    /// The class Helpers represents a collection of helper methods like write log
    /// </para>
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Write messages to file with file name
        /// </summary>
        /// <param name="fileName">fileName (as string)</param>
        /// <param name="messages">messages</param>
        public static void WriteToLog(object fileName, params string[] messages)
        {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            List<string> parameter = new List<string>(messages);

            parameter.Insert(0, DateTime.Now.ToString());
            parameter.Add("---------------------------");

            System.IO.File.AppendAllLines(path + @"/" + fileName.ToString(), parameter);
        }

        /// <summary>
        /// Throw exception with exit code
        /// </summary>
        /// <typeparam name="T">Exception</typeparam>
        /// <param name="message">Exception message</param>
        /// <param name="exitCode">int</param>
        internal static void ThrowException<T>(string message) where T : Exception, new()
        {
            Helpers.WriteToLog("ResMerger.log", message);

            throw (T)Activator.CreateInstance(typeof(T), new object[] { message });
        }
    }
}
