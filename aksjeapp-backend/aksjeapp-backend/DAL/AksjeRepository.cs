using AksjeAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AksjeAPI.DAL
{
    public class AksjeRepository : IAksjeRepository
    {
        private readonly AksjeContext _db;

        public AksjeRepository(AksjeContext db)
        {
            _db = db;
        }

        public async Task<List<Aksje>> HentAlleAksjer()
        {

            List<Aksje> aksjeListe = await _db.Aksjer.ToListAsync();
            return aksjeListe;

        }
        public async Task<AksjePriser> HentAksjePriser(string symbol, string fraDato, string tilDato) // dato skal skrives som "YYYY-MM-DD"
        {
            var aksje = await PolygonAPI.HentAksjePriser(symbol, fraDato, tilDato);

            return aksje;
        }

        public async Task<bool> KjopAksje(string symbol, int antall, string dato)
        {
            if (dato >= 0/*dagens dato*/)
            {
                var aksje = await PolygonAPI.HentAksjePriser()
            }
            return true;
        }
    }
}
