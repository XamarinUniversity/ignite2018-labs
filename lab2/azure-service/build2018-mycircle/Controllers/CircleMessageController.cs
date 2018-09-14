using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using build2018_mycircle.DataObjects;
using build2018_mycircle.Models;

namespace build2018_mycircle.Controllers
{
    public class CircleMessageController : TableController<CircleMessage>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<CircleMessage>(context, Request);
        }

        // GET tables/CircleMessage
        public IQueryable<CircleMessage> GetAllCircleMessage()
        {
            return Query(); 
        }

        // GET tables/CircleMessage/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<CircleMessage> GetCircleMessage(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/CircleMessage/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<CircleMessage> PatchCircleMessage(string id, Delta<CircleMessage> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/CircleMessage
        public async Task<IHttpActionResult> PostCircleMessage(CircleMessage item)
        {
            CircleMessage current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/CircleMessage/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteCircleMessage(string id)
        {
             return DeleteAsync(id);
        }
    }
}
