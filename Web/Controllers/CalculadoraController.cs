using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;


namespace Web.Controllers
{
    public class CalculadoraController : Controller
    {
        public IActionResult Calculadora()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Calcular(int Peso, int Calorias) {
            try
            {
                List<Elemento> elementos = new List<Elemento>
                {
                    new Elemento { Nombre = "E1", Peso = 5, Calorias = 3 },
                    new Elemento { Nombre = "E2", Peso = 3, Calorias = 5 },
                    new Elemento { Nombre = "E3", Peso = 5, Calorias = 2 },
                    new Elemento { Nombre = "E4", Peso = 1, Calorias = 8 },
                    new Elemento { Nombre = "E5", Peso = 2, Calorias = 3 }
                };

                List<Elemento> combinacion = Calculo(elementos, Calorias, Peso);

                return Json(new { data = combinacion, message = "Calculo realizado" });
            }
            catch (Exception ex)
            {
                return Json(new { exception = ex, message = "Error al realizar el calculo" });
            }
        }

        static List<Elemento> Calculo(List<Elemento> elementos, int caloriasMinimas, int pesoMaximo)
        {
            int n = elementos.Count;
            int[,] dp = new int[n + 1, pesoMaximo + 1];
            List<Elemento>[,] seleccionados = new List<Elemento>[n + 1, pesoMaximo + 1];

            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= pesoMaximo; j++)
                {
                    dp[i, j] = 0;
                    seleccionados[i, j] = new List<Elemento>();
                }
            }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 0; j <= pesoMaximo; j++)
                {
                    dp[i, j] = dp[i - 1, j];
                    seleccionados[i, j] = new List<Elemento>(seleccionados[i - 1, j]);

                    if (elementos[i - 1].Peso <= j)
                    {
                        int caloriasConElemento = dp[i - 1, j - elementos[i - 1].Peso] + elementos[i - 1].Calorias;
                        if (caloriasConElemento > dp[i, j])
                        {
                            dp[i, j] = caloriasConElemento;
                            seleccionados[i, j] = new List<Elemento>(seleccionados[i - 1, j - elementos[i - 1].Peso]);
                            seleccionados[i, j].Add(elementos[i - 1]);
                        }
                    }
                }
            }

            return seleccionados[n, pesoMaximo];
        }

    }
}