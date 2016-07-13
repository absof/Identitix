using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Facebook;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace Identitix.Classes
{
    /// <summary>
    /// This static class should be the "Gate" to the face detection API.
    /// (Microsoft/Any other product) here we'll send the images from
    ///  the facebook account with the face we want to check.
    /// </summary>
    public static class FaceDetectorManager
    {
        #region Consts

        private const string SUBSCRIPTION_TOKEN = "01881badde7147519a3ad44d21c0af44";

        #endregion

        #region Data Members

        private static IFaceServiceClient faceServiceClient;
        
        #endregion

        #region Ctor

        static FaceDetectorManager()
        {
            faceServiceClient = new FaceServiceClient(SUBSCRIPTION_TOKEN);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personGroupID"></param>
        /// <param name="personGroupName"></param>
        /// <param name="namesAndPics"></param>
        /// <returns></returns>
        public static async Task<bool> CreatePersonGroup(string personGroupID, 
                                                         string personGroupName, 
                                                         Dictionary<string, List<Stream>> namesAndPics)
        {
            ////////////////////////////// for debug /////////////////////////////////////
            if (IsGroupExist(personGroupID).Result)
            {
                faceServiceClient.DeletePersonGroupAsync(personGroupID).Wait();
            }
            //////////////////////////////////////////////////////////////////////////////

            bool created = false;

            try
            {
                // Create the group
                await faceServiceClient.CreatePersonGroupAsync(personGroupID, personGroupName);


                // Add the members
                foreach (var currName in namesAndPics.Keys)
                {
                    CreatePersonResult currentFriend =
                        await faceServiceClient.CreatePersonAsync(personGroupID, currName);

                    // Add the Pics of the current member
                    foreach (Stream currPic in namesAndPics[currName])
                    {
                        // TODO: catch the exeption when no face detected.
                        await faceServiceClient.AddPersonFaceAsync(personGroupID, 
                                                                   currentFriend.PersonId, 
                                                                   currPic);
                    }
                }

                // Train Group
                await faceServiceClient.TrainPersonGroupAsync(personGroupID).ContinueWith(x =>
                {
                    created = true;
                }
            );

                // Waiting until finish the training
                //TrainingStatus trainingStatus = null;

                //while (true)
                //{
                //    trainingStatus = await faceServiceClient.GetPersonGroupTrainingStatusAsync(personGroupID);

                //    if (trainingStatus.Status != Status.Running)
                //    {
                //        if (trainingStatus.Status == Status.Succeeded)
                //        {
                //            created = true;
                //        }
                        
                //        break;
                //    }

                //    await Task.Delay(1000);
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return created;
        }

        public static async Task<string> SearchForMember(string personGroupID, Stream MemberPic)
        {
            string memberName = "NotFound!";
            
            try
            {
                // Get the face points of the member from the picture
                var faces = await faceServiceClient.DetectAsync(MemberPic);
                var faceIds = faces.Select(face => face.FaceId).ToArray();
                var results = await faceServiceClient.IdentifyAsync(personGroupID, faceIds);

                foreach (var identifyResult in results)
                {
                    if (identifyResult.Candidates.Length != 0)
                    {
                        // Get top 1 among all candidates returned
                        var candidateId = identifyResult.Candidates[0].PersonId;
                        var person = await faceServiceClient.GetPersonAsync(personGroupID, candidateId);
                        memberName = person.Name;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return (memberName);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static async Task<bool> IsGroupExist(string groupId)
        {
            bool Exist;

            try
            {
                PersonGroup check = faceServiceClient.GetPersonGroupAsync(groupId).Result;
                Exist = true;
            }
            catch (Exception)
            {
                Exist = false;    
            }

            return (Exist);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static async Task<bool> IsMemberExist(string groupId, string name)
        {
            bool Exist = false;

            Person[] checkGroup = faceServiceClient.GetPersonsAsync(groupId).Result;
            Person checkPerson = checkGroup.FirstOrDefault(currPerson => currPerson.Name == name);

            if (checkPerson != null)
            {
                Exist = true;
            }

            return (Exist);
        }

        #endregion
    }
}