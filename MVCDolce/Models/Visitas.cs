using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDolce.Models
{
    public class Visitas
    {

    }

    public class Pdh
    {
        [Required(ErrorMessage = "El nombre es Requerido")]
        [Display(Name = "Nombre de la Asesora")]
        [RegularExpression("^(?!Aa|Bb|Cc).*$", ErrorMessage = "Para el nombre solo se admiten letras")]
        public string StrNombre { get; set; }

        [Display(Name = "Telefono fijo o Celular")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Para el Telefono solo se admiten numeros")]
        [StringLength(10, ErrorMessage = "El telefono debe contener maximo 10 caracteres")]
        public string StrTelefono { get; set; }

        [Required(ErrorMessage = "La Observacion es Requerida")]
        [Display(Name = "Observacion")]
        [MinLength(10, ErrorMessage = "Minimo debe de escribir 10 caracteres")]
        public string StrObservacion { get; set; }

        public string StrEmail { get; set; }
    }

    public class PosibleNueva
    {
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "El documento es requerido")]
        [Display(Name = "Documento No")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "En el campo documento solo se admiten numeros")]
        [MinLength(5, ErrorMessage = "La cedula debe de ser minimo de 5 caracteres")]
        public string StrDocumento { get; set; }


        [Required(ErrorMessage = "El Nombre es requerido")]
        [Display(Name = "Nombres")]
        [RegularExpression("^(?!Aa|Bb|Cc).*$", ErrorMessage = "Para el nombre solo se admiten letras")]
        [MinLength(5, ErrorMessage = "El Campo nombre debe de tener minimo 5 caracteres")]
        public string StrNombres { get; set; }

        [Required(ErrorMessage = "El Apellido es requerido")]
        [Display(Name = "Apellidos")]
        [RegularExpression("^(?!Aa|Bb|Cc).*$", ErrorMessage = "Para el apellido solo se admiten letras")]
        [MinLength(5, ErrorMessage = "El Campo apellido debe de tener minimo 5 caracteres")]
        public string StrApellidos { get; set; }

        [Required(ErrorMessage = "La direccion es requerido")]
        [Display(Name = "Direccion")]
        [RegularExpression("^(?!Aa|Bb|Cc).*$", ErrorMessage = "Para la direccion solo se admiten letras")]
        [MinLength(5, ErrorMessage = "El Campo direccion debe de tener minimo 5 caracteres")]
        public string StrDireccion { get; set; }

        [Display(Name = "Telefono fijo")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "En el campo telefono solo se admiten numeros")]
        [MinLength(5, ErrorMessage = "el telefono debe de ser minimo de 5 caracteres")]
        public string StrTelefono { get; set; }


        [Required(ErrorMessage = "El campo celular es requerido")]
        [Display(Name = "Celular")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "En el campo celular solo se admiten numeros")]
        [MinLength(10, ErrorMessage = "el celular debe de ser de 5 caracteres")]
        public string StrCelular { get; set; }


        [Required(ErrorMessage = "El campo departamento es requerido")]
        [Display(Name = "Departamento")]
        public string StrDepartamento { get; set; }

        [Required(ErrorMessage = "El campo ciudad es requerido")]
        [Display(Name = "Ciudad")]
        public string StrCiudad { get; set; }

        [Required(ErrorMessage = "El campo barrio es requerido")]
        [Display(Name = "Poblacion o Barrio")]
        public string StrBarrio { get; set; }

        public string StrImagen { get; set; }
        public string StrEmail { get; set; }

    }


    public class ListaVisitaApoyo
    {
        public Int64 Id { get; set; }
        public string StrDocumento { get; set; }
        public string StrNombre { get; set; }
        public bool LogVisita { get; set; }
    }

    public class Apoyo
    {
        public string StrDocumento { get; set; }
        [Display(Name = "Nombre de la Asesora")]
        public string StrNombre { get; set; }

        [Required(ErrorMessage = "El campo Observacion es requerido")]
        [Display(Name = "Observacion")]
        public string StrObservacion { get; set; }

        [Display(Name = "Tiene Pedido")]
        public string StrPedido { get; set; }

        public bool LogPedido { get; set; }

        public string StrEmail { get; set; }

    }

    public class Nuevas
    {
        public string StrEmail { get; set; }
        [Display(Name = "Documento No")]
        public string StrDocumento { get; set; }

        [Display(Name = "Nombre de la Asesora")]
        public string StrNombre { get; set; }


        public Int32 NumPuntos { get; set; }
        public double CurValorPedido { get; set; }

        [Display(Name = "Observacion")]
        public string StrObservacion { get; set; }

        public string StrTelefono { get; set; }


    }

    public class ListaCobranza
    {
        public string StrEmail { get; set; }
        public string StrCampaña { get; set; }
        public string StrDocumento { get; set; }
        public string StrNombre { get; set; }
        public double CurSaldo { get; set; }
        public string DtmFechaVencimiento { get; set; }
        public Int32 NumPuntos { get; set; }
        public string StrTelefono { get; set; }
        public string StrObservacion { get; set; }


    }

    public class Motivacion
    {
        public string StrEmail { get; set; }
        public string StrZona { get; set; }
        public string StrDocumento { get; set; }
        public string StrNombre { get; set; }
        public string StrTelefono { get; set; }
        public Int32 NumPuntos { get; set; }
        public string StrUltimaCampaña { get; set; }
        public string StrObservacion { get; set; }
        public string  StrTipoAsesora { get; set; }

    }

    public class PosiblesReingresos
    {
        public string StrEmail { get; set; }
        public string StrDocumento { get; set; }
        public string StrNombre { get; set; }
        public string StrTelefono { get; set; }
        public string StrUltimaCampaña { get; set; }
        public string StrObservacion { get; set; }

        public double CurSaldo { get; set; }


    }

    public class InformeVisitas
    {
        public string StrCampaña { get; set; }
        public string StrDivision { get; set; }
        public string StrZona { get; set; }
        public string StrDocumento { get; set; }
        public string StrNombre { get; set; }
        public int IdTipoVisita { get; set; }
        public string StrTipoVisita { get; set; }
        public string StrObservacion { get; set; }
        public string LogPedido { get; set; }
        public string DtmFecha { get; set; }
        public string DtmHora { get; set; }
    }

    public class InformeVisitasDiv
    {
        public string StrCampaña { get; set; }
        public string StrEmail { get; set; }
        public string StrZona { get; set; }

    }
}
