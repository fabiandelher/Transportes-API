using System.Reflection;
using System.Runtime.Intrinsics.Arm;

namespace Transportes_API.Services
{
    public class DynamicMapper
    {
        //metodo que mapea de forma dinamica diferente tippos de Objeto (Por ejemplo modelos Originales a DTO y viceversa)
        public static TDestination Map<TSource, TDestination>(TSource source)
            where TSource : class //se declara una clase abstaracta como tipo de objeto  de entrada 
            where  TDestination : class, new()//se declara una clase abstaracta como tipo de objeto de salida
        {
            //valido si existe y contiene información la clase de origen 
            if (source == null) throw new ArgumentNullException("source");

            var destination = new TDestination();//creo una instancia del objeto de salida

            //recuperar las propiedades (los atributos de mis elementos) usando la biblioteca system.reflexion
            //Mediante reflexión, puedes acceder a las propiedades de un tipo (clase, estructura, etc.) en tiempo de ejecución, incluso si no conoces el tipo exacto en tiempo de compilación.
            //GetProperties: Devuelve un array con todas las propiedades públicas del tipo especificado.
            //BindingFlags: Opciones que especifican qué miembros buscar(públicos, privados, estáticos, etc.).
            //using System.Reflection;


            var sourceProperties = typeof(TSource).GetProperties(BindingFlags.Public| BindingFlags.Instance);
            var destinationProperties = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);


            foreach(var sourceProperty in sourceProperties)
            {
                //recupero cada propiedad de laclase donde empate tanto el nombre de la propiedad como el tipo de dato (Aqí es donde se mapean los objetos)
                var destinationProperty = destinationProperties.FirstOrDefault(dp => dp.Name.ToLower() == sourceProperty.Name.ToLower()
                &&
                dp.PropertyType == sourceProperty.PropertyType);

                if(destinationProperty != null && destinationProperty.CanWrite)
                {
                    var value = sourceProperty.GetValue(source);
                    destinationProperty.SetValue(destination, value);
                }
                return destination;
                 
            }
        }

        internal static T2 Map<T1, T2>(object camion)
        {
            throw new NotImplementedException();
        }
    }
}
