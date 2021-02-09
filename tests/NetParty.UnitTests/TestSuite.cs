using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.IO;

namespace NetParty.UnitTests
{
    /// <summary>
    /// Class to costumize tests environment
    /// </summary>
    [TestClass]
    public class TestSuite
    {
        #region Properties

        internal string testDataPath;
        internal string binPath;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [SetUp]
        public void Init()
        {
            binPath = AppDomain.CurrentDomain.BaseDirectory;
            testDataPath = Path.Combine(Directory.GetParent(binPath).Parent.Parent.Parent.FullName, "testdata");
            CopyFolderData(testDataPath, binPath);
        }

        private void CopyFolderData(string sourcePath, string destinationPath)
        {
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
        }

        #endregion Methods

    }
}
