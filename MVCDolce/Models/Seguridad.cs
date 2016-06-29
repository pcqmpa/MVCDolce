using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDolce.Models
{
    public class Seguridad
    {

        [Required(ErrorMessage = "El correo es requerido")]
        [Display(Name = "Correo electronico")]
        [EmailAddress(ErrorMessage = "El correo no tiene el formato correcto")]
        [DataType(DataType.EmailAddress)]
        public string StrEmail { get; set; }

        [Required(ErrorMessage = "El password es requerido")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string StrPassword { get; set; }


        public string StrNombre { get; set; }
        public int IdGrupo { get; set; }
        public string StrNombreGrupo { get; set; }
        public string StrZona { get; set; }
        public int LogDivisional { get; set; }
        public string StrCampaña { get; set; }

    }

    public class Zonas
    {
        public string StrZona { get; set; }
    }

    public class Campañas
    {
        public string StrCampaña { get; set; }
    }


    public class DatosVarios
    {
        public List<Campañas> ListaCampañas { get; set; }
        public List<Zonas> ListaZonas { get; set; }
    }

    public class Registro
    {
        [Required(ErrorMessage = "El correo es requerido")]
        [Display(Name = "Correo electronico")]
        [EmailAddress(ErrorMessage = "El correo no tiene el formato correcto")]
        public string StrEmail { get; set; }

        [Required(ErrorMessage ="El nombre del usuario es requerido")]
        [Display(Name = "Nombre de Usuario")]
        public string StrNombre { get; set; }

        [Required(ErrorMessage = "El password es requerido")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage = "El password debe de ser minimo de 8 caracteres")]
        public string StrPassword { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Password")]
        [Compare("StrPassword",ErrorMessage = "El password no coincide intente denuevo")]
        public string StrConfirmarPassword { get; set; }


        [Required(ErrorMessage = "El campo cedula es requerido")]
        [Display(Name = "Documento No")]
        [MinLength(4,ErrorMessage = "Debe ingresar minimo 4 caracteres")]
        public string StrCedula { get; set; }
        
    }

    public class RecuperarPassword
    {
        [Required(ErrorMessage = "El numero de cedula es requerido")]
        [Display(Name = "Documento No")]
        [StringLength(20,MinimumLength = 5)]
        public string StrCedula { get; set; }

        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "El correo no tiene el formato correcto")]
        [Display(Name = "Correo electronico")]
        public string StrEmail { get; set; }

        public string  StrPassword { get; set; }
    }


    public class CabioPassword
    {

        [Required(ErrorMessage = "El password es requerido")]
        [Display(Name = "Password Anterior")]
        [DataType(DataType.Password)]
        public string StrPasswordAnterior { get; set; }

        [Required(ErrorMessage = "El nuevo password es requerido")]
        [Display(Name = "Password nuevo")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "El password debe de ser minimo de 8 caracteres")]
        public string StrPaswordNew { get; set; }

        [Required(ErrorMessage = "La confirmacion del password es requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Password")]
        [Compare("StrPaswordNew", ErrorMessage = "El password no coincide intente denuevo")]
        public string StrConfirmarPassword { get; set; }

        public string StrEmail { get; set; }

    }
}
