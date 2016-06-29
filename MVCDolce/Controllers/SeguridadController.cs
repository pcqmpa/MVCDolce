using MVCDolce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCDolce.Controllers
{
    public class SeguridadController : Controller
    {
        private string _strMensaje;
        // GET: Seguridad
        public ActionResult Index()
        {
            Session["Usuario"] = "";
            Session["LogDivisional"] = "";
            return View();
        }

        [HttpPost]
        public ActionResult Index(Seguridad  modelo)
        {

            if (ModelState.IsValid)
            {

                var vc = new Clases.SeguridadDao();

                var datos = vc.Ingresar(modelo, out _strMensaje);

                if (datos != null)
                {

                    Session["Usuario"] = datos.StrEmail;
                    Session["Nombre"] = datos.StrNombre;
                    Session["Campana"] = datos.StrCampaña;
                    Session["Zona"] = datos.StrZona;
                    Session["LogDivisional"] = datos.LogDivisional;


                    ViewBag.usuario = datos.StrEmail;

                    if (datos.LogDivisional==1)
                    {
                        return RedirectToAction("MenuDivisional", "Visitas");
                    }
                    else
                    {

                        return RedirectToAction("Index", "Visitas");
                    }
                                       
                    

                }
                else
                {
                    ViewBag.Error = _strMensaje;
                    return View();
                }


                
            }
            else
            {
                ViewBag.Error = "Los datos no estan correctos";
                return View();
            }
           

        }

        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registro(Registro model)
        {
            if (ModelState.IsValid)
            {

                var vc = new Clases.SeguridadDao();

                var respuesta = vc.AddRegistro(model, out _strMensaje);

                if (respuesta)
                {
                    ViewBag.mensaje = _strMensaje;
                    return View();
                }
                else
                {
                    ViewBag.Error = _strMensaje;
                    return View();
                }

                
            }
            else
            {
                ViewBag.Error = "Los datos no estan correctos";
                return View();
            }
        }

        public ActionResult RecuperarPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecuperarPassword(RecuperarPassword modelo)
        {
            if (ModelState.IsValid)
            {

                var vc = new Clases.SeguridadDao();

                var res = vc.RecuperarPassword(modelo, out _strMensaje);

                if (res)
                {
                    ViewBag.mensaje = _strMensaje;
                    return View();
                }
                else
                {
                    ViewBag.Error = _strMensaje;
                    return View();
                }               
               
            }
            else
            {
                ViewBag.Error = "Los datos no estan correctos";
                return View();
            }
        }


        public ActionResult CambioPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CambioPassword(CabioPassword modelo)
        {
            if (ModelState.IsValid)
            {

                var usuario = Session["Usuario"].ToString();

                modelo.StrEmail = usuario;

                var vc = new Clases.SeguridadDao();

                var res = vc.CambiodePassword(modelo, out _strMensaje);



                if (res)
                {
                    ViewBag.mensaje = _strMensaje;
                    return View();
                }
                else
                {
                    ViewBag.Error = _strMensaje;
                    return View();
                }


                
            }
            else
            {
                ViewBag.Error = "Los datos no estan correctos";
                return View();
            }
        }

        


    }
}