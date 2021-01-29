using System.ComponentModel.DataAnnotations;

namespace HXCloud.ViewModel
{
    public class DeviceCardPositionUpdateDto
    {
        [Required(ErrorMessage = "纬度不能为空")]
        public string Latitude { get; set; }
        [Required(ErrorMessage = "经度不能为空")]
        public string Longitude { get; set; }
        public string ICCID { get; set; }
        public string IMEI { get; set; }
    }
}
