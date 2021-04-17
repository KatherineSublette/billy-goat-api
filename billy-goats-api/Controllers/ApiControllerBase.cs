using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using BillyGoats.Api.Data.Services;
using BillyGoats.Api.Utils;
using BillyGoats.Api.Filter;

namespace BillyGoats.Api.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase<T> : ControllerBase where T : class
    {
        protected readonly DataService<T> DataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Bolstra.Api.Controllers.Base.ApiControllerBase`1"/> class.
        /// </summary>
        /// <param name="dataService">Data service.</param>
        public ApiControllerBase(IDataService<T> dataService)
        {
            this.DataService = dataService as DataService<T>;
        }

        /// <summary>
        /// Get the specified includes and filter.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="expand">expand.</param>
        /// <param name="filter">Filter.</param>
        [HttpGet]
        public virtual async Task<ActionResult<ICollection<T>>> Get([ModelBinder(typeof(FilterBinder))] FilterRequest filter)
        {
            var ret = await DataService.Get(filter);

            return Ok(ret);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <returns>The by identifier.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="expand">Expand.</param>
        [HttpGet]
        [Route("{id:long}")]
        public virtual async Task<ActionResult<T>> GetById(long id, [SeparatedBy(",")]string[] expand)
        {
            var data = await DataService.GetById(id, expand);

            if (data == null)
            {
                return NotFound();
            }

            DataService.Resolve(data, expand);
            return this.Ok(data);
        }

        /// <summary>
        /// Create a new Entity object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ActionResult> Add(T data)
        {
            var ret = await this.DataService.Add(data);
            return this.Ok(ret);
        }

        /// <summary>
        /// Update an existing entity object
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:long}")]
        public virtual async Task<ActionResult> Update(T data, long id)
        {
            var item = await this.DataService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

            var ret = await this.DataService.Update(data);

            return this.Ok(ret);
        }

        /// <summary>
        /// Delete an existing entity object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:long}")]
        public virtual async Task<ActionResult> Delete(long id)
        {
            await this.DataService.Delete(id);
            return this.Ok();
        }

        /// <summary>
        /// Patch the specified objectPatch and id.
        /// </summary>
        /// <returns>The patch.</returns>
        /// <param name="objectPatch">Object patch.</param>
        /// <param name="id">Identifier.</param>
        [HttpPatch]
        [Route("{id:long}")]
        public virtual async Task<ActionResult> Patch(JsonPatchDocument<T> objectPatch, long id)
        {
            var ret = await this.DataService.Patch(objectPatch, id);
            return this.Ok(ret);
        }
    }
}
