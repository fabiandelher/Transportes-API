using System.Net;
using DTO;
using Transportes_API.Models;

namespace Transportes_API.Services
{
    public interface ICamiones
    {
        //es una estructura que define un contrato o conjunto de métodos y
        //propiedades que una clase debe implementar.
        //Una interfaz establece un conjunto de requisitos que cualquier clase
        //que la implemente debe seguir. Estos requisitos son declarados en la
        //interfaz en forma de firmas de métodos y propiedades,
        //pero la interfaz en sí misma no proporciona ninguna implementación
        //de estos métodos o propiedades.Es responsabilidad de las clases que
        //implementan la interfaz proporcionar las implementaciones concretas de
        //estos miembros.

        //Las interfaces son útiles para lograr la abstracción y la reutilización
        //de código en C#.

        //GET
        List<Camiones_DTO> GetCamiones();

        //GETbyID
        Camiones_DTO GetCamiones(int id);

        //INSERT(POST)

        string InsertCamion(Camiones_DTO camiones);

        //UPDATE(POST)
        string UpdateCamion(Camiones_DTO camion);

        //DELETE (DELETE)

        string DeleteCamion(int id);
    }

    //La clase que implementa la inteerfaz y declara la implementacion de la logica de los meteodos existentes 
    public class CamionesService : ICamiones
    {

        //variable para crear el contexto
        private readonly TransportesContext _context;

        //constructor para inicializar el contexto
        public CamionesService(TransportesContext context)
        {
            _context = context;
        }

        public string DeleteCamion(int id)
        {
            try
            {
                //obtengo primero el camion de la base de datos
                Camiones _camion = _context.Camiones.Find(id);
                //valido que realmente recupere mi objetto
                if (_camion == null)
                {
                    return $"No se encontro algun objeto con identificacion{id}";

                }

                _context.Camiones.Remove(_camion);
                _context.SaveChanges();

                return $"Camion {id} eliminado con éxito";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }

        }

        public Camiones_DTO GetCamiones(int id)
        {
            //dynamic mapper
            Camiones origen = _context.Camiones.Find(id);
            Camiones_DTO resultado = DynamicMapper.Map<Camiones, Camiones_DTO>(origen);
            return resultado;
        }

        public List<Camiones_DTO> GetCamiones()
        {
            try
            {
                //Lista de camiones del original
                List<Camiones> lista_original = _context.Camiones.ToList();
                //Lista de Dios
                List<Camiones_DTO> lista_salida = new List<Camiones_DTO>();
                //recorro cada camion y genero un nuevo DTO con DinamycMapper
                foreach (var cam in lista_original)
                {
                    //usamos el dynamicmapper para convertir los objetos
                    Camiones_DTO DTO = DynamicMapper.Map<Camiones, Camiones_DTO>(cam);
                    lista_salida.Add(DTO);
                }

                return lista_salida;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }


        public string InsertCamion(Camiones_DTO camion)
        {
            try
            {
                Camiones _camion = new Camiones();
                //asigno los valores del parametro
                _camion = DynamicMapper.Map<Camiones_DTO, Camiones>(camion);
                _context.Camiones.Add(_camion);
                _context.SaveChanges();
                return "Camión insertado con exito";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public string UpdateCamion(Camiones_DTO camion)
        {
            try
            {
                Camiones _camion = new Camiones();
                //asigno los valores del parametro
                _camion = DynamicMapper.Map<Camiones_DTO, Camiones>(camion);
                _context.Entry(_camion).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return $"Camión {_camion.ID_Camion} insertado con exito";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}
