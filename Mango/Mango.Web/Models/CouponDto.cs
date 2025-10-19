namespace Mango.Web.Models;

public class CouponDto
{
    public int CoupunId { get; set; }
    public string CouponCode { get; set; }
    public double DiscountAmount { get; set; }
    public int MinAmount { get; set; }
}