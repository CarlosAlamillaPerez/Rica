using AutoMapper;
using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.DataModels;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace bepensa_biz.Proxies
{
    public class LlamadasProxy : ProxyBase, ILlamada
    {
        private readonly IMapper mapper;

        public LlamadasProxy(BepensaContext context, IMapper mapper)
        {
            DBContext = context;
            this.mapper = mapper;
        }

        public async Task<Respuesta<Empty>> RegistrarLlamada(LlamadaRequest pLlamada)
        {

            Respuesta<Empty> resultado = new()
            {
                IdTransaccion = Guid.NewGuid()
            };

            try
            {
                var valida = Extensiones.ValidateRequest(pLlamada);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;
                    
                    return resultado;
                }

                var operador = await DBContext.Operadores.FirstOrDefaultAsync(op => op.Id == pLlamada.IdOperador);

                if (operador == null)
                {
                    resultado.Codigo = (int)CodigoDeError.OperadorInvalido;
                    resultado.Mensaje = CodigoDeError.OperadorInvalido.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (pLlamada.IdUsuario != null)
                {
                    var validaUsuario = await DBContext.Usuarios.AnyAsync(us => us.Id == pLlamada.IdUsuario);

                    if (validaUsuario)
                    {
                        resultado.Codigo = (int)CodigoDeError.UsuarioInvalido;
                        resultado.Mensaje = CodigoDeError.UsuarioInvalido.GetDisplayName();
                        resultado.Exitoso = false;

                        return resultado;
                    }
                }                

                var fechaAcutal = DateTime.Now;

                BitacoraDeOperadore bdop = new()
                {
                    IdOperador = pLlamada.IdOperador,
                    IdTipoDeOperacion = (int)TipoOperacion.RegistroLlamada,
                    FechaReg = fechaAcutal,
                    Notas = TipoOperacion.RegistroLlamada.GetDescription(),
                    IdUsuarioAftd = pLlamada.IdUsuario
                };

                Llamada llamada = new()
                {
                    IdPadre = pLlamada.IdPadre,
                    Tema = pLlamada.Tema,
                    IdUsuario = pLlamada.IdUsuario,
                    Nombre = pLlamada.Nombre,
                    Telefono = pLlamada.Telefono,
                    Comentario = pLlamada.Comentario,
                    IdTipoLlamada = pLlamada.IdTipoLlamada,
                    IdSubcategoriaLlamada = pLlamada.IdSubcategoriaLlamada,
                    IdEstatusLlamada = pLlamada.IdEstatusLlamada,
                    FechaReg = fechaAcutal,
                    IdOperadorReg = pLlamada.IdOperador
                };

                operador.BitacoraDeOperadoreIdOperadorNavigations.Add(bdop);
                operador.LlamadaIdOperadorRegNavigations.Add(llamada);

                await DBContext.SaveChangesAsync();

            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }
    }
}
