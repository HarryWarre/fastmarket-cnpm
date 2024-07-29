using fastwebsite.Entities;

namespace fastwebsite.ViewModel
{
    public class CheckoutViewModel
    {
        public Cart Cart { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
