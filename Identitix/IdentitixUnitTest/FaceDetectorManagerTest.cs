using System;
using System.Collections.Generic;
using System.IO;
using Identitix.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IdentitixUnitTest
{
    [TestClass]
    public class FaceDetectorManagerTest
    {
        #region DM

        private const string SHLOMI_PICS_PATH = @"C:\Users\admin\Pictures\Identitix\Shlomi";
        private const string OHAD_PICS_PATH = @"C:\Users\admin\Pictures\Identitix\Ohad";
        private const string DANY_PICS_PATH = @"C:\Users\admin\Pictures\Identitix\Dany";
        private const string PERSON_GROUP_ID = "steel_rabbits";
        private const string PERSON_GROUP_NAME = "Steel Rabbits";
        private Dictionary<string, string> AllPaths;
        

        #endregion 

        public FaceDetectorManagerTest()
        {
            AllPaths = new Dictionary<string, string>()
            {
                { "Shlomi", SHLOMI_PICS_PATH },
                { "Ohad", OHAD_PICS_PATH},
                { "Dany", DANY_PICS_PATH }                
            };             
        }

        [TestMethod]
        public void CreateGroup()
        {
            bool GroupCreated =
                FaceDetectorManager.CreatePersonGroup(PERSON_GROUP_ID, PERSON_GROUP_NAME, GetAllPics()).Result;

            Assert.AreEqual(true, GroupCreated);
        }

        [TestMethod]
        public void CheckIfShlomi()
        {
            string imagePath = @"C:\Users\admin\Pictures\Identitix\SearchFor\Shlomi.jpg";
            string result;

            using (Stream ShlomiPic = File.OpenRead(imagePath))
            {
                result = FaceDetectorManager.SearchForMember(PERSON_GROUP_ID, ShlomiPic).Result;
            }

            Assert.AreEqual("Shlomi", result);
        }

        [TestMethod]
        public void CheckIfDany()
        {
            string imagePath = @"C:\Users\admin\Pictures\Identitix\SearchFor\Dany.jpg";
            string result;

            using (Stream DanyPic = File.OpenRead(imagePath))
            {
                result = FaceDetectorManager.SearchForMember(PERSON_GROUP_ID, DanyPic).Result;
            }

            Assert.AreEqual("Dany", result);
        }

        [TestMethod]
        public void CheckIfOhad()
        {
            string imagePath = @"C:\Users\admin\Pictures\Identitix\SearchFor\Ohad.jpg";
            string result;

            using (Stream OhadPic = File.OpenRead(imagePath))
            {
                result = FaceDetectorManager.SearchForMember(PERSON_GROUP_ID, OhadPic).Result;
            }

            Assert.AreEqual("Ohad", result);
        }

        private Dictionary<string, List<Stream>> GetAllPics()
        {
            Dictionary<string, List<Stream>> MembersAndPics = new Dictionary<string, List<Stream>>();

            foreach (string currName in AllPaths.Keys)
            {
                List<Stream> MemberPics = new List<Stream>();

                foreach (string imagePath in Directory.GetFiles(AllPaths[currName], "*.jpg"))
                {
                    MemberPics.Add(File.OpenRead(imagePath));
                }

                MembersAndPics.Add(currName, MemberPics);
            }

            return MembersAndPics;
        }
    }
}
