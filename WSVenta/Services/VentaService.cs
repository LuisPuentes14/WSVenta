using WSVenta.Models.Response;
using WSVenta.Models;
using WSVenta.Models.Request;

namespace WSVenta.Services
{
    public class VentaService : IVentaService
    {
        public void add(VentaRequest model)
        {


            using (VentaRealContext db = new VentaRealContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var venta = new Ventum();
                        venta.Total = model.Conceptos.Sum(d => d.cantidad * d.PrecioUnitario);
                        venta.Fecha = DateTime.Now;
                        venta.IdCliente = model.IdCliente;
                        db.Venta.Add(venta);
                        db.SaveChanges();

                        foreach (var modelConcepto in model.Conceptos)
                        {
                            var concepto = new Models.Concepto();
                            concepto.Cantidad = modelConcepto.cantidad;
                            concepto.IdProducto = modelConcepto.IdProducto;
                            concepto.PrecioUnitario = modelConcepto.PrecioUnitario;
                            concepto.Importe = modelConcepto.Importe;
                            concepto.IdVenta = venta.Id;
                            db.Conceptos.Add(concepto);
                            db.SaveChanges();
                        }
                        transaction.Commit();


                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw new Exception("Ocurrio un error en la inserción");
                    }

                }

            }



        }

       
    }
}
