using System;
using System.Collections.Generic;
using System.Linq; 

namespace CalculoInfracciones
{
   
    public enum CodigoInfraccion
    {
        SinLucesBajas = 1,
        FaltaMatafuego = 2,
        Sobrevelocidad = 3,
        FaltaCinturonCasco = 4,
        FaltaRespetoAutoridad = 5
    }

    public enum TipoVehiculo
    {
        UnEje = 1, 
        DosEjes = 2, 
        TresOMasEjes = 3 
    }

  
    public struct InfoTipoInfraccion
    {
        public CodigoInfraccion Codigo { get; }
        public string Denominacion { get; }
        public int LitrosNaftaBase { get; }

        public InfoTipoInfraccion(CodigoInfraccion codigo, string denominacion, int litrosNaftaBase)
        {
            Codigo = codigo;
            Denominacion = denominacion;
            LitrosNaftaBase = litrosNaftaBase;
        }
    }

  
    public class InfraccionDetalle
    {
        public InfoTipoInfraccion TipoInfraccion { get; }
        public double ValorMonetario { get; } 

        public InfraccionDetalle(InfoTipoInfraccion tipoInfraccion, double costoLitroNafta)
        {
            TipoInfraccion = tipoInfraccion;
            ValorMonetario = tipoInfraccion.LitrosNaftaBase * costoLitroNafta;
        }
    }

  
    public class TicketInfraccion
    {
        public string DniInfractor { get; }
        public string NombreInfractor { get; }
        public TipoVehiculo VehiculoInfractor { get; }
        public double CostoLitroNafta { get; }
        public List<InfraccionDetalle> DetallesInfracciones { get; }
        public bool PagoEnLugar { get; }

        public double SubtotalInfracciones { get; private set; }
        public double PorcentajeReajusteVehiculo { get; private set; }
        public double ValorReajusteVehiculo { get; private set; }
        public double SubtotalConReajuste { get; private set; }
        public double PorcentajeDescuentoPagoEnLugar { get; private set; }
        public double MontoDescuentoPagoEnLugar { get; private set; }
        public double TotalFinal { get; private set; }

        private static int _contadorTickets = 0; 
        public int NumeroTicket { get; }

        public TicketInfraccion(string dniInfractor, string nombreInfractor, TipoVehiculo vehiculoInfractor,
                                double costoLitroNafta, List<CodigoInfraccion> codigosInfracciones, bool pagoEnLugar)
        {
            NumeroTicket = ++_contadorTickets; 

            DniInfractor = dniInfractor;
            NombreInfractor = nombreInfractor;
            VehiculoInfractor = vehiculoInfractor;
            CostoLitroNafta = costoLitroNafta;
            PagoEnLugar = pagoEnLugar;
            DetallesInfracciones = new List<InfraccionDetalle>();

           
            Dictionary<CodigoInfraccion, InfoTipoInfraccion> tiposInfraccionBase = new Dictionary<CodigoInfraccion, InfoTipoInfraccion>
            {
                { CodigoInfraccion.SinLucesBajas, new InfoTipoInfraccion(CodigoInfraccion.SinLucesBajas, "Sin luces bajas", 25) },
                { CodigoInfraccion.FaltaMatafuego, new InfoTipoInfraccion(CodigoInfraccion.FaltaMatafuego, "Falta de Matafuego", 30) },
                { CodigoInfraccion.Sobrevelocidad, new InfoTipoInfraccion(CodigoInfraccion.Sobrevelocidad, "Sobrevelocidad", 100) },
                { CodigoInfraccion.FaltaCinturonCasco, new InfoTipoInfraccion(CodigoInfraccion.FaltaCinturonCasco, "Falta de cinturón de seguridad (>2 ejes) o falta de casco (1 eje)", 85) },
                { CodigoInfraccion.FaltaRespetoAutoridad, new InfoTipoInfraccion(CodigoInfraccion.FaltaRespetoAutoridad, "Falta de respeto a la autoridad", 1500) }
            };

           
            foreach (var codigo in codigosInfracciones)
            {
                if (tiposInfraccionBase.TryGetValue(codigo, out var info))
                {
                    DetallesInfracciones.Add(new InfraccionDetalle(info, costoLitroNafta));
                    SubtotalInfracciones += info.LitrosNaftaBase * costoLitroNafta;
                }
            }

            CalcularMontosFinales();
        }

        private void CalcularMontosFinales()
        {
            
            double factorReajuste = 0.0;
            switch (VehiculoInfractor)
            {
                case TipoVehiculo.UnEje:
                    factorReajuste = 0.01; 
                    PorcentajeReajusteVehiculo = 1.0; 
                    break;
                case TipoVehiculo.DosEjes:
                    factorReajuste = 0.50; 
                    PorcentajeReajusteVehiculo = 50.0;
                    break;
                case TipoVehiculo.TresOMasEjes:
                    factorReajuste = 2.00; 
                    PorcentajeReajusteVehiculo = 200.0;
                    break;
            }
            ValorReajusteVehiculo = SubtotalInfracciones * factorReajuste;
            SubtotalConReajuste = SubtotalInfracciones + ValorReajusteVehiculo;

            
            TotalFinal = SubtotalConReajuste;
            if (PagoEnLugar)
            {
                PorcentajeDescuentoPagoEnLugar = 50.0;
                MontoDescuentoPagoEnLugar = SubtotalConReajuste * 0.50;
                TotalFinal -= MontoDescuentoPagoEnLugar;
            }
        }

     
        public void ImprimirRecibo()
        {
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine($"Ticket #{NumeroTicket}");
            Console.WriteLine($"{DniInfractor} {NombreInfractor}");

            string tipoVehiculoStr = "";
            switch (VehiculoInfractor)
            {
                case TipoVehiculo.UnEje: tipoVehiculoStr = "1 eje"; break;
                case TipoVehiculo.DosEjes: tipoVehiculoStr = "2 ejes"; break;
                case TipoVehiculo.TresOMasEjes: tipoVehiculoStr = "3 ejes o más"; break;
            }
            Console.WriteLine($"Tipo vehículo: {tipoVehiculoStr} - Base: 1 L / {CostoLitroNafta:C2}");
            Console.WriteLine("Detalle:");
            foreach (var detalle in DetallesInfracciones)
            {
                Console.WriteLine($"{detalle.TipoInfraccion.Denominacion}: ({detalle.TipoInfraccion.LitrosNaftaBase}L) {detalle.ValorMonetario,30:C2}");
            }
            Console.WriteLine($"Subtotal ($ar) {SubtotalInfracciones,40:C2}");
            Console.WriteLine($"Por tipo de vehículo: (+{PorcentajeReajusteVehiculo:F0}%) {ValorReajusteVehiculo,30:C2}");
            Console.WriteLine($"    Subtotal ($ar) {SubtotalConReajuste,40:C2}");

            if (PagoEnLugar)
            {
                Console.WriteLine($"Pago en efectivo (-{PorcentajeDescuentoPagoEnLugar:F0}%) {MontoDescuentoPagoEnLugar,30:C2}");
            }
            Console.WriteLine($"    Total de la multa ($ar) {TotalFinal,30:C2}");
            Console.WriteLine("------------------------------------------------------------------");
        }
    }

  
    public class ServicioInfracciones
    {
        private double _costoLitroNaftaActual;
        private List<TicketInfraccion> _ticketsEmitidos;
        private double _recaudacionTotalGlobal;

        
        public ServicioInfracciones(double costoLitroNafta)
        {
            _costoLitroNaftaActual = costoLitroNafta;
            _ticketsEmitidos = new List<TicketInfraccion>();
            _recaudacionTotalGlobal = 0.0;
        }

     
        public void CrearNuevaInfraccion()
        {
            Console.WriteLine("\n--- REGISTRAR NUEVA INFRACCIÓN ---");

            Console.Write("DNI del infractor: ");
            string dni = Console.ReadLine();
            Console.Write("Nombre del infractor: ");
            string nombre = Console.ReadLine();

            TipoVehiculo tipoVehiculo = SolicitarTipoVehiculo();
            if (tipoVehiculo == 0) 
            {
                Console.WriteLine("Creación de infracción cancelada debido a tipo de vehículo inválido.");
                return;
            }

            List<CodigoInfraccion> infraccionesCometidas = SolicitarInfracciones();
            if (infraccionesCometidas.Count == 0)
            {
                Console.WriteLine("No se seleccionó ninguna infracción. Creación de infracción cancelada.");
                return;
            }

            bool pagoEnLugar = SolicitarPagoEnLugar();

           
            TicketInfraccion nuevoTicket = new TicketInfraccion(dni, nombre, tipoVehiculo,
                                                                 _costoLitroNaftaActual, infraccionesCometidas, pagoEnLugar);

            _ticketsEmitidos.Add(nuevoTicket);
            _recaudacionTotalGlobal += nuevoTicket.TotalFinal; 

            Console.WriteLine("\n¡Infracción registrada con éxito! Imprimiendo recibo:");
            nuevoTicket.ImprimirRecibo();
        }

       
        public void ImprimirRecaudacionTotal()
        {
            Console.WriteLine("\n==============================================");
            Console.WriteLine("--- RECAUDACIÓN TOTAL DEL DÍA ---");
            Console.WriteLine($"Recaudación total: {_recaudacionTotalGlobal:C2}");
            Console.WriteLine("==============================================");
        }

        public void ImprimirTicketMayorMonto()
        {
            if (_ticketsEmitidos.Count == 0)
            {
                Console.WriteLine("\nNo se emitieron tickets hoy.");
                return;
            }

           
            var ticketMayorMonto = _ticketsEmitidos.OrderByDescending(t => t.TotalFinal).FirstOrDefault();

            Console.WriteLine("\n==============================================");
            Console.WriteLine("--- TICKET CON EL MAYOR MONTO ---");
            if (ticketMayorMonto != null)
            {
                ticketMayorMonto.ImprimirRecibo();
            }
            Console.WriteLine("==============================================");
        }

        
        public void ImprimirTicketsPagadosEnLugarOrdenados()
        {
            var ticketsPagadosEnLugar = _ticketsEmitidos.Where(t => t.PagoEnLugar).OrderBy(t => t.TotalFinal).ToList();

            if (ticketsPagadosEnLugar.Count == 0)
            {
                Console.WriteLine("\nNo se emitieron tickets con pago en el lugar hoy.");
                return;
            }

            Console.WriteLine("\n==============================================");
            Console.WriteLine("--- TICKETS PAGADOS EN EL LUGAR (ORDENADOS POR MONTO) ---");
            foreach (var ticket in ticketsPagadosEnLugar)
            {
                ticket.ImprimirRecibo();
            }
            Console.WriteLine("==============================================");
        }

      

        private TipoVehiculo SolicitarTipoVehiculo()
        {
            Console.WriteLine("Seleccione el tipo de vehículo:");
            Console.WriteLine($"  {(int)TipoVehiculo.UnEje}. 1 Eje (Motos)");
            Console.WriteLine($"  {(int)TipoVehiculo.DosEjes}. 2 Ejes (Autos)");
            Console.WriteLine($"  {(int)TipoVehiculo.TresOMasEjes}. 3 o Más Ejes (Camiones, Colectivos)");
            Console.Write("Ingrese el número correspondiente: ");

            if (int.TryParse(Console.ReadLine(), out int tipoInt) && Enum.IsDefined(typeof(TipoVehiculo), tipoInt))
            {
                return (TipoVehiculo)tipoInt;
            }
            else
            {
                Console.WriteLine("Tipo de vehículo inválido. Por favor, intente de nuevo.");
                return 0;
            }
        }

        private List<CodigoInfraccion> SolicitarInfracciones()
        {
            List<CodigoInfraccion> infracciones = new List<CodigoInfraccion>();
            Dictionary<int, CodigoInfraccion> mapaCodigos = new Dictionary<int, CodigoInfraccion>();

            Console.WriteLine("\nSeleccione las infracciones cometidas (ingrese 0 para finalizar):");
            Console.WriteLine("  1. Sin luces bajas");
            Console.WriteLine("  2. Falta de Matafuego");
            Console.WriteLine("  3. Sobrevelocidad");
            Console.WriteLine("  4. Falta de cinturón de seguridad (>2 ejes) o falta de casco (1 eje)");
            Console.WriteLine("  5. Falta de respeto a la autoridad");

            mapaCodigos.Add(1, CodigoInfraccion.SinLucesBajas);
            mapaCodigos.Add(2, CodigoInfraccion.FaltaMatafuego);
            mapaCodigos.Add(3, CodigoInfraccion.Sobrevelocidad);
            mapaCodigos.Add(4, CodigoInfraccion.FaltaCinturonCasco);
            mapaCodigos.Add(5, CodigoInfraccion.FaltaRespetoAutoridad);

            int codigoIngresado;
            do
            {
                Console.Write("Ingrese el código de infracción: ");
                if (int.TryParse(Console.ReadLine(), out codigoIngresado))
                {
                    if (codigoIngresado == 0)
                    {
                        break; 
                    }
                    if (mapaCodigos.TryGetValue(codigoIngresado, out CodigoInfraccion infraccion))
                    {
                        if (!infracciones.Contains(infraccion)) 
                        {
                            infracciones.Add(infraccion);
                            Console.WriteLine($"  Infracción '{infraccion}' añadida.");
                        }
                        else
                        {
                            Console.WriteLine("  Esta infracción ya ha sido añadida.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("  Código de infracción inválido. Intente de nuevo.");
                    }
                }
                else
                {
                    Console.WriteLine("  Entrada inválida. Ingrese un número.");
                }
            } while (true);

            return infracciones;
        }

        private bool SolicitarPagoEnLugar()
        {
            Console.Write("¿La multa se pagó en el lugar de la infracción? (S/N): ");
            string respuesta = Console.ReadLine().ToUpper();
            return respuesta == "S";
        }
    }

    
    public class Program
    {
        private static ServicioInfracciones _servicioInfracciones;

        public static void Main(string[] args)
        {
            Console.WriteLine("¡Bienvenido al sistema de cálculo de infracciones de tránsito!");

            double costoNafta;
            do
            {
                Console.Write("Ingrese el costo actual del litro de nafta: ");
                if (!double.TryParse(Console.ReadLine(), out costoNafta) || costoNafta <= 0)
                {
                    Console.WriteLine("Costo de nafta inválido. Por favor, ingrese un número positivo.");
                }
            } while (costoNafta <= 0);

           
            _servicioInfracciones = new ServicioInfracciones(costoNafta);

            int opcion;
            do
            {
                MostrarMenuPrincipal();
                string input = Console.ReadLine();
                if (int.TryParse(input, out opcion))
                {
                    switch (opcion)
                    {
                        case 1:
                            _servicioInfracciones.CrearNuevaInfraccion();
                            break;
                        case 2:
                            _servicioInfracciones.ImprimirRecaudacionTotal();
                            break;
                        case 3:
                            _servicioInfracciones.ImprimirTicketMayorMonto();
                            break;
                        case 4:
                            _servicioInfracciones.ImprimirTicketsPagadosEnLugarOrdenados();
                            break;
                        case 5:
                            Console.WriteLine("Saliendo del programa. ¡Hasta luego!");
                            break;
                        default:
                            Console.WriteLine("Opción inválida. Por favor, intente de nuevo.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor, ingrese un número.");
                    opcion = 0; // Para que el bucle continúe
                }
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            } while (opcion != 5);
        }

        private static void MostrarMenuPrincipal()
        {
            Console.WriteLine("\n----- MENÚ PRINCIPAL -----");
            Console.WriteLine("1. Registrar nueva infracción");
            Console.WriteLine("2. Ver recaudación total del día");
            Console.WriteLine("3. Ver ticket con mayor monto");
            Console.WriteLine("4. Ver tickets pagados en el lugar (ordenados)");
            Console.WriteLine("5. Salir");
            Console.Write("Ingrese su opción: ");
        }
    }
}