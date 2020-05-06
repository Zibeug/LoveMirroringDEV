/*
 *      Auteur : Tim Allemann
 *      2020.04.27
 *      Permet d'ajouter des données pour la in memory database
 */

using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Utility
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new LoveMirroringContext(
                serviceProvider.GetRequiredService<DbContextOptions<LoveMirroringContext>>()))
            {
                // Look for any board games.
                if (!context.Sexes.Any())
                {
                    context.Sexes.AddRange(
                    new Sex
                    {
                        SexeId = 1,
                        SexeName = "Masculin"
                    },
                    new Sex
                    {
                        SexeId = 2,
                        SexeName = "Féminin"
                    });
                }
              
                context.SaveChanges();
            }
        }
    }
}
