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
                                    .FirstOrDefaultAsync().ConfigureAwait(false);

            if (intezmeny == null)
                return null;

            return new IntezmenyDetailDto
            {
                Nev = intezmeny.Nev,
                Alapitas = intezmeny.Alapitas,
                Megszunes = intezmeny.Megszunes,
                IntezmenyHelyszinek = intezmeny.IntezmenyHelyszinek.Select(ih => new IntezmenyHelyszinDto{
                    Helyszin = ih.Helyszin, Nyitas = ih.Nyitas, Koltozes = ih.Koltozes, Latitude = ih.Latitude, Longitude =  ih.Longitude}).ToList(),
                IntezmenyVezetok = intezmeny.IntezmenyVezetok.Select(iv => new IntezmenyVezetoDto{Nev = iv.Nev, Tol = iv.Tol, Ig = iv.Ig}).ToList(),
                Leiras = intezmeny.Leiras,
                Esemenyek = intezmeny.Esemenyek.Select(e => new EsemenyDto {Nev = e.Nev, Datum = e.Datum, Szervezo = e.Szervezo}).ToList(),
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
                intezemenyQuery = intezemenyQuery.Where(i => searchParams.IntezmenyTipus.Contains(i.Tipus));
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
                        (searchParams.MukodesTol == null
                            || searchParams.MukodesTol <= ih.Koltozes || ih.Koltozes == null)
                         && (searchParams.MukodesIg == null
                            || searchParams.MukodesIg >= ih.Nyitas))
                .Select( ih => new IntezmenyPinDto{
                    IntezmenyId = i.Id,
                    IntezmenyTipus = i.Tipus,
                    Latitude = ih.Latitude,
                    Longitude = ih.Longitude
            })).ToListAsync().ConfigureAwait(false);
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
