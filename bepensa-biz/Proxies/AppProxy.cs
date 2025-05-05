using AutoMapper;
using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_biz.Proxies;
using bepensa_biz.Security;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models;
using bepensa_models.App;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace bepensa_biz;

public class AppProxy : ProxyBase, IApp
{
    private readonly IMapper mapper;

    public AppProxy(BepensaContext context, IMapper mapper)
    {
        DBContext = context;
        this.mapper = mapper;
    }
    
    public async Task<Respuesta<string>> ConsultaParametro(int pParametro)
    {
        Respuesta<string> resultado = new();

        try
        {
            var img =  await DBContext.Parametros.Where(p => p.Id == pParametro).FirstOrDefaultAsync();
            

            if (img == null)
            {
                resultado.Codigo = (int)CodigoDeError.NonExistentRecord;
                resultado.Mensaje = CodigoDeError.NonExistentRecord.GetDescription();
                resultado.Exitoso = false;

                return resultado;
            }

            resultado.Data = img.Valor;

        }
        catch (Exception)
        {
            resultado.Codigo = (int)CodigoDeError.Excepcion;
            resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
            resultado.Exitoso = false;
        }

        return resultado;
    }

    public async Task<Respuesta<List<ImagenesPromocionesDTO>>> ConsultaImgPromociones(int pParametro)
    {
        Respuesta<List<ImagenesPromocionesDTO>> resultado = new();

        try
        {
            var pdo = await DBContext.Periodos.Where(p => p.Fecha.Year == DateTime.Now.Year && p.Fecha.Month == DateTime.Now.Month).FirstOrDefaultAsync();
            var img =  await DBContext.ImagenesPromociones.Where(i => i.IdCanal == pParametro && i.IdPeriodo == pdo.Id).ToListAsync();

            if (img == null)
            {
                resultado.Codigo = (int)CodigoDeError.NonExistentRecord;
                resultado.Mensaje = CodigoDeError.NonExistentRecord.GetDescription();
                resultado.Exitoso = false;

                return resultado;
            }

            resultado.Data = mapper.Map<List<ImagenesPromocionesDTO>>(img);

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
