using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EscuelaWeb.Models
{
 //En este model vamos a trabajor toda la conexcion entre nuestra escuela y la base de datos
 //Para hacer haremos que Herede de EntityFramwork DBCONTEXT 
    public class EscuelaContext : DbContext
    {
        public DbSet<Escuela> Escuelas {get;set;}
        public DbSet<Asignatura> Asignaturas {get;set;}
        public DbSet<Curso> Cursos {get;set;}
        public DbSet<Alumno> Alumnos {get;set;}
        public DbSet<Evaluación> Evaluaciones {get;set;}

        //Creamos un constructor especial, recibimos un parametro,el contexto "inyectado". que va a llamar al contructor base.
        //DbContextOptions es generico por eso le tengo que decir cual es el tipo de base de datos para estas opciones
        public EscuelaContext (DbContextOptions<EscuelaContext> options) : base(options)
        {

        }
        
        //OnModelCreating Es un metodo que se ejecuta cuando se esta creando la base de datos, en este caso vamos a sobreescribirlo para poder a
        //agregar datos en la misma. El mismo recibe un parametro denominado ModelBuilder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Le decimos que haga lo que tiene que hacer para no dañarlo y simplemente al terminar, seguimos con lo que queremos.
            base.OnModelCreating(modelBuilder);

            //Segun nuestro Modelo tenemos que:
            
            //Cargar Escuela
            var escuela = new Escuela
            {
                Nombre = "Platzi",
                Id = Guid.NewGuid().ToString(),
                Pais = "Colombia",
                Ciudad = "Bogota",
                Dirección = "Manuel Araujo 2900",
                AñoDeCreación = 2005,
                TipoEscuela = TiposEscuela.Secundaria
            };

            //Cargar Cursos
            var cursos = CargarCursos(escuela);

            //Cargar Asignaturas
            var asignaturas = CargarAsignaturas(cursos);
            
            //Cargar Alumnos
            var alumnos = CargarAlumnos(cursos);
            
            //Cargar Evaluaciones
            var evaluaciones = CargarEvaluaciones(alumnos,asignaturas);

            //Ahora incorporamos a nuestro ModelBuilder todos los datos que ya creamos,Escuela,Cursos,Asignaturas, Alumnos y Evaluaciones.
            modelBuilder.Entity<Escuela>().HasData(escuela);
            modelBuilder.Entity<Curso>().HasData(cursos.ToArray());
            modelBuilder.Entity<Asignatura>().HasData(asignaturas.ToArray());
            modelBuilder.Entity<Alumno>().HasData(alumnos.ToArray());
            modelBuilder.Entity<Evaluación>().HasData(evaluaciones.ToArray());
            
        }
        
#region 
        private static List<Curso> CargarCursos(Escuela escuela)
        {
            return new List<Curso>()
            {
                new Curso(){Id = Guid.NewGuid().ToString(), EscuelaId = escuela.Id, Nombre ="101", Jornada = TiposJornada.Mañana, Dirección = "Manuel Araujo Claypole"},
                new Curso(){Id = Guid.NewGuid().ToString(), EscuelaId = escuela.Id, Nombre ="102", Jornada = TiposJornada.Mañana, Dirección = "Manuel Araujo Claypole"},
                new Curso(){Id = Guid.NewGuid().ToString(), EscuelaId = escuela.Id, Nombre ="201", Jornada = TiposJornada.Tarde, Dirección = "Manuel Araujo Claypole"},
                new Curso(){Id = Guid.NewGuid().ToString(), EscuelaId = escuela.Id, Nombre ="202", Jornada = TiposJornada.Tarde, Dirección = "Manuel Araujo Claypole"},
                new Curso(){Id = Guid.NewGuid().ToString(), EscuelaId = escuela.Id, Nombre ="301", Jornada = TiposJornada.Noche, Dirección = "Manuel Araujo Claypole"},
                new Curso(){Id = Guid.NewGuid().ToString(), EscuelaId = escuela.Id, Nombre ="302", Jornada = TiposJornada.Noche, Dirección = "Manuel Araujo Claypole"}
            };
        }


        private static List<Asignatura> CargarAsignaturas(List<Curso> Cursos)
        {
            var listaCompleta = new List<Asignatura>();
            foreach (var curso in Cursos)
            {
                var tmpList = new List<Asignatura>
                {
                    new Asignatura {Id = Guid.NewGuid().ToString(), CursoId = curso.Id , Nombre = "Matematicas" },
                    new Asignatura {Id = Guid.NewGuid().ToString(), CursoId = curso.Id , Nombre = "Quimica" },
                    new Asignatura {Id = Guid.NewGuid().ToString(), CursoId = curso.Id , Nombre = "Fisica" },
                    new Asignatura {Id = Guid.NewGuid().ToString(), CursoId = curso.Id , Nombre = "Cs Naturales" },
                    new Asignatura {Id = Guid.NewGuid().ToString(), CursoId = curso.Id , Nombre = "Programacion" }
                };
                listaCompleta.AddRange(tmpList);
            }
            return listaCompleta;
        }


        private List<Alumno> CargarAlumnos(List<Curso> cursos)
        {
            var listaAlumnos = new List<Alumno>();

            Random rnd = new Random();
            foreach (var curso in cursos)
            {
                int cantRandom = rnd.Next(5,20);
                var tmpList = GenerarAlumnosAlAzar(cantRandom,curso);
                listaAlumnos.AddRange(tmpList);
            };
            return listaAlumnos;
        }

        private static List<Evaluación> CargarEvaluaciones(List<Alumno> Alumnos , List<Asignatura> Asignaturas)
        {
           
            var listEvaluaciones = new List<Evaluación>();
            var rnb = new Random();
           
            foreach (var asignatura in Asignaturas)
            {
                foreach (var alumno in Alumnos)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var ev = new Evaluación
                        {
                            Id = Guid.NewGuid().ToString(),
                            //Asignatura = asignatura,
                            AsignaturaId = asignatura.Id,
                            Nombre = $"{asignatura.Nombre} Ev#{i + 1}",
                            //Alumno = alumno,
                            AlumnoId = alumno.Id,
                            Nota = MathF.Round(5*(float)rnb.NextDouble(),2)
                        };
                        listEvaluaciones.Add(ev);
                    };
                };
            }
            return listEvaluaciones;
        }


        private List<Alumno> GenerarAlumnosAlAzar(int cantidad, Curso curso)
        {

            string[] nombre1 = { "Alba", "Felipa", "Eusebio", "Farid", "Donald", "Alvaro", "Nicolás" };

            string[] apellido1 = { "Ruiz", "Sarmiento", "Uribe", "Maduro", "Trump", "Toledo", "Herrera" };

            string[] nombre2 = { "Freddy", "Anabel", "Rick", "Murty", "Silvana", "Diomedes", "Nicomedes", "Teodoro" };



            var listaAlumnos = from n1 in nombre1

                               from n2 in nombre2

                               from a1 in apellido1

                               select new Alumno 
                               {  
                                   Nombre = $"{n1} {n2} {a1}" ,
                                   CursoId = curso.Id, 
                                   Id = Guid.NewGuid().ToString() };



            return listaAlumnos.OrderBy((al) => al.Id).Take(cantidad).ToList();

        }
    }
#endregion
}