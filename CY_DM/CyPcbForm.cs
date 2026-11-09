using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_DM
{
    public class CyPcbForm : BaseDM
    {
        public Guid UserID { get; set; }
        public virtual CyUser? User { get; set; }
        public string? Title { get; set; }
        public string? IPCstandardIPC_A_600 {  get; set; }
        public int PCBquantity { get; set; }
        public string? PCBmaterial { get; set; }
        public string? PartNumber { get; set; }
        public string? BoardThickness { get; set; }
        public string? CuThicknessTop { get; set; }
        public string? CuThicknessBottom { get; set; }
        public string? CuThicknessPlanes { get; set; }
        public string? CuThicknessLayers { get; set; }
        public int Layers { get; set; }
        public string? CuttingLayer { get; set; }
        public string? SurfaceFinish { get; set; }
        public string? SolderMask { get; set; }
        public string? SolderMaskLayer { get; set; }
        public string? SolderMaskThickness { get; set; }
        public string? SilkScreenLayer { get; set; }
        public string? SilkScreenColor { get; set; }
        public string? ViaFilling { get; set; }
        public string? SolderMaskColor { get; set; }
        public Guid? ZipFile {  get; set; }
        public bool MechanizedَََAssembly { get; set; }
        public string? IPC_A_610_G {  get; set; }
        public string? SoliderPaste { get; set; }
        public Guid? BomExcell {  get; set; }
        public Guid? PlaceAndPick { get; set; }
        public string? DescriptionOne { get; set; }
        public string? DescriptionTwo { get; set; }
        public Guid? StackedLayers { get; set; }
        public int PCBquantityTwo { get; set; }

    }
}
