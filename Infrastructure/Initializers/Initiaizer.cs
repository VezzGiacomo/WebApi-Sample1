using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Domain.Aggregates.Cinemas;
using Domain.Aggregates.Films;
using Domain.Aggregates.Sessions;

namespace Infrastructure.Initializers
{
    public static class Initiaizer
    {
        public static void Seed(DatabaseContext context)
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //{
            //    System.Diagnostics.Debugger.Launch();
            //}

            SeedEstructura(context);
            SeedProgramacion(context);
        }

        private static void SeedEstructura(DatabaseContext context)
        {
            //
            // Create la estructura de los cines
            //
            Cinema[] cinemas = {new Cinema("Palafox")
                ,new Cinema("Cervantes") };
            foreach (Cinema cine in cinemas)
            {
                cine.CreateScreen(name: "Aneto", rows: 5, seatsPerRow: 5);
                cine.CreateScreen(name: "Monte Perdido", rows: 6, seatsPerRow: 6);
                context.Cinemas.AddOrUpdate(c => c.Name, cine);
            }

            //
            // Crea los films 
            //
            Random rndDuracion = new Random();
            Film[] films = {new Film("ConAir", rndDuracion.Next(81, 122))
                ,new Film("Batman", rndDuracion.Next(81, 122))
                ,new Film("Pulp fiction", rndDuracion.Next(81, 122))
                ,new Film("Todo lo que siempre quiso saber sobre el sexo pero nunca se atrevió a preguntar", rndDuracion.Next(81, 122))
                ,new Film("Annie Hall", rndDuracion.Next(81, 122))
                ,new Film("Manhattan", rndDuracion.Next(81, 122))
                ,new Film("Zelig", rndDuracion.Next(81, 122))
                ,new Film("Hannah y sus hermanas", rndDuracion.Next(81, 122))
                ,new Film("Misterioso asesinato en Manhattan", rndDuracion.Next(81, 122))
                ,new Film("Balas sobre Broadway", rndDuracion.Next(81, 122))
                ,new Film("Melinda and Melinda", rndDuracion.Next(81, 122))};
            context.Films.AddOrUpdate(c => c.Title, films);
            context.SaveChanges();
        }

        private static void SeedProgramacion(DatabaseContext context)
        {
            //
            // Crea la prograamación de cada cine 
            //
            TimeSpan[] horasStart = { new TimeSpan(11, 0, 0), new TimeSpan(15, 30, 0), new TimeSpan(20, 30, 0), new TimeSpan(22, 45, 0) };
            List<Cinema> cines = context.Cinemas.Include("Screens").Include("Screens.Seats").ToList();
            foreach (Cinema cine in cines)
            {
                List<Session> cineSessions = new List<Session>();
                foreach (Film film in context.Films.Include("Cinemas"))
                {
                    //for (int i = 1; i <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
                    for (int i = 0; i <= 2; i++)
                    {
                        DateTime day = DateTime.Now.AddDays(i).Date;
                        foreach (TimeSpan hora in horasStart)
                        {
                            DateTime sessionStart = day.Add(hora);
                            foreach (Screen scr in cine.Screens)
                            {
                                int randomizerProgramation = film.DurationInMinutes + cine.Id;
                                if (randomizerProgramation % 2 == 0)
                                {
                                    if (!film.Cinemas.Select(z => z.Name).Contains(cine.Name))
                                    {
                                        film.Cinemas.Add(cine);
                                    }
                                    cineSessions.Add(cine.CreateSession(scr.Id, film, sessionStart));
                                }
                            }
                        }
                    }
                }
                context.Cinemas.AddOrUpdate(cine);
                context.Sessions.AddOrUpdate(s => new { s.ScreenId, s.FilmId, s.Start }, cineSessions.ToArray());
            }
            context.SaveChanges();
        }
    }
}
