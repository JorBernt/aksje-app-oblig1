using AksjeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AksjeAPI.DAL
{
    public interface IAksjeRepository
    {
        Task<List<Aksje>> HentAlleAksjer();
        Task<AksjePriser> HentAksjePriser(string symbol, string fraDato, string tilDato);

        Task<bool> KjopAksje(string symbol, int antall, string dato);
    }
}
