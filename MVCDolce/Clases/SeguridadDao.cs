using MVCDolce.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace MVCDolce.Clases
{
    public class SeguridadDao
    {

        DataHelper _datahelper;

        public SeguridadDao()
        {
            _datahelper = new DataHelper(new SqlConnectionStringBuilder(Properties.Settings.Default.conexion_dbDolce));
        }


        public bool AddRegistro(Registro modelo,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strEmail",modelo.StrEmail),
                    new SqlParameter("@strNombre",modelo.StrNombre),
                    new SqlParameter("@strPassword",modelo.StrPassword),
                    new SqlParameter("@strDocumento",modelo.StrCedula)
                };

                var mensaje = new SqlParameter("@strMensaje", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                var respuesta = new SqlParameter("@logRespuesta", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                parametros.Add(mensaje);
                parametros.Add(respuesta);


                var res = _datahelper.EjecutarSp<bool>("sg_spAddRegistroNew", parametros);

                if (res)
                {
                    strMensaje = mensaje.Value.ToString();
                    return Convert.ToBoolean(respuesta.Value);
                }
                else
                {
                    strMensaje = "No hay conexion con la base de datos";
                    return false;
                }


            }
            catch (Exception ex)
            {
                strMensaje = ex.Message;
                return false;
            }
        }

        public bool RecuperarPassword(RecuperarPassword modelo,out string strMensaje)
        {
            try
            {
                //Creamos el nuevo password Aleatorio
               modelo.StrPassword = Membership.GeneratePassword(8,0);

                


                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strDocumento",modelo.StrCedula),
                    new SqlParameter("@strEmail",modelo.StrEmail),
                    new SqlParameter("@strPassword",modelo.StrPassword)

                };

                var mensaje = new SqlParameter("@strMensaje", SqlDbType.VarChar,100) { Direction = ParameterDirection.Output };
                var respuesta = new SqlParameter("@logRespuesta", SqlDbType.Bit) {Direction=ParameterDirection.Output };

                parametros.Add(mensaje);
                parametros.Add(respuesta);


                var res = _datahelper.EjecutarSp<bool>("sp_sgRecuperarPassWord", parametros);

                if (res)
                {

                    if (Convert.ToBoolean(respuesta.Value))
                    {
                        var respuestaCorreo = EmailRecuperarPassword(modelo);

                        if (respuestaCorreo)
                        {
                            strMensaje = "El nuevo password fue enviado a su correo electronico verifiquelo para ingresar a la plataforma";
                            return true;
                        }
                        else
                        {
                            strMensaje = "No se pudo enviar el correo porfavor intente denuevo";
                            return false;
                        }

                    }
                    else
                    {
                        strMensaje = mensaje.Value.ToString();
                        return false;
                    }
                    
                    
                }
                else
                {
                    strMensaje = "No hay conexion con la base de datos";
                    return false;
                }

            }
            catch (Exception ex)
            {
                strMensaje = ex.Message;
                return false;
            }
        }

        private bool EmailRecuperarPassword(RecuperarPassword modelo)
        {
            try
            {
                var mensaje = new MailMessage();
                var smtp = new SmtpClient(Properties.Settings.Default.smtp_dolce);

                mensaje.From = new MailAddress(Properties.Settings.Default.correo_interno);
                mensaje.To.Add(modelo.StrEmail);
                mensaje.Subject = "Restablecer Password Dolce";
                mensaje.Body = "Su Password a sido restablecido \n Su Nuevo Password es " + modelo.StrPassword;

                smtp.Credentials = new NetworkCredential(Properties.Settings.Default.correo_usuario,Properties.Settings.Default.pass_correo);
                smtp.EnableSsl = false;

                smtp.Send(mensaje);

                return true;

            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public Seguridad Ingresar(Seguridad modelo, out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@StrEmail",modelo.StrEmail),
                    new SqlParameter("@StrPassWord",modelo.StrPassword)
                };

                var dt = _datahelper.EjecutarSp<DataTable>("sg_ValidarIngresoMobil", parametros);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        var respuesta = Convert.ToBoolean(dt.Rows[0]["logRespuesta"]);

                        if (respuesta)
                        {
                            var datos = new Seguridad();

                            datos.StrNombre = dt.Rows[0]["strNombreUsuario"].ToString();
                            datos.StrZona = dt.Rows[0]["strZona"].ToString();
                            datos.StrCampaña = dt.Rows[0]["Campaña"].ToString();
                            datos.StrEmail = modelo.StrEmail;
                            datos.LogDivisional = Convert.ToInt16(dt.Rows[0]["LogDivisional"]);

                            strMensaje = "";
                            return datos;
                        }
                        else
                        {
                            strMensaje = dt.Rows[0]["Mensaje"].ToString();
                            return null;
                        }

                    }
                    else
                    {
                        strMensaje = "No hay datos para cargar intentelo denuevo";
                        return null;
                    }
                }
                else
                {
                    strMensaje = "No hay conexion con el servidor";
                    return null;
                }

            }
            catch (Exception ex)
            {
                strMensaje = ex.Message;
                return null;
            }
        }

        public bool CambiodePassword(CabioPassword modelo,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strEmail",modelo.StrEmail),
                    new SqlParameter("@strPassWordAnterior",modelo.StrPasswordAnterior),
                    new SqlParameter("@strPasswornew",modelo.StrPaswordNew)
                };

                var mensaje = new SqlParameter("@strMensaje", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                var respuesta = new SqlParameter("@logRespuesta", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                parametros.Add(mensaje);
                parametros.Add(respuesta);

                var res = _datahelper.EjecutarSp<bool>("sp_sgCambioPassword", parametros);

                if (res)
                {
                    strMensaje = mensaje.Value.ToString();
                    return Convert.ToBoolean(respuesta.Value);
                }
                else
                {
                    strMensaje = "No hay conexion con el servidor";
                    return false;
                }

            }
            catch (Exception ex )
            {
                strMensaje = ex.Message;
                return false;
                
            }
        }


        public DatosVarios DatosIniciales(string strEmail,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strEmail",strEmail)
                };

                var dts = _datahelper.EjecutarSp<DataSet>("ma_spConsultaFiltrosDiv", parametros);

                if (dts != null)
                {
                    if (dts.Tables.Count > 0)
                    {
                        var dtCampañas = dts.Tables[0];
                        var dtZonas = dts.Tables[1];


                        var xlistado = new DatosVarios();

                        var xlistaCampa = new List<Campañas>();

                        for (int i = 0; i < dtCampañas.Rows.Count; i++)
                        {
                            xlistaCampa.Add(new Campañas { StrCampaña = dtCampañas.Rows[i]["strCampaña"].ToString() });
                        }

                        xlistado.ListaCampañas = xlistaCampa;


                        var xlistaZonas = new List<Zonas>();
                        for (int i = 0; i < dtZonas.Rows.Count; i++)
                        {
                            xlistaZonas.Add(new Zonas { StrZona = dtZonas.Rows[i]["strZona"].ToString() });

                        }

                        xlistado.ListaZonas = xlistaZonas;

                        strMensaje = "";
                        return xlistado;
                    }
                    else
                    {
                        strMensaje = "No hay datos para cargar";
                        return null;
                    }
                }
                else
                {
                    strMensaje = "No haqy conexion con el servidor";
                    return null;
                }
            }
            catch (Exception ex)
            {
                strMensaje = ex.Message;
                return null;
            }
        }
    }
}
