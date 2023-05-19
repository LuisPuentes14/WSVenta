﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WSVenta.Models;
using WSVenta.Models.Response;
using WSVenta.Models.Request;
using WSVenta.Services;
using Microsoft.AspNetCore.Authorization;

namespace WSVenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClienteController : ControllerBase
    {     

        [HttpGet]
        public IActionResult Get()
        {
            Respuesta oRespuesta = new Respuesta();
         
            try
            {

                using (VentaRealContext db = new VentaRealContext())
                {
                    var lst = db.Clientes.OrderBy(d=>d.Id).ToList();
                    oRespuesta.Exito = 1;
                    oRespuesta.Data = lst;

                }

            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message ;
              
            }

            return Ok(oRespuesta);
        }

        [HttpPost]
        public IActionResult Add(ClienteRequest oModel)
        {
            Respuesta oRespuesta = new Respuesta();

            try
            {
                using (VentaRealContext db = new VentaRealContext())
                {
                    Cliente oCliente = new Cliente();
                    oCliente.Nombre = oModel.Nombre;

                    db.Clientes.Add(oCliente);
                    db.SaveChanges();

                    oRespuesta.Exito = 1;

                }


            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
               
            }

            return Ok(oRespuesta);

        }

        [HttpPut]
        public IActionResult Edit(ClienteRequest oModel)
        {
            Respuesta oRespuesta = new Respuesta();


            try
            {
                using (VentaRealContext db = new VentaRealContext())
                {
                    Cliente oCliente = db.Clientes.Find(oModel.Id);
                    oCliente.Nombre = oModel.Nombre;

                    db.Entry(oCliente).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();

                    oRespuesta.Exito = 1;

                }


            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;

            }

            return Ok(oRespuesta);

        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Respuesta oRespuesta = new Respuesta();


            try
            {
                using (VentaRealContext db = new VentaRealContext())
                {
                    Cliente oCliente = db.Clientes.Find(id);                  

                    db.Remove(oCliente);
                    db.SaveChanges();

                    oRespuesta.Exito = 1;

                }


            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;

            }

            return Ok(oRespuesta);

        }


    }
}
