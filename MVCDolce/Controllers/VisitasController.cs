using MVCDolce.Clases;
using MVCDolce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCDolce.Controllers
{
    public class VisitasController : Controller
    {
        // GET: Visitas
        private string _strMensaje;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuDivisional()
        {
            return View();
        }

        public ActionResult Pdh()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Pdh(Pdh modelo)
        {

            if (ModelState.IsValid)
            {
                var vc = new Clases.VisitasDao();

                var usuario = Session["Usuario"].ToString();

                modelo.StrEmail = usuario;


                var res = vc.AddPdh(modelo, out _strMensaje);

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
                ViewBag.Error = "Los datos no estan completos";
                return View();
            }

        }

        public ActionResult PosibleNueva()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PosibleNueva(PosibleNueva modelo)
        {
            if (ModelState.IsValid)
            {
                var vc = new Clases.VisitasDao();

                var usuario = Session["Usuario"].ToString();

                modelo.StrEmail = usuario;

                var res = vc.AddPosibleNueva(modelo, out _strMensaje);

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
                ViewBag.Error = "Los datos no estan completos";
                return View();
            }
        }

        public ActionResult ListadoApoyo()
        {
            var vc =new Clases.VisitasDao();

            var usuario = Session["Usuario"].ToString();

            var lista = vc.ListadoVisitadeApoyo(usuario, out _strMensaje);

            if (lista != null)
            {
                ViewData["ListaApoyo"] = lista;
              
            }
            else
            {
                ViewBag.Error = _strMensaje;
            }      

            return View();
        }

        public ActionResult Apoyo(string strCedula,string strNombre)
        {
            ViewBag.nombre = strNombre;
            ViewBag.cedula = strCedula;

            return View();
        }
        [HttpPost]
        public ActionResult Apoyo(Apoyo modelo)
        {
          
            
            var vc = new Clases.VisitasDao();

            bool pedido = false;

            if (modelo.StrPedido != null)
            {
                pedido = true;
            }



            var usuario = Session["Usuario"].ToString();

            modelo.StrEmail = usuario;
            modelo.LogPedido = pedido;

            var res = vc.AddApoyo(modelo, out _strMensaje);

            if (res)
            {
                ViewBag.mensaje = _strMensaje;

            }
            else
            {
                ViewBag.Error = _strMensaje;
            }

            


            return View();
        }

        public ActionResult ListadoNuevas()
        {

            var usuario = Session["Usuario"].ToString();

            var vc = new Clases.VisitasDao();

            var datos = vc.ConsultaNuevas(usuario, out _strMensaje);

            if (datos != null)
            {
                ViewData["ListaNuevas"] = datos;
            }
            else
            {
                ViewBag.Error = _strMensaje;
            }

            return View();
        }

        public ActionResult Nuevas(string strCedula)
        {

            var usuario = Session["Usuario"].ToString();
            var vc = new Clases.VisitasDao();

            var datos = vc.ConsultaNueva(usuario, strCedula, out _strMensaje);

            ViewBag.cedula = strCedula;
            ViewBag.nombre = datos.StrNombre;
            ViewBag.valor = datos.CurValorPedido;
            ViewBag.puntos = datos.NumPuntos;

            return View();
        }

        [HttpPost]
        public ActionResult Nuevas(Nuevas modelo)
        {
            var usuario = Session["Usuario"].ToString();

            modelo.StrEmail = usuario;

            var vc = new Clases.VisitasDao();

            var res = vc.AddNueva(modelo, out _strMensaje);

            if (res)
            {
                ViewBag.mensaje = _strMensaje;
                
            }
            else
            {
                ViewBag.Error = _strMensaje;
                
            }

            return View();


        }

        public ActionResult ListadoCobranza()
        {
            var usuario = Session["Usuario"].ToString();

            var vc = new Clases.VisitasDao();

            var xlista = vc.ListadoCobranza(usuario, out _strMensaje);

            if (xlista != null)
            {
                ViewData["ListaCobranza"] = xlista;
            }
            else
            {
                ViewBag.Error = _strMensaje;
            }

            return View();

        }

        public ActionResult Cobranza(string strCedula,string strNombre,double curSaldo,string campana,Int32 puntos)
        {

            ViewBag.cedula = strCedula;
            ViewBag.nombre = strNombre;
            ViewBag.saldo = curSaldo;
            ViewBag.campana = campana;
            ViewBag.puntos = puntos;

            return View();


        }

        [HttpPost]
        public ActionResult Cobranza(ListaCobranza modelo)
        {

            ViewBag.cedula = modelo.StrDocumento;
            ViewBag.nombre = modelo.StrNombre;
            ViewBag.saldo = modelo.CurSaldo;
            ViewBag.campana = modelo.StrCampaña;
            ViewBag.puntos = modelo.NumPuntos;

            var usuario = Session["Usuario"].ToString();

            modelo.StrEmail = usuario;

            var vc = new Clases.VisitasDao();

            var res = vc.AddCobranza(modelo, out _strMensaje);

            if (res)
            {
                ViewBag.mensaje = _strMensaje;
                //return RedirectToAction("Index", "Visitas");
            }
            else
            {
                ViewBag.Error = _strMensaje;
               
            }

             return View();

        }

        public ActionResult ListaMotivacion()
        {

            var usuario = Session["Usuario"].ToString();

            var vc = new Clases.VisitasDao();

            var datos = vc.ListadoMotivacion(usuario, out _strMensaje);

            if (datos != null)
            {
                ViewData["ListaMotivacion"] = datos;
                return View();
            }
            else
            {
                ViewBag.Error = _strMensaje;
                return View();
            }

            
        }

        public ActionResult Motivacion(string cedula)
        {
            ViewBag.cedula = cedula;
            return View();
        }

        [HttpPost]
        public ActionResult Motivacion(Motivacion modelo,string strMensaje="")
        {
            if (string.IsNullOrEmpty(modelo.StrObservacion))
            {
                ViewBag.Error = "Debe ingresar una observacion";
                return View(modelo);
            }
            else
            {
                var usuario = Session["Usuario"].ToString();

                modelo.StrEmail = usuario;

                var vc = new Clases.VisitasDao();

                var res = vc.AddMotivacion(modelo, out _strMensaje);

                if (res)
                {
                    ViewBag.mensaje = _strMensaje;
                    return View(modelo);
                }
                else
                {
                    ViewBag.Error = _strMensaje;
                    return View(modelo);
                }
            }
        }


        [HttpPost]
        public ActionResult ConsultaAsesoraMotivacion(string strDocumento)
        {
            var usuario = Session["Usuario"].ToString();

            var vc = new Clases.VisitasDao();

            var xdatos = vc.ConsultaMotivacion(usuario, strDocumento, out _strMensaje);

            if (xdatos != null)
            {
                return Json(new
                {
                    datos=xdatos
                });
            }
            else
            {
                return Json(new
                {
                    Error = _strMensaje
                });
            }
        }


        public ActionResult ListadoPosiblesReingresos()
        {
            var vc = new Clases.VisitasDao();

            var usuario = Session["Usuario"].ToString();

            var xlista = vc.ListaPosiblesReingresos(usuario, out _strMensaje);

            if (xlista != null)
            {
                ViewData["ListaPosibles"] = xlista;

            }
            else
            {
                ViewBag.Error = _strMensaje;
            }

            return View();
        }

        public ActionResult PosibleReingreso(string strCedula,string strNombre,string campana)
        {

            ViewBag.cedula = strCedula;
            ViewBag.nombre = strNombre;
            ViewBag.campana = campana;

            return View();
        }


        [HttpPost]
        public ActionResult PosibleReingreso(PosiblesReingresos modelo)
        {

            ViewBag.cedula =modelo.StrDocumento;
            ViewBag.nombre = modelo.StrNombre;
            ViewBag.campana = modelo.StrUltimaCampaña;

            if (string.IsNullOrEmpty(modelo.StrObservacion))
            {
                ViewBag.Error = "Debe de ingresar una observacion";
                return View();
            }
            else
            {
                var usuario = Session["Usuario"].ToString();
                modelo.StrEmail = usuario;

                var vc = new Clases.VisitasDao();

                var res = vc.AddPosibleReingreso(modelo, out _strMensaje);

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


        }

        public ActionResult ListadoVisitas()
        {
            var campaña = Session["Campana"].ToString();
            var usuario = Session["Usuario"].ToString();

            var vc = new Clases.VisitasDao();

            var xlista = vc.InformeVisitas(campaña, string.Empty, string.Empty, usuario, out _strMensaje);


          
                                

            if (xlista != null)
            {

                var totalPdh = xlista.Where(x => x.IdTipoVisita == Convert.ToInt16(TpVisitas.Pdh)).Count();
                var totalPosibleNueva = xlista.Where(x => x.IdTipoVisita == Convert.ToInt16(TpVisitas.PosibleNueva)).Count();
                var totalApoyo = xlista.Where(x => x.IdTipoVisita == Convert.ToInt16(TpVisitas.Apoyo)).Count();
                var totalNuevas = xlista.Where(x => x.IdTipoVisita == Convert.ToInt16(TpVisitas.Nueva)).Count();
                var totalCobranza = xlista.Where(x => x.IdTipoVisita == Convert.ToInt16(TpVisitas.Cobranza)).Count();
                var totalMotivacion = xlista.Where(x => x.IdTipoVisita == Convert.ToInt16(TpVisitas.Motivacion)).Count();
                var totalPosibleReingreso = xlista.Where(x => x.IdTipoVisita == Convert.ToInt16(TpVisitas.PosibleReingreso)).Count();


                ViewBag.pdh = totalPdh;
                ViewBag.posiblenueva = totalPosibleNueva;
                ViewBag.apoyo = totalApoyo;
                ViewBag.nueva = totalNuevas;
                ViewBag.cobranza = totalCobranza;
                ViewBag.motavacion = totalMotivacion;
                ViewBag.posiblereingreso = totalPosibleReingreso;

                ViewData["ListaInforme"] = xlista;


                return View();
            }
            else
            {
                ViewBag.Error = _strMensaje;
                return View();
            }

        }

        public ActionResult InformeVisitasDivisional()
        {
            var usuario = Session["Usuario"].ToString();

            var vc = new Clases.SeguridadDao();

            var xlista = vc.DatosIniciales(usuario, out _strMensaje);

            if (xlista != null)
            {
                ViewData["Campanas"] = xlista.ListaCampañas;
                ViewData["Zonas"] = xlista.ListaZonas;
                return View();
            }
            else
            {
                ViewBag.Error = _strMensaje;
                return View();
            }


        }

        [HttpPost]
        public ActionResult InformeVisitasDivisional(InformeVisitasDiv modelo)
        {

            var usuario = Session["Usuario"].ToString();

            var clas1 = new Clases.SeguridadDao();

            var xlistaIni = clas1.DatosIniciales(usuario, out _strMensaje);

            if (xlistaIni != null)
            {
                ViewData["Campanas"] = xlistaIni.ListaCampañas;
                ViewData["Zonas"] = xlistaIni.ListaZonas;

                var vc = new Clases.VisitasDao();

                var division = Session["Zona"].ToString();


                var xlista = vc.InformeVisitas(modelo.StrCampaña, modelo.StrZona, division, usuario, out _strMensaje);

                if (xlista != null)
                {

                    var totalPdh = xlista.Where(x => x.IdTipoVisita == Convert.ToInt16(TpVisitas.Pdh)).Count();
                    var totalPosibleNueva = xlista.Where(x => x.IdTipoVisita == Convert.ToInt16(TpVisitas.PosibleNueva)).Count();
                    var totalApoyo = xlista.Where(x => x.IdTipoVisita == Convert.ToInt16(TpVisitas.Apoyo)).Count();
                    var totalNuevas = xlista.Where(x => x.IdTipoVisita == Convert.ToInt16(TpVisitas.Nueva)).Count();
                    var totalCobranza = xlista.Where(x => x.IdTipoVisita == Convert.ToInt16(TpVisitas.Cobranza)).Count();
                    var totalMotivacion = xlista.Where(x => x.IdTipoVisita == Convert.ToInt16(TpVisitas.Motivacion)).Count();
                    var totalPosibleReingreso = xlista.Where(x => x.IdTipoVisita == Convert.ToInt16(TpVisitas.PosibleReingreso)).Count();


                    ViewBag.pdh = totalPdh;
                    ViewBag.posiblenueva = totalPosibleNueva;
                    ViewBag.apoyo = totalApoyo;
                    ViewBag.nueva = totalNuevas;
                    ViewBag.cobranza = totalCobranza;
                    ViewBag.motavacion = totalMotivacion;
                    ViewBag.posiblereingreso = totalPosibleReingreso;


                    ViewData["ListaVisitas"] = xlista;
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
                ViewBag.Error = _strMensaje;
                return View();
            }




            
        }
    }
}