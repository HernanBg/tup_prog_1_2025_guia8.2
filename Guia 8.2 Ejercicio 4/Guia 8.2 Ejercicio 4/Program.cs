using System;
using System.Collections.Generic; 
using System.Linq; 

namespace FabricaBaterias
{
    
    public class ServicioBaterias
    {
       
        private const int NUMERO_LINEAS_PRODUCCION = 2;

       
        private int _totalBateriasFalladasGlobal;
        private int _contadorGlobalBaterias; 

      
        private (int numeroBateriaGlobal, double errorPorcentual) _bateriaMayorErrorPorcentualGlobal;

       
        public ServicioBaterias()
        {
            _totalBateriasFalladasGlobal = 0;
            _contadorGlobalBaterias = 0;
            _bateriaMayorErrorPorcentualGlobal = (-1, -1.0); 
        }

   
   
        public void ProcesarLineaProduccion(int numeroLinea)
        {
            Console.WriteLine($"\n--- INICIO TESTEO LÍNEA DE PRODUCCIÓN {numeroLinea} ---");

            double voltajeVacioEsperado;
            do
            {
                Console.Write("Ingrese el voltaje esperado en vacío para esta línea (ej. 10.3V para una batería de 9V): ");
                string input = Console.ReadLine();
                if (!double.TryParse(input, out voltajeVacioEsperado) || voltajeVacioEsperado <= 0)
                {
                    Console.WriteLine("Valor de voltaje en vacío inválido. Por favor, ingrese un número positivo.");
                }
            } while (voltajeVacioEsperado <= 0);

            Console.WriteLine($"Voltaje en vacío esperado para Línea {numeroLinea}: {voltajeVacioEsperado:F2}V");

            int bateriasNoFalladasLinea = 0;
            double voltajeMedido;

            do
            {
                Console.Write($"\nIngrese el voltaje medido de la batería (o -1 para finalizar el lote de esta línea): ");
                string input = Console.ReadLine();

                if (!double.TryParse(input, out voltajeMedido))
                {
                    Console.WriteLine("Entrada inválida. Por favor, ingrese un número para el voltaje.");
                    continue; 
                }

                if (voltajeMedido == -1)
                {
                    break; 
                }

                if (voltajeMedido < 0)
                {
                    Console.WriteLine("El voltaje medido no puede ser negativo. Por favor, intente de nuevo.");
                    continue;
                }

                _contadorGlobalBaterias++;

              
                bool estaFallada = voltajeMedido < voltajeVacioEsperado;
                double errorVoltaje = Math.Abs(voltajeMedido - voltajeVacioEsperado); 
                double errorPorcentual = (errorVoltaje / voltajeVacioEsperado) * 100.0; 

                Console.Write($"  Batería Global #{_contadorGlobalBaterias} (Línea {numeroLinea}): Voltaje medido={voltajeMedido:F2}V, Error={errorVoltaje:F2}V");

                if (estaFallada)
                {
                    _totalBateriasFalladasGlobal++;
                    Console.WriteLine(" => DESCARTADA (Fallada)");
                }
                else
                {
                    bateriasNoFalladasLinea++;
                    Console.WriteLine(" => ACEPTADA");
                }
                Console.WriteLine($"  Error porcentual: {errorPorcentual:F2}%");

              
                if (errorPorcentual > _bateriaMayorErrorPorcentualGlobal.errorPorcentual)
                {
                    _bateriaMayorErrorPorcentualGlobal = (_contadorGlobalBaterias, errorPorcentual);
                }

            } while (voltajeMedido != -1);

            Console.WriteLine($"\n--- RESUMEN LÍNEA {numeroLinea} ---");
            Console.WriteLine($"Total de baterías NO falladas en esta línea: {bateriasNoFalladasLinea}");
            Console.WriteLine($"--- FIN TESTEO LÍNEA DE PRODUCCIÓN {numeroLinea} ---");
        }

      
        public void MostrarResumenEmbarqueGlobal()
        {
            Console.WriteLine("\n==============================================");
            Console.WriteLine("--- RESUMEN FINAL DEL EMBARQUE (TODAS LAS LÍNEAS) ---");
            Console.WriteLine($"Total de baterías procesadas: {_contadorGlobalBaterias}");
            Console.WriteLine($"Total de baterías FALLADAS: {_totalBateriasFalladasGlobal}");

            if (_bateriaMayorErrorPorcentualGlobal.numeroBateriaGlobal != -1)
            {
                Console.WriteLine($"Batería con el MAYOR ERROR PORCENTUAL GLOBAL:");
                Console.WriteLine($"  Número de batería (global): {_bateriaMayorErrorPorcentualGlobal.numeroBateriaGlobal}");
                Console.WriteLine($"  Error porcentual: {_bateriaMayorErrorPorcentualGlobal.errorPorcentual:F2}%");
            }
            else
            {
                Console.WriteLine("No se procesaron baterías.");
            }
            Console.WriteLine("==============================================");
        }
    }

    
    public class Program
    {
    
        private static ServicioBaterias _servicioBaterias = new ServicioBaterias();

        public static void Main(string[] args)
        {
            Console.WriteLine("¡Bienvenido al sistema de testeo de baterías!");

            
            const int lineasProduccionActualmente = 2; 

            for (int i = 1; i <= lineasProduccionActualmente; i++)
            {
                Console.WriteLine($"\n***** INICIANDO LÍNEA DE PRODUCCIÓN {i} *****");
                _servicioBaterias.ProcesarLineaProduccion(i);
                Console.WriteLine("\nPresione cualquier tecla para continuar con la siguiente línea (o finalizar)...");
                Console.ReadKey();
                Console.Clear();
            }

          
            _servicioBaterias.MostrarResumenEmbarqueGlobal();

            Console.WriteLine("\nPrograma finalizado. ¡Hasta luego!");
            Console.ReadKey();
        }
    }
}