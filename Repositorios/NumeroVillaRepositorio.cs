﻿using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Repositorios.IRepositorio;

namespace MagicVilla_API.Repositorios
{
    public class NumeroVillaRepositorio : Repositorio<NumeroVilla>, INumeroVillaRepositorio
    {
        private readonly ApplicationDbContext _context;
        public NumeroVillaRepositorio(ApplicationDbContext context): base(context)
        {
            _context = context;
        }
        public async Task<NumeroVilla> Actualizar(NumeroVilla entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _context.NumeroVillas.Update(entidad);
            await _context.SaveChangesAsync();
            return entidad;
        }
    }
}
