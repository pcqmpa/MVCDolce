using MVCDolce.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDolce.Clases
{
   
   public class VisitasDao
    {

        DataHelper _datahelper;
        private string _strMensaje;
        public VisitasDao()
        {
            _datahelper = new DataHelper(new SqlConnectionStringBuilder(Properties.Settings.Default.conexion_dbDolce));
        }


        public bool AddPdh (Pdh model,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strEmail",model.StrEmail),
                    new SqlParameter("@idTipoVisita",TpVisitas.Pdh),
                    new SqlParameter("@strNombre",model.StrNombre),
                    new SqlParameter("@strObservacion",model.StrObservacion)
                };

                var mensaje = new SqlParameter("@strMensaje", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                var respuesta = new SqlParameter("@logRespuesta", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                var id = new SqlParameter("@numCodigo", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                parametros.Add(mensaje);
                parametros.Add(respuesta);
                parametros.Add(id);

                var res = _datahelper.EjecutarSp<bool>("sp_vtAddVisitasMobil", parametros);

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
            catch (Exception ex)
            {
                strMensaje = ex.Message;
                return false;
            }
        }


        public bool AddPosibleNueva(PosibleNueva modelo,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strEmail",modelo.StrEmail),
                    new SqlParameter("@idTipoVisita",TpVisitas.PosibleNueva),
                    new SqlParameter("@strDocumento",modelo.StrDocumento),
                    new SqlParameter("@strNombre",modelo.StrNombres.Trim() + ' ' + modelo.StrApellidos.Trim()),
                    new SqlParameter("@strObservacion","Posible ingreso de nueva")

                };

                var mensaje = new SqlParameter("@strMensaje", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                var respuesta = new SqlParameter("@logRespuesta", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                var id = new SqlParameter("@numCodigo", SqlDbType.BigInt) { Direction = ParameterDirection.Output };


                parametros.Add(mensaje);
                parametros.Add(respuesta);
                parametros.Add(id);

                var res = _datahelper.EjecutarSp<bool>("sp_vtAddVisitasMobil", parametros);

                if (res)
                {
                    if (Convert.ToBoolean(respuesta.Value))
                    {
                        modelo.Id = Convert.ToInt64(id.Value);
                        var ingreso = AddProspectoNueva(modelo,out _strMensaje);

                        if (ingreso)
                        {
                            strMensaje = mensaje.Value.ToString();
                            return true;
                        }
                        else
                        {
                            strMensaje = "La visita fue ingresada correctamente pero los datos de la asesora no se guardaron por favor intente de nuevo";
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
                    strMensaje = "No hay conexion con el servidor";
                    return false;
                }

            }
            catch (Exception ex)
            {
                strMensaje = ex.Message;
                return false;
            }
        }

        private bool AddProspectoNueva(PosibleNueva modelo,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@IdVisita",modelo.Id),
                    new SqlParameter("@strCedula",modelo.StrDocumento),
                    new SqlParameter("@strNombres",modelo.StrNombres),
                    new SqlParameter("@strApellidos",modelo.StrApellidos),
                    new SqlParameter("@strDireccion",modelo.StrDireccion),
                    new SqlParameter("@strTelfono",modelo.StrTelefono),
                    new SqlParameter("@strCelular",modelo.StrCelular),
                    new SqlParameter("@strDepartamento",modelo.StrDepartamento),
                    new SqlParameter("@strCiudad",modelo.StrCiudad),
                    new SqlParameter("@strBarrio",modelo.StrBarrio)

                };

                var mensaje = new SqlParameter("@strMensaje", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                var respuesta = new SqlParameter("@logRespuesta", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                parametros.Add(mensaje);
                parametros.Add(respuesta);

                var res = _datahelper.EjecutarSp<bool>("sp_vtPosibleNueva", parametros);

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

        public List<ListaVisitaApoyo> ListadoVisitadeApoyo(string usuario,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>() { new SqlParameter("@strEmail", usuario) };

                var dt = _datahelper.EjecutarSp<DataTable>("sp_vtConsultaListaVistaApoyo", parametros);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        var xLista = new List<ListaVisitaApoyo>();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            xLista.Add(new ListaVisitaApoyo
                            {
                                Id = Convert.ToInt64(dt.Rows[i]["numCodigo"]),
                                StrDocumento=dt.Rows[i]["strCedula"].ToString(),
                                StrNombre=dt.Rows[i]["Nombre"].ToString(),
                                LogVisita=Convert.ToBoolean(dt.Rows[i]["logVisitaApoyo"])
                            });
                        }

                        strMensaje = "";
                        return xLista;
                    }
                    else
                    {
                        strMensaje = "No hay datos para cargar debe de realizar primero el paso 1";
                        return null;
                    }
                }
                else
                {
                    strMensaje = "No hay conexion xon el servidor";
                    return null;
                }

            }
            catch (Exception ex)
            {
                strMensaje = ex.Message;
                return null;
                
            }
        }

        public bool AddApoyo(Apoyo modelo, out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strEmail",modelo.StrEmail),
                    new SqlParameter("@idTipoVisita",TpVisitas.Apoyo),
                    new SqlParameter("@strDocumento",modelo.StrDocumento),
                    new SqlParameter("@strNombre",modelo.StrNombre),
                    new SqlParameter("@strObservacion",modelo.StrObservacion),
                    new SqlParameter("@logPedido",modelo.LogPedido)
                };

                var mensaje = new SqlParameter("@strMensaje", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                var respuesta = new SqlParameter("@logRespuesta", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                var id = new SqlParameter("@numCodigo", SqlDbType.BigInt) { Direction = ParameterDirection.Output };

                parametros.Add(mensaje);
                parametros.Add(respuesta);
                parametros.Add(id);

                var res = _datahelper.EjecutarSp<bool>("sp_vtAddVisitasMobil", parametros);

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
            catch (Exception ex)
            {
                strMensaje = ex.Message;
                return false;
            }
        }

        public List<Nuevas> ConsultaNuevas(string strUsuario,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strEmail",strUsuario)
                };

                var dt = _datahelper.EjecutarSp<DataTable>("sp_vtConsultaNuevasVisita",parametros);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        var xlista = new List<Nuevas>();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            xlista.Add(new Nuevas
                            {
                                StrDocumento = dt.Rows[i]["strIdentificacion"].ToString(),
                                StrNombre = dt.Rows[i]["Nombre"].ToString(),
                                StrTelefono= dt.Rows[i]["Telefono"].ToString()
                            });
                        }

                        strMensaje = "";
                        return xlista;


                    }
                    else
                    {
                        strMensaje = "No hay datos para cargar";
                        return null;
                    }
                }
                else
                {
                    strMensaje = "No hay conexion con la base de datoa";
                    return null;
                }

            }
            catch (Exception ex )
            {
                strMensaje = ex.Message;
                return null;
            }
        }

        public Nuevas ConsultaNueva(string strUsuario,string strDocumento,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strEmail",strUsuario),
                    new SqlParameter("@strDocumento",strDocumento)
                };

                var dt = _datahelper.EjecutarSp<DataTable>("sp_vt_ConsultaNuevaGeneral", parametros);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        var dato = new Nuevas();

                        dato.StrDocumento = dt.Rows[0]["strIdentificacion"].ToString();
                        dato.StrNombre = dt.Rows[0]["Nombre"].ToString();
                        dato.NumPuntos = Convert.ToInt32(dt.Rows[0]["Puntos"]);
                        dato.CurValorPedido = Convert.ToDouble(dt.Rows[0]["ValorPedido"]);

                        strMensaje = "";
                        return dato;

                    }
                    else
                    {
                        strMensaje = "No hay datos para cargar";
                        return null;
                    }
                }
                else
                {
                    strMensaje = "No hay cnexion con el servisor";
                    return null;
                }

            }
            catch (Exception ex )
            {
                strMensaje = ex.Message;
                return null;
            }
        }


        public bool AddNueva(Nuevas modelo,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strEmail",modelo.StrEmail),
                    new SqlParameter("@idTipoVisita",TpVisitas.Nueva),
                    new SqlParameter("@strDocumento",modelo.StrDocumento),
                    new SqlParameter("@strNombre",modelo.StrNombre),
                    new SqlParameter("@strObservacion",modelo.StrObservacion)
                };

                var mensaje = new SqlParameter("@strMensaje", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                var respuesta = new SqlParameter("@logRespuesta", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                var id = new SqlParameter("@numCodigo", SqlDbType.BigInt) { Direction = ParameterDirection.Output };

                parametros.Add(mensaje);
                parametros.Add(respuesta);
                parametros.Add(id);

                var res = _datahelper.EjecutarSp<bool>("sp_vtAddVisitasMobil", parametros);

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
            catch (Exception ex)
            {
                strMensaje = ex.Message;
                return false;
               
            }
        }


        public List<ListaCobranza> ListadoCobranza(string strUsuario,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strEmail",strUsuario)
                };

                var dt = _datahelper.EjecutarSp<DataTable>("vt_spListadoCobranzaVisitas", parametros);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        var xlista = new List<ListaCobranza>();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            xlista.Add(new ListaCobranza
                            {
                                StrCampaña = dt.Rows[i]["Campaña"].ToString(),
                                StrDocumento = dt.Rows[i]["Cédula"].ToString(),
                                StrNombre = dt.Rows[i]["NombreAsesora"].ToString(),
                                CurSaldo = Convert.ToDouble(dt.Rows[i]["ValorSaldo"]),
                                NumPuntos=Convert.ToInt32(dt.Rows[i]["Puntos"]),
                                DtmFechaVencimiento = dt.Rows[i]["Fechavencimiento"].ToString(),
                                StrTelefono=dt.Rows[i]["Telefono"].ToString()
                            });
                        }

                        strMensaje = "";
                        return xlista;
                    }
                    else
                    {
                        strMensaje = "No hay datos para cargar";
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

        public bool AddCobranza(ListaCobranza modelo,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strEmail",modelo.StrEmail),
                    new SqlParameter("@idTipoVisita",TpVisitas.Cobranza),
                    new SqlParameter("@strDocumento",modelo.StrDocumento),
                    new SqlParameter("@strNombre",modelo.StrNombre),
                    new SqlParameter("@strObservacion",modelo.StrObservacion)
                };

                var mensaje = new SqlParameter("@strMensaje", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                var respuesta = new SqlParameter("@logRespuesta", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                var id = new SqlParameter("@numCodigo", SqlDbType.BigInt) { Direction = ParameterDirection.Output };

                parametros.Add(mensaje);
                parametros.Add(respuesta);
                parametros.Add(id);

                var res = _datahelper.EjecutarSp<bool>("sp_vtAddVisitasMobil", parametros);

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
            catch (Exception ex)
            {
                strMensaje = ex.Message;
                return false;
            }
        }

        public Motivacion ConsultaMotivacion(string strUsuario, string strDocumento,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strEmail",strUsuario),
                    new SqlParameter("@strDocumento",strDocumento)
                };


                var dt = _datahelper.EjecutarSp<DataTable>("sp_vtConsultaAsesoraMotivacion", parametros);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        var xdatos = new Motivacion();

                        xdatos.StrZona = dt.Rows[0]["strZona"].ToString();
                        xdatos.StrDocumento = dt.Rows[0]["strIdentificacion"].ToString();
                        xdatos.StrNombre = dt.Rows[0]["Nombre"].ToString();
                        xdatos.NumPuntos = Convert.ToInt32(dt.Rows[0]["Puntos"]);
                        xdatos.StrUltimaCampaña = dt.Rows[0]["UltimaCampaña"].ToString();

                        strMensaje = "";
                        return xdatos;
                    }
                    else
                    {
                        strMensaje = "La Asesora no existe o no pertenece a la zona";
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

        public bool AddMotivacion(Motivacion modelo,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strEmail",modelo.StrEmail),
                    new SqlParameter("@idTipoVisita",TpVisitas.Motivacion),
                    new SqlParameter("@strDocumento",modelo.StrDocumento),
                    new SqlParameter("@strNombre",modelo.StrNombre),
                    new SqlParameter("@strObservacion",modelo.StrObservacion)
                };

                var mensaje = new SqlParameter("@strMensaje", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                var respuesta = new SqlParameter("@logRespuesta", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                var id = new SqlParameter("@numCodigo", SqlDbType.BigInt) { Direction = ParameterDirection.Output };

                parametros.Add(mensaje);
                parametros.Add(respuesta);
                parametros.Add(id);

                var res = _datahelper.EjecutarSp<bool>("sp_vtAddVisitasMobil", parametros);

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
            catch (Exception ex)
            {
                strMensaje = ex.Message;
                return false;
            }
        }

        public List<PosiblesReingresos> ListaPosiblesReingresos(string strUsuario,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strEmail",strUsuario)
                };

                var dt = _datahelper.EjecutarSp<DataTable>("vt_spPosiblesReingresosVisitas",parametros);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        var xlista = new List<PosiblesReingresos>();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            xlista.Add(new PosiblesReingresos
                            {
                                StrDocumento = dt.Rows[i]["Cedula"].ToString(),
                                StrNombre = dt.Rows[i]["NombreAsesora"].ToString(),
                                StrUltimaCampaña = dt.Rows[i]["UltimaCampaña"].ToString(),
                                StrTelefono=dt.Rows[i]["Telefono"].ToString()
                            });
                            
                        }

                        strMensaje = "";
                        return xlista;
                    }
                    else
                    {
                        strMensaje = "No hay datos para cargar";
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

        public bool AddPosibleReingreso(PosiblesReingresos modelo,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strEmail",modelo.StrEmail),
                    new SqlParameter("@idTipoVisita",TpVisitas.PosibleReingreso),
                    new SqlParameter("@strDocumento",modelo.StrDocumento),
                    new SqlParameter("@strNombre",modelo.StrNombre),
                    new SqlParameter("@strObservacion",modelo.StrObservacion)
                };

                var mensaje = new SqlParameter("@strMensaje", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                var respuesta = new SqlParameter("@logRespuesta", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                var id = new SqlParameter("@numCodigo", SqlDbType.BigInt) { Direction = ParameterDirection.Output };

                parametros.Add(mensaje);
                parametros.Add(respuesta);
                parametros.Add(id);

                var res = _datahelper.EjecutarSp<bool>("sp_vtAddVisitasMobil", parametros);

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
            catch (Exception ex)
            {
                strMensaje = ex.Message;
                return false;
                
            }
        }

        public List<InformeVisitas> InformeVisitas(string strCampaña, string strZona, string strDivision, string strUsuario ,out string strMensaje)
        {
            try
            {

                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strCampaña",strCampaña),
                    new SqlParameter("@strDivision", (strDivision=="")? null : strDivision),
                    new SqlParameter("@strZona",(strZona=="")? null : strZona),
                    new SqlParameter("@strEmail",strUsuario)
                };

                var dt = _datahelper.EjecutarSp<DataTable>("vt_spConsultaVisitasXcampaña", parametros);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        var xlista = new List<InformeVisitas>();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            xlista.Add(new InformeVisitas
                            {
                                StrCampaña = dt.Rows[i]["Campaña"].ToString(),
                                StrDivision = dt.Rows[i]["Division"].ToString(),
                                StrZona = dt.Rows[i]["Zona"].ToString(),
                                StrDocumento = dt.Rows[i]["Identificacion"].ToString(),
                                StrNombre = dt.Rows[i]["Nombre"].ToString(),
                                IdTipoVisita = Convert.ToInt16(dt.Rows[i]["IdTipoVisita"]),
                                StrTipoVisita = dt.Rows[i]["TipoVisita"].ToString(),
                                StrObservacion = dt.Rows[i]["strObservacion"].ToString(),
                                LogPedido=dt.Rows[i]["TienePedido"].ToString(),
                                DtmFecha=dt.Rows[i]["Fecha"].ToString(),
                                DtmHora=dt.Rows[i]["Hora"].ToString()

                            });
                        }




                        strMensaje = "";
                        return xlista;
                    }
                    else
                    {
                        strMensaje = "No hay datos para cargar";
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

        public List<Motivacion> ListadoMotivacion(string strUsuario,out string strMensaje)
        {
            try
            {
                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("@strEmail",strUsuario)
                };

                var dt = _datahelper.EjecutarSp<DataTable>("vt_spConsultasVisitasMotivacion", parametros);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        var xlista = new List<Motivacion>();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            xlista.Add(new Motivacion
                            {
                                StrDocumento=dt.Rows[i]["Cedula"].ToString(),
                                StrNombre=dt.Rows[i]["Nombre"].ToString(),
                                StrTelefono=dt.Rows[i]["Telefono"].ToString(),
                                StrTipoAsesora=dt.Rows[i]["Tipo"].ToString(),
                                NumPuntos=Convert.ToInt16(dt.Rows[i]["PuntosActivos"]),
                                StrUltimaCampaña=dt.Rows[i]["UltimaCampaña"].ToString()
                            });
                        }

                        strMensaje = "";
                        return xlista;
                    }
                    else
                    {
                        strMensaje = "No hay datos para cargar";
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
    }


    public enum TpVisitas
    {
        Pdh=1,
        PosibleNueva=2,
        Apoyo=3,
        Nueva=4,
        Cobranza=5,
        Motivacion = 6,
        PosibleReingreso= 7

    }
}
