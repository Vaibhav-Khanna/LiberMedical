using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using libermedical.Server.DataObjects;
using libermedical.Server.Models;
using Microsoft.Azure.Mobile.Server;

namespace libermedical.Server.Controllers
{
	//    [Authorize]
	public class PatientController : TableController<Patient>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            ServerDBContext context = new ServerDBContext();
            DomainManager = new EntityDomainManager<Patient>(context, Request);
        }


        string GetSid(IPrincipal user)
        {
            ClaimsPrincipal claimsUser = (ClaimsPrincipal)user;

            string provider = claimsUser.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider").Value;
            string sid = claimsUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            // The above assumes WEBSITE_AUTH_HIDE_DEPRECATED_SID is true. Otherwise, use the stable_sid claim:
            // string sid = claimsUser.FindFirst("stable_sid").Value; 

            return $"{provider}|{sid}";
        }

        // GET tables/Patient
        public IQueryable<Patient> GetAllPatient()
        {

//          var sid = GetSid(User);
	        return Query().Where(c => c.UserId == "1"); 
        }

        // GET tables/Patient/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Patient> GetPatient(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Patient/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Patient> PatchPatient(string id, Delta<Patient> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Patient
        public async Task<IHttpActionResult> PostPatient(Patient item)
        {
//            var sid = GetSid(User);
            item.UserId = "1";

            Patient current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Patient/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePatient(string id)
        {
             return DeleteAsync(id);
        }
    }
}
