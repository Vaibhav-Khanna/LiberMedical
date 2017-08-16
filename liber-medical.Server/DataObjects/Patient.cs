using Microsoft.Azure.Mobile.Server;

namespace libermedical.Server.DataObjects
{
	public class Patient : EntityData
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [Newtonsoft.Json.JsonProperty("userId")]
        public string UserId { get; set; }

	    public string FirstName { set; get; }

	    public string LastName { set; get; }
		
    }
}