using System;

class Program
{
    static Servicio servicio = new Servicio();

    static void Main(string[] args)
    {
        int opcion;
        do
        {
            opcion = MostrarPantallaSolicitarOpcionMenu();
            Console.WriteLine();

            switch (opcion)
            {
                case 1:
                    MostrarPantallaSolicitarEncuesta();
                    break;
                case 2:
                    MostrarPantallaSolicitarVariasEncuestas();
                    break;
                case 3:
                    MostrarPantallaPromediosResultados();
                    break;
                case 4:
                    MostrarPantallaTotalEncuestados();
                    break;
                case 0:
                    Console.WriteLine("Programa finalizado.");
                    break;
                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }

            Console.WriteLine();
        } while (opcion != 0);
    }

    static int MostrarPantallaSolicitarOpcionMenu()
    {
        Console.WriteLine("=== ENCUESTA DE TRANSPORTE ===");
        Console.WriteLine("1. Registrar una encuesta");
        Console.WriteLine("2. Registrar varias encuestas");
        Console.WriteLine("3. Ver promedios por tipo de transporte");
        Console.WriteLine("4. Ver total de encuestados");
        Console.WriteLine("0. Salir");
        Console.Write("Seleccione una opción: ");
        int.TryParse(Console.ReadLine(), out int opcion);
        return opcion;
    }

    static void MostrarPantallaSolicitarEncuesta()
    {
        Console.Write("Tipo de transporte (1-Bici, 2-Moto, 3-Auto, 4-Público): ");
        int tipo = int.Parse(Console.ReadLine());

        Console.Write("Distancia recorrida (km): ");
        double distancia = double.Parse(Console.ReadLine());

        servicio.RegistrarEncuesta(tipo, distancia);
        Console.WriteLine("Encuesta registrada correctamente.");
    }

    static void MostrarPantallaSolicitarVariasEncuestas()
    {
        Console.Write("¿Cuántas encuestas desea registrar? ");
        int cantidad = int.Parse(Console.ReadLine());

        for (int i = 0; i < cantidad; i++)
        {
            Console.WriteLine($"\n--- Encuesta #{i + 1} ---");
            MostrarPantallaSolicitarEncuesta();
        }
    }

    static void MostrarPantallaPromediosResultados()
    {
        servicio.MostrarPromedios();
    }

    static void MostrarPantallaTotalEncuestados()
    {
        servicio.MostrarTotalEncuestados();
    }
}
public class Servicio
{
    private int cantidadEncuestados;

    private int contadorBici;
    private int contadorMoto;
    private int contadorAuto;
    private int contadorPublico;

    private double acumuladorDistanciaBici;
    private double acumuladorDistanciaMoto;
    private double acumuladorDistanciaAuto;
    private double acumuladorDistanciaPublico;

    public void RegistrarEncuesta(int tipoTransporte, double distancia)
    {
        cantidadEncuestados++;

        switch (tipoTransporte)
        {
            case 1: 
                contadorBici++;
                acumuladorDistanciaBici += distancia;
                break;
            case 2: 
                contadorMoto++;
                acumuladorDistanciaMoto += distancia;
                break;
            case 3: 
                contadorAuto++;
                acumuladorDistanciaAuto += distancia;
                break;
            case 4: 
                contadorPublico++;
                acumuladorDistanciaPublico += distancia;
                break;
        }
    }

    public double CalcularPromedioPorTipo(int tipoTransporte)
    {
        switch (tipoTransporte)
        {
            case 1: return contadorBici > 0 ? acumuladorDistanciaBici / contadorBici : 0;
            case 2: return contadorMoto > 0 ? acumuladorDistanciaMoto / contadorMoto : 0;
            case 3: return contadorAuto > 0 ? acumuladorDistanciaAuto / contadorAuto : 0;
            case 4: return contadorPublico > 0 ? acumuladorDistanciaPublico / contadorPublico : 0;
            default: return 0;
        }
    }

    public void MostrarPromedios()
    {
        Console.WriteLine("\n=== Promedio de distancia por tipo de transporte ===");
        Console.WriteLine($"Bicicleta: {CalcularPromedioPorTipo(1):F2} km");
        Console.WriteLine($"Moto: {CalcularPromedioPorTipo(2):F2} km");
        Console.WriteLine($"Auto: {CalcularPromedioPorTipo(3):F2} km");
        Console.WriteLine($"Transporte público: {CalcularPromedioPorTipo(4):F2} km");
    }

    public void MostrarTotalEncuestados()
    {
        Console.WriteLine($"\nTotal de personas encuestadas: {cantidadEncuestados}");
    }
}

