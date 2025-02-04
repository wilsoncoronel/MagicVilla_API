using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Repositorios.IRepositorio;

namespace MagicVilla_API.Repositorios
{
    public class VillaRepositorio : Repositorio<Villa>, IVillaRepositorio
    {
        private readonly ApplicationDbContext _context;
        public VillaRepositorio(ApplicationDbContext context): base(context)
        {
            _context = context;
        }
        public async Task<Villa> Actualizar(Villa entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _context.Update(entidad);
            await _context.SaveChangesAsync();
            return entidad;
        }
    }
}
