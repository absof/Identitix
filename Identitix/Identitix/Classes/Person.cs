using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Facebook;

namespace Identitix.Classes
{
    public class Person
    {
        #region Consts
        #endregion 

        #region Data Members

        public FacebookClient FacebookAccount { get; set; }
        public byte[] MainPicture { get; set; }
        public List<byte[]> PicturesToCompare { get; set; }      

        #endregion

        #region Ctor

        public Person(string facebookToken)
        {
            this.FacebookAccount = new FacebookClient(facebookToken);       
        }
                                                                  // Call Person(string facebookToken)
        public Person(string facebookToken, byte[] mainPicture) : this(facebookToken)
        {
            this.MainPicture = mainPicture;
        }
                                                                                                // Call Person(string facebookToken, byte[] mainPicture)
        public Person(string facebookToken, byte[] mainPicture, List<byte[]> picturesToCompare) : this(facebookToken, mainPicture)
        {
            this.MainPicture = mainPicture;
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion
    }
}