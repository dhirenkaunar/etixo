using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oxite.Data;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeComment : IComment
    {
        #region IComment Members

        public Guid PostID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IPost Post
        {
            get { throw new NotImplementedException(); }
        }

        public Guid ID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Guid CreatorUserID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IUser CreatorUser
        {
            get { throw new NotImplementedException(); }
        }

        public Guid LanguageID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ILanguage Language
        {
            get { throw new NotImplementedException(); }
        }

        public long CreatorIP
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string UserAgent
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public byte State
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime? Created
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime? Modified
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Title
        {
            get { throw new NotImplementedException(); }
        }

        public string Body
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime? Published
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string CreatorName
        {
            get { throw new NotImplementedException(); }
        }

        public string CreatorEmail
        {
            get { throw new NotImplementedException(); }
        }

        public string CreatorHashedEmail
        {
            get { throw new NotImplementedException(); }
        }

        public string CreatorUrl
        {
            get { throw new NotImplementedException(); }
        }

        public IArea Area
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<ITag> Tags
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
