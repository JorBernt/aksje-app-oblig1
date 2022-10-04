using AksjeAPI.DAL;
using AksjeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AksjeAPI.Controller
{
   [Route("[controller]/[action]")]
    public class AksjeController : ControllerBase
    {
        private readonly IAksjeRepository _db;

        public AksjeController(IAksjeRepository db)
        {
            _db = db;
        }

      
        public async Task<List<Aksje>> HentAlleAksjer()
        {
            return await _db.HentAlleAksjer();
        }
        public async Task<AksjePriser> HentAksjePriser(string symbol, string fraDato, string tilDato) // dato skal skrives som "YYYY-MM-DD"
        {
            return await _db.HentAksjePriser(symbol, fraDato, tilDato);
        }
        
        public async Task<bool> KjopAksje(string symbol, int antall, string dato)
        {
            return await _db.KjopAksje(symbol, antall, dato);
        }
    }
}
