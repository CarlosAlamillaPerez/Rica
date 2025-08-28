using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_models.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Drawing;


namespace bepensa_biz.Proxies
{
    public class ExportacionProxy : IExportacion
    {
        private readonly GlobalSettings _ajustes;
        private IReporte _reporte { get; set; }

        public ExportacionProxy(IOptionsSnapshot<GlobalSettings> ajustes, IReporte reporte)
        {
            _ajustes = ajustes.Value;
            _reporte = reporte;
        }

        /// <summary>
        /// Exporta colleccion de datos segun reportes dados de alta
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requerimiento"></param>
        /// <param name="NameSheet"></param>
        /// <param name="logo"></param>
        /// <param name="data"></param>
        /// <param name="StyleTable"></param>
        /// <param name="PeriodoI"></param>
        /// <param name="PeriodoF"></param>
        /// <param name="reporte"></param>
        /// <returns></returns>

        public byte[] GeneraExportacion<T>(string requerimiento, string NameSheet, FileInfo logo, List<T> data, int StyleTable, DateTime? PeriodoI, DateTime? PeriodoF, string reporte)
        {
            byte[] result = null;

            using (var package = new ExcelPackage())
            {
                //Estilos de la tabla de datos
                var estilodata = OfficeOpenXml.Table.TableStyles.Medium1;

                estilodata = StyleTable switch
                {
                    1 => OfficeOpenXml.Table.TableStyles.Medium2,
                    _ => OfficeOpenXml.Table.TableStyles.Medium1,
                };

                // Add a new worksheet to the empty workbook
                ExcelWorksheet ws = package.Workbook.Worksheets.Add(NameSheet);

                #region Encabezado
                #region imagen
                ExcelPicture pic = ws.Drawings.AddPicture("Logo LMS", logo);
                pic.SetPosition(1, 0, 0, 0);
                #endregion

                #region Renglon 1
                using (ExcelRange rng = ws.Cells[8, 1, 8, 2])
                {
                    rng.Merge = true;
                    rng.Value = "Proyecto:";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[8, 3, 8, 5])
                {
                    rng.Merge = true;
                    rng.Value = "Bepensa";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[8, 6])
                {
                    rng.Value = "Fecha de consulta";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[8, 7])
                {
                    rng.Value = DateTime.Now.ToString("dd/MM/yyyy");
                    rng.Style.Font.Size = 14;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns(21);
                }
                #endregion

                #region Renglon 2
                using (ExcelRange rng = ws.Cells[9, 1, 9, 2])
                {
                    rng.Merge = true;
                    rng.Value = "Nombre del cliente:";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[9, 3, 9, 5])
                {
                    rng.Merge = true;
                    rng.Value = "Bepensa";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[9, 6])
                {
                    rng.Value = "Hora de consulta";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[9, 7])
                {
                    rng.Value = DateTime.Now.ToString("h:mm:ss tt");
                    rng.Style.Font.Size = 14;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns();
                }
                #endregion

                #region Renglon 3
                using (ExcelRange rng = ws.Cells[10, 1, 10, 2])
                {
                    rng.Merge = true;
                    rng.Value = (PeriodoI != null && PeriodoF != null) ? "Periodo:" : "";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[10, 3, 10, 5])
                {
                    rng.Merge = true;
                    rng.Value = (PeriodoI != null && PeriodoF != null) ? String.Format("{0:d} - {1:d}", PeriodoI, PeriodoF) : "";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[10, 6])
                {
                    rng.Value = "Requerimiento:";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[10, 7])
                {
                    rng.Value = requerimiento;
                    rng.Style.Font.Size = 14;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns();
                }

                #endregion
                #endregion

                #region Datos
                if (data != null && data.Count > 0)
                {
                    ws.Cells["A12"].LoadFromCollection(data, true, estilodata);
                }
                else
                {
                    using ExcelRange rng = ws.Cells[12, 1, 12, 7];

                    rng.Merge = true;
                    rng.Value = "No hay Información";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                    goto DocumentProperties;
                }

                #endregion

                #region Formato columnas
                switch (reporte)
                {
                    case "Usuarios Registrados":
                    case "Usuarios Activos":
                    case "Usuarios Por Activar":
                    case "Representantes Medicos":
                    case "Medicos":
                    case "Representantes Medicos Activos":
                    case "Medicos Activos":
                    case "Representantes Por Activar":
                    case "Medicos Por Activar":
                        ws.Cells["A12"].Value = "Codigo Unico Cliente";
                        ws.Cells["C12"].Value = "Correo Electronico";
                        ws.Cells["D12"].Value = "Fecha Registro";
                        ws.Cells["D:D"].Style.Numberformat.Format = "dd/mm/yyyy";
                        //ws.Cells["D:D"].Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                        //.Cells["AH:AH"].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                        ws.Cells["E:E"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["F:F"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case "Pacientes":
                    case "Pacientes Activos":
                    case "Pacientes Por Activar":
                        ws.Cells["A12"].Value = "Codigo Unico Cliente";
                        ws.Cells["D12"].Value = "Correo Electronico";
                        ws.Cells["F12"].Value = "Fecha Registro";
                        ws.Cells["I12"].Value = "Medicamento Registrado";
                        ws.Cells["J12"].Value = "Id Médico";
                        ws.Cells["K12"].Value = "Nombre Médico";
                        ws.Cells["F:F"].Style.Numberformat.Format = "dd/mm/yyyy";
                        //ws.Cells["D:D"].Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                        //.Cells["AH:AH"].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                        ws.Cells["F:F"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["J:J"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;

                    case "Puntos Acumulados":
                        ws.Cells["A12"].Value = "Codigo Unico Cliente";
                        ws.Cells["F12"].Value = "Fecha Registro";
                        ws.Cells["F:F"].Style.Numberformat.Format = "dd/mm/yyyy";
                        break;
                    case "Puntos Redimidos":
                    case "Puntos Disponibles":
                    case "Puntos Cancelados":
                    case "Puntos Vencidos":
                        ws.Cells["A12"].Value = "Codigo Unico Cliente";
                        ws.Cells["D12"].Value = "Fecha Registro";
                        ws.Cells["D:D"].Style.Numberformat.Format = "dd/mm/yyyy";
                        break;
                    case "Llamadas":
                        ws.Cells["B12"].Value = "Codigo Unico Cliente";
                        ws.Cells["F12"].Value = "Tipo Llamada";
                        ws.Cells["I12"].Value = "Fecha Registro";
                        ws.Cells["I:I"].Style.Numberformat.Format = "dd/mm/yyyy";
                        ws.Cells["J12"].Value = "Operador Registro";
                        ws.Cells["K12"].Value = "Operador Modifico";
                        break;
                    case "Llamadas Farmacovigilancia":
                        ws.Cells["A12"].Value = "Codigo Llamada";
                        ws.Cells["B12"].Value = "Categoria Llamada";
                        ws.Cells["C12"].Value = "Subcategoria Llamada";
                        ws.Cells["D12"].Value = "Fecha / Hora Registro";
                        ws.Cells["D:D"].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                        ws.Cells["E12"].Value = "Nombre Informante";
                        ws.Cells["F12"].Value = "Telefono Informante";
                        ws.Cells["G12"].Value = "Email Informante";
                        ws.Cells["H12"].Value = "Nombre Paciente";
                        ws.Cells["I12"].Value = "Genero Paciente";
                        ws.Cells["J12"].Value = "Edad Paciente";
                        ws.Cells["K12"].Value = "Estado Paciente";
                        ws.Cells["L12"].Value = "Municipio Paciente";
                        ws.Cells["M12"].Value = "Colonia Paciente";
                        ws.Cells["N12"].Value = "Medicamento";
                        ws.Cells["O12"].Value = "Dosis";
                        ws.Cells["P12"].Value = "No lote";
                        ws.Cells["Q12"].Value = "Fecha vencimiento Medicamento";
                        ws.Cells["Q:Q"].Style.Numberformat.Format = "dd/mm/yyyy";
                        ws.Cells["R12"].Value = "Descripcion";
                        break;
                    case "Tickets Cargados":
                        ws.Cells["A12"].Value = "Id paciente";
                        ws.Cells["C12"].Value = "Correo electónico paciente";
                        ws.Cells["D12"].Value = "Celular Paciente";
                        ws.Cells["E12"].Value = "Fecha carga ticket";
                        ws.Cells["E:E"].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                        ws.Cells["F12"].Value = "Hora carga ticket";
                        ws.Cells["H12"].Value = "Fecha ticket";
                        ws.Cells["H:H"].Style.Numberformat.Format = "dd/mm/yyyy";
                        ws.Cells["I12"].Value = "No ticket";
                        ws.Cells["L12"].Value = "Estado Ticket";
                        ws.Cells["M12"].Value = "Motivo rechazo";
                        break;
                    case "Comunicaciones":
                        ws.Cells["A12"].Value = "Id cliente";
                        ws.Cells["C12"].Value = "Nombre cliente";
                        ws.Cells["D12"].Value = "Correo electrónico";
                        ws.Cells["F12"].Value = "Tipo Envio";
                        ws.Cells["H12"].Value = "Fecha envio";
                        ws.Cells["H:H"].Style.Numberformat.Format = "dd/mm/yyyy";
                        ws.Cells["I12"].Value = "Hora Envio";
                        ws.Cells["J12"].Value = "Texto SMS";
                        ws.Cells["L12"].Value = "Cantidad SMS";
                        break;
                    default:
                        break;
                }

                ws.Cells.AutoFitColumns();
            #endregion

            DocumentProperties:
                #region propiedades del documento
                package.Workbook.Properties.Title = "Reporte " + NameSheet;
                package.Workbook.Properties.Author = "CRM - Loyalty Marketing Services";
                package.Workbook.Properties.Comments = "Reporte de " + NameSheet + " Bepensa";

                package.Workbook.Properties.Company = "Loyalty Marketing Services";

                package.Workbook.Properties.SetCustomPropertyValue("Generado por", "Loyalty Marketing Services");
                package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "Loyalty Marketing Services");
                #endregion

                result = package.GetAsByteArray();

                //var xlFile = GetFileInfo("Reporte" + NameSheet + ".xlsx");
                //package.SaveAs(xlFile);
                //return xlFile.FullName;
            }
            return result;
        }

        /// <summary>
        /// Exporta colleccion de datos segun reportes dados de alta de forma Dinamica
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="IdReporte"></param>
        /// <param name="logo"></param>
        /// <param name="data"></param>
        /// <param name="PeriodoI"></param>
        /// <param name="PeriodoF"></param>
        /// <returns></returns>
        public byte[] GeneraExportacionDinamica<T>(int IdReporte, FileInfo logo, List<T> data, DateTime? PeriodoI, DateTime? PeriodoF)
        {
            ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");
            ReporteDTO ReporteConfig = _reporte.GetDataReporte(IdReporte).Data;

            byte[] result = null;
            using (var package = new ExcelPackage())
            {
                // Add a new worksheet to the empty workbook
                ExcelWorksheet ws = package.Workbook.Worksheets.Add(ReporteConfig.Nombre);

                #region Encabezado
                #region imagen
                ExcelPicture pic = ws.Drawings.AddPicture("Logo LMS", logo);
                pic.SetPosition(1, 0, 0, 0);
                #endregion

                #region Renglon 1
                using (ExcelRange rng = ws.Cells[8, 1, 8, 2])
                {
                    rng.Merge = true;
                    rng.Value = "Proyecto:";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[8, 3, 8, 5])
                {
                    rng.Merge = true;
                    rng.Value = _ajustes.AppName;
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[8, 6])
                {
                    rng.Value = "Fecha de consulta";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[8, 7])
                {
                    rng.Value = DateTime.Now.ToString("dd/MM/yyyy");
                    rng.Style.Font.Size = 14;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns(21);
                }
                #endregion

                #region Renglon 2
                using (ExcelRange rng = ws.Cells[9, 1, 9, 2])
                {
                    rng.Merge = true;
                    rng.Value = "Nombre del cliente:";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[9, 3, 9, 5])
                {
                    rng.Merge = true;
                    rng.Value = _ajustes.ClientName;
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[9, 6])
                {
                    rng.Value = "Hora de consulta";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[9, 7])
                {
                    rng.Value = DateTime.Now.ToString("h:mm:ss tt");
                    rng.Style.Font.Size = 14;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns();
                }
                #endregion

                #region Renglon 3
                using (ExcelRange rng = ws.Cells[10, 1, 10, 2])
                {
                    rng.Merge = true;
                    rng.Value = (PeriodoI != null && PeriodoF != null) ? "Periodo:" : "";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[10, 3, 10, 5])
                {
                    rng.Merge = true;
                    rng.Value = (PeriodoI != null && PeriodoF != null) ? String.Format("{0:d} al {1:d}", PeriodoI, PeriodoF) : "";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[10, 6])
                {
                    rng.Value = "Requerimiento:";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[10, 7])
                {
                    rng.Value = "Reporte de " + ReporteConfig.Nombre;
                    rng.Style.Font.Size = 14;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns();
                }

                #endregion
                #endregion

                #region Datos

                if (data != null)
                {
                    var datos = data.Select(x => x.ToString())
                                      .Select(x => JObject.Parse(x))
                                      .Select(x => x.ToObject<Dictionary<string, object>>())
                                      .Select(x => x.ToDictionary(y => y.Key, y => y.Value))
                                      .ToList();

                    if (datos != null && datos.Count > 0)
                    {
                        ws.Cells["A12"].LoadFromCollection(datos, true, (TableStyles)ReporteConfig.EstiloTabla);
                    }
                }
                else
                {
                    using ExcelRange rng = ws.Cells[12, 1, 12, 7];

                    rng.Merge = true;
                    rng.Value = "No hay Información";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                    goto DocumentProperties;
                }

                #endregion

                #region Formato columnas


                ws.Cells.AutoFitColumns();
            #endregion

            DocumentProperties:
                #region propiedades del documento
                package.Workbook.Properties.Title = "Reporte de " + ReporteConfig.Nombre;
                package.Workbook.Properties.Author = "CRM - Loyalty Marketing Services";
                package.Workbook.Properties.Comments = "Reporte de " + ReporteConfig.Nombre + " " + _ajustes.ClientName;

                package.Workbook.Properties.Company = "Loyalty Marketing Services";

                package.Workbook.Properties.SetCustomPropertyValue("Generado por", "Loyalty Marketing Services");
                package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "Loyalty Marketing Services");
                #endregion

                result = package.GetAsByteArray();

            }
            return result;
        }

        public async Task<byte[]> GeneraExportacionDinamicaAsync<T>(int IdReporte, FileInfo logo, List<T> data, DateTime? PeriodoI, DateTime? PeriodoF)
        {
            ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");
            ReporteDTO ReporteConfig = _reporte.GetDataReporte(IdReporte).Data;
            byte[] result;
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add(ReporteConfig.Nombre);

                #region Encabezado
                #region imagen
                ExcelPicture pic = ws.Drawings.AddPicture("Logo LMS", logo);
                pic.SetPosition(1, 0, 0, 0);
                #endregion

                #region Renglon 1
                using (ExcelRange rng = ws.Cells[8, 1, 8, 2])
                {
                    rng.Merge = true;
                    rng.Value = "Proyecto:";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[8, 3, 8, 5])
                {
                    rng.Merge = true;
                    rng.Value = _ajustes.AppName;
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[8, 6])
                {
                    rng.Value = "Fecha de consulta";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[8, 7])
                {
                    rng.Value = DateTime.Now.ToString("dd/MM/yyyy");
                    rng.Style.Font.Size = 14;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns(21);
                }
                #endregion

                #region Renglon 2
                using (ExcelRange rng = ws.Cells[9, 1, 9, 2])
                {
                    rng.Merge = true;
                    rng.Value = "Nombre del cliente:";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[9, 3, 9, 5])
                {
                    rng.Merge = true;
                    rng.Value = _ajustes.ClientName;
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[9, 6])
                {
                    rng.Value = "Hora de consulta";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[9, 7])
                {
                    rng.Value = DateTime.Now.ToString("h:mm:ss tt");
                    rng.Style.Font.Size = 14;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns();
                }
                #endregion

                #region Renglon 3
                using (ExcelRange rng = ws.Cells[10, 1, 10, 2])
                {
                    rng.Merge = true;
                    rng.Value = (PeriodoI != null && PeriodoF != null) ? "Periodo:" : "";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[10, 3, 10, 5])
                {
                    rng.Merge = true;
                    rng.Value = (PeriodoI != null && PeriodoF != null) ? String.Format("{0:d} al {1:d}", PeriodoI, PeriodoF) : "";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[10, 6])
                {
                    rng.Value = "Requerimiento:";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }

                using (ExcelRange rng = ws.Cells[10, 7])
                {
                    rng.Value = "Reporte de " + ReporteConfig.Nombre;
                    rng.Style.Font.Size = 14;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.AutoFitColumns();
                }

                #endregion
                #endregion

                #region Datos
                if (data != null)
                {
                    var datos = data.Select(x => x.ToString())
                                    .Select(x => JObject.Parse(x))
                                    .Select(x => x.ToObject<Dictionary<string, object>>())
                                    .Select(x => x.ToDictionary(y => y.Key, y => y.Value))
                                    .ToList();

                    if (datos != null && datos.Count > 0)
                    {
                        ws.Cells["A12"].LoadFromCollection(datos, true, (TableStyles)ReporteConfig.EstiloTabla);
                    }
                }
                else
                {
                    using ExcelRange rng = ws.Cells[12, 1, 12, 7];
                    rng.Merge = true;
                    rng.Value = "No hay Información";
                    rng.Style.Font.Size = 14;
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Name = "Arial";
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);
                    rng.AutoFitColumns();
                }
                #endregion

                #region Formato y Propiedades
                ws.Cells.AutoFitColumns();

                package.Workbook.Properties.Title = "Reporte de " + ReporteConfig.Nombre;
                package.Workbook.Properties.Author = "CRM - Loyalty Marketing Services";
                package.Workbook.Properties.Comments = "Reporte de " + ReporteConfig.Nombre + " " + _ajustes.ClientName;
                package.Workbook.Properties.Company = "Loyalty Marketing Services";
                package.Workbook.Properties.SetCustomPropertyValue("Generado por", "Loyalty Marketing Services");
                package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "Loyalty Marketing Services");
                #endregion

                result = await package.GetAsByteArrayAsync();
            }

            return result;
        }


        static DirectoryInfo _outputDir = null;
        public static DirectoryInfo OutputDir
        {
            get
            {
                return _outputDir;
            }
            set
            {
                _outputDir = value;
                if (!_outputDir.Exists)
                {
                    _outputDir.Create();
                }
            }
        }

        public static FileInfo GetFileInfo(string file, bool deleteIfExists = true)
        {
            var fi = new FileInfo(@"f:/" + file);
            if (deleteIfExists && fi.Exists)
            {
                fi.Delete();  // ensures we create a new workbook
            }
            return fi;

        }
    }
}
