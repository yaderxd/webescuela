using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace webescuela.Controllers
{
    public class AlumnosController : ApiController
    {
        [HttpGet]

        public IHttpActionResult Get()
        {
            List<universitarios> list = new List<universitarios>();

            using (Models.escuelaEntities db = new Models.escuelaEntities())
            {
                list = (from l in db.Alumnos
                        select new universitarios
                        {
                            IdEstudiante = l.IdAlumno,
                            Apellidos = l.Apellidos,
                            Nombre = l.Nombre,
                            Direccion = l.Direccion,
                            FechaIngreso = l.FechaIngreso,
                            Telefono = l.Telefono,
                        }).ToList();
            }

            return Ok(list);
        }


        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            universitarios estudiante = null;
            using (Models.escuelaEntities db = new Models.escuelaEntities())
            {
                estudiante = db.Alumnos.Where(l => l.IdAlumno == id)
                    .Select(l => new universitarios()
                    {
                        IdEstudiante = l.IdAlumno,
                        Apellidos = l.Apellidos,
                        Nombre = l.Nombre,
                        Direccion = l.Direccion,
                        FechaIngreso = l.FechaIngreso,
                        Telefono = l.Telefono,
                    }).FirstOrDefault<universitarios>();

            }
            if (estudiante == null)
                return NotFound();
            return Ok(estudiante);
        }

        [HttpPost]
        public IHttpActionResult Add(universitarios estudiantes)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            using (Models.escuelaEntities db = new Models.escuelaEntities())
            {
                var oEstudiante = new Models.Alumnos();
                oEstudiante.Apellidos = estudiantes.Apellidos;
                oEstudiante.Nombre = estudiantes.Nombre;
                oEstudiante.Direccion = estudiantes.Direccion;
                oEstudiante.FechaIngreso = estudiantes.FechaIngreso;
                oEstudiante.Telefono = estudiantes.Telefono;
                db.Alumnos.Add(oEstudiante);
                db.SaveChanges();
            }
            return Ok("Registro agregado correctamente");
        }

        [HttpPut]
        public IHttpActionResult Put(universitarios model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            using (var db = new Models.escuelaEntities())
            {
                var oEstudiante = db.Alumnos.Where(l => l.IdAlumno == model.IdEstudiante)
                    .FirstOrDefault<Models.Alumnos>();
                if (oEstudiante != null)
                {
                    oEstudiante.Apellidos = model.Apellidos;
                    oEstudiante.Nombre = model.Nombre;
                    oEstudiante.Direccion = model.Direccion;
                    oEstudiante.FechaIngreso = model.FechaIngreso;
                    oEstudiante.Telefono = model.Telefono;
                    db.SaveChanges();
                }
                else
                    return NotFound();
            }
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("No es un id de un Estudiante válido");
            using (Models.escuelaEntities db = new Models.escuelaEntities())
            {
                var estudiante = db.Alumnos.Where(l => l.IdAlumno == id)
                    .FirstOrDefault();
                db.Entry(estudiante).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            return Ok();
        }
    }
}