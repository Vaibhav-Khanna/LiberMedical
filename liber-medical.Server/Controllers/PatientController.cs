﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using liber_medical.Server.DataObjects;
using liber_medical.Server.Models;
using Microsoft.WindowsAzure.Mobile.Service;

namespace liber_medical.Server.Controllers
{
	public class PatientController : TableController<Patient>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Patient>(context, Request, Services);
        }

        // GET tables/Patient
        public IQueryable<Patient> GetAllPatient()
        {
            return Query(); 
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