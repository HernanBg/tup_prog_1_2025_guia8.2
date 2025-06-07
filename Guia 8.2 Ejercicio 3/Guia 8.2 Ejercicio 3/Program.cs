using System;
using System.Collections.Generic; 

namespace TurismoRecorridos
{
    
    public class ServicioRecorridos
    {
       
        private const double PRECIO_TICKET = 3.00;
        private const double SEGURO_ADICIONAL = 1.00;
        private const double DESCUENTO_MENOR_12 = 0.50; 
        private const double DESCUENTO_ENTRE_12_Y_17 = 0.25; 

        
        private double _recaudacionTotalGlobal;

        
        public ServicioRecorridos()
        {
            _recaudacionTotalGlobal = 0.0;
        }

       
        public void ProcesarRecorrido(int numeroRecorrido)
        {
            Console.WriteLine($"\n--- INICIO DEL RECORRIDO {numeroRecorrido} ---");

            int cantidadMenores12 = 0;
            double recaudacionViajeActual = 0.0;
            int numeroPasajeroMenorEdad = -1; 
            int menorEdadEncontrada = int.MaxValue; 
            int contadorPasajeros = 0; 

            int edad;
            do
            {
                Console.Write($"\nIngrese la edad del pasajero {contadorPasajeros + 1} (o -1 para finalizar este recorrido): ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out edad))
                {
                    Console.WriteLine("Entrada inválida. Por favor, ingrese un número para la edad.");
                    continue; 
                }

                if (edad == -1)
                {
                    break; 
                }

                if (edad < -1)
                {
                    Console.WriteLine("La edad no puede ser negativa (excepto -1 para salir). Por favor, intente de nuevo.");
                    continue;
                }

                contadorPasajeros++; 

                double costoTicket = PRECIO_TICKET;
                double costoSeguro = 0.0;

            
                if (edad < 12)
                {
                    cantidadMenores12++;
                    costoTicket *= (1 - DESCUENTO_MENOR_12); 
                }
                else if (edad >= 12 && edad < 17)
                {
                    costoTicket *= (1 - DESCUENTO_ENTRE_12_Y_17); 
                }

                
                if (edad >= 12)
                {
                    costoSeguro = SEGURO_ADICIONAL;
                }

                double totalAPagar = costoTicket + costoSeguro;
                recaudacionViajeActual += totalAPagar;

                Console.WriteLine($"  Edad: {edad} años. Costo del ticket: {costoTicket:C2}. Costo del seguro: {costoSeguro:C2}. Total a pagar por este pasajero: {totalAPagar:C2}");


               
                if (edad < menorEdadEncontrada)
                {
                    menorEdadEncontrada = edad;
                    numeroPasajeroMenorEdad = contadorPasajeros; 
                }

            } while (edad != -1);

           
            Console.WriteLine($"\n--- RESUMEN DEL RECORRIDO {numeroRecorrido} ---");
            Console.WriteLine($"Cantidad de menores a 12 años: {cantidadMenores12}");
            Console.WriteLine($"Recaudación de este viaje: {recaudacionViajeActual:C2}");
            if (numeroPasajeroMenorEdad != -1)
            {
                Console.WriteLine($"El número de pasaje con la menor edad ({menorEdadEncontrada} años) es: {numeroPasajeroMenorEdad}");
            }
            else
            {
                Console.WriteLine("No se registraron pasajeros en este recorrido.");
            }

            
            _recaudacionTotalGlobal += recaudacionViajeActual;

            Console.WriteLine($"--- FIN DEL RECORRIDO {numeroRecorrido} ---");
        }

      
        public void MostrarRecaudacionTotalGlobal()
        {
            Console.WriteLine("\n==============================================");
            Console.WriteLine($"RECAUDACIÓN TOTAL DE TODOS LOS VIAJES: {_recaudacionTotalGlobal:C2}");
            Console.WriteLine("==============================================");
        }
    }

   
    public class Program
    {
        
        private static ServicioRecorridos _servicioRecorridos = new ServicioRecorridos();

        public static void Main(string[] args)
        {
            Console.WriteLine("¡Bienvenido al sistema de gestión de recorridos turísticos!");

         
            int cantidadPaseosDiarios;
            do
            {
                Console.Write("\n¿Cuántos paseos se planifican para hoy (2 o 3)? ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out cantidadPaseosDiarios) || (cantidadPaseosDiarios < 2 || cantidadPaseosDiarios > 3))
                {
                    Console.WriteLine("Entrada inválida. Por favor, ingrese 2 o 3.");
                }
            } while (cantidadPaseosDiarios < 2 || cantidadPaseosDiarios > 3);

           
            for (int i = 1; i <= cantidadPaseosDiarios; i++)
            {
                Console.WriteLine($"\n***** Preparando Viaje {i} de {cantidadPaseosDiarios} *****");
                _servicioRecorridos.ProcesarRecorrido(i);
                Console.WriteLine("\nPresione cualquier tecla para continuar con el siguiente viaje (o finalizar)...");
                Console.ReadKey();
                Console.Clear();
            }

            
            _servicioRecorridos.MostrarRecaudacionTotalGlobal();

            Console.WriteLine("\nPrograma finalizado. ¡Hasta luego!");
            Console.ReadKey();
        }
    }
}