using DIMU.BLL.ServiceInterfaces;
using DIMU.DAL;
using DIMU.DAL.Dto;
using DIMU.DAL.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMU.BLL.Services
{
    public class IntezmenyService : IIntezmenyService
    {
        private readonly DimuContext context;

        public IntezmenyService(DimuContext context)
        {
            this.context = context;
        }

        public async Task<IntezmenyDetailDto> GetIntezmenyAsync(Guid id)
        {
            var intezmeny = await context.Intezmenyek
                                    .Include(i => i.IntezmenyHelyszinek)
                                    .Include(i => i.IntezmenyVezetok)
                                    .Include(i => i.Esemenyek)
                                    .Where(i => i.Id == id)
                                    .FirstOrDefaultAsync();

            if (intezmeny == null)
                return null;

            List<string> ivk = new List<string>();
            foreach(var iv in intezmeny.IntezmenyVezetok)
            {
                if (iv.Ig != null)
                {
                    ivk.Add(new string(iv.Nev + " " + iv.Tol.ToString() + " - " + iv.Ig.ToString()));
                }
                else
                {
                    ivk.Add(new string(iv.Nev + " " + iv.Tol.ToString() + "-" + "től"));
                }
            }

            return new IntezmenyDetailDto
            {
                Nev = intezmeny.Nev,
                Alapitas = intezmeny.Alapitas,
                Megszunes = intezmeny.Megszunes,
                IntezmenyHelyszinek = intezmeny.IntezmenyHelyszinek.Select(ih => ih.Helyszin + " (" + ih.Nyitas.ToString() + " - " + ih.Koltozes?.ToString() + ")").ToList(),
                IntezmenyVezetok = ivk,
                Leiras = intezmeny.Leiras,
                Esemenyek = intezmeny.Esemenyek.Select(e => e.Datum + " " + e.Szervezo + ": " + e.Nev).ToList(),
                Link = intezmeny.Link,
                Fotok = intezmeny.Fotok,
                Social = intezmeny.Social,
                Videok = intezmeny.Videok
            };
        }

        public async Task<IEnumerable<IntezmenyPinDto>> GetIntezmenyekAsync(IntezmenySearchParams searchParams)
        {
           IQueryable<Intezmeny> intezemenyQuery = context.Intezmenyek
                                    .Include(i => i.Esemenyek)
                                    .Include(i => i.IntezmenyHelyszinek)
                                    .Include(i => i.IntezmenyVezetok);

            if (!String.IsNullOrEmpty(searchParams.IntezmenyCim))
            {
                intezemenyQuery = intezemenyQuery.Where(i => i.IntezmenyHelyszinek.Any(ih => ih.Helyszin.Contains(searchParams.IntezmenyCim)));
            }

            if (!String.IsNullOrEmpty(searchParams.IntezmenyNev))
            {
                intezemenyQuery = intezemenyQuery.Where(i => i.Nev.Contains(searchParams.IntezmenyNev));
            }
            //TODO: idot is nezzen ha kell(nincs tisztazva)
            if (!String.IsNullOrEmpty(searchParams.IntezmenyVezeto))
            {
                intezemenyQuery = intezemenyQuery.Where(i => i.IntezmenyVezetok.Any(iv => iv.Nev.Contains(searchParams.IntezmenyCim)));
            }

            if(searchParams.IntezmenyTipus != null)
            {
                intezemenyQuery = intezemenyQuery.Where(i => i.Tipus == searchParams.IntezmenyTipus);
            }

            if (searchParams.MukodesIg != null)
            {
                //intezmenyre vonatkozo
                intezemenyQuery = intezemenyQuery.Where(i => i.Alapitas <= searchParams.MukodesIg);                
            }

            if (searchParams.MukodesTol != null)
            {                
                intezemenyQuery = intezemenyQuery.Where(i => i.Megszunes >= searchParams.MukodesTol || i.Megszunes == null);
            }

            return await intezemenyQuery.SelectMany( i => i.IntezmenyHelyszinek
                .Where(ih => 
                        (searchParams.MukodesTol != null 
                            ? searchParams.MukodesTol <= ih.Koltozes || ih.Koltozes == null 
                            : true) 
                         && (searchParams.MukodesIg != null 
                            ? searchParams.MukodesIg >= ih.Nyitas 
                            : true))
                .Select( ih => new IntezmenyPinDto{
                    IntezmenyId = i.Id,
                    IntezmenyTipus = i.Tipus,
                    Latitude = ih.Latitude,
                    Longitude = ih.Longitude
            })).ToListAsync();
        }

        private bool IntezmenyExists(Guid id)
        {
            return context.Intezmenyek.Any(e => e.Id == id);
        }

        public async Task<bool> PutIntezmeny(Guid id, Intezmeny intezmeny)
        {
            context.Entry(intezmeny).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IntezmenyExists(id))
                    return false;
                else
                    throw;
            }

            return true;
        }

        public async Task<Intezmeny> PostIntezmeny(Intezmeny intezmeny)
        {
            context.Intezmenyek.Add(intezmeny);
            await context.SaveChangesAsync();
            return intezmeny;
        }

        public async Task<bool> DeleteIntezmeny(Guid id)
        {
            var intezmeny = await context.Intezmenyek.FindAsync(id);

            if (intezmeny == null)
                return false;

            context.Intezmenyek.Remove(intezmeny);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
