﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using efcv2.Models;

namespace efcv2.Controllers
{
    public class ValuesController : ApiController
    {
        private efcv2DBEntities db = new efcv2DBEntities();

        // GET: api/Values
        [HttpGet]
        [Route("api/values/all")]
        public IQueryable<Value> GetValues()
        {
            return db.Values;
        }

        // GET: api/Values/5
        [ResponseType(typeof(Value))]
        [Route("api/values/{id}")]
        public IHttpActionResult GetValue(long id)
        {
            //Value value = db.Values.Find(id);
            //if (value == null)
            //{
            //    return NotFound();
            //}

            return Ok(id+1);
        }

        // PUT: api/Values/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutValue(long id, Value value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != value.ValueID)
            {
                return BadRequest();
            }

            db.Entry(value).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("api/value")]
        public IHttpActionResult kaneKati(IEnumerable<long> valueList)
        {
            foreach (long v in valueList)
            {
                Valuesi value = new Valuesi();
                value.Value = v.ToString();
                value.TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                value.SensoriID = 1;
                db.Valuesis.Add(value);
            }
            db.SaveChanges();
            return Ok(valueList);
        }


        // DELETE: api/Values/5
        [ResponseType(typeof(Value))]
        public IHttpActionResult DeleteValue(long id)
        {
            Value value = db.Values.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            db.Values.Remove(value);
            db.SaveChanges();

            return Ok(value);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ValueExists(long id)
        {
            return db.Values.Count(e => e.ValueID == id) > 0;
        }
    }
}